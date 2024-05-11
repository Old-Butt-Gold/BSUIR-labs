import java.io.*;//почему не File
import java.util.Scanner;
public class LabFourthBlockSecond {
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

    public static int dimensionIn(final int MIN_NUM){
        boolean isIncorrect;
        int n = 0;
        do {
            System.out.println("Введите размерность подматрицы n матрицы 2n: ");
            isIncorrect = false;
            try {
                n = Integer.parseInt(scan.nextLine());
            } catch (Exception E) {
                System.err.println("Повторите ввод: ");
                isIncorrect = true;
            }
            if (!isIncorrect && (n < MIN_NUM)){
                System.err.println("Должно быть > 1");
                isIncorrect = true;
            }
        } while (isIncorrect);
        return n;
    }

    public static int[][] fillMatrix(int n) {
        boolean isIncorrect;
        int arr[][];
        arr = new int[2*n][2*n];
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

    public static String takeFileWay(int x) {
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
            } else if (x == 1) {
                isIncorrect = isTakeInformationFromFile(way);
                //System.out.println(isIncorrect);
            }
        } while (isIncorrect);
        return way;
    }

    public static boolean isTakeInformationFromFile(String way) {
        int n = 0, newN, elem;
        boolean isIncorrect = false;
        try (BufferedReader br = new BufferedReader(new FileReader(way))) {
            Scanner scanFile = new Scanner(br);
            n = Integer.parseInt(scanFile.nextLine()); // после этого finally
            if (n < 2) throw new Exception ("Размерность подматрицы должна быть > 1");
            newN = 4*n*n;//кинется exception, попаду в блок catch, условие дальше не будет срабатываться(оптимизация)
            for (int i = 0; i < newN; i++) {
                try {
                    elem = Integer.parseInt(scanFile.next());  //Почему br.read() нельзя?
                } catch (Exception e) {
                    System.err.println("Проверьте правильность данных матрицы, введенной в документе");
                    System.err.println(e.getMessage());
                    isIncorrect = true;
                }
            }
            if (scanFile.hasNextLine()) {
                isIncorrect = true;
                System.err.println("Уберите лишние данные");
            }
            scanFile.close();
        } catch (Exception e) {
            System.err.println("Проверьте правильность данных, введенных в документе");
            System.err.println(e.getMessage()); //Выдает в чем именно ошибка
            isIncorrect = true;
        }
        return isIncorrect;
    }

    public static int[][] takeArrFromFile(String way){
        int[][] arr = new int[0][0];
        int n = 0, l = 0;
        //append
        try (BufferedReader br = new BufferedReader(new FileReader(way))) {
            Scanner scanFile = new Scanner(br);
            n = Integer.parseInt(scanFile.nextLine());
            arr = new int[2*n][2*n];
            for (int i = 0; i < arr.length; i++) {
                for (int j = 0; j < arr[0].length; j++) {
                    arr[i][j] = Integer.parseInt(scanFile.next()); //Почему br.read нельзя?
                }
            }
            scanFile.close();
        } catch (Exception e) {
            System.err.println("Проверьте правильность данных, введенных в документе");
        }
        return arr;
    }

    public static void writeFirstMatrix(int [][] arr) {
        for (int i = 0; i < arr.length; i++){
            for (int j = 0; j < arr[0].length; j++)
                System.out.printf("%5d", arr[i][j]);
            System.out.print("\n");
        }
    }

    public static int[][] convertMatrix(int[][] arr) {
        int x;
        for (int i = 0; i < (arr.length/2); i++) {
            for (int j = 0; j < (arr[0].length / 2); j++) {
                x = arr[i][j];
                arr[i][j] = arr[i + arr.length / 2][j];
                arr[i + arr.length / 2][j] = arr[i][j + arr.length / 2];
                arr[i][j + arr.length / 2] = arr[i + arr.length / 2][j + arr.length / 2];
                arr[i + arr.length / 2][j + arr.length / 2] = x;
            }
        }
        return arr;
    }

    public static void writeNewMatrix(int[][] arr) {
        arr = convertMatrix(arr);
        for (int i = 0; i < arr.length; i++){
            for (int j = 0; j < arr[0].length; j++)
                System.out.printf("%5d", arr[i][j]);
            System.out.print("\n");
        }
    }

    public static void writeFirstMatrixF(String way, int[][] arr) {
        try (PrintWriter pw = new PrintWriter(new FileWriter(way, false))) { //append true дописывает в конец файла, append false – перезаписывает
            pw.write("Ваша первоначальная матрица:\n");
            for (int i = 0; i < arr.length; i++) { //(false тут, чтобы с прошлой программы перезаписывать то, что было раньше и начинать с 0
                for (int j = 0; j < arr[0].length; j++) {
                    pw.printf("%5d", arr[i][j]); //используется printf PrintWriter
                }
                pw.write("\n");  //используется write FileWriter
            }
        } catch (Exception e) {
            System.err.println("Ошибка файла");
            System.out.println(e.getMessage());
        }
    }

    public static void writeNewMatrixF(String way, int[][] arr) {
        arr = convertMatrix(arr);
        try (PrintWriter pw = new PrintWriter(new FileWriter(way, true))) {
            pw.write("Ваша преобразованная матрица: \n");
            for (int i = 0; i < arr.length; i++) {
                for (int j = 0; j < arr[0].length; j++) {
                    pw.printf("%5d", arr[i][j]);
                }
                pw.write("\n");
            }
        } catch (Exception e) {
            System.err.println("Ошибка файла");
            System.out.println(e.getMessage());
        }

    }

    public static int[][] finalMatrixIn(final int MIN_NUM, boolean isInput) {
        int [][] arr;
        int n;
        String way;
        if (isInput) {
            n = dimensionIn(MIN_NUM);
            arr = fillMatrix(n);
        } else {
            way = takeFileWay(1);
            arr = takeArrFromFile(way);
        }
        return arr;
    }

    public static void finalMatrixOut(boolean isOutput, int[][] arr) {
        String way;
        if (isOutput) {
            System.out.println("Ваша матрица: ");
            writeFirstMatrix(arr);
            System.out.println("Преобразованная матрица: ");
            writeNewMatrix(arr);
        } else {
            way = takeFileWay(11);
            writeFirstMatrixF(way, arr);
            writeNewMatrixF(way, arr);
        }
    }

    public static void main(String[] args) {
        boolean isInput, isOutput;
        int arr[][];
        final int MIN_NUM = 2;
        System.out.println("Данная программа меняет подматрицы N вашей матрицы 2N местами");
        System.out.print("Введите 0, если хотите ввод на консоль, 1 — ввод из файла: ");
        isInput = consoleChoice();
        arr = finalMatrixIn(MIN_NUM, isInput);
        System.out.println("Введите 0, если хотите вывод на консоль, 1 – вывод в файл: ");
        isOutput = consoleChoice();
        finalMatrixOut(isOutput, arr);
        scan.close();
    }
}
