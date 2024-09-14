package com.epam.rd.autotasks;

public class HalvingCarouselRun extends CarouselRun {
    public HalvingCarouselRun(int[] elements) {
        super(elements);
    }

    @Override
    protected int getElement() {
        int result = elements[currentIndex];
        elements[currentIndex] /= 2;
        currentIndex = (currentIndex + 1) % elements.length;
        return result;
    }
}
