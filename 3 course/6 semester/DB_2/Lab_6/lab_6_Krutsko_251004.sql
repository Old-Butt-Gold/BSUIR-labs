-- 1. Создать хранимую функцию, получающую на вход идентификатор читателя 
-- и возвращающую список идентификаторов книг, которые он уже прочитал и вернул в библиотеку.
CREATE OR ALTER FUNCTION sfn_get_returned_books(@subscriber_id INT) 
RETURNS @returned_books TABLE (sb_book INT)
AS
BEGIN
	INSERT INTO @returned_books
		SELECT sb_book
		FROM subscriptions
		WHERE sb_subscriber = @subscriber_id
		AND sb_is_active = 'N';

	RETURN;
END;

-- 3. Создать хранимую функцию, получающую на вход идентификатор читателя и воз-вращающую 1, 
-- если у читателя на руках сейчас менее десяти книг, и 0 в противном случае.
CREATE OR ALTER FUNCTION sfn_check_book_count(@subscriber_id INT) 
RETURNS INT
AS
BEGIN
	DECLARE @count INT;

	SET @count = (
				SELECT COUNT(*) 
				FROM subscriptions
				WHERE sb_is_active = 'Y' AND
				sb_subscriber = @subscriber_id
				);
	IF @count < 10 
	BEGIN
		RETURN 1;
	END;

	RETURN 0;
END;

-- 4. Создать хранимую функцию, получающую на вход год издания книги и возвращающую 1, 
-- если книга издана менее ста лет назад, и 0 в противном случае.

-- 5. Создать хранимую процедуру, обновляющую все поля типа DATE (если такие есть) 
-- всех записей указанной таблицы на значение текущей даты.
CREATE OR ALTER PROCEDURE spr_update_date_fields
    @table_name NVARCHAR(128)
AS
BEGIN

    DECLARE @sql NVARCHAR(MAX);
    
    SET @sql = (
		SELECT STRING_AGG(
        'UPDATE ' + QUOTENAME(@table_name) + 
        ' SET ' + QUOTENAME(c.name) + ' = GETDATE();', 
        CHAR(10) -- '\n'
    )
		FROM sys.columns AS c
		INNER JOIN sys.types AS t 
		ON c.user_type_id = t.user_type_id
		WHERE c.object_id = OBJECT_ID(@table_name)
		  AND t.name = 'date'
	);

	IF @sql IS NOT NULL
    BEGIN
        EXEC sp_executesql @sql;
    END
END;

-- 8. Создать хранимую процедуру, запускаемую по расписанию раз в неделю и оптимизирующую 
-- (дефрагментирующую, компактифицирующую) все таблицы базы данных, 
-- в которых находится не менее одного миллиона записей.

CREATE OR ALTER VIEW vw_database_info_full 
AS
(
	SELECT [tables].[name] AS [table_name],
	 [indexes].[name] AS [index_name],
	 [indexes].[type] AS [index_type],
	 [stats].[index_type_desc] AS [index_type_desc],
	 [indexes].[object_id] AS [index_object_id],
	 [columns].[name] AS [column_name],
	 [stats].[avg_fragmentation_in_percent] AS [avg_fragm_perc],
	 [stats].[avg_page_space_used_in_percent] AS [avg_space_perc],
	 [p].[rows] AS row_count
	FROM sys.indexes AS [indexes]
		INNER JOIN sys.index_columns AS [index_columns]
		ON [indexes].[object_id] = [index_columns].[object_id]
		AND [indexes].[index_id] = [index_columns].[index_id]
		INNER JOIN sys.columns AS [columns]
		ON [index_columns].[object_id] = [columns].[object_id]
		AND [index_columns].[column_id] = [columns].[column_id]
		INNER JOIN sys.tables AS [tables]
		ON [indexes].[object_id] = [tables].[object_id]
		INNER JOIN sys.dm_db_index_physical_stats(DB_ID(DB_NAME()),
		NULL, NULL, NULL, 'SAMPLED') AS [stats]
		ON [indexes].[object_id] = [stats].[object_id]
		AND [indexes].[index_id] = [stats].[index_id]
		INNER JOIN sys.partitions AS p
        ON [indexes].[object_id] = [p].[object_id]
        AND [indexes].[index_id] = [p].[index_id]
 );

 GO

 CREATE OR ALTER VIEW vw_database_info_main
 AS
 (
 SELECT DISTINCT
	[tables].[name] AS [table_name],
	[indexes].[name] AS [index_name],
	[stats].[avg_fragmentation_in_percent] AS [avg_fragm_perc],
	[p].[rows] AS row_count
FROM sys.indexes AS [indexes]
	INNER JOIN sys.tables AS [tables]
	ON [indexes].[object_id] = [tables].[object_id]
	INNER JOIN sys.dm_db_index_physical_stats(DB_ID(DB_NAME()),
	NULL, NULL, NULL, 'SAMPLED') AS [stats]
	ON [indexes].[object_id] = [stats].[object_id]
	AND [indexes].[index_id] = [stats].[index_id]
	INNER JOIN sys.partitions AS p
        ON [indexes].[object_id] = [p].[object_id]
        AND [indexes].[index_id] = [p].[index_id]
	WHERE [indexes].[type] = 1
 );

 GO

