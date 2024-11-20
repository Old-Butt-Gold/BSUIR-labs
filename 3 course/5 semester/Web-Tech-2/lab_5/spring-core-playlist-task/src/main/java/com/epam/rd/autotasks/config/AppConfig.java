package com.epam.rd.autotasks.config;

import com.epam.rd.autotasks.Singer;
import com.epam.rd.autotasks.Song;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Scope;

import java.util.Random;

public class AppConfig {

    private final Random _random = new Random();

    @Bean
    @Scope("singleton")
    public Singer singer(){
        return new Singer("Elton John");
    }

    @Bean
    @Scope("prototype")
    public Song song(){
        return new Song(String.valueOf(_random.nextDouble()));
    }
}
