package edu.epam.fop.jdbc.prepared;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertFalse;
import static org.junit.jupiter.api.Assertions.assertNotEquals;
import static org.junit.jupiter.api.Assertions.assertNotNull;
import static org.junit.jupiter.api.Assertions.assertNull;
import static org.junit.jupiter.api.Assertions.assertTrue;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.ResultSet;
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
import org.junit.jupiter.api.AfterEach;
import org.junit.jupiter.api.BeforeAll;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.DisplayNameGeneration;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.Arguments;
import org.junit.jupiter.params.provider.MethodSource;

@DisplayNameGeneration(ReplaceCamelCase.class)
class DatabaseTest {

	private static final String JDBC_URL = "jdbc:h2:mem:test;DB_CLOSE_DELAY=-1";
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
							"id INTEGER GENERATED ALWAYS AS IDENTITY (START WITH 1) PRIMARY KEY, " +
							"group_name VARCHAR(255) not null " +
					")");
			statement.execute("CREATE TABLE students ( " +
							"id INTEGER GENERATED ALWAYS AS IDENTITY PRIMARY KEY, " +
							"first_name VARCHAR(255) not null, " +
							"last_name VARCHAR(255) not null, " +
							"group_id INTEGER," +
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
	void testInsertGroup() throws SQLException {
		String randomGroupName = "Group " + UUID.randomUUID().toString();
		Group randomGroup = new Group(randomGroupName);

		boolean isInserted = DbManager.insertGroup(connection, randomGroup);
		assertTrue(isInserted, "The return value should be \"true\"");

		String query = "SELECT * FROM groups WHERE group_name = '" + randomGroupName + "'";
		int fetchedGroupId = -1;
		try (Statement statement = connection.createStatement()) {
			ResultSet resultSet = statement.executeQuery(query);
			if (resultSet.next()) {
				fetchedGroupId = resultSet.getInt(1);
			}
		}

		assertNotEquals(-1, fetchedGroupId, "The group was not found in the database");
		assertEquals(fetchedGroupId, randomGroup.getId(), "Group ID doesn't match");
	}

	@Test
	void testInsertStudent() throws SQLException {
		String randomFirstName = UUID.randomUUID().toString();
		String randomLastName = UUID.randomUUID().toString();
		Random rnd = new Random();
		int randomGroupIndex = rnd.nextInt(groupNames.length);
		String randomGroupName = groupNames[randomGroupIndex];
		int randomGroupId = randomGroupIndex + 1;
		Group randomGroup = new Group(randomGroupId, randomGroupName);
		Student randomStudent = new Student(randomFirstName, randomLastName, randomGroup);

		boolean isInserted = DbManager.insertStudent(connection, randomStudent);
		assertTrue(isInserted, "The return value should be \"true\"");

		String query = "SELECT * FROM students WHERE first_name = '$1' AND students.last_name = '$2'".replace("$1", randomFirstName).replace("$2", randomLastName);
		int fetchedStudentId = -1;
		int fetchedGroupId = -1;
		try (Statement statement = connection.createStatement()) {
			ResultSet resultSet = statement.executeQuery(query);
			if (resultSet.next()) {
				fetchedStudentId = resultSet.getInt(1);
				fetchedGroupId = resultSet.getInt(4);
			}
		}

		assertNotEquals(-1, fetchedStudentId, "The student was not found in the database");
		assertEquals(fetchedStudentId, randomStudent.getId(), "Student ID doesn't match");
		assertEquals(randomGroupId, fetchedGroupId, "Group ID doesn't match");
	}

	static String[] provideGroupNames() {
		return groupNames;
	}

	@ParameterizedTest
	@MethodSource("provideGroupNames")
	void testFindFirstGroupByName(String expectedGroupName) throws SQLException {
		String query = "SELECT * FROM groups WHERE group_name = '" + expectedGroupName + "'";
		int expectedGroupId = -1;
		try (Statement statement = connection.createStatement()) {
			ResultSet resultSet = statement.executeQuery(query);
			if (resultSet.next()) {
				expectedGroupId = resultSet.getInt(1);
			}
		}
		Group foundGroup = DbManager.findFirstGroupByName(connection, expectedGroupName);

		assertNotNull(foundGroup, "Group should not be null");
		assertEquals(expectedGroupId, foundGroup.getId(), "Group ID doesn't match");
		assertEquals(expectedGroupName, foundGroup.getName(), "Group name doesn't match");
	}

	@Test
	void testFindGroupByWrongName() throws SQLException {
		String randomGroupName = "Group " + UUID.randomUUID().toString();
		Group foundGroup = DbManager.findFirstGroupByName(connection, randomGroupName);
		assertNull(foundGroup, "Group should be null");
	}

	static Stream<Arguments> provideStudentNames() {
		return IntStream.range(0, Math.min(firstNames.length, lastNames.length))
				.mapToObj(i -> Arguments.of(firstNames[i], lastNames[i]));
	}

	@ParameterizedTest
	@MethodSource("provideStudentNames")
	void testFindFirstStudentByName(String expectedFirstName, String expectedLastName) throws SQLException {
		String query = "SELECT * FROM students WHERE first_name = '" + expectedFirstName + "' AND last_name ='"
				+ expectedLastName + "'";
		int expectedStudentId = -1;
		int expectedGroupId = -1;
		try (Statement statement = connection.createStatement()) {
			ResultSet resultSet = statement.executeQuery(query);
			if (resultSet.next()) {
				expectedStudentId = resultSet.getInt(1);
				expectedGroupId = resultSet.getInt(4);
			}
		}
		Student foundStudent = DbManager.findFirstStudentByName(connection, expectedFirstName, expectedLastName);

		assertNotNull(foundStudent, "Student should not be null");
		assertEquals(expectedStudentId, foundStudent.getId(), "Student ID doesn't match");
		assertEquals(expectedFirstName, foundStudent.getFirstName(), "Student first name doesn't match");
		assertEquals(expectedLastName, foundStudent.getLastName(), "Student last name doesn't match");
		assertEquals(expectedGroupId, foundStudent.getGroup().getId(), "Group ID doesn't match");
		assertEquals(groupNames[expectedGroupId - 1], foundStudent.getGroup().getName(), "Group name doesn't match");
	}

	@Test
	void testFindStudentByWrongName() throws SQLException {
		String randomFirstName = UUID.randomUUID().toString();
		String randomLastName = UUID.randomUUID().toString();
		Student foundStudent = DbManager.findFirstStudentByName(connection, randomFirstName, randomLastName);
		assertNull(foundStudent, "Student should be null");
	}

	static IntStream provideGroupIds() {
		return IntStream.range(1, groupNames.length + 1);
	}

	@ParameterizedTest
	@MethodSource("provideGroupIds")
	void testFindStudentsByGroup(int groupId) throws SQLException {
		String query = "SELECT * FROM groups WHERE id = " + groupId;
		int expectedGroupId = -1;
		String expectedGroupName = null;
		try (Statement statement = connection.createStatement()) {
			ResultSet resultSet = statement.executeQuery(query);
			if (resultSet.next()) {
				expectedGroupId = resultSet.getInt(1);
				expectedGroupName = resultSet.getString(2);
			}
		}
		String expected = IntStream.range(0, firstNames.length).filter(i -> groupId == groupIds[i])
				.mapToObj(i -> firstNames[i] + " " + lastNames[i] + " " + groupNames[groupId - 1]).sorted()
				.collect(Collectors.joining(", "));
		Group expectedGroup = new Group(expectedGroupId, expectedGroupName);

		List<Student> foundStudents = DbManager.findStudentsByGroup(connection, expectedGroup);
		assertNotNull(foundStudents, "Student list should not be null");

		String actual = foundStudents.stream().map(Object::toString).sorted().collect(Collectors.joining(", "));
		assertEquals(expected, actual, "Student list doesn't match");
	}

	@Test
	void testFindStudentsByWrongGroup() throws SQLException {
		String randomGroupName = "Group " + UUID.randomUUID().toString();
		int groupId = groupNames.length + 1;
		Group randomGroup = new Group(groupId, randomGroupName);

		List<Student> foundStudents = DbManager.findStudentsByGroup(connection, randomGroup);

		assertNotNull(foundStudents, "Student list should not be null");
		assertTrue(foundStudents.isEmpty(), "Student list should be empty");
	}

	@ParameterizedTest
	@MethodSource("provideGroupIds")
	void testUpdateGroupById(int groupId) throws SQLException {
		String randomGroupName = "Group " + UUID.randomUUID().toString();
		Group randomGroup = new Group(groupId, randomGroupName);

		boolean isUpdated = DbManager.updateGroupById(connection, randomGroup);
		assertTrue(isUpdated, "The return value should be \"true\"");

		String query = "SELECT * FROM groups WHERE group_name = '" + randomGroupName + "'";
		int fetchedGroupId = -1;
		try (Statement statement = connection.createStatement()) {
			ResultSet resultSet = statement.executeQuery(query);
			if (resultSet.next()) {
				fetchedGroupId = resultSet.getInt(1);
			}
		}

		assertNotEquals(-1, fetchedGroupId, "The group was not found in the database");
		assertEquals(fetchedGroupId, randomGroup.getId(), "Group ID doesn't match");
	}

	@Test
	void testUpdateGroupByWrongId() throws SQLException {
		String randomGroupName = "Group " + UUID.randomUUID().toString();
		int groupId = groupNames.length + 1;
		Group randomGroup = new Group(groupId, randomGroupName);

		boolean isUpdated = DbManager.updateGroupById(connection, randomGroup);
		assertFalse(isUpdated, "The return value should be \"false\"");
	}

	static IntStream provideStudentIds() {
		return IntStream.range(1, firstNames.length + 1);
	}

	@ParameterizedTest
	@MethodSource("provideStudentIds")
	void testUpdateStudentById(int studentId) throws SQLException {
		String randomFirstName = UUID.randomUUID().toString();
		String randomLastName = UUID.randomUUID().toString();
		Random rnd = new Random();
		int randomGroupIndex = rnd.nextInt(groupNames.length);
		String randomGroupName = groupNames[randomGroupIndex];
		int randomGroupId = randomGroupIndex + 1;
		Group randomGroup = new Group(randomGroupId, randomGroupName);
		Student randomStudent = new Student(studentId, randomFirstName, randomLastName, randomGroup);

		boolean isUpdated = DbManager.updateStudentById(connection, randomStudent);
		assertTrue(isUpdated, "The return value should be \"true\"");

		String query = "SELECT * FROM students WHERE first_name = '$1' AND students.last_name = '$2'".replace("$1", randomFirstName).replace("$2", randomLastName);
		int fetchedStudentId = -1;
		int fetchedGroupId = -1;
		try (Statement statement = connection.createStatement()) {
			ResultSet resultSet = statement.executeQuery(query);
			if (resultSet.next()) {
				fetchedStudentId = resultSet.getInt(1);
				fetchedGroupId = resultSet.getInt(4);
			}
		}

		assertNotEquals(-1, fetchedStudentId, "The student was not found in the database");
		assertEquals(fetchedStudentId, randomStudent.getId(), "Student ID doesn't match");
		assertEquals(randomGroupId, fetchedGroupId, "Group ID doesn't match");
	}

	@Test
	void testUpdateStudentByWrongId() throws SQLException {
		int studentId = firstNames.length + 1;
		String randomFirstName = UUID.randomUUID().toString();
		String randomLastName = UUID.randomUUID().toString();
		Random rnd = new Random();
		int randomGroupIndex = rnd.nextInt(groupNames.length);
		String randomGroupName = groupNames[randomGroupIndex];
		int randomGroupId = randomGroupIndex + 1;
		Group randomGroup = new Group(randomGroupId, randomGroupName);
		Student randomStudent = new Student(studentId, randomFirstName, randomLastName, randomGroup);

		boolean isUpdated = DbManager.updateStudentById(connection, randomStudent);
		assertFalse(isUpdated, "The return value should be \"false\"");
	}
}