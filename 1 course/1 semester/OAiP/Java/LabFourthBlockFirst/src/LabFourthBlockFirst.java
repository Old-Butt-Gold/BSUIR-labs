import java.util.Scanner;
public class LabFourthBlockFirst {
    public static void main(String[] args) {
        Scanner scan = new Scanner(System.in);
        int n = 0;
        float perimetr = 0;
        float[] abscisses, ordinates;
        boolean isIncorrect;
        final int MIN_NUM = 3;
        do {
            System.out.print("Введите число сторон многоугольника N, N > 2: ");
            isIncorrect = false;
            try {
                n = Integer.parseInt(scan.nextLine());
            } catch (Exception e) {
                System.err.println("Проверьте правильность ввода данных");
                isIncorrect = true;
            }
            if (!isIncorrect && n < MIN_NUM) {
                System.err.println("Проверьте правильность ввода данных в нужном диапазоне");
                isIncorrect = true;
            }
        } while (isIncorrect);
        abscisses = new float[n];
        ordinates = new float[n];
        for (int i = 0; i < n; ++i) {
            do {
                isIncorrect = false;
                System.out.println("Abscissa[" + (i + 1) + "]:");
                try {
                    abscisses[i] = Float.parseFloat(scan.nextLine());
                } catch (Exception e) {
                    System.err.println("Проверьте правильность ввода данных в нужном диапазоне");
                    isIncorrect = true;
                }
            } while(isIncorrect);
        }
        for (int i = 0; i < n; ++i) {
            do {
                isIncorrect = false;
                System.out.println("Ordinate[" + (i + 1) + "]:");
                try {
                    ordinates[i] = Float.parseFloat(scan.nextLine());
                } catch (Exception e) {
                    System.err.println("Проверьте правильность ввода данных");
                    isIncorrect = true;
                }
            } while (isIncorrect);
        }
        scan.close();
        for (int i = 0; i < n - 1; ++i) {
            perimetr += Math.sqrt((abscisses[i] - abscisses[i + 1]) * (abscisses[i] - abscisses[i + 1]) + (ordinates[i] - ordinates[i + 1]) * (ordinates[i] - ordinates[i + 1]));
        }
            perimetr += Math.sqrt((abscisses[0] - abscisses[n - 1]) * (abscisses[0] - abscisses[n - 1]) + (ordinates[0] - ordinates[n - 1]) * (ordinates[0] - ordinates[n - 1]));
            System.out.println("Периметр равен: " + perimetr);
    }
}


