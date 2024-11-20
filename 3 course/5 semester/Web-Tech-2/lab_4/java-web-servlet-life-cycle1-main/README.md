# Visit Counter 

The goal of this task is to create servlets and recognize the stages of a servlet's lifecycle.

Duration: *45 minutes*

## Description

Create a simple servlet to count and display the number of visits to a page.

## Requirements

1) Implement a `VisitCounterServlet` class. Add the `@WebServlet("/VisitCounter")` annotation to map the servlet to a URL.

2) Implement the `init()` method to initialize the hit counter in the servlet context. Create a `visitCount` attribute for the counter. Read the previous counter value from the file into the `visitCount` attribute. If the file does not exist, set the counter value to `0`. Log the servlet initialization debug message.

3) Implement the `service()` method to process the GET request. 
Increment the value of the visit counter. Access the servlet context and update the counter value. Set the response content type to `text/html`. Generate a response with information about the number of visits as an HTML page. Log a debug message about handling requests from the user.

4) Implement the `destroy()` method. 
It should save the value of the counter of visits to a file. Log the servlet completion debug message.

## Examples

A sample implementation of the servlet's service method is shown below:

```
protected void service(HttpServletRequest request, HttpServletResponse response)
		throws ServletException, IOException {
	...
	int currentCounter = visitCount.incrementAndGet();

	response.setContentType("text/html");
	PrintWriter out = response.getWriter();
	out.println("<!DOCTYPE html><html lang=\"en\"><head><title>Visit Counter</title></head><body>");
	out.println("<h2>Visit counter: " + currentCounter + "</h2>");
	out.println("</body></html>");
	...
}
```