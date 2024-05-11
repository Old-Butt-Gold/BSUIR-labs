import java.util.Scanner;
import java.io.*;

public class LabFirstBlockThird {
    static Scanner scan = new Scanner(System.in);

    public static int inputAmountOfOccurences() {
        int n = 0;
        boolean isIncorrect;
        final int MIN_NUM = 1, MAX_NUM = 255;
        System.out.println("Введите число, количество вхождений строки 1 в строку 2, k = ");
        do {
            isIncorrect = false;
            try {
                n = Integer.parseInt(scan.nextLine());
            } catch (NumberFormatException E) {
                System.err.println("Повторите ввод: ");
                isIncorrect = true;
            }
            if (!isIncorrect && (n < MIN_NUM || n > MAX_NUM)) {
                System.err.println("Повторите ввод: ");
                isIncorrect = true;
            }
        } while (isIncorrect);
        return n;
    }

    public static String chooseOption(){
        boolean isIncorrect;
        String input;
        System.out.println("Введите console, если хотите использовать консоль, file, если файл");
        do {
            isIncorrect = false;
            input = scan.nextLine();
            if (!input.equalsIgnoreCase("console") && !input.equalsIgnoreCase("file")){
                System.err.println("Повторите ввод: ");
                isIncorrect = true;
            }
        } while (isIncorrect);
        return input;
    }

    public static String chooseConsoleWayToFill() {
        boolean isIncorrect;
        String input;
        System.out.println("Введите console, если хотите ввести данные с консоли, random, чтобы сгенерировать строки файл");
        do {
            isIncorrect = false;
            input = scan.nextLine();
            if (!input.equalsIgnoreCase("console") && !input.equalsIgnoreCase("random")){
                System.err.println("Повторите ввод: ");
                isIncorrect = true;
            }
        } while (isIncorrect);
        return input;
    }

    public static String inputStrConsole() {
        String str;
        boolean isIncorrect;
        do {
            isIncorrect = false;
            str = scan.nextLine();
            if (str.isEmpty() || str.charAt(0) == ' ') { //str.equals("");
                isIncorrect = true;
                System.err.println("Повторите ввод");
            }
        } while (isIncorrect);
        return str;
    }

    public static int calculateAnswer(String str1, String str2, int k) {
        int countN = 0, n = 0;
        for (int i = 0; i <= str2.length() - str1.length(); i++) {
            if (str2.substring(i, str1.length() + i).equals(str1)) {
                ++countN;
                if (countN == k) {
                    n = i + 1;
                }
            }
        }
        return n;
    }

    public static String randomStrConsole() {
        double n;
        String str = "", upper, lower, numbers, allCurrent;
        upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        lower = upper.toLowerCase();
        numbers = "0123456789";
        allCurrent = upper + lower + numbers;
        n = Math.random() * 255;
        for (int i = 0; i < n; i++) {
            str += allCurrent.charAt((int) Math.round(Math.random() * 61));
        }
        return str;
    }

    public static int consoleInput(String input) {
        boolean isLengthRight;
        int k, n;
        String str1 = "", str2 = "";
        if (input.equalsIgnoreCase("console")) {
            do {
                isLengthRight = false;
                System.out.println("Введите строку 1(первый символ не должен быть пробелом)");
                str1 = inputStrConsole();
                System.out.println("Введите строку 2(первый символ не должен быть пробелом)");
                str2 = inputStrConsole();
                if (str1.length() > str2.length()) {
                    System.err.println("Повторите ввод строк, чтобы размер вашей первой строки был МЕНЬШЕ размера второй строки");
                    isLengthRight = true;
                }
            } while (isLengthRight);
        } else {
            do {
                isLengthRight = false;
                str1 = randomStrConsole();
                str2 = randomStrConsole();
                if (str1.length() > str2.length()) {
                    isLengthRight = true;
                }
            } while (isLengthRight);
            System.out.println("Строки сгенерированы.");
            System.out.println(str1);
            System.out.println(str2);
        }
        k = inputAmountOfOccurences();
        n = calculateAnswer(str1, str2, k);
        return n;
    }

