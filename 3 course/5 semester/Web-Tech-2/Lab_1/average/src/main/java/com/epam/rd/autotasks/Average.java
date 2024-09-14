package com.epam.rd.autotasks;

import java.util.Scanner;

public class Average {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        // Use Scanner methods to read input

        if (scanner.hasNext()) {
            int avg = 0;
            int counter = 0;
            while (scanner.hasNext()) {
                avg += scanner.nextInt();
                counter++;
            }
            counter--; //remove one last zero
            System.out.println(avg / counter);
        } else {
            System.out.println(0);
        }

    }

}