import java.util.Scanner;
import java.io.*;
import java.util.HashSet;
import java.util.Set;
public class LabSecondBlockThird {
    static Scanner scan = new Scanner(System.in);

    public static String chooseOption() {
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

    public static String inputStrConsole() {
        String str;
        boolean isIncorrect;
        do {
            str = scan.nextLine();
            isIncorrect = (str.isEmpty() || str.charAt(0) == ' ');
        } while (isIncorrect);
        return str;
    }

    public static HashSet <Character>  findAnswer(String str) {
        HashSet<Character> finalSet = new HashSet<>();
        HashSet<Character> mySet = new HashSet<>(Set.of('0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '+', '-', '=', ':', '<', '>', '(', ')'));
        for (int i = 0; i < str.length(); i++)
            if (mySet.contains(str.charAt(i))) {
                finalSet.add(str.charAt(i));
                mySet.remove(str.charAt(i));
            }
        return finalSet;
    }

    public static String takeFileWay() {
        String way;
        boolean isIncorrect;
        do {
            isIncorrect = false;
            System.out.println("Введите путь к файлу");
            way = scan.nextLine();
            File file = new File(way);
            if (!file.exists() | !way.endsWith(".txt") | file.isDirectory()) {
                isIncorrect = true;
                System.err.println("Проверьте параметры файла");
            }
        } while (isIncorrect);
        return way;
    }

    public static String inputStringFromFile() {
        String str = "", way;
        boolean isIncorrect;
        do {
            way = takeFileWay();
            try (BufferedReader br = new BufferedReader(new FileReader(way))) {
                if ((str = br.readLine()) != null) {
                    isIncorrect = (str.isEmpty() || str.charAt(0) == ' ');
                } else {
                    isIncorrect = true;
                    System.err.println("В файле нет данных для строки");
                }
                if (!isIncorrect & br.readLine() != null) {
                    isIncorrect = true;
                    System.err.println("Уберите лишние данные");
                }
            } catch (IOException e) {
                System.err.println(e.getMessage());
                isIncorrect = true;
            }
        } while (isIncorrect);
        return str;
    }

    public static void writeAnswerInFile(HashSet<Character> finalSet) {
        boolean isIncorrect;
        String way;
        do {
            way = takeFileWay();
            isIncorrect = false;
            try (PrintWriter pw = new PrintWriter(new FileWriter(way, false))) {
                if (finalSet.isEmpty())
                    pw.write("Нет нужных символов");
                else
                    pw.write(String.valueOf(finalSet));
            } catch (IOException e) {
                System.err.println(e.getMessage());
                isIncorrect = true;
            }
        } while (isIncorrect);
    }

    public static HashSet<Character> takeFinalInformation(String input) {
        String str;
        HashSet<Character> finalSet;
        str = (input.equalsIgnoreCase("console")) ? inputStrConsole() : inputStringFromFile();
        finalSet = findAnswer(str);
        return finalSet;
    }
    public static void outputAnswer(String outputChoice, HashSet<Character> finalSet) {
        if (outputChoice.equalsIgnoreCase("console")) {
            if (finalSet.isEmpty())
                System.out.println("Нет нужных символов");
            else
                System.out.println(finalSet);
        } else {
            writeAnswerInFile(finalSet);
        }
    }

    public static void main(String[] args) {
        String inputChoice, outputChoice;
        HashSet<Character> finalSet;
        System.out.println("Данная программа находит символы арифметических операций и цифры в строке");
        inputChoice = chooseOption();
        finalSet = takeFinalInformation(inputChoice);
        outputChoice = chooseOption();
        outputAnswer(outputChoice, finalSet);
    }
}
