<?php

if (isset($argc)) {
    if ($argc > 1) {
        $words = [];
        for ($i = 1; $i < $argc; $i++)
            $words[] = $argv[$i];
        $length = 0;
        for ($i = 0; $i < count($words); $i++) {
            if ($length < mb_strlen($words[$i])) {
                $length = mb_strlen($words[$i]);
            }
        }
        foreach ($words as $word) {
            if (mb_strlen($word) == $length) {
                echo "<h1>Your longest word is $word!</h1>\n";
            }
        }
    }
    else
        echo "Check your parameters";
}
else
    exit("argc and argv disabled\n");
