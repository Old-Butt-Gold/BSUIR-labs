package edu.epam.fop.spring.injection;

import java.time.Duration;
import java.time.LocalDate;

public class PeriodImpl implements Period {
    private final Duration paymentPeriod;
    private final LocalDate endDate;

    public PeriodImpl(Duration paymentPeriod, LocalDate endDate) {
        this.paymentPeriod = paymentPeriod;
        this.endDate = endDate;
    }

    @Override
    public Duration paymentPeriod() {
        return paymentPeriod;
    }

    @Override
    public LocalDate endDate() {
        return endDate;
    }
}
