<?php

error_reporting(0);

$sql = new mysqli("localhost", "old-butt-gold", "HyperPROROK2019");

if (!$sql->connect_error) {
    $sql->set_charset("UTF8");
    echo "Версия клиента: {$sql->get_client_info()}";
    $sql->close();
} else {
    echo "Your couldn't connect to MySQL";
}


