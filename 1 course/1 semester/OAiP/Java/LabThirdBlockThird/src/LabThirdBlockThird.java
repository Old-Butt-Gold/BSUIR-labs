import java.util.Scanner;
import java.io.*;

public class LabThirdBlockThird {
    static Scanner scan = new Scanner(System.in);

    public static int inputSizeOfArray() {
        int n = 0;
        boolean isIncorrect;
        final int MIN_NUM = 2;
        System.out.println("Введите размерность массива");
        do {
            isIncorrect = false;
            try {
                n = Integer.parseInt(scan.nextLine());
            } catch (NumberFormatException E) {
                System.err.println("Повторите ввод:\s");
                isIncorrect = true;
            }
            if (!isIncorrect && n < MIN_NUM) {
                System.err.println("Размерность должна быть > 1:\s");
                isIncorrect = true;
            }
        } while (isIncorrect);
        return n;
    }

    public static int takeCellIntoArray() {
        int temp = 0;
        boolean isIncorrect;
        do {
            isIncorrect = false;
            try {
                temp = Integer.parseInt(scan.nextLine());
            } catch (NumberFormatException E) {
                System.err.println("Повторите ввод");
                isIncorrect = true;
            }
        } while (isIncorrect);
        return temp;
    }
    public static int[] FillArray() {
        int n = inputSizeOfArray();
        int[] arr = new int [n];
        for (int i = 0; i < arr.length; i++) {
            System.out.println("Введите элемент:\s" + (i+1));
            arr[i] = takeCellIntoArray();
        }
        return arr;
    }
    public static String chooseOption(){
        boolean isIncorrect;
        String input;
        System.out.println("Введите console, если хотите использовать консоль, file, если файл");
        do {
            isIncorrect = false;
            input = scan.nextLine();
            if (!input.equalsIgnoreCase("console") && !input.equalsIgnoreCase("file")){
                System.err.println("Повторите ввод:\s");
                isIncorrect = true;
            }
        } while (isIncorrect);
        return input;
    }

    public static String chooseConsoleWayToFill() {
        boolean isIncorrect;
        String input;
        System.out.println("Введите console, если хотите ввести данные с консоли, random, чтобы сгенерировать случайный массив");
        do {
            isIncorrect = false;
            input = scan.nextLine();
            if (!input.equalsIgnoreCase("console") && !input.equalsIgnoreCase("random")) {
                System.err.println("Повторите ввод:\s");
                isIncorrect = true;
            }
        } while (isIncorrect);
        return input;
    }

    public static int takeRandomSizeOfArray() {
        int n;
        n = (int) Math.round((Math.random() * 200) + 2);
        return n;
    }

    public static int[] takeRandomElementsOfArray() {
        int n = takeRandomSizeOfArray();
        int[] arr = new int [n];
        for (int i = 0; i < arr.length; i++) {
            arr[i] = (int) (Math.round(Math.random() * 1000) - Math.round(Math.random() * 1000));
        }
        return arr;
    }

    public static int[] shellsSort(int[] arr) {
        int gap = arr.length / 2;
        int temp, j;
        while (gap > 0) {
            for (int i = 0; i < arr.length - gap; i++) {
                j = i;
                temp = arr[j + gap];
                while (j >= 0 && arr[j] > temp) {
                    arr[j + gap] = arr[j];
                    arr[j] = temp;
                    j -= gap;
                }
            }
            gap /= 2;
        }
        return arr;
    }

