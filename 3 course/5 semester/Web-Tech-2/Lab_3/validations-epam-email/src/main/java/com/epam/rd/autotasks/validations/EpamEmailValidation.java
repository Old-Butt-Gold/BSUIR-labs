package com.epam.rd.autotasks.validations;

import java.util.regex.Matcher;
import java.util.regex.Pattern;

public class EpamEmailValidation {

    public static boolean validateEpamEmail(String email) {
        if (email == null) {
            return false;
        }

        String regex = "^[a-zA-Z]+_[a-zA-Z]+([1-9]+)?@epam.com";

        return Pattern.matches(regex, email);
    }
}





