# Getting Student Information

The goal of this task is to give you some practice working with the JDBC API.

Duration: *40 minutes*

## Description

In this task, you will create two tables, students and groups, and then perform various CRUD operations using SQL queries and the JDBC API. 

## Requirements

1) Use the classes `Group` and `Student`.

2) If necessary, you can use your own relational database. Your database should contain two tables to represent the `Group` and `Student` objects: `groups (id, group_name)` and `students (id, first_name, last_name, group_id)`. 

3) Or you can add the required dependency to your `pom.xml` to install the correct driver for connecting to your database. 

4) If you are using your own database, create the following stored procedures:

- `COUNT_GROUPS` counts the total number of records in the `groups` table. This stored procedure has an output parameter called `totalGroups` that holds the number of records in the `groups` table.

- `COUNT_STUDENTS` counts the total number of records in the `students` table. This stored procedure has an output parameter called `totalStudents` that holds the number of records in the `students` table.

- `COUNT_STUDENTS_BY_GROUP_ID` counts the number of students in the `group` with the specified ID. This stored procedure has two parameters: an input parameter named `groupId` to specify the group ID for which you want to count the students and an output parameter called `totalStudents` that holds the number of students records for the specified group. 

5) Provide an implementation of the following `DbManager` class methods using the `java.sql.CallableStatement` object:

- `callCountGroups` calls the stored procedure `COUNT_GROUPS` and returns the total number of groups.

- `callCountStudents` calls the stored procedure `COUNT_STUDENTS` and returns the total number of students.

- `callCountStudentsByGroupId` calls the stored procedure `COUNT_STUDENTS_BY_GROUP_ID` and returns the number of students in the group with the specified ID.

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

To create the `COUNT_GROUPS` stored procedure in the MySQL console, use the following commands:

```sql
DELIMITER //
CREATE PROCEDURE COUNT_GROUPS(OUT totalGroups INT)
BEGIN
  SELECT COUNT(*) INTO totalGroups FROM groups;
END //
DELIMITER ;
```

The following sequence of MySQL console commands will call the `COUNT_GROUPS` stored procedure and display the total number of groups:

```sql
SET @groupCount = 0;
CALL COUNT_GROUPS(@groupCount);
SELECT @groupCount;
```

To create the `COUNT_STUDENTS_BY_GROUP_ID` stored procedure in the MySQL console, use the following commands:

```sql
DELIMITER //
CREATE PROCEDURE COUNT_STUDENTS_BY_GROUP_ID(IN groupId INT, OUT totalStudents INT)
BEGIN
  SELECT COUNT(*) INTO totalStudents FROM students WHERE group_id = groupId;
END //
DELIMITER ;
```

The following sequence of MySQL console commands will call the COUNT_STUDENTS_BY_GROUP_ID stored procedure and display the number of students in the group specified:

```sql
SET @inputGroupId = 1;
SET @studentCount = 0;
CALL COUNT_STUDENTS_BY_GROUP_ID(@inputGroupId, @studentCount);
SELECT @studentCount;
```

An example of calling methods of the DbManager class:

```java
try (Connection connection = DriverManager.getConnection(JDBC_URL)) {
	int groupCount = DbManager.callCountGroups(connection);
	LOGGER.log(System.Logger.Level.INFO, "Total number of groups: " + groupCount);

	int studentCount = DbManager.callCountStudents(connection);
	LOGGER.log(System.Logger.Level.INFO, "Total number of students: " + studentCount);

	studentCount = DbManager.callCountStudentsByGroupId(connection, 1);
	LOGGER.log(System.Logger.Level.INFO, "Number of students in the first group: " + studentCount);

	studentCount = DbManager.callCountStudentsByGroupId(connection, 2);
	LOGGER.log(System.Logger.Level.INFO, "Number of students in the second group: " + studentCount);

	studentCount = DbManager.callCountStudentsByGroupId(connection, 3);
	LOGGER.log(System.Logger.Level.INFO, "Number of students in the third group: " + studentCount);
} catch (SQLException e) {
	// TODO: Error handling.
	throw e;
}
```
