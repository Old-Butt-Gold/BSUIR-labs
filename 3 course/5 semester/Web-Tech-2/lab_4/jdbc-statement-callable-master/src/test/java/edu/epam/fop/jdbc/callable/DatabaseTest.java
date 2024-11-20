package edu.epam.fop.jdbc.callable;

import static org.junit.jupiter.api.Assertions.assertEquals;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.Arrays;
import java.util.Random;
import java.util.UUID;
import java.util.stream.IntStream;

import org.junit.jupiter.api.AfterAll;
import org.junit.jupiter.api.BeforeAll;
import org.junit.jupiter.api.DisplayNameGeneration;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.MethodSource;

@DisplayNameGeneration(ReplaceCamelCase.class)
public class DatabaseTest {

	private static final String JDBC_URL = "jdbc:derby:memory:test;create=true";

	private static final int GROUP_NUM = new Random().nextInt(2, 10);
	private static final int STUD_NUM = GROUP_NUM + 50;

	private static String[] groupNames = new String[GROUP_NUM];
	private static String[] firstNames = new String[STUD_NUM];
	private static String[] lastNames = new String[STUD_NUM];
	private static int[] groupIds = new int[STUD_NUM];

	private static Connection connection;

	public static void countGroupsDerby(int[] totalGroups) throws SQLException {
		try (Connection connection = DriverManager.getConnection(JDBC_URL)) {
			String query = "SELECT COUNT(*) FROM groups";
			totalGroups[0] = 0;
			try (PreparedStatement preparedStatement = connection.prepareStatement(query);
					ResultSet resultSet = preparedStatement.executeQuery()) {
				if (resultSet.next()) {
					totalGroups[0] = resultSet.getInt(1);
				}
			}
		}
	}

	public static void countStudentsDerby(int[] totalStudents) throws SQLException {
		try (Connection connection = DriverManager.getConnection(JDBC_URL)) {
			String query = "SELECT COUNT(*) FROM students";
			totalStudents[0] = 0;
			try (PreparedStatement preparedStatement = connection.prepareStatement(query);
					ResultSet resultSet = preparedStatement.executeQuery()) {
				if (resultSet.next()) {
					totalStudents[0] = resultSet.getInt(1);
				}
			}
		}
	}

	public static void countStudentsByGroupIdDerby(int groupId, int[] result) throws SQLException {
		try (Connection connection = DriverManager.getConnection(JDBC_URL)) {
			String query = "SELECT COUNT(*) FROM students WHERE group_id = ?";
			result[0] = 0;
			try (PreparedStatement preparedStatement = connection.prepareStatement(query)) {
				preparedStatement.setInt(1, groupId);
				try (ResultSet resultSet = preparedStatement.executeQuery()) {
					if (resultSet.next()) {
						result[0] = resultSet.getInt(1);
					}
				}
			}
		}
	}

	@BeforeAll
	static void init() throws SQLException {
		connection = DriverManager.getConnection(JDBC_URL);
		Arrays.setAll(groupNames, s -> UUID.randomUUID().toString());
		Arrays.setAll(firstNames, s -> UUID.randomUUID().toString());
		Arrays.setAll(lastNames, i -> UUID.randomUUID().toString());
		Random rnd = new Random();
		Arrays.setAll(groupIds, s -> rnd.nextInt(groupNames.length) + 1);
		try (Statement statement = connection.createStatement()) {
			statement.execute("""
					CREATE TABLE groups (
					  id INTEGER GENERATED ALWAYS AS IDENTITY (START WITH 1) PRIMARY KEY,
					  group_name VARCHAR(255) not null
					)
					""");
			statement.execute("""
					CREATE TABLE students (
					  id INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY,
					  first_name VARCHAR(255) not null,
					  last_name VARCHAR(255) not null,
					  group_id INTEGER,
					  FOREIGN KEY (group_id) REFERENCES groups (id)
					)
					""");
			for (String group : groupNames) {
				statement.execute("INSERT INTO groups (group_name) VALUES ('" + group + "')");
			}
			for (int i = 0; i < STUD_NUM; ++i) {
				statement.execute("INSERT INTO students (first_name, last_name, group_id) VALUES ('" + firstNames[i]
						+ "', '" + lastNames[i] + "', " + groupIds[i] + ")");
			}
			statement.execute("""
					CREATE PROCEDURE COUNT_GROUPS(OUT totalGroups INTEGER)
					PARAMETER STYLE JAVA READS SQL DATA LANGUAGE JAVA EXTERNAL NAME
					'edu.epam.fop.jdbc.callable.DatabaseTest.countGroupsDerby'
					""");
			statement.execute("""
					CREATE PROCEDURE COUNT_STUDENTS(OUT totalStudents INTEGER)
					PARAMETER STYLE JAVA READS SQL DATA LANGUAGE JAVA EXTERNAL NAME
					'edu.epam.fop.jdbc.callable.DatabaseTest.countStudentsDerby'
					""");
			statement.execute("""
					CREATE PROCEDURE COUNT_STUDENTS_BY_GROUP_ID(IN id INTEGER, OUT result INTEGER)
					PARAMETER STYLE JAVA READS SQL DATA LANGUAGE JAVA EXTERNAL NAME
					'edu.epam.fop.jdbc.callable.DatabaseTest.countStudentsByGroupIdDerby'
					""");
		}
	}

	@AfterAll
	static void shutdown() throws SQLException {
		connection.close();
	}

	@Test
	void testCallCountGroups() throws SQLException {
		int expectedTotalGroups = GROUP_NUM;
		int actualTotalGroups = DbManager.callCountGroups(connection);
		assertEquals(expectedTotalGroups, actualTotalGroups, "The total number of groups is calculated incorrectly");
	}

	@Test
	void testCallCountStudents() throws SQLException {
		int expectedTotalStudents = STUD_NUM;
		int actualTotalStudents = DbManager.callCountStudents(connection);
		assertEquals(expectedTotalStudents, actualTotalStudents,
				"The total number of students is calculated incorrectly");
	}

	static IntStream provideGroupIds() {
		return IntStream.range(1, groupNames.length + 2);
	}

	@ParameterizedTest
	@MethodSource("provideGroupIds")
	void testCallCountStudentsByGroupId(int groupId) throws SQLException {
		String query = "SELECT COUNT(*) FROM students WHERE group_id = " + Integer.toString(groupId);
		int expectedTotalStudents = -1;
		try (Statement statement = connection.createStatement()) {
			ResultSet resultSet = statement.executeQuery(query);
			if (resultSet.next()) {
				expectedTotalStudents = resultSet.getInt(1);
			}
		}
		int actualTotalStudents = DbManager.callCountStudentsByGroupId(connection, groupId);
		assertEquals(expectedTotalStudents, actualTotalStudents,
				"The total number of students in the group is calculated incorrectly");
	}
}