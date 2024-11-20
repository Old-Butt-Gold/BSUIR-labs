package com.epam.rd.autotasks;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertNotNull;

import com.epam.rd.autotasks.config.SongConfig;
import org.junit.jupiter.api.Test;
import org.springframework.context.annotation.AnnotationConfigApplicationContext;

class SongTest {

    @Test
    void testSongConfiguration() {
        try (AnnotationConfigApplicationContext context = new AnnotationConfigApplicationContext(SongConfig.class)) {

            Song song = context.getBean(Song.class);
            assertNotNull(song, "Song should not be null");
            assertEquals("Only the young", song.getTitle(), "Song title should be \"Only the young\"");
            assertEquals("Taylor Swift", song.getArtist(), "Song artist should be \"Taylor Swift\"");
            assertEquals("2020", song.getYear(), "Song year should be \"2020\"");
        }
    }
}