    public static boolean isElemsOfArrayInFileCorrect(String tempStr, int n) {
        String[] elems = tempStr.split("\s");
        int temp = 0;
        boolean isIncorrect = false;
        if (elems.length == n) {
            for (int i = 0; i < n && !isIncorrect; i++) {
                try {
                    temp = Integer.parseInt(elems[i]);
                } catch (NumberFormatException E) {
                    isIncorrect = true;
                    System.err.println("Число под индексом\s" + (i + 1) + "\sнеправильное");
                }
            }
        } else {
            isIncorrect = true;
            System.err.println("Закончились данные для считывания размерности матрицы на элементе\s" + elems.length);
        }
        return isIncorrect;
    }
    public static boolean isTakeInformationFromFile(String way) {
        int n = 0, temp = 0;
        String inputSize, inputElemsOfArray;
        final int MIN_NUM = 2;
        boolean isIncorrect = false;
        try (BufferedReader br = new BufferedReader(new FileReader(way))) {
            if ((inputSize = br.readLine()) != null) {
                try {
                    n = Integer.parseInt(inputSize);
                } catch(NumberFormatException E) {
                    isIncorrect = true;
                    System.err.println("Проверьте соответствие размерности к целому числу");
                }
                if (!isIncorrect & n < MIN_NUM) {
                    isIncorrect = true;
                    System.err.println("Число должно быть > 1");
                }
            } else {
                isIncorrect = true;
                System.err.println("В файле нет данных размерности массива");
            }
            if ((inputElemsOfArray = br.readLine()) != null) {
                if (!isIncorrect) {
                    isIncorrect = isElemsOfArrayInFileCorrect(inputElemsOfArray, n);
                }
            } else {
                isIncorrect = true;
                System.err.println("Нет данных для элементов массива");
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
            if (!file.exists() || !file.canWrite() || !file.canRead() || !way.endsWith(".txt") || file.isDirectory()) {
                isIncorrect = true;
                System.err.println("Файл не найден, или неверный формат файла; повторите попытку ввода");
            } else if (temp.equals("In")) {
                isIncorrect = isTakeInformationFromFile(way);
            }
        } while (isIncorrect);
        return way;
    }

    public static int[] consoleInput(String inputChoice) {
        int[] arr;
        arr = (inputChoice.equalsIgnoreCase("console")) ? FillArray() : takeRandomElementsOfArray();
        return arr;
    }

    public static int takeFromFileArraySize(String way) {
        int n = 0;
        try (BufferedReader br = new BufferedReader(new FileReader(way))) {
            n = Integer.parseInt(br.readLine());
        } catch (IOException e) {
            System.err.println("Ошибка файла");
            System.err.println(e.getMessage());
        }
        return n;
    }

    public static int[] takeFromFileArray(String way) {
        int n = takeFromFileArraySize(way);
        int[] arr = new int[n];
        try (BufferedReader br = new BufferedReader(new FileReader(way))) {
            br.readLine();
            String[] elems = br.readLine().split("\s");
            for (int i = 0; i < n; i++) {
                arr[i] = Integer.parseInt(elems[i]);
            }
        } catch (IOException e) {
            System.err.println("Ошибка файла");
            System.err.println(e.getMessage());
        }
        return arr;
    }
    public static int[] takeFinalInformaton(String inputChoice) {
        int[] arr;
        String way;
        if (inputChoice.equalsIgnoreCase("console")) {
            inputChoice = chooseConsoleWayToFill();
            arr = consoleInput(inputChoice);
        } else {
            way = takeFileWay("In");
            arr = takeFromFileArray(way);
        }
        return arr;
    }

    public static void outputFirstArray(int[] arr) {
        System.out.println("Первоначальный массив: ");
        for (int i : arr) { //int i = 0; i < arr.length; i++
            System.out.printf("%5d", i);
        }
        System.out.println('\n');
    }

    public static void outputNewArray(int[] arr) {
        System.out.println("Новый массив:\s");
        for (int i : arr) {
            System.out.printf("%5d", i);
        }
    }

    public static void outputFirstArrayInFile(int[] arr, String way){
        try (PrintWriter pw = new PrintWriter(new FileWriter(way, false))) {
            pw.write("Первоначальный массив:\s\n");
            for (int i : arr) {
                pw.printf("%5d", i);
            }
            pw.write('\n');
        } catch (IOException e) {
            System.err.println("Ошибка файла");
            System.err.println(e.getMessage());
        }
    }

    public static void outputNewArrayInFile(int[] arr, String way){
        try (PrintWriter pw = new PrintWriter(new FileWriter(way, true))) {
            pw.write("Новый массив:\s\n");
            for (int i : arr) {
                pw.printf("%5d", i);
            }
        } catch (IOException e) {
            System.err.println("Ошибка файла");
            System.err.println(e.getMessage());
        }
    }

    public static void outputFinalInformation(int[] arr, String outputChoice) {
        String way;
        if (outputChoice.equalsIgnoreCase("console")) {
            outputFirstArray(arr);
            arr = shellsSort(arr);
            outputNewArray(arr);
        } else {
            way = takeFileWay("Out");
            outputFirstArrayInFile(arr, way);
            arr = shellsSort(arr);
            outputNewArrayInFile(arr, way);
        }
    }

    public static void main(String[] args) {
        String inputChoice, outputChoice;
        int[] arr;
        System.out.println("Данная программа сортирует массив методом Шелла");
        inputChoice = chooseOption();
        arr = takeFinalInformaton(inputChoice);
        outputChoice = chooseOption();
        outputFinalInformation(arr, outputChoice);
    }
}
