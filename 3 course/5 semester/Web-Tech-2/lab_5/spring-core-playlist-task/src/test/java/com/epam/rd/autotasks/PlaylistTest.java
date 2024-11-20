package com.epam.rd.autotasks;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertNotEquals;
import static org.junit.jupiter.api.Assertions.assertNotNull;
import static org.junit.jupiter.api.Assertions.assertTrue;

import com.epam.rd.autotasks.config.AppConfig;
import org.junit.jupiter.api.Test;
import org.springframework.context.annotation.AnnotationConfigApplicationContext;

class PlaylistTest {

    @Test
    void testSingerConfiguration() {
        try (AnnotationConfigApplicationContext context = new AnnotationConfigApplicationContext(AppConfig.class)) {

            Singer singer = context.getBean(Singer.class);
            assertNotNull(singer, "Singer should not be null");
            assertEquals("Elton John", singer.getName(), "Singer name should be \"Elton John\"");
            assertEquals("singleton", context.getBeanFactory().getBeanDefinition("singer").getScope(),
                    "Singer scope should be \"singleton\"");
        }
    }

    @Test
    void testSongConfiguration() {
        try (AnnotationConfigApplicationContext context = new AnnotationConfigApplicationContext(AppConfig.class)) {

            Song first_song = context.getBean(Song.class);
            Song second_song = context.getBean(Song.class);
            assertNotNull(first_song, "Song should not be null");
            assertNotNull(second_song, "Song should not be null");
            assertNotEquals(first_song, second_song, "Song objects should be different");
            assertNotEquals(first_song.getTitle(), second_song.getTitle(), "Song titles should be different");
            assertEquals("prototype", context.getBeanFactory().getBeanDefinition("song").getScope(),
                    "Song scope should be \"prototype\"");
        }
    }
}
