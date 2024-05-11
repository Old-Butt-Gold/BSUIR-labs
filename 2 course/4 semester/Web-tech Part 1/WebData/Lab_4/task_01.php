<?php

require "Classes/FormBuilder.php";

$formBuilder = new FormBuilder(FormBuilder::METHOD_POST, './task_01.php', 'Send!');
$formBuilder->addTextField('someName', 'Default value');
$formBuilder->addRadioGroup('someRadioName', ['A', 'B']);
echo $formBuilder->getForm();

echo "<hr/>";

$formBuilder = new FormBuilder(FormBuilder::METHOD_GET, './task_01.php', 'Send!');
$formBuilder->addTextField('someNameOther', 'New Value');
$formBuilder->addRadioGroup('someRadioNameOther', ['C', 'D', 'E']);
echo $formBuilder->getForm();
