package com.epam.rd.autotasks;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertNotNull;

import com.epam.rd.autotasks.config.MusicServiceConfig;
import org.junit.jupiter.api.Test;
import org.springframework.context.annotation.AnnotationConfigApplicationContext;

class MusicServiceTest {

    @Test
    void testMusicServiceConfiguration() {
        try (AnnotationConfigApplicationContext context = new AnnotationConfigApplicationContext(MusicServiceConfig.class)) {

            MusicService musicService = context.getBean(MusicService.class);

            assertNotNull(musicService, "MusicService must not be null");
            assertEquals(3, musicService.getSongs().size(), "Song list must contain 3 songs");
        }
    }
}
