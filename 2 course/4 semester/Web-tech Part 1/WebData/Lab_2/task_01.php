<?php

    #declare(strict_types=1);

    if (isset($argc)) {
        for ($i = 1; $i < $argc; $i++) {
            //gettype($argv[$i]);
            $type = "string";
            if ($argv[$i] == (double)$argv[$i])
                $type = "double";
            if ($argv[$i] == (int)$argv[$i])
                $type = "integer";
            echo "$argv[$i] — $type" . PHP_EOL;
        }
    }
    else
        exit("argc and argv disabled" . PHP_EOL);
