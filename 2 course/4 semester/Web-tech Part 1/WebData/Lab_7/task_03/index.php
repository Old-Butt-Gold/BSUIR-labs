<!DOCTYPE html>
<html lang="en">
<head>
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Прогноз погоды</title>
    <link rel="stylesheet" href="./style.css">
</head>
<body>
<div class="container">
    <h1>Прогноз погоды</h1>
    <form action="./forecast.php" method="post">
        <label for="url">Введите город для прогноза погоды на завтра!<br/>(Желательно на английской раскладке)</label>
        <input type="text" name="city" required>
        <button type="submit">Рассчитать</button>
    </form>
</div>
</body>
</html>
