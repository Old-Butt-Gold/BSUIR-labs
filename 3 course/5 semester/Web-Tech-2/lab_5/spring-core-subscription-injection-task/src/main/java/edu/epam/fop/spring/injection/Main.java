package edu.epam.fop.spring.injection;

import org.springframework.context.ApplicationContext;
import org.springframework.context.annotation.AnnotationConfigApplicationContext;

public class Main {
    public static void main(String[] args) {
        ApplicationContext context = new AnnotationConfigApplicationContext(AppConfig.class);
        Subscription subscription = context.getBean(Subscription.class);
        System.out.println(subscription);
    }
}
