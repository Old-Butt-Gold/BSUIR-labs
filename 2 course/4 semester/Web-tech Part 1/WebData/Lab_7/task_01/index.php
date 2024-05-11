<!DOCTYPE html>
<html lang="en">
<head>
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Загрузчик фотографий</title>
    <link rel="stylesheet" href="./style.css">
</head>
<body>
<div class="container">
    <h1>Загрузчик фотографий</h1>
    <form action="./download.php" method="post">
        <label for="url">Введите URL:</label>
        <input type="text" name="url" required>
        <label for="directory">Укажите каталог для сохранения:</label>
        <input type="text" name="directory" value="images" required>
        <button type="submit">Скачать изображения</button>
    </form>
</div>
</body>
</html>
