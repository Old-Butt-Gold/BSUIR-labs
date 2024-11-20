package edu.epam.fop.jdbc.create;

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

	public static List<Group> fetchGroups(Connection connection) throws SQLException {
		List<Group> groups = new ArrayList<>();
		String query = "SELECT id, group_name FROM groups";
		try (var statement = connection.createStatement();
			 ResultSet resultSet = statement.executeQuery(query)) {
			while (resultSet.next()) {
				int id = resultSet.getInt("id");
				String groupName = resultSet.getString("group_name");
				groups.add(new Group(id, groupName));
			}
		}
		return groups;
	}

	public static List<Student> fetchStudents(Connection connection) throws SQLException {
		List<Student> students = new ArrayList<>();
		String query = "SELECT students.id, first_name, last_name, group_id, group_name " +
				"FROM students " +
				"INNER JOIN groups ON groups.id = students.group_id";
		try (var statement = connection.createStatement();
			 ResultSet resultSet = statement.executeQuery(query)) {
			while (resultSet.next()) {
				int studentId = resultSet.getInt("id");
				String firstName = resultSet.getString("first_name");
				String lastName = resultSet.getString("last_name");
				int groupId = resultSet.getInt("group_id");
				String groupName = resultSet.getString("group_name");
				Group group = new Group(groupId, groupName);
				students.add(new Student(studentId, firstName, lastName, group));
			}
		}
		return students;
	}
}
