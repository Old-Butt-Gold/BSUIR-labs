import java.util.Scanner;
public class LabSecondBlockSecond {
    static Scanner scan = new Scanner(System.in);
    public static int numberIn(final int MIN_NUM){
        int numb = 0;
        boolean isIncorrect;
        do {
            isIncorrect = false;
            try {
                numb = Integer.parseInt(scan.nextLine());
            } catch(Exception E) {
                System.out.print("Ошибка, Введите целочисленное число! \n Повторите ввод натуральной переменной: ");
                isIncorrect = true;
            }
            if (!isIncorrect && (numb < MIN_NUM )){
                System.out.print("Введите натуральное число, большее 1: ");
                isIncorrect = true;
            }
        } while (isIncorrect);
        return numb;
    }
    public static boolean[] zanulenie(boolean[] A, int n) {
        for (int i = 1; i <= n; i++) {
            A[i] = false;
        }
        return A;
    }
    public static boolean[] findProst(boolean[] A, int n) {
        int j, i = 2;
        A = zanulenie(A, n);
        while (i * i <= n) {
            j = i * i;
            while (j <= n) {
                A[j] = true;
                j += i;
            }
            do {
                i++;
            } while (A[i]);
        }
        return A;
    }
    public static void writeProst(boolean[] A, int n) {
        int i = 2;
        while (i <= n) {
            if (!A[i]) {
                System.out.println(i);
            }
            i++;
        }
    }
    public static void main(String[] args) {
        final int MIN_NUM = 2;
        boolean[] arr;
        int p;
        System.out.println("Данная программа находит все простые числа, не превосходящие P");
        System.out.print("Введите натуральное число P: ");
        p = numberIn(MIN_NUM);
        scan.close();
        arr = new boolean[p+1];
        arr = findProst(arr, p);
        System.out.println("Числа, которые подходят по условию: ");
        writeProst(arr, p);
    }
}