<?php

require "Classes/SafeFormBuilder.php";

$formBuilder = new SafeFormBuilder(FormBuilder::METHOD_POST, './task_02.php', 'Send!');
$formBuilder->addTextField('someName1', 'Default post value');
$formBuilder->addRadioGroup('someRadioName1', ['A', 'B', 'C', 'D', 'E']);
echo $formBuilder->getForm();

echo "<hr/>";

$formBuilder = new SafeFormBuilder(FormBuilder::METHOD_GET, './task_02.php', 'Send!');
$formBuilder->addTextField('someName2', 'New Value');
$formBuilder->addRadioGroup('someRadioName2', ['C', 'D', 'E']);
echo $formBuilder->getForm();