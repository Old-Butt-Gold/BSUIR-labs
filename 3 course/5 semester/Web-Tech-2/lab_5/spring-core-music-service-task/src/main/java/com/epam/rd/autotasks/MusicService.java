package com.epam.rd.autotasks;

import org.springframework.stereotype.Service;

import javax.annotation.PostConstruct;
import javax.annotation.PreDestroy;
import java.util.ArrayList;
import java.util.List;

@Service
public class MusicService {
    private List<Song> songs;

    public MusicService() {
        songs = new ArrayList<>();
    }

    public List<Song> getSongs() {
        return songs;
    }

    @PostConstruct
    public void init() {
        songs.add(new Song("Bohemian Rhapsody", "Queen", 1975));
        songs.add(new Song("Imagine", "John Lennon", 1971));
        songs.add(new Song("Billie Jean", "Michael Jackson", 1982));
    }

    @PreDestroy
    public void destroy() {
        System.out.println("MusicService is shutting down.");
    }
}
