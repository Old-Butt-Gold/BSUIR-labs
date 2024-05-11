import java.util.Scanner;
public class LabSecondBlockFirst {
    public static void main(String[] args) {
        Scanner scan = new Scanner(System.in);
        boolean isIncorrect;
        int n = 0;
        float sum = 0, numb = 1;
        final int MIN_NUM = 1, MAX_NUM = 24;
        System.out.println("Программа выводит на экран сумму." +
                "Диапазон значений для ввода числа N: 1..24");
        //проводится проверка ввода данных;
        do {
            System.out.println("Введите N: ");
            isIncorrect = false;
            try {
                n = Integer.parseInt(scan.nextLine());
            } catch (Exception e) {
                System.err.println("Проверьте правильность ввода данных");
                isIncorrect = true;
            }
            if (!isIncorrect && (n < MIN_NUM || n > MAX_NUM)) {
                System.err.println("Проверьте правильность ввода данных");
                isIncorrect = true;
            }
        } while (isIncorrect);
        scan.close();
        for (int i = 1; i < n + 1; ++i) {
            numb = numb * 2;
            sum += (1 / numb);
        }
        System.out.println("Сумма равна: " + sum);
    }
}