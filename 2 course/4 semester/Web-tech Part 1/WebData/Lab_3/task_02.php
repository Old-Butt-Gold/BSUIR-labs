<?php

    if (isset($_GET['words'])) {
        $words = $_GET['words'];
        if (empty($words)) {
            $wordsArray = explode(" ", $words); #split removed XD

            foreach ($wordsArray as $key => $word) {
                if ($word === "")
                    unset($wordsArray[$key]);
            }

            echo "<ul>Your words: ";
            $index = 0;
            foreach($wordsArray as $word) {
                $index++;
                if ($index == 3) {
                    $word = mb_strtoupper($word);
                    $index = 0;
                }

                $length = mb_strlen($word);
                $newWord = '';
                for ($i = 0; $i < $length; $i++) {
                    $newWord .= ($i + 1) % 3 == 0
                        ? "<span style='color: rebeccapurple;'>" . mb_substr($word, $i, 1) . "</span>"
                        : mb_substr($word, $i, 1);
                }
                echo "<li>$newWord</li>";
            }
            echo "</ul>";
        }
        else
            echo "<h1>You didn't write any words</h1>";
    }
    else
        echo "<h1>Your \$GET-request parameters must contain parameter named \"words\"</h1>";