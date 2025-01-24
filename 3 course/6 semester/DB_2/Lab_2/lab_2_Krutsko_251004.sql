-- №2. Показать всю информацию о жанрах
SELECT * FROM genres
ORDER BY g_id;

-- №6. Показать, сколько всего раз читателям выдавались книги
SELECT COUNT(sb_book) AS bookCount
FROM subscriptions;

-- №10. Показать книги, количество экземпляров которых меньше среднего в библиотеке
SELECT *
FROM books
WHERE b_quantity < (
				SELECT AVG(b_quantity)
				FROM books
                );

-- №14. Показать идентификатор "читателя-рекордсмена", 
-- взявшего в библиотеке больше книг, чем любой другой читатель
SELECT TOP 1 sb_subscriber, COUNT(*) AS BookCount
FROM subscriptions
GROUP BY sb_subscriber
ORDER BY 2 DESC;

-- №17. Показать, сколько книг было возвращено и не возвращено в библиотеку (СУБД должна оперировать исходными значениями поля sb_is_active (т.е. «Y» и «N»), 
-- а после подсчёта значения «Y» и «N» должны быть преобразованы в «Returned» и «Not returned»).
SELECT IIF(sb_is_active = 'Y', 'Not returned', 'Returned') AS Status, 
	   COUNT(*) AS BookCount
FROM subscriptions
GROUP BY sb_is_active;