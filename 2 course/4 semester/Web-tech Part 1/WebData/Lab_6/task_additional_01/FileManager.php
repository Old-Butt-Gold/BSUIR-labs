<?php

class FileManager
{
    private $uploadDirectory;

    public function __construct($uploadDirectory)
    {
        $this->uploadDirectory = $uploadDirectory;
    }

    public function uploadFiles($files): bool
    {
        $result = true;
        foreach ($files["name"] as $key => $name) {
            $targetFile = $this->uploadDirectory . basename($name);
            if (!move_uploaded_file($files["tmp_name"][$key], $targetFile)) {
                $result = false;
            }
        }
        return $result;
    }

    public function getUploadedFiles() : array
    {
        $files = scandir($this->uploadDirectory);
        return array_diff($files, array('.', '..'));
    }

    public function showUploadedFiles() : void {
        $uploadedFiles = self::getUploadedFiles();
        foreach ($uploadedFiles as $file) {
            $preview = '';

            $temp = $this->uploadDirectory . $file;

            if ($this::isImage($file)) {
                $preview = "<img src='$temp' width='100' height='100'><br/>";
            } elseif ($this::isTextFile($file)) {
                $preview = $this::getTextPreview($file) . "<br/>";
            }
            echo
            "<li>
                $preview
                <hr/>
                <a href='$temp' download>$file</a>
                <a href='./delete.php?filename=$file'>Удалить</a>
             </li>";
        }
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

    private function isImage($filename): bool
    {
        $imageExtensions = ['jpg', 'jpeg', 'png', 'gif'];
        $ext = strtolower(pathinfo($filename, PATHINFO_EXTENSION));
        return in_array($ext, $imageExtensions);
    }

    private function isTextFile($filename): bool
    {
        $textExtensions = ['txt', 'doc', 'docx', 'pdf'];
        $ext = strtolower(pathinfo($filename, PATHINFO_EXTENSION));
        return in_array($ext, $textExtensions);
    }

    public function getTextPreview($filename): string
    {
        $filePath = $this->uploadDirectory . $filename;

        $content = file_get_contents($filePath);

        $lines = explode("\n", $content);

        $count = count($lines);
        return implode("<br>", array_slice($lines, 0, min($count, 3)));
    }

}