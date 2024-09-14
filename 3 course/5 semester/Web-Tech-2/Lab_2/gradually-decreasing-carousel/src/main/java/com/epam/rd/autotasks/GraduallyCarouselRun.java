package com.epam.rd.autotasks;

import java.lang.reflect.Array;
import java.util.Arrays;

public class GraduallyCarouselRun extends CarouselRun {
    public GraduallyCarouselRun(int[] elements) {
        super(elements);
        indexDecrementing = new int[elements.length];
        Arrays.fill(indexDecrementing, 1);
    }

    private int[] indexDecrementing;

    @Override
    protected int getElement() {
        int result = elements[currentIndex];
        elements[currentIndex] -= indexDecrementing[currentIndex];
        indexDecrementing[currentIndex]++;
        currentIndex = (currentIndex + 1) % elements.length;
        return result;
    }
}
