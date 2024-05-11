<?php
    if (isset($argc)) {
            $count = (int)$argv[1];
            if ($count === 0) {
                echo "<h1>invalid input!</h1>";
            } else {
                echo "<table>\n";
                for ($i = 0; $i < $count; $i++) {
                    echo "<tr><td>row number:" . ($i + 1) . "!</td></tr>\n";
                }
                echo "</table>";
            }
    } else {
        exit("argc and argv disabled\n");
    }