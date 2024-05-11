<?php

require "Classes/Logger.php";

if (isset($_GET["path"])) {
    $screenLogger = new Logger($_GET["path"]);
    $screenLogger->printMessage("Goodbye, cruel world");
} else {
    $screenLogger = new Logger();
    $screenLogger->printMessage("Hello, world");
}


