<?php

class FileSystemObject {

    private $name;
    private $size;
    private $type;

    public function __construct($filepath)
    {
        $this->name = $filepath;
        $this->size = filesize($filepath);

        if (is_file($filepath)) {
            $this->type = 'file';
        } elseif (is_dir($filepath)) {
            $this->type = 'directory';
        } elseif (is_link($filepath)) {
            $this->type = 'link';
        } else {
            $this->type = 'unknown';
        }
    }

    public function getSize()
    {
        return $this->size;
    }

    public function getType()
    {
        return $this->type;
    }
}

$size = getTotalDirSize("D:/WebData");

if (isset($_GET['path'])) {
    $path = $_GET['path'];
    if (file_exists($path)) {

        $size = getTotalDirSize($path);
        echo "<h1>Общий размер: $size Байт</h1>";
    }
} else {
    $size = getTotalDirSize(__DIR__);
    echo "<h1>Общий размер: $size Байт</h1>";
}

function getTotalDirSize($baseDir) {
    $sum = 0;
    $files = scandir($baseDir);
    foreach ($files as $file) {
        if ($file !== '.' && $file !== '..') {
            $newPath = $baseDir[mb_strlen($baseDir) - 1] !== DIRECTORY_SEPARATOR &&
                        $baseDir[mb_strlen($baseDir) - 1] !== "/"
                ? $baseDir . DIRECTORY_SEPARATOR . $file
                : $baseDir . $file;
            $file_obj = new FileSystemObject($newPath);
            $sum += $file_obj->getType() === 'directory'
                ? getTotalDirSize($newPath . DIRECTORY_SEPARATOR)
                : $file_obj->getSize();
        }
    }
    return $sum;
}
