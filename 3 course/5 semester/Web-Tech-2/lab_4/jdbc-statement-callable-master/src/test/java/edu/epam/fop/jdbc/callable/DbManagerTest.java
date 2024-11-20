package edu.epam.fop.jdbc.callable;

import static org.mockito.ArgumentMatchers.anyInt;
import static org.mockito.ArgumentMatchers.anyString;

import java.sql.CallableStatement;
import java.sql.Connection;
import java.sql.SQLException;

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
	private CallableStatement callableStatement;

	@Test
	void testCallCountGroups() throws SQLException {
		Mockito.when(connection.prepareCall(anyString())).thenReturn(callableStatement);
		InOrder inOrder = Mockito.inOrder(connection, callableStatement);

		DbManager.callCountGroups(connection);

		inOrder.verify(connection).prepareCall(anyString());
		inOrder.verify(callableStatement).registerOutParameter(anyInt(), anyInt());
		inOrder.verify(callableStatement).close();
	}

	@Test
	void testCallCountStudents() throws SQLException {
		Mockito.when(connection.prepareCall(anyString())).thenReturn(callableStatement);
		InOrder inOrder = Mockito.inOrder(connection, callableStatement);

		DbManager.callCountStudents(connection);

		inOrder.verify(connection).prepareCall(anyString());
		inOrder.verify(callableStatement).registerOutParameter(anyInt(), anyInt());
		inOrder.verify(callableStatement).close();
	}

	@Test
	void testCallCountStudentsByGroupId() throws SQLException {
		Mockito.when(connection.prepareCall(anyString())).thenReturn(callableStatement);
		InOrder inOrder = Mockito.inOrder(connection, callableStatement);

		DbManager.callCountStudentsByGroupId(connection, 1);

		inOrder.verify(connection).prepareCall(anyString());
		inOrder.verify(callableStatement).registerOutParameter(anyInt(), anyInt());
		inOrder.verify(callableStatement).close();
	}
}