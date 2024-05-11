<?php

require "Classes/SmartDate.php";

$dateParam = $_GET['date'] ?? null;

if ($dateParam == null) {
    echo "Check your \"date\" parameter!";
} else {
    $smartDate = new SmartDate($dateParam);
    if ($smartDate->isCorrectDate()) {
        $res1 = $smartDate->isLeapYear() ? "Да" : "Нет";
        $res2 = $smartDate->isWeekend() ? "Да" : "Нет";
        echo "Високосный год: $res1</br>";
        echo "Является выходным днем: $res2</br>";
        echo "Разница в днях до даты: {$smartDate->distanceToToday()}</br>";
    } else {
        echo "Your date isn't correct. The correct style is: Y-m-d H:i:s";
    }
}