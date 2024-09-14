package com.epam.rd.autotasks;

public class DecrementingCarouselWithLimitedRun extends DecrementingCarousel{
    final int actionLimit;

    public DecrementingCarouselWithLimitedRun(final int capacity, final int actionLimit) {
        super(capacity);
        this.actionLimit = actionLimit;
    }

    @Override
    public CarouselRun run() {
        if (isRunning) {
            return null;
        }
        isRunning = true;
        return new LimitCarouselRun(elements, actionLimit);
    }
}
