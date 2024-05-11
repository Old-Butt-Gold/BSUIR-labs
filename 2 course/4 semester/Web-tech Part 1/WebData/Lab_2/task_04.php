<?php

if (isset($argc)) {
    if ($argc > 1 && $argv[1] == (int)$argv[1]) {
        $number = (int)$argv[1];
        $sum = 0;
        while ($number > 0) {
            $sum += $number % 10;
            $number = (int)($number / 10);
        }
        echo "<h1>Your sum is: $sum</h1>";
    }
    else
        echo "Check your parameters";
}
else
    exit("argc and argv disabled\n");
