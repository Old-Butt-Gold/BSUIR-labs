<?php

require "../Classes/CryptoManager.php";

error_reporting(0);

$cipher = $_POST['cipher'] === "AES" ? CryptoManager::AES : CryptoManager::DES;

$method = new CryptoManager($cipher, $_POST['key'] ?? "");

if (isset($_POST['Encrypt']))
    echo "<p>CIPHER: {$method->encrypt($_POST["text"])}</p>";

if (isset($_POST['Decrypt']))
    echo "<p>MESSAGE: {$method->decrypt($_POST["text"])}</p>";


