-- №1. 1.	Добавить в базу данных информацию о троих новых читателях: 
-- «Орлов О.О.», «Со-колов С.С.», «Беркутов Б.Б.».
INSERT INTO subscribers(s_name) VALUES
(N'Орлов О. О.'),
(N'Соколов С.С.'),
(N'Беркутов Б.Б.');

-- №4.Отметить все выдачи с идентификаторами ≤ 50 как возвращённые.
UPDATE subscriptions
SET sb_is_active = 'N'
WHERE sb_id <= 50;

-- №8.	Удалить все книги, относящиеся к жанру «Классика».
DELETE
FROM books
WHERE b_id IN (
	SELECT b_id 
	FROM genres
		INNER JOIN m2m_books_genres
		ON m2m_books_genres.g_id = genres.g_id
	WHERE g_name = N'Классика'
)

-- №9. Удалить информацию обо всех выдачах книг, 
-- произведённых после 20-го числа любого месяца любого года.
DELETE FROM subscriptions
WHERE DAY(sb_start) > 20;

-- №13. Обновить все имена авторов, добавив в конец имени « [+]», 
-- если в библиотеке есть более трёх книг этого автора, 
-- или добавив в конец имени « [-]» в противном случае.	
UPDATE authors
SET a_name = IIF(
	(SELECT COUNT(*) FROM m2m_books_authors 
                     WHERE m2m_books_authors.a_id = authors.a_id) > 3,
	a_name + ' [+]',
	a_name + ' [-]');
