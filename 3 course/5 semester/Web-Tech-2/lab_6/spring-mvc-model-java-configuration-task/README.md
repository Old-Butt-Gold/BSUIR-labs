# Spring MVC Java configuration

This assignment provides practical experience in handling form data in Spring MVC using Thymeleaf for view rendering. It will enhance your understanding of model attributes, data binding, and server-side template rendering in a web application context.

Duration: _2 hour_

## Description
Create a Spring MVC application using web.xml configuration to learn about handling forms. The task involves building a form with various field types and processing the form data on submission. Thymeleaf will be used as the ViewResolver.

## Requirements:
* Create a class that implements `WebApplicationInitializer` interface to register `Spring DispatcherServlet` and servlet mapping.
* Develop a class with Spring `@Controller` annotation.
* Create two methods: one for displaying the form `@GetMapping("/form")` and another for processing the form data `@PostMapping("/processForm")`.
