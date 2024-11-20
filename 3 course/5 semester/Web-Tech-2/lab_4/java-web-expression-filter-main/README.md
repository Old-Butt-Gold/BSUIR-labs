# Expression Filter

The goal of this task is to extend a basic expression calculator with additional capabilities - including request encoding, response encoding, logging, and validation - using separate filters.

*Duration*: 45 minutes.

## Description

The application receives mathematical expressions as HTTP POST requests. These requests should be processed using a chain of servlet filters that handle encoding, logging, and input validation before reaching the `CalculatorServlet`, which performs the calculation.

The servlet and filters should align with the single responsibility principle.
   - `EncodingFilter` sets the encoding for requests and responses.
   - `LoggingFilter` logs incoming request parameters.
   - `ValidationFilter` validates the input expressions against a regular expression defined in the web application context.

It is enough to implement evaluation of expressions containing simple arithmetic operations ("+," "-," "*," or "/") for two operands of the "double" type.

The solution should follow the post–redirect–get pattern.

## Requirements

1) The application should accept user input in HTML format, perform calculations, store the history in an HTTP session, and display the results. You can enter an expression for a calculation using the `index.html` page.

2) The expression validation pattern is provided as the context parameter "expression-regex" in the deployment descriptor `web.xml`. You must retrieve and use this pattern in both `CalculatorServlet` to evaluate an expression and `ValidationFilter` to validate a request.

3) Implement the `CalculatorServlet.init` method to set the `CalculatorServlet.pattern` field to the regular expression using the initialization parameter "expression-regex".

4) Implement the `CalculatorServlet.doPost` method.
   - Check for the "action" parameter. If its value is equal to "clearHistory", remove the "history" session attribute and redirect the client to the `result.jsp` page.
   - Parse the "expression" parameter fetched from the request using `CalculatorServlet.pattern`.
   - Evaluate the "expression" and display the results.
   - If an error occurs during calculation, add an attribute named "error" containing the error message to the current session and redirect the client to the `error.jsp` page.
   - If there are no errors, accumulate the calculation results in the "history" attribute of the current session as an ArrayList of strings, remove the "error" attribute, and redirect the client to the `result.jsp` page.

5) Override the `CalculatorServlet.doGet` method. This method should define the error message "Please use the POST method for calculations" as the session attribute "error" and redirect to `error.jsp`.

6) The value for setting the request/response encoding is specified by the "encoding" initialization parameter of the `EncodingFilter` filter in the deployment descriptor `web.xml`.

7) Develop an `EncodingFilter` to set the request and response encoding to the value specified by the filter's initialization parameter named "encoding":
   - Use the `EncodingFilter.init` method to set the `EncodingFilter.encoding` field to the value of the initialization parameter "encoding".
   - Receive a request and a response, set their character encoding, and then pass them down the filter chain.

8) Construct a `LoggingFilter` to log request parameters. Read the parameters in the HTTP requests and write the parameter names and values to the log. Use the provided method `logRequestParameter`. The log entries should be in the format "Request parameter param_name: [parameter values]".

9) Build a `ValidationFilter` to validate the request parameters using the validation pattern defined in the servlet context initialization parameter named "expression-regex":
   - Use the `ValidationFilter.init` method to set the `ValidationFilter.pattern` field using the value of the initialization parameter "expression-regex".
   - The validation concerns input for a mathematical expression (e.g., `5 + 3` or `6 * 2`).
   - If the expression does not match the validation pattern, an error message should be set in the session as the "error" attribute, and the client should be redirected to the `error.jsp` page.

## Examples

An example of setting a regular expression using context initialization parameters is shown below:

```java
ServletContext context = getServletContext();
String regex = context.getInitParameter("expression-regex");
Pattern pattern = Pattern.compile(regex);
```

How to get the data to evaluate the expression using the regular expression provided is shown below:

```java
String expression = "2*2";
...
Matcher matcher = pattern.matcher(expression);
if (!matcher.matches()) {
	throw new IllegalArgumentException("Invalid expression: \"" + expression + "\"");
}
String operator = matcher.group("operation");
double operand1 = Double.parseDouble(matcher.group("operand1"));
double operand2 = Double.parseDouble(matcher.group("operand2"));
```

Some examples of correct expressions for calculations and the corresponding results stored in the calculation history are shown below:

```
8/4        8/4=2.0
1+-1       1+-1=0.0
-2+.1      -2+.1=-1.9
-1.1+-0.0  -1.1+-0.0=-1.1
2*.0       2*.0=0.0
5--5       5--5=10.0
```

An example of an incorrect expression and the corresponding error message is shown below:

```
+         Invalid expression: "+"
111       Invalid expression: "111"
.0/.0     Division by zero!
```

Some examples of `LoggingFilter` messages are shown below:

```
INFO: Request parameter expression: [8/4]
INFO: Request parameter expression: [1+-1]
INFO: Request parameter expression: [-2+.1]
INFO: Request parameter expression: [+]
INFO: Request parameter expression: [111]
INFO: Request parameter expression: [.0/.0]
```
