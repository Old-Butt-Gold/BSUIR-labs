package com.epam.rd.autotasks;

public class HalvingCarousel extends DecrementingCarousel {

    public HalvingCarousel(final int capacity) {
        super(capacity);
    }

    @Override
    public CarouselRun run() {
        if (isRunning) {
            return null;
        }
        isRunning = true;
        return new HalvingCarouselRun(elements);
    }
}
