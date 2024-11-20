package edu.epam.fop.jdbc.transactions;

import static org.mockito.ArgumentMatchers.anyString;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.SQLException;
import java.util.Arrays;
import java.util.List;

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
	private PreparedStatement preparedStatement;

	@Test
	void testSetGroupForStudents() throws SQLException {
		Mockito.when(connection.getAutoCommit()).thenReturn(true);
		Mockito.when(connection.prepareStatement(anyString())).thenReturn(preparedStatement);
		InOrder inOrder = Mockito.inOrder(connection);

		Group group = new Group(1, "Group A");
		List<Student> students = Arrays.asList(new Student(1, "John", "Doe", group),
				new Student(2, "Jane", "Doe", group));
		DbManager.setGroupForStudents(connection, group, students);

		inOrder.verify(connection).setAutoCommit(false);
		inOrder.verify(connection).rollback();
	}

	@Test
	void testDeleteStudents() throws SQLException {
		Mockito.when(connection.getAutoCommit()).thenReturn(true);
		Mockito.when(connection.prepareStatement(anyString())).thenReturn(preparedStatement);
		InOrder inOrder = Mockito.inOrder(connection);

		Group group = new Group(1, "Group A");
		List<Student> students = Arrays.asList(new Student(1, "John", "Doe", group),
				new Student(2, "Jane", "Doe", group));
		DbManager.deleteStudents(connection, students);

		inOrder.verify(connection).setAutoCommit(false);
		inOrder.verify(connection).rollback();
	}
}