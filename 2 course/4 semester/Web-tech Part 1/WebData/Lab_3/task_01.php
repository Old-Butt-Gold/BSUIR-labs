<?php

    if (isset($_GET['cities'])) {
        $words = $_GET['cities'];
        if (empty($words)) {
            $wordsArray = explode(" ", $words); #split removed XD

            foreach ($wordsArray as $key => $word) {
                if ($word === "")
                    unset($wordsArray[$key]);
            }

            $unique_cities = array_unique($wordsArray);
            natcasesort($unique_cities);
            echo "<ul>Sorted list of cities";
            foreach ($unique_cities as $city) {
                echo "<li>$city</li>";
            }
            echo "</ul>";
        } else
            echo "<h1>You didn't write any cities</h1>";
    }
    else
        echo "<h1>Your \$GET-request parameters must contain parameter named \"cities\"</h1>";