<?php

class DirectoryScanner{
    public static function Scan($dir) : array
    {
        $files = [];
        $items = scandir($dir);
        foreach ($items as $item) {
            if ($item !== '.' && $item !== '..') {
                $path = $dir[mb_strlen($dir) - 1] !== DIRECTORY_SEPARATOR &&
                        $dir[mb_strlen($dir) - 1] !== "/"
                    ? $dir . DIRECTORY_SEPARATOR . $item
                    : $dir . $item;
                $path = $dir . DIRECTORY_SEPARATOR . $item;
                if (is_dir($path)) {
                    $files += self::Scan($path);
                } else {
                    $files[] = $path;
                }
            }
        }
        return $files;
    }

    public static function FindDuplicates($files) : array {
        $hashes = [];
        $duplicates = [];
        foreach ($files as $file) {
            $hash = md5_file($file);
            if (isset($hashes[$hash])) {
                $duplicates[$hash][] = $file;
            } else {
                $hashes[$hash] = $file;
            }
        }
        return $duplicates;
    }

    public static function FindOrigins($files) : array {
        $hashes = [];
        $duplicates = [];
        foreach ($files as $file) {
            $hash = md5_file($file);
            if (isset($hashes[$hash])) {
                $duplicates[$hash][] = $file;
            } else {
                $hashes[$hash] = $file;
            }
        }
        return $hashes;
    }
}

if (!isset($_GET['dir'])) {
    echo "GET данные отправляются с помощью: \"dir=\"!<br/>Без него берется папка по умолчанию";
}

$directory = $_GET['dir'] ?? __DIR__;

$files = DirectoryScanner::Scan($directory);
$duplicates = DirectoryScanner::FindDuplicates($files);
$hashes = DirectoryScanner::FindOrigins($files);

echo "<h1>Результат: </h1>";
foreach ($duplicates as $hash => $duplicateFiles) {
    echo "<h3>Первоначальный файл: {$hashes[$hash]}</h3>";
    echo "Дубликаты с хешем $hash:<br>\n";
    foreach ($duplicateFiles as $file) {
        echo "- $file<br>";
    }
    echo "<hr>";
}

