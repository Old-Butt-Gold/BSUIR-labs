package edu.epam.fop.jdbc.transactions;

import java.sql.Connection;
import java.sql.SQLException;
import java.util.List;

public class DbManager {

	private DbManager() {
		throw new UnsupportedOperationException();
	}

	public static boolean setGroupForStudents(Connection connection, Group group, List<Student> students) throws SQLException {
		String updateGroupQuery = "UPDATE students SET group_id = ? WHERE id = ?";
		boolean originalAutoCommit = connection.getAutoCommit();

		try (var preparedStatement = connection.prepareStatement(updateGroupQuery)) {
			connection.setAutoCommit(false);
			preparedStatement.setInt(1, group.getId());

			for (var student : students) {
				preparedStatement.setInt(2, student.getId());
				int rowsAffected = preparedStatement.executeUpdate();

				if (rowsAffected == 0) {
					throw new SQLException("Failed to update student: " + student.getId());
				}
			}

			connection.commit();
			return true;
		} catch (SQLException ex) {
			connection.rollback();
		} finally {
			connection.setAutoCommit(originalAutoCommit);
		}
		return false;
	}


	public static boolean deleteStudents(Connection connection, List<Student> students) throws SQLException {
		String deleteGroupQuery = "DELETE FROM students WHERE id = ?";
		boolean originalAutoCommit = connection.getAutoCommit();

		try (var preparedStatement = connection.prepareStatement(deleteGroupQuery)) {
			connection.setAutoCommit(false);

			for (var student : students) {
				preparedStatement.setInt(1, student.getId());
				int rowsAffected = preparedStatement.executeUpdate();

				if (rowsAffected == 0) {
					throw new SQLException("Failed to delete student: " + student.getId());
				}
			}

			connection.commit();
			return true;
		} catch (SQLException ex) {
			connection.rollback();
		} finally {
			connection.setAutoCommit(originalAutoCommit);
		}
		return false;
	}
}