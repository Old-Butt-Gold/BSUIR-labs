package edu.epam.fop.jdbc.create;

import static org.mockito.ArgumentMatchers.anyString;
import static org.mockito.Mockito.when;

import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.DisplayNameGeneration;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.InOrder;
import org.mockito.Mock;
import org.mockito.Mockito;
import org.mockito.junit.jupiter.MockitoExtension;

@DisplayNameGeneration(ReplaceCamelCase.class)
@ExtendWith(MockitoExtension.class)
class DbManagerTest {

	@Mock
	private Connection connection;

	@Mock
	private Statement statement;

	@Mock
	private ResultSet resultSet;

	@BeforeEach
	void setUp() throws SQLException {
		when(connection.createStatement()).thenReturn(statement);
		when(statement.executeQuery(anyString())).thenReturn(resultSet);
	}

	@Test
	void testFetchGroups() throws SQLException {
		InOrder inOrder = Mockito.inOrder(connection, statement, resultSet);

		DbManager.fetchGroups(connection);

		inOrder.verify(connection).createStatement();
		inOrder.verify(statement).executeQuery(anyString());
		inOrder.verify(resultSet).close();
		inOrder.verify(statement).close();
	}

	@Test
	void testFetchStudents() throws SQLException {
		InOrder inOrder = Mockito.inOrder(connection, statement, resultSet);

		DbManager.fetchStudents(connection);

		inOrder.verify(connection).createStatement();
		inOrder.verify(statement).executeQuery(anyString());
		inOrder.verify(resultSet).close();
		inOrder.verify(statement).close();
	}
}