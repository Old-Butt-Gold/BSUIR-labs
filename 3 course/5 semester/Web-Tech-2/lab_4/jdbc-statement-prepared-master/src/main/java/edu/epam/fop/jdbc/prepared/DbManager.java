package edu.epam.fop.jdbc.prepared;

import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.ArrayList;
import java.util.List;

public class DbManager {

	private DbManager() {
		throw new UnsupportedOperationException();
	}

	public static boolean insertGroup(Connection connection, Group group) throws SQLException {
		String query = "INSERT INTO groups(group_name) VALUES (?)";
		try (var preparedStatement = connection.prepareStatement(query, Statement.RETURN_GENERATED_KEYS)) {
			preparedStatement.setString(1, group.getName());
			int affectedRows = preparedStatement.executeUpdate();

			if (affectedRows > 0) {
				try (ResultSet generatedKeys = preparedStatement.getGeneratedKeys()) {
					if (generatedKeys.next()) {
						group.setId(generatedKeys.getInt(1));
					}
				}
			}
			return affectedRows > 0;
		}
	}

	public static boolean insertStudent(Connection connection, Student student) throws SQLException {
		String query = "INSERT INTO students(first_name, last_name, group_id) VALUES (?, ?, ?)";
		try (var preparedStatement = connection.prepareStatement(query, Statement.RETURN_GENERATED_KEYS)) {
			preparedStatement.setString(1, student.getFirstName());
			preparedStatement.setString(2, student.getLastName());
			preparedStatement.setInt(3, student.getGroup().getId());
			int affectedRows = preparedStatement.executeUpdate();

			if (affectedRows > 0) {
				try (ResultSet generatedKeys = preparedStatement.getGeneratedKeys()) {
					if (generatedKeys.next()) {
						student.setId(generatedKeys.getInt(1));
					}
				}
			}
			return affectedRows > 0;
		}
	}

	public static Group findFirstGroupByName(Connection connection, String name) throws SQLException {
		String query = "SELECT id, group_name FROM groups WHERE group_name = ?";
		try (var preparedStatement = connection.prepareStatement(query)) {
			preparedStatement.setString(1, name);
			try (ResultSet resultSet = preparedStatement.executeQuery()) {
				if (resultSet.next()) {
					return new Group(resultSet.getInt("id"), resultSet.getString("group_name"));
				}
			}
		}
		return null;
	}

	public static Student findFirstStudentByName(Connection connection, String firstName, String lastName)
			throws SQLException {
		String query = "SELECT students.id, first_name, last_name, group_id, group_name " +
				"FROM students " +
				"INNER JOIN groups ON groups.id = students.group_id " +
				"WHERE first_name = ? AND last_name = ?";
		try (var preparedStatement = connection.prepareStatement(query)) {
			preparedStatement.setString(1, firstName);
			preparedStatement.setString(2, lastName);
			try (ResultSet resultSet = preparedStatement.executeQuery()) {
				if (resultSet.next()) {
					Group group = new Group(resultSet.getInt("group_id"), resultSet.getString("group_name"));
					return new Student(resultSet.getInt("students.id"), resultSet.getString("first_name"),
							resultSet.getString("last_name"), group);
				}
			}
		}
		return null;
	}

	public static List<Student> findStudentsByGroup(Connection connection, Group group) throws SQLException {
		String query = "SELECT id, first_name, last_name FROM students WHERE group_id = ?";
		List<Student> students = new ArrayList<>();
		try (var preparedStatement = connection.prepareStatement(query)) {
			preparedStatement.setInt(1, group.getId());
			try (ResultSet resultSet = preparedStatement.executeQuery()) {
				while (resultSet.next()) {
					students.add(new Student(resultSet.getInt("id"), resultSet.getString("first_name"),
							resultSet.getString("last_name"), group));
				}
			}
		}
		return students;
	}

	public static boolean updateGroupById(Connection connection, Group group) throws SQLException {
		String query = "UPDATE groups SET group_name = ? WHERE id = ?";
		try (var preparedStatement = connection.prepareStatement(query)) {
			preparedStatement.setString(1, group.getName());
			preparedStatement.setInt(2, group.getId());
			return preparedStatement.executeUpdate() > 0;
		}
	}

	public static boolean updateStudentById(Connection connection, Student student) throws SQLException {
		String query = "UPDATE students SET first_name = ?, last_name = ?, group_id = ? WHERE id = ?";
		try (var preparedStatement = connection.prepareStatement(query)) {
			preparedStatement.setString(1, student.getFirstName());
			preparedStatement.setString(2, student.getLastName());
			preparedStatement.setInt(3, student.getGroup().getId());
			preparedStatement.setInt(4, student.getId());
			return preparedStatement.executeUpdate() > 0;
		}
	}
}