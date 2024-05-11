import java.io.*;//почему не File
import java.util.Scanner;
public class LabThirdBlockSecond {
    static Scanner scan = new Scanner(System.in);
    public static boolean consoleChoice(){
        boolean isIncorrect, isTrue;
        byte k = 0;
        final int MIN_NUM = 0, MAX_NUM = 1;
        do {
            isIncorrect = false;
            try {
                k = Byte.parseByte(scan.nextLine());
            } catch (Exception E) {
                System.err.println("Повторите ввод: ");
                isIncorrect = true;
            }
            if (!isIncorrect && (k < MIN_NUM || k > MAX_NUM)){
                System.err.println("Повторите ввод: ");
                isIncorrect = true;
            }
        } while (isIncorrect);
        if (k == 0)
            isTrue = true;
        else
            isTrue = false;
        //isTrue = k == 0;
        return isTrue;
    }
    public static int strokaIn(final int MIN_NUM){
        boolean isIncorrect;
        int n = 0;
        do {
            System.out.println("Введите количество строк: ");
            isIncorrect = false;
            try {
                n = Integer.parseInt(scan.nextLine());
            } catch (Exception E) {
                System.err.println("Повторите ввод: ");
                isIncorrect = true;
            }
            if (!isIncorrect && (n < MIN_NUM)){
                System.err.println("Повторите ввод: ");
                isIncorrect = true;
            }
        } while (isIncorrect);
        return n;
    }
    public static int stolbetsIn(final int MIN_NUM){
        boolean isIncorrect;
        int l = 0;
        do {
            System.out.println("Введите количество столбцов: ");
            isIncorrect = false;
            try {
                l = Integer.parseInt(scan.nextLine());
            } catch (Exception E) {
                System.err.println("Повторите ввод: ");
                isIncorrect = true;
            }
            if (!isIncorrect && (l < MIN_NUM)){
                System.err.println("Повторите ввод: ");
                isIncorrect = true;
            }
        } while (isIncorrect);
        return l;
    }
    public static int[][] fillMatrix(int[][] arr) {
        boolean isIncorrect;
        for (int i = 0; i < arr.length; i++) {
            for (int j = 0; j < arr[0].length; j++) {
                do {
                    System.out.println("Введите ячейку "+ (i+1) + " строки "+ (j+1) + " столбца: ");
                    isIncorrect = false;
                    try {
                        arr[i][j] = Integer.parseInt(scan.nextLine());
                    } catch (Exception E) {
                        System.err.println("Ошибка, введите целочисленное число");
                        isIncorrect = true;
                    }
                } while(isIncorrect);
            }
        }
        return arr;
    }
    public static void writeMatrix(int [][] arr) {
        for (int i = 0; i < arr.length; i++){
            for (int j = 0; j < arr[0].length; j++)
                System.out.printf("%5d", arr[i][j]);
            System.out.print("\n");
        }
    }
    public static int minStroka(int[][] arr, int i){
        int strokaMin;
        strokaMin = arr[i][0];
        for (int j = 1; j < arr[0].length; j++) {
            if (arr[i][j] < strokaMin)
                strokaMin = arr[i][j];
        }
        return strokaMin;
    }
    public static int maxStolbets(int [][]arr, int strokaMin, int i){
        int stolbetsMax = 0;
        for (int j = 0; j < arr[0].length; j++) {
            if (arr[i][j] == strokaMin)
                stolbetsMax = j;
        }
        return stolbetsMax;
    }
    public static boolean sedlovayaTochka(int [][] arr, int strokaMin, int stolbetsMax, int i) {
        boolean isTrue;
        int m = 0;
        isTrue = true;
        while (m < arr.length && isTrue) {
            if (strokaMin < arr[m][stolbetsMax]) {
                isTrue = false;
            } else {
                m++;
            }
        }
        return isTrue;
    }

    public static void writeSedlovaya(boolean isTrue, int stolbetsMax, int strokaMin, int i) {
        if (isTrue)
            System.out.println("Седловая точка в " + (i + 1) + " строке и " + (stolbetsMax + 1) + " столбце = " + strokaMin);
    }

    public static String takeFileWay(int x) {
        String way;
        boolean isIncorrect = false;
        do {
            System.out.println("Введите путь к файлу");
            way = scan.nextLine();
            File file = new File(way);
            if (!file.exists() || !file.canWrite() ||!file.canRead() || !way.endsWith(".txt") || file.isDirectory()) {
                isIncorrect = true;
                System.err.println("Файл не найден, или неверный формат файла; повторите попытку ввода");
            } else if (x == 1) {
                isIncorrect = isTakeInformationFromFile(way);
                //System.out.println(isIncorrect);
            }
        } while (isIncorrect);
        return way;
    }

