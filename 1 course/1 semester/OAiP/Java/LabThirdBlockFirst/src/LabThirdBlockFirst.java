import java.util.Scanner;

public class LabThirdBlockFirst {
    public static void main(String[] args) {
        Scanner scan = new Scanner(System.in);
        boolean isIncorrect;
        float a = 0, h = 0;
        System.out.println("Изменяя x от a с шагом h, определить, при каком значении x, SIN(x) станет больше COS(x).");
                //проводится проверка ввода данных;
        do {
            System.out.println("Введите A — начальное значение X: ");
            isIncorrect = false;
            try {
                a = Float.parseFloat(scan.nextLine());
            } catch (Exception e) {
                System.err.println("Проверьте правильность ввода данных");
                isIncorrect = true;
            }
        } while (isIncorrect);

        do {
            System.out.println("Введите шаг изменения X — H: ");
            isIncorrect = false;
            try {
                h = Float.parseFloat(scan.nextLine());
            } catch (Exception e) {
                System.out.println("Проверьте правильность ввода");
                isIncorrect = true;
            }
        } while(isIncorrect);
        scan.close();
        while (!(Math.sin(a) > Math.cos(a))) {
            a += h;
        }
        System.out.println("sin(x) больше cos(x) при x, равном: " + a + "\n" +
                "sin(x) = " + Math.sin(a) + "; cos(x) = " + Math.cos(a));
    }
}
