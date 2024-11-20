# Simple Web Calculator

The goal of this task is to create a simple Web calculator using Servlets and JSP to handle HTTP GET and POST requests.

Duration: *45 minutes*

## Description

You must create a Java servlet named `CalculatorServlet` to perform arithmetic operations and return a result.

Using the `index.html` page, users can enter two numbers and choose an arithmetic operation to perform on those numbers.

The servlet must process this information, perform the calculation, and then use the `result.jsp` page to produce the result of the operation.

## Requirements

1) Develop a Java servlet named `CalculatorServlet`.

2) The servlet receives inputs from either GET or POST HTTP requests with the following parameters:
   - `num1`: The first number to perform the operation on
   - `num2`: The second number to perform the operation on
   - `operation`: The operation that will be perform (Valid values are "add", "subtract", "multiply", and "divide")

3) Perform the requested operation and create a `Calculation` object containing the two operands, the operation, and the result. Set this `Calculation` object as a request attribute named "calculation" and forward the request to the `result.jsp` page.

4) The servlet should validate the parameters properly:
   - If the given operation is unknown, it should respond with an HTTP `400 Bad Request` status and the error message "Unknown operation".
   - If the provided `num1` or `num2` cannot be parsed as a double, the servlet should also respond with an HTTP `400 Bad Request` status and the error message "Invalid number".

## Examples

An example implementation of the servlet's `doGet` method is shown below:

```java
protected void doGet(HttpServletRequest request, HttpServletResponse response)
		throws ServletException, IOException {
	try {
		double num1 = Double.parseDouble(request.getParameter("num1"));
		...
		request.getRequestDispatcher("result.jsp").forward(request, response);
	} catch (NumberFormatException e) {
		response.sendError(HttpServletResponse.SC_BAD_REQUEST, "Invalid number");
	} catch (IllegalArgumentException e) {
		response.sendError(HttpServletResponse.SC_BAD_REQUEST, "Unknown operation");
	}
}
```

An example of an HTTP GET request to a web calculator is shown below:

```
http://localhost:8080/simple-web-calculator/CalculatorServlet?num1=2&num2=2&operation=add
```

An example of the web calculator's response to this request is shown below:

```html
<!DOCTYPE html>
<html>
<head>
<meta charset="UTF-8">
<title>Calculator Result</title>
</head>
<body>
	<h2>Result</h2>
	4.0
	<hr>
	<br> Operation: add
	<br> num1: 2.0
	<br> num2: 2.0
</body>
</html>
```