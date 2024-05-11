<?php

require_once './ImageDownloader.php';
require_once './FileShower.php';

if ($_SERVER["REQUEST_METHOD"] == "POST" && isset($_POST["url"]) && isset($_POST["directory"])) {
    $url = $_POST["url"];
    $directory = $_POST["directory"];
    $imageDownloader = new ImageDownloader($directory);
    $numImagesDownloaded = $imageDownloader->downloadImages($url);

    echo "<link rel='stylesheet' href='style.css'>";
    echo "Загружено $numImagesDownloaded изображений<br/>";

    $directory .= DIRECTORY_SEPARATOR;

    if ($numImagesDownloaded > 0) {
        $fileManager = new FileShower($directory);
        $fileManager->showUploadedFiles();
    }
}
