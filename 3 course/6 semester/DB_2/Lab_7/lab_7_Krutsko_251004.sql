-- 1. Создать хранимую процедуру, которая:
-- a. добавляет каждой книге два случайных жанра;
-- b. отменяет совершённые действия, если в процессе работы 
-- хотя бы одна операция вставки завершилась ошибкой в силу 
-- дублирования значения первич-ного ключа таблицы «m2m_books_genres» 
-- (т.е. у такой книги уже был такой жанр).
CREATE OR ALTER PROCEDURE spr_add_every_book_genres
AS
BEGIN
    -- Отключаем режим неявных транзакций
    SET IMPLICIT_TRANSACTIONS OFF;

    DECLARE @Counter INT = 0;

    DECLARE @BookID INT;
    DECLARE @GenreID INT;

    WHILE @Counter < 2
    BEGIN
		-- Начинаем явную транзакцию
		BEGIN TRANSACTION
        -- Вставляем данные для каждой книги и случайного жанра
        INSERT INTO m2m_books_genres (b_id, g_id)
        SELECT 
            b.b_id, 
            g.g_id
        FROM 
            books AS b
        CROSS JOIN (
            SELECT TOP 1 g_id 
            FROM genres 
            ORDER BY NEWID()
        ) AS g;

        -- Проверяем на дубликаты после каждой вставки
        IF EXISTS (
            SELECT b_id, g_id
            FROM m2m_books_genres
            GROUP BY b_id, g_id
            HAVING COUNT(*) > 1
        )
        BEGIN
            -- Если найдены дубликаты, откатываем транзакцию
            ROLLBACK TRANSACTION
        END
		ELSE
		BEGIN
			COMMIT TRANSACTION;
		END;

        SET @Counter = @Counter + 1;
    END;

	SET IMPLICIT_TRANSACTIONS ON;

END;


-- 2. Создать хранимую процедуру, которая:
-- a. увеличивает значение поля «b_quantity» для всех книг в два раза;
-- b. отменяет совершённое действие, если по итогу выполнения 
-- операции среднее количество экземпляров книг превысит значение 50.
CREATE OR ALTER PROCEDURE spr_increase_book_quantity
AS
BEGIN
	BEGIN TRANSACTION;

	UPDATE books
    SET b_quantity = b_quantity * 2;

	DECLARE @avg_quantity FLOAT;
    SELECT @avg_quantity = AVG(b_quantity) FROM books;

	IF @avg_quantity > 50
    BEGIN
        ROLLBACK TRANSACTION;
    END
    ELSE
    BEGIN
        COMMIT TRANSACTION;
    END
END;

-- 6. Создать на таблице «subscriptions» триггер, определяющий уровень изолированности
-- транзакции, в котором сейчас проходит операция обновления,
-- и отменяющий операцию, если уровень изолированности транзакции
--  отличен от REPEATABLE READ.
CREATE OR ALTER TRIGGER trg_check_isolation_level
ON subscriptions
AFTER UPDATE
AS
BEGIN
	DECLARE @isolation_level INT;

	SET @isolation_level =
	(
		 SELECT [transaction_isolation_level]
		 FROM [sys].[dm_exec_sessions]
		 WHERE [session_id] = @@SPID
	);

	/*
		1: Read Uncommitted
		2: Read Committed
		3: Repeatable Read
		4: Serializable
		5: Snapshot
	*/

	IF (@isolation_level <> 3)
	BEGIN
		ROLLBACK TRANSACTION;
		THROW 50000, N'Уровень изоляции должен быть REPEATABLE READ', 1;
	END;

END;

GO

BEGIN TRANSACTION;
SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;
EXEC spr_check_isolation_level;

-- 7. Создать хранимую функцию, порождающую исключительную ситуацию 
-- в случае, если выполняются оба условия 
-- (подсказка: эта задача имеет решение только для MS SQL Server):
-- a. режим автоподтверждения транзакций выключен;
-- b. функция запущена из вложенной транзакции.

-- процедура
CREATE OR ALTER PROCEDURE spr_check_transaction_state
AS
BEGIN

    -- Проверка: автоподтверждение выключено
    IF XACT_STATE() <> 0
    BEGIN
        -- Если транзакция вложена, и режим автоподтверждения выключен
        IF @@TRANCOUNT > 1
        BEGIN
			DECLARE @ErrorMessage NVARCHAR(2048) = N'автоподтверждение транзакций выключено и функция запущена из вложенной транзакции';
			THROW 50000, @ErrorMessage, 1;
        END
    END
END;

GO

-- функция
CREATE OR ALTER FUNCTION sfn_check_transaction_state()
RETURNS INT
AS
BEGIN
	DECLARE @state INT = 0;
    -- Проверка: автоподтверждение выключено
    IF XACT_STATE() <> 0
    BEGIN
        -- Если транзакция вложена, и режим автоподтверждения выключен
        IF @@TRANCOUNT > 1
        BEGIN
			SET @state = 1;
        END
    END;

	IF (@state = 1) 
	BEGIN
		RETURN CAST(N'автоподтверждение транзакций выключено и функция запущена из вложенной транзакции' AS INT);
	END;

	RETURN 0;
END;

GO

EXEC spr_check_transaction_state;

GO

EXEC sfn_check_transaction_state;

-- 8. Создать хранимую процедуру,
-- выполняющую подсчёт количества записей в указанной таблице таким образом, 
-- чтобы она возвращала максимально корректные данные, 
-- даже если для достижения этого результата придётся пожертвовать производительностью.
CREATE OR ALTER PROCEDURE spr_count_records
    @table_name NVARCHAR(128)
AS
BEGIN
    DECLARE @RecordCount BIGINT;

	-- узнать уровень изоляции 
	-- (после окончания, даже при изменении уровня транзакции, вернется в начальное состояние)
	/*DECLARE @isolation_level NVARCHAR(50);

	SET @isolation_level =
	(
	 SELECT [transaction_isolation_level]
	 FROM [sys].[dm_exec_sessions]
	 WHERE [session_id] = @@SPID
	); */

    BEGIN TRANSACTION

    BEGIN TRY
        
		-- избегаем фантомных записей
        SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

        DECLARE @Sql NVARCHAR(MAX) = N'SELECT @Result = COUNT(*) FROM ' + QUOTENAME(@table_name);

        EXEC sp_executesql @Sql, N'@Result BIGINT OUTPUT', @Result = @RecordCount OUTPUT;

        COMMIT TRANSACTION;

        -- Возвращаем результат
        SELECT @RecordCount AS RecordCount;
    END TRY
    BEGIN CATCH

        ROLLBACK TRANSACTION;

        DECLARE @ErrorMessage NVARCHAR(2048) = ERROR_MESSAGE();
        THROW 50000, @ErrorMessage, 1;
    END CATCH
END;

GO

EXEC spr_count_records 'books';