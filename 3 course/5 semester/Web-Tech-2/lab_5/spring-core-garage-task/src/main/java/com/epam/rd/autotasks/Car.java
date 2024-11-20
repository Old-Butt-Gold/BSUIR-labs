package com.epam.rd.autotasks;

import org.springframework.beans.factory.annotation.Autowired;

public class Car {
    private String model;
    private String year;

    private Owner owner;

    public Car() { }

    public String getModel() {
        return model;
    }

    public String getYear() {
        return year;
    }

    public Owner getOwner() {
        return owner;
    }

    public void setModel(String model) {
        this.model = model;
    }

    public void setYear(String year) {
        this.year = year;
    }

    @Autowired
    public void setOwner(Owner owner) {
        this.owner = owner;
    }
}
