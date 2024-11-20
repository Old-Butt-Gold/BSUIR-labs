# Task

The goal of this task is to practice with a _@Qualifier_ annotation.

Duration: _15 minutes_

## Description

1. Create a `Employee` class with the following attributes: _name_ and _position_. The class should have appropriate getter and setter methods for its attributes.
2. Create a `Task` class with the following attributes: _description_, _assignee_ and _reviewer_. The class should have appropriate getter and setter methods for its attributes.
3. Use a Spring configuration class `AppConfig` to define and manage beans for the `Task`, and `Employee` classes:
    - `Task` bean must have a description "New feature".
    - `Assignee` bean of a type `Employee` must have a name "John Doe" and a position "Junior Software Engineer".
    - `Reviewer` bean of a type `Employee` must have a name "Emily Brown" and a position "Senior Software Engineer".
4. Follow best practices for Spring Dependency Injection to inject `Assignee` and `Reviewer` beans into the `Task` class.
