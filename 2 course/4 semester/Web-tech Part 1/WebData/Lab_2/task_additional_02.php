<?php

$count = count($_GET);
if ($count === 1) {
    foreach ($_GET as $value) {
        if (is_numeric($value)) {
            $rowCount = (int)$value;
            echo "<table>";
            for ($i = 0; $i < $rowCount; $i++) {
                $colorBgValue = 255 - $i * (255 / ($rowCount - 1));
                $colorTextValue = $i * (255 / ($rowCount - 1));
                $bgColor = "rgb($colorBgValue, $colorBgValue, $colorBgValue)";
                $textColor = "rgb($colorTextValue, $colorTextValue, $colorTextValue)";
                echo "<tr style='background-color: $bgColor'>
                          <td style='color: $textColor'>Row $i</td>
                      </tr>";
            }
            echo "</table>";
        } else {
            echo "<h1>Неправильно введенный параметр GET-запроса</h1>";
        }
    }

}
else
    echo "<h1>Неправильный GET-запрос</h1>";

