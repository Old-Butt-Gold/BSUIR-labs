package edu.epam.fop.jdbc.prepared;

import static org.mockito.ArgumentMatchers.anyInt;
import static org.mockito.ArgumentMatchers.anyString;
import static org.mockito.ArgumentMatchers.eq;
import static org.mockito.Mockito.atLeast;
import static org.mockito.Mockito.when;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;

import org.junit.jupiter.api.DisplayNameGeneration;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.ArgumentMatchers;
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
	private PreparedStatement preparedStatement;

	@Mock
	private ResultSet resultSet;

	@Test
	void testInsertGroup() throws SQLException {
		when(connection.prepareStatement(anyString(), ArgumentMatchers.eq(Statement.RETURN_GENERATED_KEYS)))
				.thenReturn(preparedStatement);
		when(preparedStatement.executeUpdate()).thenReturn(1);
		when(preparedStatement.getGeneratedKeys()).thenReturn(resultSet);
		InOrder inOrder = Mockito.inOrder(connection, preparedStatement, resultSet);

		Group group = new Group("Group A");
		DbManager.insertGroup(connection, group);

		inOrder.verify(connection).prepareStatement(anyString(), eq(Statement.RETURN_GENERATED_KEYS));
		inOrder.verify(preparedStatement).executeUpdate();
		inOrder.verify(preparedStatement).getGeneratedKeys();
		inOrder.verify(resultSet).close();
		inOrder.verify(preparedStatement).close();
	}

	@Test
	void testInsertStudent() throws SQLException {
		when(connection.prepareStatement(Mockito.anyString(), ArgumentMatchers.eq(Statement.RETURN_GENERATED_KEYS)))
				.thenReturn(preparedStatement);
		when(preparedStatement.executeUpdate()).thenReturn(1);
		when(preparedStatement.getGeneratedKeys()).thenReturn(resultSet);
		InOrder inOrder = Mockito.inOrder(connection, preparedStatement, resultSet);

		Group group = new Group(1, "Group A");
		Student student = new Student("John", "Doe", group);
		DbManager.insertStudent(connection, student);

		inOrder.verify(connection).prepareStatement(anyString(), ArgumentMatchers.eq(Statement.RETURN_GENERATED_KEYS));
		inOrder.verify(preparedStatement).executeUpdate();
		inOrder.verify(preparedStatement).getGeneratedKeys();
		inOrder.verify(resultSet).close();
		inOrder.verify(preparedStatement).close();
}

	@Test
	void testFindFirstGroupByName() throws SQLException {
		when(connection.prepareStatement(Mockito.anyString())).thenReturn(preparedStatement);
		when(preparedStatement.executeQuery()).thenReturn(resultSet);
		InOrder inOrder = Mockito.inOrder(connection, preparedStatement, resultSet);

		DbManager.findFirstGroupByName(connection, "Group A");

		inOrder.verify(connection).prepareStatement(anyString());
		inOrder.verify(preparedStatement).setString(anyInt(), anyString());
		inOrder.verify(preparedStatement).executeQuery();
		inOrder.verify(resultSet).close();
		inOrder.verify(preparedStatement).close();
	}

	@Test
	void testFindFirstStudentByName() throws SQLException {
		when(connection.prepareStatement(Mockito.anyString())).thenReturn(preparedStatement);
		when(preparedStatement.executeQuery()).thenReturn(resultSet);
		InOrder inOrder = Mockito.inOrder(connection, preparedStatement, resultSet);

		DbManager.findFirstStudentByName(connection, "John", "Doe");

		inOrder.verify(connection).prepareStatement(anyString());
		inOrder.verify(preparedStatement, atLeast(1)).setString(anyInt(), anyString());
		inOrder.verify(preparedStatement).executeQuery();
		inOrder.verify(resultSet).close();
		inOrder.verify(preparedStatement).close();
	}

	@Test
	void testFindStudentsByGroup() throws SQLException {
		when(connection.prepareStatement(Mockito.anyString())).thenReturn(preparedStatement);
		when(preparedStatement.executeQuery()).thenReturn(resultSet);
		InOrder inOrder = Mockito.inOrder(connection, preparedStatement, resultSet);

		Group group = new Group(1, "Group A");
		DbManager.findStudentsByGroup(connection, group);

		inOrder.verify(connection).prepareStatement(anyString());
		inOrder.verify(preparedStatement).executeQuery();
		inOrder.verify(resultSet).close();
		inOrder.verify(preparedStatement).close();
	}

	@Test
	void testUpdateGroupById() throws SQLException {
		when(connection.prepareStatement(Mockito.anyString())).thenReturn(preparedStatement);
		when(preparedStatement.executeUpdate()).thenReturn(1);
		InOrder inOrder = Mockito.inOrder(connection, preparedStatement);

		Group group = new Group(1, "Group A");
		DbManager.updateGroupById(connection, group);

		inOrder.verify(connection).prepareStatement(anyString());
		inOrder.verify(preparedStatement, atLeast(1)).setString(anyInt(), anyString());
		inOrder.verify(preparedStatement).executeUpdate();
		inOrder.verify(preparedStatement).close();
	}

	@Test
	void testUpdateStudentById() throws SQLException {
		when(connection.prepareStatement(Mockito.anyString())).thenReturn(preparedStatement);
		when(preparedStatement.executeUpdate()).thenReturn(1);
		InOrder inOrder = Mockito.inOrder(connection, preparedStatement);

		Group group = new Group(1, "Group A");
		Student student = new Student(1, "John", "Doe", group);
		DbManager.updateStudentById(connection, student);

		inOrder.verify(connection).prepareStatement(anyString());
		inOrder.verify(preparedStatement, atLeast(1)).setString(anyInt(), anyString());
		inOrder.verify(preparedStatement).executeUpdate();
		inOrder.verify(preparedStatement).close();
	}
}