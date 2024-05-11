<?php

error_reporting(0);

require './simple_html_dom.php';

class ImageDownloader {
    private $targetDirectory;
    private $context;

    public function __construct($targetDirectory) {
        $this->targetDirectory = $targetDirectory;

        $opts = [
            "ssl" => [
                "verify_peer" => false,
                "verify_peer_name" => false,
            ],
            "http" => [
                "user_agent" => "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/121.0.0.0 Safari/537.36 OPR/107.0.0.0",
            ],
        ];
        $this->context = stream_context_create($opts);
    }

    private function downloadImages($url, $dir): int
    {
        $html = file_get_html($url, false, $this->context);
        if (!$html) {
            echo "Ошибка при загрузке страницы: $url<br>";
            return 0;
        }

        $imageUrls = [];

        foreach ($html->find('img') as $element) {
            $imageUrl = $element->src;
            $imageUrls[] = $imageUrl;
        }

        if (!is_dir($dir)) {
            mkdir($dir, 0777, true);
        }

        $count = 0;
        foreach ($imageUrls as $imageUrl) {
            $imageName = basename($imageUrl);
            $savePath = $dir . DIRECTORY_SEPARATOR . $imageName;
            if (!file_exists($savePath)) {
                $imageContent = file_get_contents($imageUrl);
                #if (!empty($imageContent)) { Все изображения получаются битыми
                file_put_contents($savePath, $imageContent);
                $count++;
                #}
            }
        }
        return $count;
    }

    public function downloadImagesCatalog($url) : int {
        $html = file_get_html($url, false, $this->context);
        if (!$html) {
            echo "Ошибка при загрузке страницы: $url<br>";
            return 0;
        }

        $count = 0;

        $parse = parse_url($url);
        $host = $parse['host'];
        $scheme = $parse['scheme'];

        if (!is_dir($this->targetDirectory)) {
            mkdir($this->targetDirectory, 0777, true);
        }

        $urls = [];
        $temp = $html->find('a');
        foreach ($temp as $item) {
            if (mb_strpos($item->href, "wallpapers"))
                $urls[] = $item->href;
        }

        foreach ($urls as $url) {
            $currentPage = $scheme . "://" . $host . $url;
            $dir = $this->targetDirectory . DIRECTORY_SEPARATOR . $url;
            $count += $this->downloadImages($currentPage, $dir);
        }

        return $count;
    }

}

$temp = new ImageDownloader("images");
echo "Загружено {$temp->downloadImagesCatalog("https://wallpapercave.com/categories/anime")} изображений<br/>";