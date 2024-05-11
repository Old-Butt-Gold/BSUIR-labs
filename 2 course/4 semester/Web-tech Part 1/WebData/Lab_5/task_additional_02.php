<?php

error_reporting(0);

class TableBuilder {
    private $rows = [];
    private $headerAdded = false;

    public function addHeader(array $row_data) {
        if (!$this->headerAdded) {
            array_unshift($this->rows, $row_data);
            $this->headerAdded = true;
        }
    }

    public function addRow(array $row_data) {
        $this->rows[] = $row_data;
    }

    public function build() {
        $table_html = "<table style='border: 1px solid black;'>\n";
        foreach ($this->rows as $row) {
            $table_html .= "  <tr style='border: 1px solid black;'>\n";
            foreach ($row as $cell) {
                if (!$this->headerAdded) {
                    $table_html .= "    <td style='border: 1px solid black; padding: 5px;'>{$cell}</td>\n";
                } else {
                    $table_html .= "    <th style='border: 1px solid black; padding: 5px;'>{$cell}</th>\n";
                    $this->headerAdded = false;
                }
            }
            $table_html .= "  </tr>\n";
        }
        $table_html .= "</table>";
        return $table_html;
    }
}

$sql = new mysqli("localhost", "old-butt-gold", "HyperPROROK2019", "php_webtech");

$query = "SELECT * FROM tabletest";
$result = $sql->query($query);

echo "affected $result->num_rows</br><hr/>";

$table_html = new TableBuilder();

$header = [];
foreach ($result->fetch_fields() as $row) {
    $header[] = $row->name;
}
$table_html->addHeader($header);

$rows = $result->fetch_all();
foreach ($rows as $row) {
    $table_html->addRow($row);
}

echo $table_html->build();

$sql->close();