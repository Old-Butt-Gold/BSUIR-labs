package com.epam.rd.autotasks.config;

import com.epam.rd.autotasks.Owner;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

@Configuration
public class OwnerConfig {
    @Bean
    public Owner owner() {
        Owner owner = new Owner();
        owner.setName("John Doe");
        owner.setTaxNumber("19671223-0000");
        return owner;
    }
}
