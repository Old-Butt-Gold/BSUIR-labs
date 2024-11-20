# Update Students

The goal of this task is to give you some practice working with the JDBC API.

Duration: *40 minutes*

## Description

In this task, you will create two tables — `students` and `groups` — and then perform various CRUD operations using SQL queries and the JDBC API. The CRUD operations must be implemented using transactions.

## Requirements

1) Use the classes provided: `Group` and `Student`.

2) If you are using your own database, you should create two tables to represent the `Group` and `Student` objects: `groups (id, group_name)` and `students (id, first_name, last_name, group_id)`. In this case, you should also add the required dependency to your `pom.xml` to install the appropriate driver for connecting to your database.

3) Provide an implementation of the following `DbManager` class methods using transactions:

- `setGroupForStudents` either assigns all of the specified students to the specified group or none of them: `setGroupForStudents (groupA, student1, student2, student3)` sets students `student1`, `student2`, and `student3` to group `groupA`. If it is not possible to set a group for at least one student, you must roll back a transaction.

- `deleteStudents` either removes all the specified students or none of them.

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
    ...
    boolean result = DbManager.setGroupForStudents(connection, group, students);
    LOGGER.log(System.Logger.Level.INFO, "result: " + result);
    ...
    result = DbManager.deleteStudents(connection, students);
    LOGGER.log(System.Logger.Level.INFO, "result: " + result);
    ...
} catch (SQLException e) {
	// TODO: Error handling.
	throw e;
}
```
