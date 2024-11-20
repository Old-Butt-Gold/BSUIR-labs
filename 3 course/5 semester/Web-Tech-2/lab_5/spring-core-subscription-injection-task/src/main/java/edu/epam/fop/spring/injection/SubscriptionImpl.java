package edu.epam.fop.spring.injection;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Component;

@Component
public class SubscriptionImpl implements Subscription {
    private final Account account;  // Constructor Injection
    private Payment payment;        // Field Injection
    private Period period;          // Setter Injection

    @Autowired
    public SubscriptionImpl(Account account) {
        this.account = account;
    }

    @Autowired
    private void setPayment(Payment payment) {
        this.payment = payment;
    }

    @Autowired
    @Override
    public void setPeriod(Period period) {
        this.period = period;
    }

    @Override
    public Account getUser() {
        return account;
    }

    @Override
    public Payment getPayment() {
        return payment;
    }

    @Override
    public Period getPeriod() {
        return period;
    }

    @Override
    public String toString() {
        return "SubscriptionImpl{" +
                "account=" + account.getName() +
                ", payment=" + payment.getAmount() + " " + payment.getType() +
                ", period=" + period.paymentPeriod() + " until " + period.endDate() +
                '}';
    }
}
