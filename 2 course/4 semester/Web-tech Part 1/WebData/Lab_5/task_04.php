<?php

class StringLetterRecogniser {
    public static function Recognise($text) : array {
        $letterCounts = [];
        $letterCount = mb_strlen($text);
        for ($i = 0; $i < $letterCount; $i++) {
            $symbol = mb_substr($text, $i, 1); #Проблема кодировки
            if (!isset($letterCounts[$symbol])) {
                $letterCounts[$symbol] = 1;
            } else {
                $letterCounts[$symbol]++;
            }
        }
        return $letterCounts;
    }
}

if (isset($_GET['text'])) {
    $text = $_GET['text'];
    $arrText = StringLetterRecogniser::Recognise($text);
    echo "<h1>В тексте \"$text\":</h1>";
    foreach ($arrText as $letter => $count) {
        echo "<h3>Буква $letter встречается $count раз(а).</h3>";
        echo "<hr/>";
    }
} else {
    echo "<h2>Ваш параметр GET должен содержать \"text=\"!</h2>";
}