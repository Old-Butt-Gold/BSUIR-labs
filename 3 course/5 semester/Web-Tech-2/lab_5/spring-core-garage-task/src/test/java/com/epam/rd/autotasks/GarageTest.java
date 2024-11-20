package com.epam.rd.autotasks;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertNotNull;

import com.epam.rd.autotasks.config.CarConfig;
import org.junit.jupiter.api.Test;
import org.springframework.context.annotation.AnnotationConfigApplicationContext;

class GarageTest {

        @Test
        public void testCarConfiguration() {
            try (AnnotationConfigApplicationContext context = new AnnotationConfigApplicationContext(CarConfig.class)) {
                Car car = context.getBean(Car.class);
                assertNotNull(car, "Car should not be null");
                assertEquals("Tesla Model X", car.getModel(), "Car model should be \"Tesla Model X\"");
                assertEquals("2022", car.getYear(), "Car year should be \"2022\"");
                assertNotNull(car.getOwner(), "Car model should not be null");
            }
        }

        @Test
        public void testOwnerConfiguration() {
            try (AnnotationConfigApplicationContext context = new AnnotationConfigApplicationContext(CarConfig.class)) {
                Owner owner = context.getBean(Owner.class);
                assertNotNull(owner, "Owner should not be null");
                assertEquals("John Doe", owner.getName(), "Owner name should be \"John Doe\"");
                assertEquals("19671223-0000", owner.getTaxNumber(), "Owner tax number should be \"19671223-0000\"");
            }
        }
}
