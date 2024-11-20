# Guess the Number (Cookies)

The goal of this task is to give you some practice storing data in cookies, retrieving it, and managing cookies. 

*Duration*: 60 minutes.

## Description

Create a simple servlet to implement the game Guess the Number, saving user data in cookies.

## Requirements

1) Develop a Java class called `GuessNumberCookieController`.

2) Implement the `init()` method to initialize the parameters for generating random numbers.
- Create `minNumber` and `maxNumber` variables as parameters for generating random numbers.
- Read the initial parameter values from the servlet initialization parameters named `minNumber` and `maxNumber`.
- If the specified parameters are missing, set the default values: `minNumber = 1` and `maxNumber = 50`.
- Add `minNumber` and `maxNumber` with the corresponding values to the `ServletContext` attributes.
- Log the servlet initialization debug message.

3) Implement the `service()` method to process GET requests.
The servlet should retrieve all `Cookies` from the request.

When the value of the `command` parameter is equal to `start`, the request should be forwarded to the `start.jsp` page, adding the `name` attribute with the entered username. The value of the `name` attribute is used to conveniently enter the username by setting the `value` attribute on the form at the `start.jsp` page. The username should be stored in a `Cookie` called `name`. It is necessary to generate a random number in the range from `minNumber` to `maxNumber` inclusive and store it in a `Сookie` named `randomNumber`.

When the value of the `command` parameter is equal to `tryIt`, consider the following cases:
   - If the number of attempts `tryCount` is greater than `10`, the request should be forwarded to the `finish.jsp` page, adding the `result` attribute with the `You lose` message.
   - If the number of attempts `tryCount` is less than `10` and the entered number equals the `randomNumber` value, the request should be forwarded to the `finish.jsp` page, adding the `result` attribute with the message `You win!`. 
- Otherwise, the request should be forwarded to the `tryIt.jsp` page, adding the `tryCount` attribute with the number of attempts (which should be stored in a `Cookie` named `tryIt`) and the `result` attribute with the following message to the request object:
      - `Your number is less than the number being guessed` - if the number entered is less than the number generated
      - `Your number is greater than the number being guessed` - if the number entered is greater than the number generated
      - `Invalid value entered` - if the value entered is not a number
      - `Please enter a number` - if no value is entered

Any other request should be forwarded to the `index.jsp` page.

Log a debug message about handling requests from the user.

4) Run the application on the server (e.g., Apache Tomcat).
Open a web browser and enter the address `http://localhost:8080/name-of-your-project/`. Using your browser's Dev Tools (press F12), verify that the appropriate cookies are being sent with the HTTP response and received with the HTTP request.

5) Check the info and debug messages to understand how cookies work.

## Examples

An example of reading a `Cookie` is shown below:

```
Cookie[] cookies = request.getCookies(); 
for (Cookie cookie : cookies) { 
    if (cookie.getName().equals(name)) { 
        ...		// required cookie found 
    } 
} 
```

An example of creating a `Cookie` is shown below:

```
Cookie nameCookie = new Cookie("name", name);
```

An example of deleting a `Cookie` is shown below:

```
tryCountCookie.setMaxAge(0);
```

An example of sending a `Cookie` is shown below:

```
response.addCookie(randomNumberCookie);
```
