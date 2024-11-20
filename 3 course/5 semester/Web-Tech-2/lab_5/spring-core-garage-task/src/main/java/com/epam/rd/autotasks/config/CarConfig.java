package com.epam.rd.autotasks.config;

import com.epam.rd.autotasks.Car;
import com.epam.rd.autotasks.Owner;
import org.springframework.context.annotation.Bean;

public class CarConfig {

    @Bean
    public Car car() {
        Car car = new Car();
        car.setModel("Tesla Model X");
        car.setYear("2022");
        return car;
    }

    @Bean
    public Owner owner() {
        Owner owner = new Owner();
        owner.setName("John Doe");
        owner.setTaxNumber("19671223-0000");
        return owner;
    }
}
