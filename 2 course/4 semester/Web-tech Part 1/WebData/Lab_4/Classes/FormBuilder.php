<?php

class FormBuilder {
    const METHOD_POST = "post";
    const METHOD_GET = "get";

    protected $method;
    protected $dir;
    protected $submitValue;
    protected $fields = [];

    public function __construct($method, $dir, $submitValue) {
        $this->method = $method;
        $this->dir = $dir;
        $this->submitValue = $submitValue;
    }

    public function addTextField($name, $value) : void
    {
        $this->fields[] = "<p><input type=\"text\" name=\"$name\" value=\"$value\" /></p>";
    }

    public function addRadioGroup($name, $values, $selected = null) : void
    {
        foreach ($values as $value) {
            $str = "";
            if ($selected === $value)
                $str = " checked ";
            $this->fields[] = "<p><input type=\"radio\" name=\"$name\" $str value=\"$value\" /></p>";
        }
    }

    public function getForm() {
        $fieldsHtml = implode("", $this->fields);
        return "<p><form method=\"$this->method\" action=\"$this->dir\">"
            . "$fieldsHtml"
            . "<p><input type=\"submit\" value=\"$this->submitValue\" /></p>"
            . "</form></p>";
    }
}