    public static boolean isTakeInformationFromFile(String way) {
        int n = 0, l = 0;
        boolean isIncorrect = false;
        try (BufferedReader br = new BufferedReader(new FileReader(way))) {
            n = Integer.parseInt(br.readLine());
            // после этого finally
            if (n < 2) throw new Exception ("Количество строк должно быть > 1"); //кинется exception, попаду в блок catch, условие дальше не будет срабатываться(оптимизация)
            l = Integer.parseInt(br.readLine());
            if (l < 2) throw new Exception ("Количество столбцов должно быть > 1");
            for (int i = 0; i < n*l; i++) {
                try {
                    Integer.parseInt(br.readLine());  //Почему br.read() нельзя?
                } catch (Exception e) {
                    System.err.println("Проверьте количество элементов матрицы");
                }
            }
            if (br.readLine() != null)
                isIncorrect = true;
                //System.out.println("Уберите лишние данные");
        } catch (Exception e) {
            System.err.println("Проверьте правильность данных матрицы, введенной в документе");
            System.out.println(e.getMessage()); //Выдает в чем именно ошибка
            isIncorrect = true;
        } finally {
            if (isIncorrect)
                System.err.println("Уберите лишние данные");
        }
        //System.out.println(isIncorrect);
        return isIncorrect;
    }

    public static int[][] takeArrFromFile(String way){
        int[][] arr = new int[0][0];
        int n = 0, l = 0;
        //append
        try (BufferedReader br = new BufferedReader(new FileReader(way))) {
            n = Integer.parseInt(br.readLine());
            l = Integer.parseInt(br.readLine());
            arr = new int[n][l];
            for (int i = 0; i < arr.length; i++) {
                for (int j = 0; j < arr[0].length; j++) {
                    arr[i][j] = Integer.parseInt(br.readLine()); //Почему br.read нельзя?
                }
            }
        } catch (Exception e) {
            System.err.println("Проверьте правильность данных, введенных в документе");
        }
        return arr;
    }

    public static void writeMatrixF(String way, int[][] arr){
        try (PrintWriter pw = new PrintWriter(new FileWriter(way, false))) { //append true дописывает в конец файла, append false – перезаписывает
                for (int i = 0; i < arr.length; i++) { //(false тут, чтобы с прошлой программы перезаписывать то, что было раньше и начинать с 0
                    for (int j = 0; j < arr[0].length; j++) {
                        pw.printf("%3d", arr[i][j]); //используется printf PrintWriter
                    }
                    pw.write("\n");  //используется write FileWriter
                }
            } catch (Exception e) {
            System.err.println("Ошибка файла");
            System.out.println(e.getMessage());
        }
    }

    public static void writeSedlovayaF(String way, boolean isTrue, int stolbetsMax, int strokaMin, int i) {
        try (FileWriter fileOut = new FileWriter(way, true)) {
            if (isTrue)
                fileOut.write("Седловая точка в " + (i + 1) + " строке и " + (stolbetsMax + 1) + " столбце = " + strokaMin);
        } catch (Exception e) {
            System.err.println("Ошибка файла");
            System.out.println(e.getMessage());
        }
    }

    public static int[][] matrixIn(boolean isInput, final int MIN_NUM) {
        int n, l;
        String way;
        int [][] arr;
        if (isInput) {
            n = strokaIn(MIN_NUM);
            l = stolbetsIn(MIN_NUM);
            arr = new int[n][l];
            arr = fillMatrix(arr);
        } else {
            way = takeFileWay(1);
            arr = takeArrFromFile(way);
        }
        return arr;
    }

    public static void matrixOut(int [][] arr, boolean isOutput) {
        int strokaMin, stolbetsMax;
        boolean isTrue = false;
        String way;
        if (isOutput) {
            writeMatrix(arr);
            for (int i = 0; i < arr.length; i++) {
                strokaMin = minStroka(arr, i);
                stolbetsMax = maxStolbets(arr, strokaMin, i);
                isTrue = sedlovayaTochka(arr, strokaMin, stolbetsMax, i);
                writeSedlovaya(isTrue, stolbetsMax, strokaMin, i);
            }
            if (!(isTrue)) {
                System.out.println("Седловых точек нет");
            }
        } else {
            way = takeFileWay(11);
            writeMatrixF(way, arr);
            for (int i = 0; i < arr.length; i++) {
                strokaMin = minStroka(arr, i);
                stolbetsMax = maxStolbets(arr, strokaMin, i);
                isTrue = sedlovayaTochka(arr, strokaMin, stolbetsMax, i);
                writeSedlovayaF(way, isTrue, stolbetsMax, strokaMin, i);
            }
            try (FileWriter fileOut = new FileWriter(way, true)) {
                if (!(isTrue)) {
                    fileOut.write("\nСедловых точек нет");
                }
            } catch(Exception e) {
                System.err.println("Ошибка файла");
                System.out.println(e.getMessage());
            }
        }
    }

    public static void main(String[] args) {
        boolean isInput, isOutput;
        String way;
        int arr[][];
        final int MIN_NUM = 2;
        System.out.println("Данная программа находит седловую точку в матрице");
        System.out.println("Введите 0, если хотите ввод на консоль, 1 — ввод из файла: ");
        isInput = consoleChoice();
        arr = matrixIn(isInput, MIN_NUM);
        System.out.println("Введите 0, если хотите вывод на консоль, 1 – вывод в файл: ");
        isOutput = consoleChoice();
        matrixOut(arr, isOutput);
        scan.close();
        }

    }