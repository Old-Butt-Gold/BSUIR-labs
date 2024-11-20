# Fetching Students

The goal of this task is to give you some practice working with the JDBC API.

Duration: *30 minutes*

## Description

In this task, you will create two tables, students and groups, and then perform various CRUD operations using SQL queries and the JDBC API. 

## Requirements

1) Use the classes `Group` and `Student`.

2) If necessary, you can use your own relational database. Your database should contain two tables to represent the `Group` and `Student` objects: `groups (id, group_name)` and `students (id, first_name, last_name, group_id)`. 

3) Or you can add the required dependency to your `pom.xml` to install the correct driver for connecting to your database. 

4) Provide an implementation of the following `DbManager` class methods using the `java.sql.Statement` object: 

- `fetchGroups` returns a list of groups whose data is fetched from the database. 

- `fetchStudents` returns a list of students whose data is fetched from the database. 

## Examples

An example of creating tables for an H2 or Apache Derby database:

```sql
CREATE TABLE groups (
  id INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  group_name VARCHAR(255) not null
)

CREATE TABLE students (
  id INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
  first_name VARCHAR(255) not null,
  last_name VARCHAR(255) not null,
  group_id INTEGER,
  FOREIGN KEY (group_id) REFERENCES groups (id)
)
```

An example of creating tables for a MySQL database: 

```sql
CREATE TABLE groups (
  id INT AUTO_INCREMENT PRIMARY KEY,
  group_name VARCHAR(255) not null
)

CREATE TABLE students (
  id INT AUTO_INCREMENT PRIMARY KEY,
  first_name VARCHAR(255) not null,
  last_name VARCHAR(255) not null,
  group_id INT,
  FOREIGN KEY (group_id) REFERENCES groups (id)
)
```

An example of calling methods of the DbManager class:

```java
try (Connection connection = DriverManager.getConnection(JDBC_URL)) {
	List<Group> groups = DbManager.fetchGroups(connection);
	LOGGER.log(System.Logger.Level.INFO, groups);

	List<Student> students = DbManager.fetchStudents(connection);
	LOGGER.log(System.Logger.Level.INFO, students);
} catch (SQLException e) {
	// TODO: Error handling.
	throw e;
}
```
