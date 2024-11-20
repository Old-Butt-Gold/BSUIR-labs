package com.epam.rd.autotasks.config;

import com.epam.rd.autotasks.Song;
import org.springframework.beans.factory.annotation.Value;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.PropertySource;

@PropertySource("classpath:/application.properties")
public class SongConfig {

    @Bean
    public Song song(@Value("${Title}") String title, @Value("${Artist}") String artist, @Value("${Year}") String year){
        return new Song(title, artist, year);
    }

}

