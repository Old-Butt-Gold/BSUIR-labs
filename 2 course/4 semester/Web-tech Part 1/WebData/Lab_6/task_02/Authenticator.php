<?php

class Authenticator {
    private $usersFile;

    public function __construct($usersFile) {
        $this->usersFile = $usersFile;
    }

    public function authenticate($username, $password): bool
    {
        $users = $this->readUsersFile();
        return isset($users[$username]) && $users[$username] === $password;
    }

    private function readUsersFile(): array
    {
        $users = [];
        $lines = file($this->usersFile, FILE_IGNORE_NEW_LINES | FILE_SKIP_EMPTY_LINES);
        foreach ($lines as $line) {
            list($username, $password) = explode(':', $line);
            $users[$username] = $password;
        }
        return $users;
    }
}