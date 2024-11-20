# Guess the Number

The goal of this task is to create servlets and recognize the stages of a servlet's lifecycle.

Duration: *45 minutes*

## Description

Create a simple servlet to implement the Guess the Number game using a simple command pattern.

## Requirements

1) Implement a `GuessNumberController` Java class. Add the `@WebServlet("/GuessNumber")` annotation to map the servlet to a URL.

2) Implement the `init()` method to initialize the parameters for generating random numbers.
- Create `minNumber` and `maxNumber` variables for generating random numbers.
- Read the initial parameter values from the servlet initialization parameters named `minNumber` and `maxNumber`.
- If the specified parameters are missing, set the default values: `minNumber = 1` and `maxNumber = 50`.
- Add `minNumber` and `maxNumber` with the corresponding values to the `ServletContext` attributes.
- Log the servlet initialization debug message.

3) Implement the `service()` method to process GET requests.

When the value of the `command` parameter is equal to `start`, the request should be forwarded to the `start.jsp` page, adding the `name` attribute with the value username. It is necessary to generate a random number in the range `minNumber` to `maxNumber` inclusive and write it into the `randomNumber` variable. 

When the value of the `command` parameter is equal to `tryIt` consider the following cases:

- If the number of attempts `tryCount` is greater than `10`, the request should be forwarded to the `finish.jsp` page, adding the`result` attribute with the `You lose` message.

- If the entered number is equal to the `randomNumber` value, the request should be forwarded to the `finish.jsp` page, adding the `result` attribute with the message `You win!`.

- Otherwise, the request should be forwarded to the `tryIt.jsp` page, adding the `tryCount` attribute with the counter of attempts and the `result` attribute with the following message to the request object:
    - `Your number is less than the number being guessed` - if the number entered is less than the number generated
    - `Your number is greater than the number being guessed` - if the number entered is greater than the number generated
    - `Invalid value entered` - if the value entered is not a number
    - `Please enter a number` - if no value is entered

Any other request should be forwarded to the `index.html` page.

Log a debug message about handling requests from the user.

4) Implement the `destroy()` method.
Log the servlet completion debug message.

## Examples

An example of forwarding to `page.jsp` is shown below:

```
String page = "/WEB-INF/page.jsp";
request.getRequestDispatcher(page).forward(request, response);
```