package edu.epam.fop.jdbc.transactions;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertFalse;
import static org.junit.jupiter.api.Assertions.assertTrue;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;
import java.util.Random;
import java.util.UUID;
import java.util.stream.Collectors;
import java.util.stream.IntStream;

import org.junit.jupiter.api.AfterAll;
import org.junit.jupiter.api.AfterEach;
import org.junit.jupiter.api.BeforeAll;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.DisplayNameGeneration;
import org.junit.jupiter.api.Test;

@DisplayNameGeneration(ReplaceCamelCase.class)
class DatabaseTest {

	private static final String JDBC_URL = "jdbc:h2:mem:test;DB_CLOSE_DELAY=-1";
//	private static final String JDBC_URL = "jdbc:derby:memory:test;create=true";

	private static final int GROUP_NUM = 3;
	private static final int STUD_NUM = 100;

	private static String[] groupNames = new String[GROUP_NUM];
	private static String[] firstNames = new String[STUD_NUM];
	private static String[] lastNames = new String[STUD_NUM];
	private static int[] groupIds = new int[STUD_NUM];

	private static Connection connection;

	@BeforeAll
	static void init() throws SQLException {
		connection = DriverManager.getConnection(JDBC_URL);
		Arrays.setAll(groupNames, i -> UUID.randomUUID().toString());
		Arrays.setAll(firstNames, i -> UUID.randomUUID().toString());
		Arrays.setAll(lastNames, i -> UUID.randomUUID().toString());
		Random rnd = new Random();
		Arrays.setAll(groupIds, i -> rnd.nextInt(groupNames.length) + 1);
	}

	@AfterAll
	static void shutdown() throws SQLException {
		connection.close();
	}

