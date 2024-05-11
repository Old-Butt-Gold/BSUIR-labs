<?php

require_once './WeatherAggregator.php';

if ($_SERVER["REQUEST_METHOD"] == "POST" && isset($_POST["city"])) {
    $city = $_POST["city"];
    $weather = new WeatherForecastAggregator($city);

    $celcium = $weather->getWeather() . " °C";

    $output = "<link rel='stylesheet' href='style.css'>";
    $output .= "<h1>Прогноз погоды на завтра в городе $city: $celcium!</h1>";
    echo $output;
}
