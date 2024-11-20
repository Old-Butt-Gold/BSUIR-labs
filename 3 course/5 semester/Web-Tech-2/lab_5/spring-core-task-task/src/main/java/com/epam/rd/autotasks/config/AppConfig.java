package com.epam.rd.autotasks.config;

import com.epam.rd.autotasks.Employee;
import com.epam.rd.autotasks.Task;
import org.springframework.beans.factory.annotation.Qualifier;
import org.springframework.context.annotation.Bean;

public class AppConfig {

    @Bean
    @Qualifier("assignee")
    public Employee assignee() {
        return new Employee("John Doe", "Junior Software Engineer");
    }

    @Bean
    @Qualifier("reviewer")
    public Employee reviewer() {
        return new Employee("Emily Brown", "Senior Software Engineer");
    }

    @Bean
    public Task task() {
        return new Task("New feature");
    }
}
