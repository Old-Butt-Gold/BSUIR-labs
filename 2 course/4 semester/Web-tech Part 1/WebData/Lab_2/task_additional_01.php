<?php

foreach ($_GET as $key => $value) {
    $type = "string";
    if (is_numeric($value)) {
        $type = str_contains($value, '.') ? "double" : "integer";
    }
    echo "<h1>$key : $value — $type</h1>\n";
}