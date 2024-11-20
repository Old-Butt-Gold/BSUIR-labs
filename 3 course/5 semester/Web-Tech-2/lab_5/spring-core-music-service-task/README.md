# Music Service

The goal of this task is to practice with `@PostConstruct` and `@PreDestroy` annotations.

Duration: _10 minutes_

## Description

1. Create a `MusicService` class with a `List` attribute _songs_. The class should have an appropriate getter method for its attribute.
2. Create a `Song` class with the following attributes: _title_, _artist_ and _year_. The class should have appropriate getter and setter methods for its attributes.
3. Add an `init` method with a `@PostConstruct` annotation to the `MusicService` class that adds 3 songs to the service list.
4. Add a `destroy` method with a `@PreDestroy` annotation to the `MusicService` class that writes a message to the console that `MusicService` is shutting down.
5. Use a Spring configuration class `MusicServiceConfig` to define and manage the `MusicService` bean.
