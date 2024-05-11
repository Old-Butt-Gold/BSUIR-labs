<?php

    if (isset($_GET['words'])) {
        $words = $_GET['words'];
        if (empty($words)) {
            $wordsArray = explode(" ", $words); #split removed XD

            foreach ($wordsArray as $key => $word) {
                if ($word === "")
                    unset($wordsArray[$key]);
            }

            echo "<ul>Your reversed words: ";
            $len = count($wordsArray);
            for ($i = 0; $i < $len / 2; $i++) {
                $temp = $wordsArray[$i];
                $wordsArray[$i] = $wordsArray[$len - 1 - $i];
                $wordsArray[$len - 1 - $i] = $temp;
            }
            foreach ($wordsArray as $word)
                echo "<li>$word</li>";
            echo "</ul>";
        }
        else
            echo "<h1>You didn't write any words</h1>";
    }
    else
        echo "<h1>Your \$GET-request parameters must contain parameter named \"words\"</h1>";