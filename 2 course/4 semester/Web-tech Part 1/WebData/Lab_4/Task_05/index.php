<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Encryption Form</title>
</head>
<body>
    <h2>Encryption Form</h2>

    <form method="post" target="_self">
        <label for="text">Text:</label><br>
        <textarea id="text" name="text"></textarea><br>
        <label for="key">Encryption key:</label><br>
        <input type="text" id="key" name="key"><br>
        <label for="cipher">Select encryption type:</label><br>
        <select id="cipher" name="cipher">
            <option value="AES">AES</option>
            <option value="DES">DES</option>
        </select><br><br>
        <hr/>
        <input name="Encrypt" type="submit" value="Encrypt">
        <input name="Decrypt" type="submit" value="Decrypt">
    </form>

    <?php include_once "./task_05.php"; ?>
</body>
</html>
