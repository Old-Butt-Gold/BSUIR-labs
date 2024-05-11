<?php

class FileSystemObject {

    const BYTES = "B";
    const KILOBYTES = "KB";
    const MEGABYTES = "MB";
    const GIGABYTES = "GB";
    const TERABYTES = "TB";
    const PETABYTES = "PB";

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

    public function getInfo($sizeFormat = self::MEGABYTES)
    {
        return "<p>{$this->name} | {$this->getType()} | {$this->getSize($sizeFormat)} | $sizeFormat</p>\n";
    }

    private function getSize($format)
    {
        switch ($format) {
            case self::BYTES:
                return $this->size;
            case self::KILOBYTES:
                return $this->size >> 10;
            case self::MEGABYTES:
                return $this->size >> 20;
            case self::GIGABYTES:
                return $this->size >> 30;
            case self::TERABYTES:
                return $this->size >> 40;
            case self::PETABYTES:
                return $this->size >> 50;
            default:
                return 0;
        }
    }

    public function getType()
    {
        return $this->type;
    }
}

if (isset($_GET['path'])) {
    $path = $_GET['path'];
    if (file_exists($path)) {
        scanDirectories($path);
    }
} else {
    scanDirectories(__DIR__);
}

function scanDirectories($baseDir) {
    $files = scandir($baseDir);
    foreach ($files as $file) {
        if ($file !== '.' && $file !== '..') {
            $newPath = $baseDir[mb_strlen($baseDir) - 1] !== DIRECTORY_SEPARATOR
                        && $baseDir[mb_strlen($baseDir) - 1] !== "/"
                ? $baseDir . DIRECTORY_SEPARATOR . $file
                : $baseDir . $file;
            $file_obj = new FileSystemObject($newPath);
            echo $file_obj->getInfo(FileSystemObject::BYTES);
            echo "<hr/>";
            if ($file_obj->getType() === 'directory') {
                scanDirectories($newPath . DIRECTORY_SEPARATOR);
            }
        }
    }
}