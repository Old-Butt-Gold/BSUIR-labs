package edu.epam.fop.spring.injection;

import org.springframework.context.annotation.Bean;

public class PaymentImpl implements Payment {
    private final long amount;
    private final String type;

    public PaymentImpl(long amount, String type) {
        this.amount = amount;
        this.type = type;
    }

    @Override
    public long getAmount() {
        return amount;
    }

    @Override
    public String getType() {
        return type;
    }
}
