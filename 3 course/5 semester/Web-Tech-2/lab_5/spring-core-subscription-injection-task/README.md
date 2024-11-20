# Subscription Spring Injection

The goal of this task is practicing with the different Spring Bean injections.

Duration: _30 minutes_

## Description

You are provided with the four interfaces:
* Account
* Payment
* Period
* Subscription

You must declare Spring Beans for each interface and inject `Account`, `Payment`, and `Period`
into the `Subscription` bean usign different Spring Bean injection types.

For the `Account` bean you must use *constructor* injection.

For the `Payment` bean you must use *field* injection.

For the `Period` bean you must use *setter* (or *method*) injection.

In the `main` method you must create Spring Context, get `Subscription` bean from it and print
all of its fields (`Account`, `Payment`, `Period`).

## Requirements

* Spring Context is valid and correct.
* All the required beans are created.
* `Account` bean is injected into the `Subscription` bean via constructor injection.
* `Payment` bean is injected into the `Subscription` bean via field injection.
* `Period` bean is injected into the `Subscription` bean via setter injection.
* The resulting `Subscription` bean is fetched from Spring Context and printed to `System.out`
