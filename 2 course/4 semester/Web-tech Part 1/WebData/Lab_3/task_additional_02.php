<?php

    if ($argc === 2) {
        $path = $argv[1];
        if (file_exists($path)) {
            $lines = file($path, FILE_IGNORE_NEW_LINES | FILE_SKIP_EMPTY_LINES);
            asort($lines);
            $file = fopen($path, "w");
            foreach ($lines as $line) {
                fwrite($file, $line . "\n");
            }
            fclose($file);
        }
        else
            echo "Your file doesn't exist. Choose another file";
    }
    else
        echo "Check your parameters. It should have only path to the file!";