package com.epam.rd.autotasks;

class CycleSwap {
    static void cycleSwap(int[] array) {
        cycleSwap(array, 1);
    }

    static void cycleSwap(int[] array, int shift) {
        if (array.length == 0) return;

        shift %= array.length;

        if (shift == 0) return;

        for (int i = 0; i < array.length / 2; i++)
        {
            int temp = array[i];
            array[i] = array[array.length - i - 1];
            array[array.length - i - 1] = temp;
        }

        for (int i = 0; i < shift / 2; i++)
        {
            int temp = array[i];
            array[i] = array[shift - i - 1];
            array[shift - i - 1] = temp;
        }

        for (int i = shift; i < (array.length + shift) / 2; i++)
        {
            int temp = array[i];
            array[i] = array[array.length - i + shift - 1];
            array[array.length - i + shift - 1] = temp;
        }
    }
}
