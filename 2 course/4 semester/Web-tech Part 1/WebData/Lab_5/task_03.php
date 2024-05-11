<?php

class UniqueArrayCreator {


    private static function getOneDimensionNumbers(&$arrUnique, $multi_dimensional_array) : void {
        if (is_array($multi_dimensional_array)) {
            foreach ($multi_dimensional_array as $subArray) {
                self::getOneDimensionNumbers($arrUnique, $subArray);
            }
        } else {
            if (!in_array($multi_dimensional_array, $arrUnique)) {
                $arrUnique[] = $multi_dimensional_array;
            }
        }
    }

    public static function MakeUnique($multi_dimensional_array) : array {
        $arrUnique = [];
        self::getOneDimensionNumbers($arrUnique, $multi_dimensional_array);
        return $arrUnique;
    }
}

$multi_dimensional_array = [
    [1, 2, 3, 10, 25],
    [4, 5, 6],
    [4, 8, 5],
    [8, 7, 6],
    [1, 2, 9],
];

$arrUnique = UniqueArrayCreator::MakeUnique($multi_dimensional_array);

foreach ($arrUnique as $key => $item) {
    echo "<h2>index:$key – $item</h2>";
    echo "<hr/>";
}

