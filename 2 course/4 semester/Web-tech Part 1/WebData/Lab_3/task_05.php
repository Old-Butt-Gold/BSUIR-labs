<?php

$multi_dimensional_array = [
    "integer" => 5,
    "float" => 1.554999999999999,
    "string" => "hello",
    "inner_array_1" => [
        "integer" => 10,
        "float" => 2.71576,
        "string" => "hell"
    ],
    "inner_array_2" => [
        "integer" => 7,
        "float" => 1.41987,
        "string" => "to"
    ],
    "inner_array_3" => [
    "integer" => 0,
    "float" => 3.14123,
    "string" => "world"
    ]
];

function processArrayRecursive(&$array) {
    foreach ($array as &$value) {
        if (is_int($value)) {
            $value *= 2;
        } elseif (is_float($value)) {
            $value = round($value, 2);
        } elseif (is_string($value)) {
            $value = mb_strtoupper($value);
        } elseif (is_array($value)) {
            processArrayRecursive($value);
        }
    }
}

function displayArrayRecursive($array) {
    foreach ($array as $key => $value) {
        if (is_array($value)) {
            echo "<h2>$key:</h2></br>";
            displayArrayRecursive($value);
        }
        else
            echo "$key: $value</br>";
    }
}

processArrayRecursive($multi_dimensional_array);

displayArrayRecursive($multi_dimensional_array);