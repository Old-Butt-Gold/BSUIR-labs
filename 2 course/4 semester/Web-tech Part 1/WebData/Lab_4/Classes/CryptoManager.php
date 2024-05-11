<?php

class CryptoManager {
    private $cipher;
    private $key;

    public const AES = 'AES-192-CBC';
    public const DES = 'DES-ECB';

    public function __construct($algorithm, $key) {
        $this->cipher = $algorithm;
        $this->key = $key;
    }

    public function encrypt($text) {
        return openssl_encrypt($text, $this->cipher, $this->key, 0, $this->key);
    }

    public function decrypt($encryptedText) {
        return openssl_decrypt($encryptedText, $this->cipher, $this->key, 0, $this->key);
    }
}