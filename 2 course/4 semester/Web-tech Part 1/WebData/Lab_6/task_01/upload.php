<?php

require_once 'FileManager.php';

$uploadDirectory = "uploads" . DIRECTORY_SEPARATOR;
$fileManager = new FileManager($uploadDirectory);

if ($_SERVER["REQUEST_METHOD"] == "POST" && isset($_FILES["fileToUpload"])) {
    $uploaded = $fileManager->uploadFile($_FILES["fileToUpload"]);
    if ($uploaded) {
        header("Location: index.php");
        exit();
    } else {
        echo "Error uploading file.";
    }
}


