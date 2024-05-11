<?php

if (isset($_GET['level'])) {
    if ($_GET['level'] == (int)$_GET['level']) {
        $level = (int)$_GET['level'];
        $arr = generateMultiArray($level);
        displayArray($arr);
    }
    else
        echo "<h1>Не удалось сгенерировать многомерный массив</h1>";
}
else
    echo "<h1>Не удалось сгенерировать многомерный массив</h1>";

function displayArray($array, $level = 1) {
    echo "<ul>";
    foreach ($array as $value) {
        echo "<li style='color:" . getColor($level) . "'>level: $level: ";
        if (is_array($value)) {
            displayArray($value, $level + 1);
        } else {
            echo "value: $value";
        }
        echo "</li>";
    }
    echo "</ul>";
}

function generateMultiArray($level) {
    if ($level < 0) {
        return null;
    }

    $array = [];
    if ($level > 1) {
        $array["level-$level"] = generateMultiArray($level - 1);
    } else {
        $array["value-$level"] = "$level";
    }
    return $array;
}

function getColor($level) {
    switch ($level) {
        case 1:
            return "red";
        case 2:
            return "blue";
        case 3:
            return "green";
        case 4:
            return "purple";
        default:
            return "yellow";
    }
}

