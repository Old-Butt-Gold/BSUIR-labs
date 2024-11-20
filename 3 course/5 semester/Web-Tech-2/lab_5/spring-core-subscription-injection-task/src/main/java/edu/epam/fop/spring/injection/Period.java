package edu.epam.fop.spring.injection;

import java.time.Duration;
import java.time.LocalDate;

public interface Period {

    Duration paymentPeriod();

    LocalDate endDate();
}
