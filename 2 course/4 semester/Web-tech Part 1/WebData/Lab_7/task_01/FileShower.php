<?php

class FileShower
{
    private $uploadDirectory;

    public function __construct($uploadDirectory)
    {
        $this->uploadDirectory = $uploadDirectory;
    }

    public function getUploadedFiles() : array
    {
        $files = scandir($this->uploadDirectory);
        return array_diff($files, array('.', '..'));
    }

    public function showUploadedFiles() : void {
        $uploadedFiles = self::getUploadedFiles();
        $result = "<div class='imagesURL'>
                    <ul>";
        foreach ($uploadedFiles as $file) {
            $temp = $this->uploadDirectory . $file;
            $preview = "<img src='$temp' width='100' height='100'><br/>";

            $result .= "<li>
                        $preview
                        <hr/>
                        <a href='$temp' download>$file</a>
                        </li>";
        }
        $result .= "</ul></div>";
        echo $result;
    }
}

