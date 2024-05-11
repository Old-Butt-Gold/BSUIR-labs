<?php


class Logger {

    const LOGGER_FILE = "file";
    const LOGGER_ECHO = "echo";

    private $inputType;
    private $inputFile;
    public function __construct($filePath = null) {
        $this->inputType = self::LOGGER_ECHO;
        if ($filePath != null) {
            $this->inputFile = fopen($filePath, "at");
            if (!$this->inputFile) {
                echo "Error: Unable to open file for logging.\n";
            } else {
                $this->inputType = self::LOGGER_FILE;
            }
        }
    }

    public function printMessage($message) : void {
        $dateTime = date('Y-m-d H:i:s');
        $eol = $this->inputType == self::LOGGER_FILE ? "\n" : "<br/>";
        $message = "[$dateTime] $message $eol";

        switch ($this->inputType) {
            case self::LOGGER_FILE:
                if ($this->inputFile) {
                    fwrite($this->inputFile, $message);
                } else {
                    echo "Error: No file opened for logging.\n";
                }
                break;
            case self::LOGGER_ECHO:
                echo $message . $eol;
                break;
        }
    }

    public function __destruct(){
        if ($this->inputFile) {
            fclose($this->inputFile);
        }
    }
}