package edu.epam.fop.jdbc.callable;

import java.sql.CallableStatement;
import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.SQLException;

public class DbManager {

	private DbManager() {
		throw new UnsupportedOperationException();
	}

	public static int callCountGroups(Connection connection) throws SQLException {
		String sql = "{CALL COUNT_GROUPS(?)}";

		try (CallableStatement callableStatement = connection.prepareCall(sql)) {
			callableStatement.registerOutParameter(1, java.sql.Types.INTEGER);
			callableStatement.execute();
			return callableStatement.getInt(1);
		}
	}

	public static int callCountStudents(Connection connection) throws SQLException {
		String sql = "{CALL COUNT_STUDENTS(?)}";

		try (CallableStatement callableStatement = connection.prepareCall(sql)) {
			callableStatement.registerOutParameter(1, java.sql.Types.INTEGER);
			callableStatement.execute();
			return callableStatement.getInt(1);
		}
	}

	public static int callCountStudentsByGroupId(Connection connection, int groupId) throws SQLException {
		String sql = "{CALL COUNT_STUDENTS_BY_GROUP_ID(?, ?)}";

		try (CallableStatement callableStatement = connection.prepareCall(sql)) {
			callableStatement.setInt(1, groupId);
			callableStatement.registerOutParameter(2, java.sql.Types.INTEGER);
			callableStatement.execute();
			return callableStatement.getInt(2);
		}
	}
}
