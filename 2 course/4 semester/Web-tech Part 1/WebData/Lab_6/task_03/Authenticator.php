<?php

class Authenticator
{
    private $conn;

    public function __construct($conn)
    {
        $this->conn = $conn;
    }

    public function authenticate($username, $password): bool
    {
        $sql = "SELECT * FROM users WHERE email = '$username' AND password = '$password' LIMIT 1";
        $result = $this->conn->query($sql);

        return $result->num_rows > 0;
    }
}