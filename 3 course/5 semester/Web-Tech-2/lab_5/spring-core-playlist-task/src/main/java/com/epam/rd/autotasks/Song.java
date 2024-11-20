package com.epam.rd.autotasks;

import org.springframework.context.annotation.Scope;

@Scope("prototype")
public class Song {

    private String title;

    public Song() { }

    public Song(String title)
    {
        this.title=title;
    }

    public String getTitle() {
        return title;
    }

    public void setTitle(String title) {
        this.title = title;
    }
}
