<?php

error_reporting(0);

class WeatherForecastAggregator {
    private $api = [];
    private $city;
    public function __construct($city) {
        $this->city = $city;

        #current or forecast
        $apiKey = '629293611b8b8eefbc7f18cba73571b3';
        $this->api[] = "https://api.openweathermap.org/data/2.5/forecast?q=$city&units=metric&appid=$apiKey";

        #current or forecast //current.json
        $apiKey = 'a92b8b1a6077445fb9c163537240903';
        $this->api[] = "http://api.weatherapi.com/v1/forecast.json?key=$apiKey&q=$city&aqi=no&days=2";

        #forecast! #best api
        $apiKey = 'R2zSK9qH02191A9MpJEleiw9KUCfk1za';
        $this->api[] = "https://api.tomorrow.io/v4/weather/forecast?accept=application/json&location=$city&apikey=$apiKey";

        #forecast!
        $apiKey = 'NMVRQTJRTNMQLDTBTMMZ9VTWR';
        $this->api[] = "https://weather.visualcrossing.com/VisualCrossingWebServices/rest/services/timeline/$city?unitGroup=metric&key=$apiKey&contentType=json";
    }

    private function parseData($data) : float {
        if (isset($data['list'])) {
            $index = 0;
            $count = count($data['list']);
            for ($i = 0; $i < $count; $i++) {
                if (strpos($data['list'][$i]['dt_txt'], "00:00:00")) {
                    $index = $i;
                    break;
                }
            }

            $temp = 0.0;
            for ($i = $index; $i < $index + 8; $i++) {
                $temp += $data['list'][$i]['main']['temp'];
            }
            return $temp /= 8;
        }
        if (isset($data['forecast']['forecastday'][1])) {
            return $data['forecast']['forecastday'][1]['day']['avgtemp_c'];
        }
        if (isset($data['timelines']['daily'])) {
            return $data['timelines']['daily'][1]['values']['temperatureAvg'];
        }
        if (isset($data['days'][1])) {
            return $data['days'][1]['temp'];
        }
        return 0.0;
    }

    public function getWeather() : float {
        $weatherData = [];

        $temperature = 0.0;
        foreach ($this->api as $apiUrl) {
            $json = file_get_contents($apiUrl);
            $data = json_decode($json, true);
            $temperature += $this->parseData($data);
        }
        return $temperature / count($this->api);
    }
}