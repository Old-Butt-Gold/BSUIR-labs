<?php

declare(strict_types=1);

namespace Example\Service;

class TemplateEngine
{
    public function __construct(private string $templatesBaseDir = '')
    {
    }

    /**
     * @param array<string, string> $parameters
     */
    public function render(string $template, array $parameters): void
    {
        $path = $this->getFullPath($template);

        if (!file_exists($path)) {
            return;
        }

        $content = file_get_contents($path);

        foreach ($parameters as $key => $value) {
            $content = preg_replace(sprintf('/{%s}/', $key), $value, $content);
        }

        echo $content;
    }

    private function getFullPath(string $template): string
    {
        return $this->templatesBaseDir . $template;
    }
}
