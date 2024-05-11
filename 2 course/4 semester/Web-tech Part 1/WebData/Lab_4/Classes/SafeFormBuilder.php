<?php

require "FormBuilder.php";

class SafeFormBuilder extends FormBuilder {
    private $reference = [];

    public function __construct($method, $dir, $submitValue)
    {
        parent::__construct($method, $dir, $submitValue);
        $this->reference = $method === self::METHOD_GET ? $_GET : $_POST;
    }

    public function addTextField($name, $value) : void {
        $value = $this->reference[$name] ?? $value;
        parent::addTextField($name, $value);
    }

    public function addRadioGroup($name, $values, $selected=null) : void
    {
        if ($this->reference != null)
            $selected = $this->reference[$name] ?? $selected;

        parent::addRadioGroup($name, $values, $selected);
    }

}