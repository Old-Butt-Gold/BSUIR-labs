# Task Manager

The goal of this task is to give you some practice working with an HTTP client API in Java.

Duration: *40 minutes*

## Description

In this task, you will create a Java-based client application that can take input from the user to create, fetch, update, and delete tasks. The application should handle raw HTTP requests and responses.

## Requirements

1) Develop a `TaskManager` class to perform create, retrieve, update, and delete operations using HTTP methods.

2) Implement the `TaskManager.createTask` method to create a new task using a POST request. It accepts the base URL, task ID, task name, and task status as parameters. Use a JSON string as the request body with the task parameters in the following format: `{"taskId":"A","taskName":"B","taskStatus":"C"}`, where
   - A is a task ID
   - B is a task name
   - C is a task status

   The content type header should be set as "application/json", which means the data sent is in JSON format. 

3) Implement the `TaskManager.retrieveTask` method to retrieve an existing task using a GET request. This method takes the base URL and the task ID as parameters, which are used to form the complete URL. In this case, the endpoint is the base URL appended with the task ID. A GET request to this URL will retrieve the task with the matching ID.

4) Implement the `TaskManager.updateTask` method to update an existing task using a PUT request. Similar to `createTask`, it accepts the base URL, task ID, task name, and task status as parameters. The endpoint should be created by appending the task ID provided to the base URL. Use a JSON string as the request body with the task parameters.

5) Implement the `TaskManager.deleteTask` method to delete an existing task using a DELETE request. This method accepts the base URL and task ID as parameters. The endpoint should be formed using these parameters, which point to the task that needs to be deleted.

6) To test the implemented functionality, you can use a local server that is added to the project using Maven (or any test server). For example, [https://httpbin.org](https://httpbin.org) is a simple HTTP request and response service that is good for testing HTTP clients. 
 
## Examples

An example of sending requests to "https://httpbin.org/anything" and printing responses is given below:

An example of sending requests to [https://httpbin.org/anything](https://httpbin.org/anything) and printing responses is given below:

```java
final String baseUrl = "https://httpbin.org/anything";
final HttpClient client = HttpClient.newHttpClient();
String taskId = "1";
String taskName = "New Task";
String taskStatus = "Started";
HttpRequest request = TaskManager.createTask(baseUrl, taskId, taskName, taskStatus);
HttpResponse<String> response = client.send(request, HttpResponse.BodyHandlers.ofString());
System.out.println("Response code: " + response.statusCode());
System.out.println("Response body: " + response.body());
```

The output is given below:

```
Response code: 200
Response body: {
  "args": {}, 
  "data": "{\n  \"taskId\": \"1\",\n  \"taskName\": \"New Task\",\n  \"taskStatus\": \"Started\"\n}\n", 
  "files": {}, 
  "form": {}, 
  "headers": {
    "Content-Length": "73", 
    "Content-Type": "application/json", 
    "Host": "httpbin.org", 
    "User-Agent": "Java-http-client/17.0.9", 
    "X-Amzn-Trace-Id": "Root=1-6543e0b1-626bdfe447a1981301c4c5d3"
  }, 
  "json": {
    "taskId": "1", 
    "taskName": "New Task", 
    "taskStatus": "Started"
  }, 
  "method": "POST", 
  "origin": "178.151.178.65", 
  "url": "https://httpbin.org/anything"
}
```