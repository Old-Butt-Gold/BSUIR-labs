package com.epam.rd.autotasks;

public class DecrementingCarousel
{
    protected final int[] elements;
    protected int currentIndex = 0;
    protected boolean isRunning = false;

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