CREATE OR ALTER PROCEDURE OPTIMIZE_ALL_TABLES
AS
BEGIN
	DECLARE @table_name NVARCHAR(200);
	DECLARE @index_name NVARCHAR(200);
	DECLARE @avg_fragm_perc DOUBLE PRECISION;
	DECLARE @query_text NVARCHAR(MAX);
	DECLARE indexes_cursor CURSOR LOCAL FAST_FORWARD FOR
		SELECT table_name, index_name, avg_fragm_perc, row_count
        FROM vw_database_info_main
        WHERE row_count >= 1000000; -- Условие: не менее миллиона записей
	OPEN indexes_cursor;
	FETCH NEXT FROM indexes_cursor INTO 
		@table_name,
		@index_name,
		@avg_fragm_perc;
	WHILE @@FETCH_STATUS = 0
	BEGIN
	 IF (@avg_fragm_perc >= 5.0) AND (@avg_fragm_perc <= 30.0)
	 BEGIN
		 SET @query_text = CONCAT('ALTER INDEX [', @index_name,
		 '] ON [', @table_name, '] REORGANIZE');
		 PRINT CONCAT('Index [', @index_name,'] on [', @table_name,
		 '] will be REORGANIZED...');
		 EXECUTE sp_executesql @query_text;
	 END;
	 IF (@avg_fragm_perc > 30.0)
	 BEGIN
		 SET @query_text = CONCAT('ALTER INDEX [', @index_name,'] ON [',
		 @table_name, '] REBUILD');
		 PRINT CONCAT('Index [', @index_name,'] on [', @table_name,
		 '] will be REBUILT...');
		 EXECUTE sp_executesql @query_text;
	 END;
	 IF (@avg_fragm_perc < 5.0)
	 BEGIN
		PRINT CONCAT('Index [', @index_name,'] on [', @table_name, '] needs no optimization...');
	 END;

	 FETCH NEXT FROM indexes_cursor INTO 
		@table_name,
		@index_name,
		@avg_fragm_perc;
	END;
	CLOSE indexes_cursor;
	DEALLOCATE indexes_cursor;
END;

GO

EXEC msdb.dbo.sp_add_job
    @job_name = N'OptimizeIndexesWeekly';

EXEC msdb.dbo.sp_add_jobstep
    @job_name = N'OptimizeIndexesWeekly',
    @step_name = N'RunOptimization',
    @subsystem = N'TSQL',
    @command = N'EXEC OPTIMIZE_ALL_TABLES;',
    @on_success_action = 1;

EXEC msdb.dbo.sp_add_schedule
    @schedule_name = N'WeeklyOptimization',
    @freq_type = 8, -- Еженедельно
    @freq_interval = 1, -- Каждую неделю
    @active_start_time = 010000; -- Время начала: 01:00

EXEC msdb.dbo.sp_attach_schedule
    @job_name = N'OptimizeIndexesWeekly',
    @schedule_name = N'WeeklyOptimization';

EXEC msdb.dbo.sp_add_jobserver
    @job_name = N'OptimizeIndexesWeekly';