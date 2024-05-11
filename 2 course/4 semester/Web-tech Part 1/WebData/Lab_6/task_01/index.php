<?php

require_once '.\FileManager.php';

$uploadDirectory = "uploads" . DIRECTORY_SEPARATOR;
$fileManager = new FileManager($uploadDirectory);
$uploadedFiles = $fileManager->getUploadedFiles();
?>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Файловый менеджер</title>
    <link rel="stylesheet" href="./style.css">
</head>
<body>
    <div class="container">
        <h1>Файловый менеджер</h1>

        <form action="./upload.php" method="post" enctype="multipart/form-data">
            <h2>Загрузить файл</h2>
            <input type="file" name="fileToUpload">
            <input type="submit" value="Загрузить" name="submit">
        </form>

        <h2>Список загруженных файлов</h2>
        <ul>
            <?php
            foreach ($uploadedFiles as $file) {
                echo
                "<li>
                    <a href='uploads/$file' download>$file</a>
                    <a href='./delete.php?filename=$file'>Удалить</a>
                </li>";
            }

            ?>
        </ul>
    </div>
</body>

</html>