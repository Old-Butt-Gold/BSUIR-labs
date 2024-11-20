package edu.epam.fop.jdbc.create;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertNotNull;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.Arrays;
import java.util.List;
import java.util.Random;
import java.util.UUID;
import java.util.stream.Collectors;
import java.util.stream.IntStream;
import java.util.stream.Stream;

import org.junit.jupiter.api.AfterAll;
import org.junit.jupiter.api.BeforeAll;
import org.junit.jupiter.api.DisplayNameGeneration;
import org.junit.jupiter.api.Test;

@DisplayNameGeneration(ReplaceCamelCase.class)
class DatabaseTest {

	private static final String JDBC_URL = "jdbc:postgresql://localhost:2004/testdb"; // Измените на имя вашей базы данных
	private static final String JDBC_USER = "postgres"; // Установите вашего пользователя PostgreSQL
	private static final String JDBC_PASSWORD = "HyperPROROK2019"; // Установите ваш пароль
//	private static final String JDBC_URL = "jdbc:derby:memory:test;create=true";

	private static final int GROUP_NUM = 3;
	private static final int STUD_NUM = 10;

	private static String[] groupNames = new String[GROUP_NUM];
	private static String[] firstNames = new String[STUD_NUM];
	private static String[] lastNames = new String[STUD_NUM];
	private static int[] groupIds = new int[STUD_NUM];

	private static Connection connection;

	@BeforeAll
	static void init() throws SQLException {
		connection = DriverManager.getConnection(JDBC_URL, JDBC_USER, JDBC_PASSWORD);
		Arrays.setAll(groupNames, s -> UUID.randomUUID().toString());
		Arrays.setAll(firstNames, s -> UUID.randomUUID().toString());
		Arrays.setAll(lastNames, i -> UUID.randomUUID().toString());
		Random rnd = new Random();
		Arrays.setAll(groupIds, s -> rnd.nextInt(groupNames.length) + 1);
		try (Statement statement = connection.createStatement()) {
			statement.execute("DROP TABLE IF EXISTS students");
			statement.execute("DROP TABLE IF EXISTS groups");

			statement.execute("CREATE TABLE groups (" +
							"id SERIAL PRIMARY KEY," +
							"group_name VARCHAR(255) NOT NULL " +
					")");
			statement.execute("CREATE TABLE students ( " +
							"id SERIAL PRIMARY KEY, " +
							"first_name VARCHAR(255) NOT NULL, " +
							"last_name VARCHAR(255) NOT NULL, " +
							"group_id INT, " +
							"FOREIGN KEY (group_id) REFERENCES groups (id)" +
					")");

			for (String group : groupNames) {
				statement.execute("INSERT INTO groups (group_name) VALUES ('" + group + "')");
			}
			for (int i = 0; i < STUD_NUM; ++i) {
				statement.execute("INSERT INTO students (first_name, last_name, group_id) VALUES ('" + firstNames[i]
						+ "', '" + lastNames[i] + "', " + groupIds[i] + ")");
			}
		}
	}

	@AfterAll
	static void shutdown() throws SQLException {
		connection.close();
	}

	@Test
	void testFetchGroups() throws SQLException {
		String expected = Stream.of(groupNames).sorted().collect(Collectors.joining(", "));
		List<Group> groups = DbManager.fetchGroups(connection);
		assertNotNull(groups, "Group list should not be null");

		String actual = groups.stream().map(Object::toString).sorted().collect(Collectors.joining(", "));
		assertEquals(expected, actual, "Group list doesn't match");
	}

	@Test
	void testFetchStudents() throws SQLException {
		String expected = IntStream.range(0, firstNames.length)
				.mapToObj(i -> firstNames[i] + " " + lastNames[i] + " " + groupNames[groupIds[i] - 1]).sorted()
				.collect(Collectors.joining(", "));
		List<Student> students = DbManager.fetchStudents(connection);
		assertNotNull(students, "Student list should not be null");

		String actual = students.stream().map(Object::toString).sorted().collect(Collectors.joining(", "));
		assertEquals(expected, actual, "Student list doesn't match");
	}
}