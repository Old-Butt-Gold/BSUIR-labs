<?php

require_once 'FileManager.php';

$uploadDirectory = "uploads" . DIRECTORY_SEPARATOR;
$fileManager = new FileManager($uploadDirectory);

if ($_SERVER["REQUEST_METHOD"] == "GET" && isset($_GET["filename"])) {
    $filename = $_GET["filename"];
    $deleted = $fileManager->deleteFile($filename);
    if ($deleted) {
        header("Location: index.php");
        exit();
    } else {
        echo "Error deleting file.";
    }
}