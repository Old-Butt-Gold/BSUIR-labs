# Garage

The goal of this task is to create a simple garage system using Spring Dependency Injection.

Duration: _20 minutes_

## Description

1. Create a `Car` class to represent a car in the garage with the following attributes: _model_, _year_ and _owner_. The class should have appropriate getter and setter methods for its attributes.
2. Create a `Owner` class to represent the owner of a car in the garage with the following attributes: _name_, and _taxNumber_. The class should have appropriate getter and setter methods for its attributes.
3. Follow best practices for Spring Dependency Injection by using constructor injection to inject dependencies (`Owner`) into the `Car` class.
4. Use Spring configuration classes to define and manage beans for the `Car`, and `Owner` classes. Utilize a separate configuration class for each component (i.e., `CarConfig` and `OwnerConfig`):
    - `CarConfig` creates a car of model "Tesla Model X" and year "2022".
    - `OwnerConfig` creates an owner named "John Doe" and with a tax number "19671223-0000".
