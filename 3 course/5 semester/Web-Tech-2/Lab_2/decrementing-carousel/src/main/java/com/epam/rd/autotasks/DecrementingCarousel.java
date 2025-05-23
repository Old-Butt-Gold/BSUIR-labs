package com.epam.rd.autotasks;

public class DecrementingCarousel {

    private final int[] elements;
    private int currentIndex = 0;
    private boolean isRunning = false;

    public DecrementingCarousel(int capacity) {
        elements = new int[capacity];
    }

    public boolean addElement(int element) {
        if (isRunning || currentIndex >= elements.length || element <= 0) {
            return false;
        }
        elements[currentIndex++] = element;
        return true;
    }

    public CarouselRun run() {
        if (isRunning) {
            return null;
        }
        isRunning = true;
        return new CarouselRun(elements);
    }
}
