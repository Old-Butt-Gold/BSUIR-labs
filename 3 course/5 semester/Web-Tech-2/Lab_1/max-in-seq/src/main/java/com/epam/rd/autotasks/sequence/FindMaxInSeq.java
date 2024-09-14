package com.epam.rd.autotasks.sequence;
import java.util.Scanner;

public class FindMaxInSeq {
    public static int max() {

        // Put your code here
        Scanner scanner = new Scanner(System.in);
        if (scanner.hasNext()) {
            int max = scanner.nextInt();
            int num = scanner.nextInt();
            while (num != 0) {
                max = num > max ? num : max;
                num = scanner.nextInt();
            }

            return max;
        }

        return 0;
    }

    public static void main(String[] args) {

        System.out.println("Test your code here!\n");

        // Get a result of your code

        System.out.println(max());
    }
}
