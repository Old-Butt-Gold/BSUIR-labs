<?php

$multi_dimensional_array = [
    [1, 2, 3],
    [4, 5, 6],
    [4, 8, 5],
    [8, 7, 6],
    [1, 2, 9],
];

$result_array = makeArrUnique($multi_dimensional_array);

print_r($multi_dimensional_array);

echo "<br/>";
echo "<br/>";
echo "<br/>";

print_r($result_array);

function makeArrUnique($multi_dimensional_array) {
    $arrUnique = [];
    getOneDimensionNumbers($arrUnique, $multi_dimensional_array);

    removeDuplicateNumbers($arrUnique, $multi_dimensional_array);
    return $multi_dimensional_array;
}

function removeDuplicateNumbers(&$arrUnique, &$multi_dimensional_array) : void {
    if (is_array($multi_dimensional_array)) {
        foreach ($multi_dimensional_array as $key => &$value) {
            removeDuplicateNumbers($arrUnique, $value);

            if (!is_array($value)) {
                if (in_array($value, $arrUnique)) {
                    $subkey = array_search($value, $arrUnique);
                    unset($arrUnique[$subkey]);
                } else
                    unset($multi_dimensional_array[$key]);
            } else {
                if (count($value) == 0)
                    unset($multi_dimensional_array[$key]);
            }


        }
    }
}

function getOneDimensionNumbers(&$arrUnique, $multi_dimensional_array) : void {
    if (is_array($multi_dimensional_array)) {
        foreach ($multi_dimensional_array as $subArray) {
            getOneDimensionNumbers($arrUnique, $subArray);
        }
    } else {
        if (!in_array($multi_dimensional_array, $arrUnique)) {
            $arrUnique[] = $multi_dimensional_array;
        }
    }
}