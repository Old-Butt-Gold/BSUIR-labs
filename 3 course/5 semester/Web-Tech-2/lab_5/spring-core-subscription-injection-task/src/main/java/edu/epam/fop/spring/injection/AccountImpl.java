package edu.epam.fop.spring.injection;

public class AccountImpl implements Account {
    private final String name;

    public AccountImpl(String name) {
        this.name = name;
    }

    @Override
    public String getName() {
        return name;
    }
}
