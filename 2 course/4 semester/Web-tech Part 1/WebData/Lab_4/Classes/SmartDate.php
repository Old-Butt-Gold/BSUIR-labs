<?php

class SmartDate
{
    private $date;
    private bool $isCorrect = false;

    public function __construct($date)
    {
        $dateTime = DateTime::createFromFormat('Y-m-d H:i:s', $date);
        if ($dateTime !== false) {
            $this->date = $dateTime;
            $this->isCorrect = true;
        } else {
            $this->isCorrect = false;
        }
    }

    public function isCorrectDate() {
        return $this->isCorrect;
    }

    public function isWeekend()
    {
        $dayOfWeek = $this->date->format('N');
        return $dayOfWeek >= 6;
    }

    public function distanceToToday()
    {
        $today = new DateTime();
        $diff = $today->diff($this->date);
        return $diff->format('%a days, %h hours, %i minutes, %s seconds');
    }

    public function isLeapYear()
    {
        $year = intval($this->date->format('Y'));
        return ($year % 4 == 0 && $year % 100 != 0) || ($year % 400 == 0);
    }
}