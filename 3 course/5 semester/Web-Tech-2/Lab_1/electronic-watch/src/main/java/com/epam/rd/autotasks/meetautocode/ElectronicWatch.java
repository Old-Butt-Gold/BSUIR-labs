package com.epam.rd.autotasks.meetautocode;

import java.util.Scanner;

public class ElectronicWatch {
    public static void main(String[] args) {
        Scanner scanner = new Scanner(System.in);
        int seconds = scanner.nextInt();

        seconds %= 60 * 60 * 24;

        int hours = seconds / 3600;

        seconds %= 3600;

        int minutes = seconds / 60;

        seconds %= 60;

        System.out.printf("%01d:%02d:%02d", hours, minutes, seconds);
    }
}
