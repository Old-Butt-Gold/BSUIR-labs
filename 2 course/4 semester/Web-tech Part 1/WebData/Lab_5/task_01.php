<?php

class AgeCalculator {
    private $year;
    private $month;
    private $day;
    public function __construct($year, $month, $day) {

        $this->year = $year;
        $this->month = $month;
        $this->day = $day;
    }

    public function calculateAge() {
        $currentDate = new DateTime();

        $birthDate = new DateTime("$this->year-$this->month-$this->day");

        $diff = $currentDate->diff($birthDate);

        return "<h1>Возраст: $diff->y лет, $diff->m месяцев, $diff->d дней</h1>";
    }
}

if (isset($_GET['year']) && isset($_GET['month']) && isset($_GET['day'])) {
    $year = $_GET['year'];
    $month = $_GET['month'];
    $day = $_GET['day'];
    if (checkdate($month, $day, $year)) {
        $calculator = new AgeCalculator($year, $month, $day);
        echo $calculator->calculateAge();
    } else {
        echo "<h1>Проверьте правильность введенных данных!</h1>";
    }
} else {
    echo "<h1>Проверьте наличие параметров: year, month, day</h1>";
}