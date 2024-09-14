package com.epam.rd.autotasks;

public class LimitCarouselRun extends CarouselRun {
    private int remainingCalls;

    public LimitCarouselRun(int[] elements, int actionLimit) {
        super(elements);
        this.remainingCalls = actionLimit;
    }

    @Override
    public int next() {
        if (isFinished() || remainingCalls == 0) {
            return -1;
        }

        while (elements[currentIndex] <= 0) {
            currentIndex = (currentIndex + 1) % elements.length;
        }

        int item = getElement();
        remainingCalls--;
        return item;
    }

    @Override
    public boolean isFinished() {
        return super.isFinished() || remainingCalls == 0;
    }
}
