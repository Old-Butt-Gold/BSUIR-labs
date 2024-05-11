import java.util.Scanner;
public class Program {
    public static void main(String[] args) {
        Scanner scan = new Scanner(System.in);
        boolean isIncorrect;
        float abscissa = 0, ordinate = 0;
        System.out.println("Даны координаты точки М(х,у). Определите, принадлежит " +
                           "ли данная точка замкнутому множеству D.");
        //проводится проверка ввода данных;
        do {
            System.out.println("Введите ось абсцисс: ");
            isIncorrect = false;
            try {
                abscissa = Float.parseFloat(scan.nextLine());
            } catch (Exception e) {
                System.err.println("Проверьте правильность ввода данных");
                isIncorrect = true;
            }
        } while (isIncorrect);

        do {
            System.out.println("Введите ось ординат: ");
            isIncorrect = false;
            try {
                ordinate = Float.parseFloat(scan.nextLine());
            } catch (Exception e) {
                System.err.println("Проверьте правильность ввода данных");
                isIncorrect = true;
            }
        } while (isIncorrect);
        scan.close();

        if ((ordinate > -abscissa/2 + 1) || (ordinate < 0)  || (abscissa > 2) || (abscissa < 0))
            System.out.println("Точка M не принадлежит замнкнутому множеству D");
        else
            System.out.println("Точка M принадлежит замнкнутому множеству D");
    }
}