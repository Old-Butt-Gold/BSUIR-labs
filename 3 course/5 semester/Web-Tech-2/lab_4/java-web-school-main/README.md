# School Management System

The goal of this task is to give you some practice working with Java servlets and listeners in the context of a Jakarta EE web application.

*Duration*: 45 minutes.

## Description

Create a web application that interacts with a database to retrieve users' data.

The data should be categorized by roles such as students and teachers and displayed on a web page.

The application should also be able to track the number of active sessions created by users.

Use [Apache Commons DBCP](https://commons.apache.org/proper/commons-dbcp/index.html) to connect to the database.

Be sure the actual database implementation is set up properly and the necessary classes are available on your classpath (including your chosen JDBC driver).

## Requirements

1) Implement the `MyServletContextListener.contextInitialized` method. This method ensures that the data source is set up and the initial number of active users is set to 0 when the application is starting up, establishing the functionality you need for your servlets to work with active user sessions and a database connection.

   - Get the `ServletContext` object. The `ServletContextEvent` parameter that is passed in provides an instance of the `ServletContext`.

   - Create a new instance of `BasicDataSource` using the `MyServletContextListener.createDataSource` method. This is an Apache Commons class that provides basic DataSource implementation.

   - Set the necessary properties for the data source, including the name of the JDBC driver class (`BasicDataSource.setDriverClassName`), the JDBC connection URL (`BasicDataSource.setUrl`), the username (`BasicDataSource.setUsername`), and the password (`BasicDataSource.setPassword`) for connecting to the database. These parameters should be retrieved from the initial parameters defined in the `web.xml` file.
   
   - Set the data source and the number of active users for the `ServletContext` using the attributes `dataSource` and `activeUsersCounter`, respectively. Use the `AtomicInteger` object to count the number of active users.

2) Implement the `MyServletContextListener.contextDestroyed` method. This method ensures that before the application is stopped, any active database connection is closed and cleaned up, preventing potential memory leaks and other issues.

   - Get the `ServletContext` object. The `ServletContextEvent` parameter provides an instance of the `ServletContext`.

   - Remove the `activeUsersCounter` and `dataSource` attributes from the `ServletContext`. This effectively cleans up the attributes that were set in `contextInitialized`.

   - Close the DataSource to free up system resources.

3) Implement a `HttpSessionListener` named `MyHttpSessionListener` to track the number of active user sessions.

   - Override the `sessionCreated` method, which is called whenever a new session is created. In this method, you need to get the atomic integer representing the active user count from the servlet context and increment it.

   - Override the `sessionDestroyed` method, which is called whenever a session is invalidated or times out. In this method, you perform a similar action as with `sessionCreated`, but instead of incrementing the active user count, you decrement it.

4) Implement the `MyServlet.doGet` method to handle a request, call the `dbManager` to retrieve data from the database, and forward the request to the `result.jsp` page for rendering.

   - Retrieve the DataSource object that was stored in `ServletContext` during application startup using `MyServletContextListener`, which holds the database connection information.

   - Initialize the `DbManager` object using the `MyServlet.createDbManager` method and pass to it the DataSource object you retrieved earlier. The `DbManager` object will handle all database-related operations.

   - Use the `DbManager` object to retrieve the data from the database. To fetch the list of students and teachers, use the `getUsersByRole` method.

   - Add the student and teacher lists to the request attributes named `studentList` and `teacherList`, respectively. These attributes can then be accessed on the JSP page that you forward the request to.

   - Forward the request to the `/WEB-INF/result.jsp` page for rendering using the `RequestDispatcher` object.

## Examples

An example of initializing the H2 database is given below:

```java
final String creatUsersSql = """
        CREATE TABLE users (
          id INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
          name VARCHAR(255) not null,
          role INTEGER
        )
        """;
final String[] insertUsersSqls = { """
        INSERT INTO users (name, role) VALUES ('John Doe', 1)
        """, """
        INSERT INTO users (name, role) VALUES ('Jane Doe', 1)
        """, """
        INSERT INTO users (name, role) VALUES ('Charlie Brown', 2)
        """ };
Class.forName("org.h2.Driver");
try (Connection connection = dataSource.getConnection()) {
    try (Statement statement = connection.createStatement()) {
        statement.execute(creatUsersSql);
        for (String sql : insertUsersSqls) {
            statement.execute(sql);
        }
    }
}
```
