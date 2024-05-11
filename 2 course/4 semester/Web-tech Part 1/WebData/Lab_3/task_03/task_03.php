<?php

if ($_SERVER['REQUEST_METHOD'] == "POST") {
    $input_arr_1 = $_POST["arr_1"] ?? "";
    $input_arr_2 = $_POST["arr_2"] ?? "";

    $arr_1 = splitStringIntoNumbers($input_arr_1);
    $arr_2 = splitStringIntoNumbers($input_arr_2);

    foreach ($arr_2 as $value) {
        if (!in_array($value, $arr_1)) {
            $arr_1[] = $value;
        }
    }

    echo "<h1>";
    foreach ($arr_1 as $value) {
        echo "<span>$value</span>\t";
    }
    echo "</h1>";
}

function splitStringIntoNumbers($arr) {
    $res_arr = explode(" ", $arr);

    foreach ($res_arr as $key => $word) {
        if ($word === "")
            unset($res_arr[$key]);
    }

    return $res_arr;
}