    public static boolean isTakeInformationFromFile(String way) {
        int k = 0;
        final int MIN_NUM = 1, MAX_NUM = 255;
        String str1, str2;
        boolean isIncorrect = false;
        try (BufferedReader br = new BufferedReader(new FileReader(way))) {
            if ((str1 = br.readLine()) != null) {
                if (str1.isEmpty() || str1.charAt(0) == ' ') { //str1.isEmpty()
                    isIncorrect = true;
                    System.err.println("Проверьте правильность введенной 1-ой строки");
                }
            } else {
                isIncorrect = true;
                System.err.println("В файле нет данных для 1-ой строки");
            }
            if (!isIncorrect & (str2 = br.readLine()) != null) {
                if (str2.isEmpty() || str2.charAt(0) == ' ') { //str2.isEmpty()
                    isIncorrect = true;
                    System.err.println("Проверьте правильность введенной 2-ой строки");
                }
            } else {
                isIncorrect = true;
                System.err.println("В файле нет данных для 2-ой строки");
            }
            if (!isIncorrect & str1.length() > str2.length()) {
                System.err.println("Размер вашей строки 2 должен быть БОЛЬШЕ размера строки 1");
                isIncorrect = true;
            }
            if (!isIncorrect) {
                try {
                    k = Integer.parseInt(br.readLine());
                } catch (NumberFormatException E) {
                    isIncorrect = true;
                    System.err.println("Проверьте соответствие k к целому числу");
                }
                if (!isIncorrect & (k < MIN_NUM || k > MAX_NUM)) {
                    isIncorrect = true;
                    System.err.println("Ваше число должно быть в диапазоне " + MIN_NUM + " до " + MAX_NUM);
                }
            }
            if (!isIncorrect & br.readLine() != null) {
                isIncorrect = true;
                System.out.println("Уберите лишние данные");
            }
        } catch (IOException e) {
            System.err.println("Ошибка файла");
            System.err.println(e.getMessage());
            isIncorrect = true;
        }
        return isIncorrect;
    }

    public static String takeFileWay(String temp) {
        String way;
        boolean isIncorrect;
        do {
            isIncorrect = false;
            System.out.println("Введите путь к файлу");
            way = scan.nextLine();
            File file = new File(way);
            if (!file.exists() || !file.canWrite() ||!file.canRead() || !way.endsWith(".txt") || file.isDirectory()) {
                isIncorrect = true;
                System.err.println("Файл не найден, или неверный формат файла; повторите попытку ввода");
            } else if (temp.equals("In")) {
                isIncorrect = isTakeInformationFromFile(way);
            }
        } while (isIncorrect);
        return way;
    }

    public static String inputStringFromFile(String way, int p) {
    String str = "";
        try (BufferedReader br = new BufferedReader(new FileReader(way))) {
            if (p == 1) {
                br.readLine();
            }
            str = br.readLine();
        } catch (IOException e) {
            System.err.println("Ошибка файла");
            System.err.println(e.getMessage());
        }
        return str;
    }

    public static int inputAmountOfOccurencesFromFile(String way) {
        int k = 0;
        try (BufferedReader br = new BufferedReader(new FileReader(way))) {
        br.readLine();
        br.readLine();
        k = Integer.parseInt(br.readLine());
        } catch (IOException e) {
            System.err.println("Ошибка файла");
            System.err.println(e.getMessage());
        }
        return k;
    }
        public static int takeFinalInformation(String input) {
        String way, str1, str2;
        int n = 0, k;
        if (input.equalsIgnoreCase("console")) {
            input = chooseConsoleWayToFill();
            n = consoleInput(input);
        } else {
            way = takeFileWay("In");
            str1 = inputStringFromFile(way, 0);
            str2 = inputStringFromFile(way, 1);
            k = inputAmountOfOccurencesFromFile(way);
            n = calculateAnswer(str1, str2, k);
            System.out.println("Данные считаны успешно");
        }
        return n;
    }

    public static void writeAnswerConsole(int n) {
        System.out.println("Программа завершилась успешно.\n" + "номер позиции k-го вхождения: " + n);
    }

    public static void writeAnswerFile(String way, int n) {
        try (PrintWriter pw = new PrintWriter(new FileWriter(way, false))) {
            pw.write("Программа завершилась успешно.\n" + "номер позиции k-го вхождения: " + n);
        } catch (IOException e) {
            System.err.println("Ошибка файла");
            System.err.println(e.getMessage());
        }
    }
    public static void outputAnswer(String outputChoice, int n) {
        String way;
        if (outputChoice.equalsIgnoreCase("console")) {
            writeAnswerConsole(n);
        } else {
            way = takeFileWay("Out");
            writeAnswerFile(way, n);
        }
    }

    public static void main(String[] args) {
    String inputChoice, outputChoice;
    int n;
    System.out.println("Данная программа находит количество вхождений(задаваемого пользователем) первой строки во вторую");
    inputChoice = chooseOption();
    n = takeFinalInformation(inputChoice);
    outputChoice = chooseOption();
    outputAnswer(outputChoice, n);
    }
}
