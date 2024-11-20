# Playlist

The goal of this task is to practice with different Spring bean scopes.

Duration: _10 minutes_

## Description

1. Create a `Singer` class with a scope _singleton_ and an attribute _name_. The class should have appropriate getter and setter methods for its attribute.
2. Create a `Song` class with a scope _prototype_ and an attribute _title_. The class should have appropriate getter and setter methods for its attribute.
3. Follow best practices for Spring Dependency Injection by using constructor injection to inject dependencies (`Owner`) into the `Car` class.
4. Use a Spring configuration class `AppConfig` to define and manage beans for the `Singer`, and `Song` classes:
    - `Singer` bean must have a name "Elton John".
    - `Song` beans should have different names, e.g. using randomiser.
