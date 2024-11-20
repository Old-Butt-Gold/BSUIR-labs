# Finding Students

The goal of this task is to give you some practice working with the JDBC API.

Duration: *50 minutes*

## Description

In this task, you will create two tables, students and groups, and then perform various CRUD operations using SQL queries and the JDBC API. 

## Requirements

1) Use the classes `Group` and `Student`.

2) If necessary, you can use your own relational database. Your database should contain two tables to represent the `Group` and `Student` objects: `groups (id, group_name)` and `students (id, first_name, last_name, group_id)`. 

3) Or you can add the required dependency to your `pom.xml` to install the correct driver for connecting to your database. 

4) Provide an implementation of the following `DbManager` class methods using the `java.sql.PreparedStatement` object: 

- `insertGroup` adds a group to the database.

- `insertStudent` adds a student to the database.

- `findFirstGroupByName` finds a group with the specified name in the database.

- `findFirstStudentByName` finds a student in the database with the specified first and last name.

- `findStudentsByGroup` finds in the database all the students of the specified group.

- `updateGroupById` updates the name of a group in the database based on the specified ID.

- `updateStudentById` updates student data in the database based on the specified ID.

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
	Group group1 = new Group("Group C");
	DbManager.insertGroup(connection, group1);
	LOGGER.log(System.Logger.Level.INFO, group1 + ", id: " + group1.getId());

	Student student1 = new Student("Richard", "Roe", group1);
	DbManager.insertStudent(connection, student1);
	LOGGER.log(System.Logger.Level.INFO, student1 + ", id: " + student1.getId());

	Group group2 = DbManager.findFirstGroupByName(connection, "Group A");
	LOGGER.log(System.Logger.Level.INFO, group2);

	Student student2 = DbManager.findFirstStudentByName(connection, "Jane", "Doe");
	LOGGER.log(System.Logger.Level.INFO, student2);

	List<Student> students = DbManager.findStudentsByGroup(connection, group2);
	LOGGER.log(System.Logger.Level.INFO, students);

	group2.setName("Group D");
	DbManager.updateGroupById(connection, group2);
	students = DbManager.findStudentsByGroup(connection, group2);
	LOGGER.log(System.Logger.Level.INFO, students);

	students = DbManager.findStudentsByGroup(connection, group1);
	LOGGER.log(System.Logger.Level.INFO, students);
	student2.setGroup(group1);
	DbManager.updateStudentById(connection, student2);
	students = DbManager.findStudentsByGroup(connection, group1);
	LOGGER.log(System.Logger.Level.INFO, students);
} catch (SQLException e) {
	// TODO: Error handling.
	throw e;
}
```
