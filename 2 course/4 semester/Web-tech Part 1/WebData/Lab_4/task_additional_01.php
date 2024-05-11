<?php
class TableBuilder {
    private $rows = [];

    public function addRow($row_data) {
        $this->rows[] = $row_data;
    }

    public function build() {
        $table_html = "<table style='border: 1px solid black;'>\n";
        foreach ($this->rows as $row) {
            $table_html .= "  <tr style='border: 1px solid black;'>\n";
            foreach ($row as $cell) {
                $table_html .= "    <td style='border: 1px solid black; padding: 5px;'>{$cell}</td>\n";
            }
            $table_html .= "  </tr>\n";
        }
        $table_html .= "</table>";
        return $table_html;
    }
}

$table_builder = new TableBuilder();
$table_builder->addRow(["Name", "Age", "Country"]);
$table_builder->addRow(["John", "30", "USA"]);
$table_builder->addRow(["Alice", "25", "Canada"]);

echo $table_builder->build();
