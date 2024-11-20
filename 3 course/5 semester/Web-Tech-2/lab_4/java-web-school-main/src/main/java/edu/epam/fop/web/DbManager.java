package edu.epam.fop.web;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.ArrayList;
import java.util.List;

import javax.sql.DataSource;

public class DbManager {
	private DataSource dataSource;

	public DbManager(DataSource dataSource) {
		this.dataSource = dataSource;
	}

	public List<String> getUsersByRole(int role) {
		ArrayList<String> users = new ArrayList<>();

		final String query = "SELECT * FROM users WHERE role = ?";

		try (Connection connection = dataSource.getConnection();
				PreparedStatement ps = connection.prepareStatement(query)) {
			ps.setInt(1, role);
			try (ResultSet resultSet = ps.executeQuery()) {
				while (resultSet.next()) {
					users.add(resultSet.getString("name"));
				}
			}
		} catch (SQLException e) {
			e.printStackTrace();
		}
		return users;
	}
}