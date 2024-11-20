package edu.epam.fop.spring.injection;

import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.ComponentScan;
import org.springframework.context.annotation.Configuration;

import java.time.Duration;
import java.time.LocalDate;

@Configuration
@ComponentScan("edu.epam.fop.spring.injection")
public class AppConfig {

    @Bean
    public Account account() {
        return new AccountImpl("John Doe");
    }

    @Bean
    public Payment payment() {
        return new PaymentImpl(1000, "Credit Card");
    }

    @Bean
    public Period period() {
        return new PeriodImpl(Duration.ofDays(30), LocalDate.now().plusMonths(1));
    }
}
