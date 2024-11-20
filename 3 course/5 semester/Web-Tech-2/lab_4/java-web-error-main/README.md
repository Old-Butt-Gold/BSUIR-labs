# Handling Errors in an Online Library

The goal of this task is to give you some practice handling errors in Jakarta Servlets and JSP.

Duration: *45 minutes*

## Description

Provide error handling for an online library management system. The system should include functionality that allows users to log in and search for books. The main goal is to handle all kinds of exceptions and return appropriate HTTP status codes to make the system robust and user-friendly.

## Requirements

1) Users should be able to log in by providing details such as username and password using the `login.jsp` page. Implement the `LoginServlet.doPost` method. It takes the `username` and `password` as request parameters, checks them for validity, and, if they are valid (not null and not empty), creates a new session for the user, defines a username as the session attribute `user`, and redirects the response to the `home.jsp` page. If the username or password is invalid, an `IllegalArgumentException` is thrown, captured, and forwarded to the `login.jsp` page to be displayed to the user. Use the request attribute `errorMessage` to specify the appropriate error message (e.g., "Invalid username or password. Try again.").

2) Users should be able to search for books by ISBN. Implement the `BookSearchServlet.doGet` method, which takes inputs from the search form (`home.jsp`) and forwards a request to the `result.jsp` page. Use the request attribute `result` to specify the appropriate message (e.g., "Book search result. ISBN: 1234567890123"). If the user tries to search using an invalid ISBN (i.e., it does not match the regular expression "[0-9]{13}"), throw a custom `InvalidISBNException` with the appropriate error message (e.g., "The ISBN entered is not valid. Please enter exactly 13 digits.").

3) Implement the `FinesServlet.doGet` method, which defines the request attribute `result` to specify the appropriate message (e.g., "View fines result. User: John") and forwards a request to the `result.jsp` page. If the user tries to view fines without being logged in (check the session attribute `user`), the custom `UserNotLoggedInException` should be thrown with the appropriate error message (e.g., "You need to be logged in to view or pay fines."), and a `403` status code should be returned using the `HttpServletResponse.sendError(int sc, String msg)` method.

4) Check the configuration of the deployment descriptor (`web.xml`):
   - Any servlet execution that throws the `InvalidISBNException` should redirect the user to the `error.jsp` page.
   - For HTTP `404` error, the user should be redirected to the `error.jsp` page.
   - For HTTP `403` error, the user should be redirected to the `login.jsp` page.

## Examples

An example of configuring a web application to display the `error.jsp` page when the `InvalidISBNException` occurs is shown below:

```java
<error-page>
    <exception-type>edu.epam.fop.web.exceptions.InvalidISBNException</exception-type>
    <location>/error.jsp</location>
</error-page>
```
