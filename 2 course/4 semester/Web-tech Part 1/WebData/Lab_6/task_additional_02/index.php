<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <link rel="stylesheet" href="./style.css">
    <script type="module" src="https://unpkg.com/ionicons@7.1.0/dist/ionicons/ionicons.esm.js"></script>
    <title>Форма авторизации</title>
</head>

<body>
    <?php
    if (isset($_COOKIE["user_email"])) {
        header("Location: ../task_additional_01/index.php");
        exit();
    }
    ?>
    <section>
        <form action="./login.php" method="post">
            <h1>Авторизация</h1>
            <div class="inputbox">
                <ion-icon name="mail-outline"></ion-icon>
                <input type="email" name="email" required>
                <label>Email</label>
            </div>
            <div class="inputbox">
                <ion-icon name="lock-closed-outline"></ion-icon>
                <input type="password" name="password" required>
                <label>Пароль</label>
            </div>
            <div class="forget">
                <label><input type="checkbox" name="remember_me">Запомнить меня</label>
            </div>
            <button>Войти</button>
        </form>
    </section>
</body>

</html>