	@BeforeEach
	void createTables() throws SQLException {
		try (Statement statement = connection.createStatement()) {
			statement.execute("CREATE TABLE groups (" +
							"id INTEGER GENERATED ALWAYS AS IDENTITY (START WITH 1) PRIMARY KEY," +
							"group_name VARCHAR(255) not null " +
					")");
			statement.execute("CREATE TABLE students (" +
							"id INTEGER GENERATED ALWAYS AS IDENTITY (START WITH 1) PRIMARY KEY, " +
							"first_name VARCHAR(255) not null, " +
							"last_name VARCHAR(255) not null, " +
							"group_id INTEGER, " +
							"FOREIGN KEY (group_id) REFERENCES groups (id) " +
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

	@AfterEach
	void tearDown() throws SQLException {
		connection.createStatement().executeUpdate("DROP TABLE students");
		connection.createStatement().executeUpdate("DROP TABLE groups");
	}

	@Test
	void setGroupForCorrectStudentList() throws SQLException {

		List<Student> initStudents = new ArrayList<>();
		IntStream.range(0, firstNames.length).mapToObj(i -> new Student(i + 1, firstNames[i], lastNames[i],
				new Group(groupIds[i], groupNames[groupIds[i] - 1]))).forEach(initStudents::add);

		Random rnd = new Random();
		int randomGroupIndex = rnd.nextInt(groupNames.length);
		String randomGroupName = groupNames[randomGroupIndex];
		int randomGroupId = randomGroupIndex + 1;
		Group randomGroup = new Group(randomGroupId, randomGroupName);

		boolean result = DbManager.setGroupForStudents(connection, randomGroup, initStudents);
		assertTrue(result, "The result of a successful operation must be \"true\"");

		String fetchStudentsQuery = "SELECT * FROM students";
		List<Student> actualStudents = new ArrayList<>();
		try (Statement statement = connection.createStatement();
				ResultSet resultSet = statement.executeQuery(fetchStudentsQuery)) {
			while (resultSet.next()) {
				int id = resultSet.getInt(1);
				String firstName = resultSet.getString(2);
				String lastName = resultSet.getString(3);
				int groupId = resultSet.getInt(4);
				actualStudents.add(new Student(id, firstName, lastName, new Group(groupId, groupNames[groupId - 1])));
			}
		}

		String expected = initStudents.stream()
				.map(student -> student.getFirstName() + " " + student.getLastName() + " " + randomGroupId).sorted()
				.collect(Collectors.joining(", "));
		String actual = actualStudents.stream()
				.map(student -> student.getFirstName() + " " + student.getLastName() + " " + student.getGroup().getId())
				.sorted().collect(Collectors.joining(", "));
		assertEquals(expected, actual, "The group must be changed for all students");
	}

	@Test
	void setGroupForIncorrectStudentList() throws SQLException {

		List<Student> initStudents = new ArrayList<>();
		IntStream.range(0, firstNames.length).mapToObj(i -> new Student(i + 1, firstNames[i], lastNames[i],
				new Group(groupIds[i], groupNames[groupIds[i] - 1]))).forEach(initStudents::add);

		Random rnd = new Random();
		int randomStudentIndex = rnd.nextInt(firstNames.length);
		initStudents.get(randomStudentIndex).setId(firstNames.length + 999);
		int randomGroupIndex = rnd.nextInt(groupNames.length);
		String randomGroupName = groupNames[randomGroupIndex];
		int randomGroupId = randomGroupIndex + 1;
		Group randomGroup = new Group(randomGroupId, randomGroupName);

		boolean result = DbManager.setGroupForStudents(connection, randomGroup, initStudents);
		assertFalse(result, "The result of an unsuccessful operation must be \"false\"");

		String fetchStudentsQuery = "SELECT * FROM students";
		List<Student> actualStudents = new ArrayList<>();
		try (Statement statement = connection.createStatement();
				ResultSet resultSet = statement.executeQuery(fetchStudentsQuery)) {
			while (resultSet.next()) {
				int id = resultSet.getInt(1);
				String firstName = resultSet.getString(2);
				String lastName = resultSet.getString(3);
				int groupId = resultSet.getInt(4);
				actualStudents.add(new Student(id, firstName, lastName, new Group(groupId, groupNames[groupId - 1])));
			}
		}

		String expected = initStudents.stream()
				.map(student -> student.getFirstName() + " " + student.getLastName() + " " + student.getGroup().getId())
				.sorted().collect(Collectors.joining(", "));
		String actual = actualStudents.stream()
				.map(student -> student.getFirstName() + " " + student.getLastName() + " " + student.getGroup().getId())
				.sorted().collect(Collectors.joining(", "));
		assertEquals(expected, actual, "The group should not be changed for any student");
	}

	@Test
	void deleteStudentsUsingCorrectStudentList() throws SQLException {

		List<Student> initStudents = new ArrayList<>();
		IntStream.range(0, firstNames.length).mapToObj(i -> new Student(i + 1, firstNames[i], lastNames[i],
				new Group(groupIds[i], groupNames[groupIds[i] - 1]))).forEach(initStudents::add);

		boolean result = DbManager.deleteStudents(connection, initStudents);
		assertTrue(result, "The result of a successful operation must be \"true\"");

		String fetchStudentsQuery = "SELECT COUNT(*) FROM students";
		int count = -1;
		try (Statement statement = connection.createStatement();
				ResultSet resultSet = statement.executeQuery(fetchStudentsQuery)) {
			resultSet.next();
			count = resultSet.getInt(1);
		}
		assertEquals(0, count, "All students should be removed");
	}

	@Test
	void deleteStudentsUsingIncorrectStudentList() throws SQLException {

		List<Student> initStudents = new ArrayList<>();
		IntStream.range(0, firstNames.length).mapToObj(i -> new Student(i + 1, firstNames[i], lastNames[i],
				new Group(groupIds[i], groupNames[groupIds[i] - 1]))).forEach(initStudents::add);

		Random rnd = new Random();
		int randomStudentIndex = rnd.nextInt(firstNames.length);
		initStudents.get(randomStudentIndex).setId(firstNames.length + 999);

		boolean result = DbManager.deleteStudents(connection, initStudents);
		assertFalse(result, "The result of an unsuccessful operation must be \"false\"");

		String fetchStudentsQuery = "SELECT * FROM students";
		List<Student> actualStudents = new ArrayList<>();
		try (Statement statement = connection.createStatement();
				ResultSet resultSet = statement.executeQuery(fetchStudentsQuery)) {
			while (resultSet.next()) {
				int id = resultSet.getInt(1);
				String firstName = resultSet.getString(2);
				String lastName = resultSet.getString(3);
				int groupId = resultSet.getInt(4);
				actualStudents.add(new Student(id, firstName, lastName, new Group(groupId, groupNames[groupId - 1])));
			}
		}

		String expected = initStudents.stream()
				.map(student -> student.getFirstName() + " " + student.getLastName() + " " + student.getGroup().getId())
				.sorted().collect(Collectors.joining(", "));
		String actual = actualStudents.stream()
				.map(student -> student.getFirstName() + " " + student.getLastName() + " " + student.getGroup().getId())
				.sorted().collect(Collectors.joining(", "));
		assertEquals(expected, actual, "No student should be removed");
	}
}