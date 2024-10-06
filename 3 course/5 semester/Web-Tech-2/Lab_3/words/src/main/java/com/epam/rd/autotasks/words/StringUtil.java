package com.epam.rd.autotasks.words;

import java.util.Arrays;
import java.util.StringJoiner;

public class StringUtil {
    public static int countEqualIgnoreCaseAndSpaces(String[] words, String sample) {
        if (words == null || sample == null || words.length == 0) {
            return 0;
        }

        String normalizedSample = sample.trim().toLowerCase();

        int count = 0;
        for (String word : words) {
            if (word.trim().toLowerCase().equals(normalizedSample)) {
                count++;
            }
        }
        return count;
    }

    public static String[] splitWords(String text) {
        if (text == null || text.isEmpty() || text.matches("^[^a-zA-Z0-9]+$")) {
            return null;
        }

        return Arrays.stream(text.split("[,.;: ?!]+"))
                .filter(word -> !word.isEmpty())
                .map(String::trim)
                .toArray(String[]::new);
    }

    public static String convertPath(String path, boolean toWin) {
        if (path == null || path.isEmpty()) {
            return null;
        }

        if (path.startsWith("~\\")) {
            return null;
        }

        if (path.contains("~") && (path.indexOf("~") != 0 || path.lastIndexOf("~") != 0)) {
            return null;
        }
        if (path.startsWith("C:") && path.indexOf("C:", 2) != -1) {
            return null;
        }
        if (path.contains("/") && path.startsWith("C:")) {
            return null;
        }

        if (path.contains("\\") && path.contains("/")) return null;

        if (path.startsWith("..")) {
            if (toWin) {
                return path.replace("/", "\\");
            } else {
                return path.replace("\\", "/");
            }
        }

        boolean isUnix = path.startsWith("/") || path.startsWith("~");
        boolean isWindows = path.startsWith("C:") || path.startsWith("\\");

        if (toWin) {
            if (isUnix) {
                if (path.startsWith("~")) {
                    return "C:\\User" + path.substring(1).replace("/", "\\");
                }
                return "C:" + path.replace("/", "\\");
            }
        } else {
            if (isWindows) {
                if (path.startsWith("C:\\")) {
                    if (path.equals("C:\\")) {
                        return "/";
                    }
                    if (!path.startsWith("C:\\User")) {
                        return path.substring(2).replace("\\", "/");
                    }
                    return "~" + path.substring(7).replace("\\", "/");
                }
                return path.replace("\\", "/");
            }
        }

        if (!isUnix && !isWindows) {
            if (!toWin) {
                return path.replace("\\", "/");
            } else {
                return path.replace("/", "\\");
            }
        }

        return path;
    }

    public static String joinWords(String[] words) {

        if (words == null || words.length == 0) {
            return null;
        }

        StringJoiner joiner = new StringJoiner(", ");
        for (String word : words) {
            if (word != null && !word.isEmpty()) {
                joiner.add(word);
            }
        }

        if (joiner.length() == 0) {
            return null;
        }

        return "[" + joiner + "]";
    }

    public static void main(String[] args) {
        System.out.println("Test 1: countEqualIgnoreCaseAndSpaces");
        String[] words = new String[]{" WordS    \t", "words", "w0rds", "WOR  DS", };
        String sample = "words   ";
        int countResult = countEqualIgnoreCaseAndSpaces(words, sample);
        System.out.println("Result: " + countResult);
        int expectedCount = 2;
        System.out.println("Must be: " + expectedCount);

        System.out.println("Test 2: splitWords");
        String text = "   ,, first, second!!!! third";
        String[] splitResult = splitWords(text);
        System.out.println("Result : " + Arrays.toString(splitResult));
        String[] expectedSplit = new String[]{"first", "second", "third"};
        System.out.println("Must be: " + Arrays.toString(expectedSplit));

        System.out.println("Test 3: convertPath");
        String unixPath = "/some/unix/path";
        String convertResult = convertPath(unixPath, true);
        System.out.println("Result: " + convertResult);
        String expectedWinPath = "C:\\some\\unix\\path";
        System.out.println("Must be: " + expectedWinPath);

        System.out.println("Test 4: joinWords");
        String[] toJoin = new String[]{"go", "with", "the", "", "FLOW"};
        String joinResult = joinWords(toJoin);
        System.out.println("Result: " + joinResult);
        String expectedJoin = "[go, with, the, FLOW]";
        System.out.println("Must be: " + expectedJoin);
    }
}