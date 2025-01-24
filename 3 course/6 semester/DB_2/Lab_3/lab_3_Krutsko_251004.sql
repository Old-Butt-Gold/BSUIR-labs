-- №1. Показать список книг, у которых более одного автора.
SELECT b_name
FROM books
	INNER JOIN m2m_books_authors 
	ON books.b_id = m2m_books_authors.b_id
GROUP BY b_name
HAVING COUNT(*) > 1;

-- №6. Показать список книг, которые никто из читателей никогда не брал.
SELECT b_name
	FROM books
	LEFT JOIN subscriptions
	ON subscriptions.sb_book = books.b_id
WHERE sb_subscriber IS NULL;

-- №11. Показать книги, относящиеся к более чем одному жанру.
SELECT b_name
FROM books
	INNER JOIN m2m_books_genres
	ON books.b_id = m2m_books_genres.b_id
GROUP BY b_name
HAVING COUNT(*) > 1;

-- №16. Показать всех читателей, не вернувших книги, 
-- и количество невозвращённых книг по каждому такому читателю.
SELECT s_id, s_name, COUNT(*) AS ReservationCount
FROM subscribers
	INNER JOIN subscriptions
	ON subscribers.s_id = subscriptions.sb_subscriber
WHERE sb_is_active = 'Y'
GROUP BY s_id, s_name;

-- 17. Показать читаемость жанров, т.е. все жанры и то количество раз, 
-- которое книги этих жанров были взяты читателями.
SELECT g_name, COUNT(*) AS SubscriptionTakenCount
FROM genres
	INNER JOIN m2m_books_genres
	ON genres.g_id = m2m_books_genres.g_id
	INNER JOIN books
	ON m2m_books_genres.b_id = books.b_id
	INNER JOIN subscriptions
	ON books.b_id = subscriptions.sb_book
GROUP BY g_name;