<?php

declare(strict_types=1);

require_once 'Service/TemplateEngine.php';

use Example\Service\TemplateEngine;

$templateEngine = new TemplateEngine('Resources/views/');

$templateEngine->render(
    'email.html',
    [
        'title' => 'Test Email',
        'styles' => 'Resources/styles/style.css',
        'subject' => 'test subject',
        'company' => 'test company',
        'position' => 'test position',
        'previous company' => 'test previous company',
        'previous position' => 'test previous position',
        'name' => 'test name',
    ]
);
