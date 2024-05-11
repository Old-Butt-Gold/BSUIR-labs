<?php

class FileManager
{
    private $uploadDirectory;

    public function __construct($uploadDirectory)
    {
        $this->uploadDirectory = $uploadDirectory;
    }

    public function uploadFile($file): bool
    {
        $targetFile = $this->uploadDirectory . basename($file["name"]);
        return move_uploaded_file($file["tmp_name"], $targetFile);
    }

    public function getUploadedFiles()
    {
        $files = scandir($this->uploadDirectory);
        return array_diff($files, array('.', '..'));
    }

    public function deleteFile($filename): bool
    {
        $filePath = $this->uploadDirectory . $filename;
        if (file_exists($filePath)) {
            unlink($filePath);
            return true;
        } else {
            return false;
        }
    }
}