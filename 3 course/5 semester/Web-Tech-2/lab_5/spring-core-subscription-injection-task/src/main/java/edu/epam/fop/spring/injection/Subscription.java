package edu.epam.fop.spring.injection;

public interface Subscription {

    Account getUser();

    Payment getPayment();

    Period getPeriod();

    void setPeriod(Period period);
}
