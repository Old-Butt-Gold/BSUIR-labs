<?php

require_once './Authenticator.php';

$usersFile = "users.txt";
$authenticator = new Authenticator($usersFile);

if ($_SERVER["REQUEST_METHOD"] == "POST" && isset($_POST["email"]) && isset($_POST["password"])) {
    $username = $_POST["email"];
    $password = $_POST["password"];
    if ($authenticator->authenticate($username, $password)) {
        header("Location: ../task_01/index.php");
        exit();
    } else {
        echo "Неправильный email или пароль";
    }
}