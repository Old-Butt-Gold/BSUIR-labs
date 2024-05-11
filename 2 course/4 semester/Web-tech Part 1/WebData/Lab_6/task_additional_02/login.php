<?php

require_once './Authenticator.php';

$servername = "localhost";
$username = "old-butt-gold";
$password = "HyperPROROK2019";
$dbname = "php_webtech";

$conn = new mysqli($servername, $username, $password, $dbname);

if ($conn->connect_error) {
    echo "Connection failed: " . $conn->connect_error;
    exit();
}

$authenticator = new Authenticator($conn);

if ($_SERVER["REQUEST_METHOD"] == "POST" && isset($_POST["email"]) && isset($_POST["password"])) {
    $email_user = $_POST["email"];
    $password_user = $_POST["password"];
    if ($authenticator->authenticate($email_user, $password_user)) {
        if (isset($_POST['remember_me']) && $_POST['remember_me'] == "on") {
            setcookie("user_email", $email_user, time() + 86400);
        }
        header("Location: ../task_additional_01/index.php");
        exit();
    } else {
        echo "Неправильный email или пароль";
    }
}

$conn->close();