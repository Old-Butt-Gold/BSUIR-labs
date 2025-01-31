-- 1. Создать представление, позволяющее получать список читателей с количеством находящихся 
-- у каждого читателя на руках книг, но отображающее только таких читателей, 
-- по которым имеются задолженности, т.е. на руках у читателя есть хотя бы одна книга, 
-- которую он должен был вернуть до наступления текущей даты.
CREATE OR ALTER VIEW vw_subscribers_with_books_overdue
AS
	SELECT s_id, s_name, COUNT(sb_book) AS overdue_count
	FROM subscribers
		INNER JOIN subscriptions
		ON subscribers.s_id = subscriptions.sb_id
	WHERE sb_is_active = 'Y' AND subscriptions.sb_finish < GETDATE()
	GROUP BY s_id, s_name

GO

SELECT * FROM vw_subscribers_with_books_overdue;

-- 2. Создать кэширующее представление, позволяющее получать список всех книг и их жанров
-- (две колонки: первая – название книги, вторая – жанры книги, перечислен-ные через запятую).
DROP TABLE IF EXISTS book_genre_cache;

GO

-- Создание таблицы для кэша
CREATE TABLE book_genre_cache
(
    bgc_book NVARCHAR(255),
    bgc_genres NVARCHAR(MAX)
);

GO

TRUNCATE TABLE book_genre_cache

GO

INSERT INTO book_genre_cache

SELECT 
	books.b_name AS [Book Name],
	STRING_AGG(genres.g_name, ', ') AS Genres
FROM 
	books
	LEFT JOIN m2m_books_genres AS bg 
	ON books.b_id = bg.b_id
	LEFT JOIN genres 
	ON bg.g_id = genres.g_id
GROUP BY 
	books.b_name


GO 

-- Создание триггера для обновления кэша
CREATE OR ALTER TRIGGER trg_update_book_cache_books
ON books
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
	TRUNCATE TABLE book_genre_cache;

	INSERT INTO book_genre_cache

	SELECT 
		books.b_name AS [Book Name],
		STRING_AGG(genres.g_name, ', ') AS Genres
	FROM 
		books
		LEFT JOIN m2m_books_genres AS bg 
		ON books.b_id = bg.b_id
		LEFT JOIN genres 
		ON bg.g_id = genres.g_id
	GROUP BY 
		books.b_name

END;

GO

-- Создание триггера для обновления кэша
CREATE OR ALTER TRIGGER trg_update_book_cache_genres
ON genres
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
	TRUNCATE TABLE book_genre_cache;

	INSERT INTO book_genre_cache

	SELECT 
		books.b_name AS [Book Name],
		STRING_AGG(genres.g_name, ', ') AS Genres
	FROM 
		books
		LEFT JOIN m2m_books_genres AS bg 
		ON books.b_id = bg.b_id
		LEFT JOIN genres 
		ON bg.g_id = genres.g_id
	GROUP BY 
		books.b_name

END;

GO

CREATE OR ALTER VIEW vw_book_genre 
AS
SELECT * FROM book_genre_cache

GO

SELECT * FROM vw_book_genre;

-- 3. Создать кэширующее представление, позволяющее получать список всех авторов
-- и их книг (две колонки: первая – имя автора, вторая – написанные автором книги, перечисленные через запятую).
DROP TABLE IF EXISTS author_book_cache;

GO

-- Создание таблицы для кэша
CREATE TABLE author_book_cache
(
    abc_author NVARCHAR(255),
    abc_books NVARCHAR(MAX)
);

GO

TRUNCATE TABLE author_book_cache

GO

INSERT INTO author_book_cache

SELECT 
	authors.a_name AS [Author Name],
	STRING_AGG(books.b_name, ', ') AS Books
FROM 
	authors
	LEFT JOIN m2m_books_authors AS ba 
	ON authors.a_id = ba.a_id
	LEFT JOIN books
	ON ba.b_id = books.b_id
GROUP BY 
	authors.a_name

GO 

-- Создание триггера для обновления кэша
CREATE OR ALTER TRIGGER trg_update_author_cache_books
ON books
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
	TRUNCATE TABLE author_book_cache;

	INSERT INTO author_book_cache

	SELECT 
		authors.a_name AS [Author Name],
		STRING_AGG(books.b_name, ', ') AS Books
	FROM 
		authors
		LEFT JOIN m2m_books_authors AS ba 
		ON authors.a_id = ba.a_id
		LEFT JOIN books
		ON ba.b_id = books.b_id
	GROUP BY 
		authors.a_name

END;

GO

-- Создание триггера для обновления кэша
CREATE OR ALTER TRIGGER trg_update_author_cache_authors
ON authors
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
	TRUNCATE TABLE author_book_cache;

	INSERT INTO author_book_cache

	SELECT 
		authors.a_name AS [Author Name],
		STRING_AGG(books.b_name, ', ') AS Books
	FROM 
		authors
		LEFT JOIN m2m_books_authors AS ba 
		ON authors.a_id = ba.a_id
		LEFT JOIN books
		ON ba.b_id = books.b_id
	GROUP BY 
		authors.a_name

END;

GO

CREATE OR ALTER VIEW vw_author_book 
AS
	SELECT * FROM author_book_cache;

GO

SELECT * FROM vw_author_book;

-- 4. Создать представление, через которое невозможно получить информацию о том, 
-- какая конкретно книга была выдана читателю в любой из выдач.
CREATE OR ALTER VIEW vw_subscriptions_cache
AS
	SELECT sb_id, sb_subscriber, sb_start, sb_finish, sb_is_active
	FROM subscriptions

GO

SELECT * FROM vw_subscriptions_cache;

-- 13.	Создать триггер, не позволяющий добавить в базу данных информацию о выдаче книги,
--  если выполняется хотя бы одно из условий:
-- • дата выдачи или возврата приходится на воскресенье;
-- • читатель брал за последние полгода более 100 книг;
-- • промежуток времени между датами выдачи и возврата менее трёх дней.
CREATE OR ALTER TRIGGER trg_validate_subscription
ON subscriptions
INSTEAD OF INSERT
AS
BEGIN
	DECLARE @count INT;

	SET @count = (SELECT COUNT(*)
                FROM subscriptions s
                WHERE 
                    s.sb_subscriber = sb_subscriber
                    AND s.sb_start >= DATEADD(MONTH, -6, GETDATE())
				);

    IF NOT EXISTS (
        SELECT 1
        FROM inserted
        WHERE 
            -- Условие 1: Дата выдачи или возврата приходится на воскресенье
            (DATEPART(WEEKDAY, sb_start) = 1 OR DATEPART(WEEKDAY, sb_finish) = 1)
            OR
            -- Условие 2: Читатель взял за последние полгода более 100 книг
            @count > 100
            OR
            -- Условие 3: Промежуток между выдачей и возвратом менее 3 дней
            DATEDIFF(DAY, sb_start, sb_finish) < 3
    )
    BEGIN
        INSERT INTO subscriptions (sb_subscriber, sb_book, sb_start, sb_finish, sb_is_active)
        SELECT sb_subscriber, sb_book, sb_start, sb_finish, sb_is_active
        FROM inserted;
    END
END;
