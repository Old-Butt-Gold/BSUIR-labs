package com.epam.rd.autotasks.validations;

import java.util.regex.Pattern;

public class ColorCodeValidation {
    public static boolean validateColorCode(String color) {
        if (color == null) {
            return false;
        }

        String regex = "^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$";

        return Pattern.matches(regex, color);
    }
}
