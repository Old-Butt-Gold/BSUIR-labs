package com.epam.rd.autotasks.config;

import com.epam.rd.autotasks.MusicService;
import org.springframework.context.annotation.Bean;

public class MusicServiceConfig {

    @Bean
    public MusicService musicService() {
        return new MusicService();
    }
}
