package com.epam.rd.autotasks;

public class CarouselRun {

    private final int[] elements;
    private int currentIndex = 0;

    public CarouselRun(int[] elements) {
        this.elements = elements;
    }

    public int next() {
        if (isFinished()) {
            return -1;
        }

        while (elements[currentIndex] <= 0) {
            currentIndex = (currentIndex + 1) % elements.length;
        }

        int result = elements[currentIndex];
        elements[currentIndex]--;
        currentIndex = (currentIndex + 1) % elements.length;
        return result;
    }

    public boolean isFinished() {
        for (int element : elements) {
            if (element > 0) {
                return false;
            }
        }
        return true;
    }
}
