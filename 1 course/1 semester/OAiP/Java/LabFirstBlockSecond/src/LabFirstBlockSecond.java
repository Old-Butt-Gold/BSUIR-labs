import java.util.Scanner;
public class LabFirstBlockSecond {
    public static void main(String[] args) {
    Scanner scan = new Scanner(System.in);
    boolean isIncorrect, isTrue;
    int n = 0, k = 1;
    float[] abscisses, ordinates;
    float crossLine1, crossLine2;
    final int MIN_NUM = 3;
    do {
        System.out.println("Введите число сторон многоугольника N, N > 2: ");
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
    } while(isIncorrect);
    abscisses = new float[n + 3];
    ordinates = new float[n + 3];
        for (int i = 1; i <= n; ++i) {
            do {
                isIncorrect = false;
                System.out.print("Abscissa[" + i + "]: ");
                try {
                    abscisses[i] = Float.parseFloat(scan.nextLine());
                } catch (Exception e) {
                    System.err.println("Проверьте правильность ввода данных");
                            isIncorrect = true;
                }
            } while(isIncorrect);
        }
        for (int i = 1; i <= n; ++i) {
            do {
                isIncorrect = false;
                System.out.print("Ordinate[" + i + "]: ");
                try {
                    ordinates[i] = Float.parseFloat(scan.nextLine());
                } catch (Exception e) {
                    System.err.println("Проверьте правильность ввода данных");
                    isIncorrect = true;
                }
            } while (isIncorrect);
        }
        scan.close();
        isTrue = true;
        abscisses[0] = abscisses[n];
        ordinates[0] = abscisses[n];
        abscisses[n + 1] = abscisses[1];
        abscisses[n + 2] = abscisses[2];
        ordinates[n + 1] = ordinates[1];
        ordinates[n + 2] = ordinates[2];
        while (k <= n && isTrue)
        {
            crossLine1 = (abscisses[k - 1] - abscisses[k]) * (ordinates[k + 1] - ordinates[k]) - (ordinates[k - 1] - ordinates[k]) * (abscisses[k + 1] - abscisses[k]);
            crossLine2 = (abscisses[k] - abscisses[k + 1]) * (ordinates[k + 2] - ordinates[k + 1]) - (ordinates[k] - ordinates[k + 1]) * (abscisses[k + 2] - abscisses[k + 1]);
            if (crossLine1 * crossLine2 < 0)
                isTrue = false;
            else
                k++;
        }
        if (isTrue)
            System.out.println("Выпуклый");
        else
            System.out.println("Не выпуклый");
    }
}
