package edu.epam.fop.web;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.mockito.ArgumentMatchers.anyInt;
import static org.mockito.ArgumentMatchers.eq;
import static org.mockito.Mockito.doReturn;
import static org.mockito.Mockito.mock;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

import java.sql.Connection;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.concurrent.atomic.AtomicInteger;

import org.apache.commons.dbcp2.BasicDataSource;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.DisplayNameGeneration;
import org.junit.jupiter.api.Test;
import org.mockito.ArgumentCaptor;
import org.mockito.Mock;
import org.mockito.MockitoAnnotations;
import org.mockito.Spy;

import jakarta.servlet.ServletContext;
import jakarta.servlet.ServletContextEvent;

@DisplayNameGeneration(ReplaceCamelCase.class)
public class MyServletContextListenerTest {

	private static final String JDBC_DRIVER_CLASS_NAME_PARAMETER = "jdbcDriverClassName";
	private static final String JDBC_URL_PARAMETER = "jdbcUrl";
	private static final String USERNAME_PARAMETER = "username";
	private static final String PASSWORD_PARAMETER = "password";
	private static final String DATA_SOURCE_ATTRIBUTE = "dataSource";
	private static final String ACTIVE_USERS_COUNTER_ATTRIBUTE = "activeUsersCounter";

	private final String jdbcDriverClassName = "com.mysql.cj.jdbc.Driver";
	private final String jdbcUrl = "jdbc:mysql://localhost:3306/mysql";
	private final String username = "root";
	private final String password = "password";

	@Mock
	private ServletContextEvent servletContextEventMock;

	@Mock
	private ServletContext servletContextMock;

	@Mock
	private BasicDataSource dataSourceMock;

	@Spy
	private MyServletContextListener servletContextListenerSpy;

	@BeforeEach
	public void setUp() {
		MockitoAnnotations.openMocks(this);
		when(servletContextEventMock.getServletContext()).thenReturn(servletContextMock);
	}

	@Test
	public void testContextInitialized() throws ClassNotFoundException, SQLException {
		when(servletContextMock.getInitParameter(JDBC_DRIVER_CLASS_NAME_PARAMETER)).thenReturn(jdbcDriverClassName);
		when(servletContextMock.getInitParameter(JDBC_URL_PARAMETER)).thenReturn(jdbcUrl);
		when(servletContextMock.getInitParameter(USERNAME_PARAMETER)).thenReturn(username);
		when(servletContextMock.getInitParameter(PASSWORD_PARAMETER)).thenReturn(password);

		doReturn(dataSourceMock).when(servletContextListenerSpy).createDataSource();

		Connection connectionMock = mock(Connection.class);
		when(dataSourceMock.getConnection()).thenReturn(connectionMock);
		Statement statementMock = mock(Statement.class);
		when(connectionMock.createStatement()).thenReturn(statementMock);
		when(connectionMock.createStatement(anyInt(), anyInt())).thenReturn(statementMock);
		when(connectionMock.createStatement(anyInt(), anyInt(), anyInt())).thenReturn(statementMock);

		servletContextListenerSpy.contextInitialized(servletContextEventMock);

		verify(dataSourceMock).setDriverClassName(jdbcDriverClassName);
		verify(dataSourceMock).setUrl(jdbcUrl);
		verify(dataSourceMock).setUsername(username);
		verify(dataSourceMock).setPassword(password);

		verify(servletContextMock).setAttribute(DATA_SOURCE_ATTRIBUTE, dataSourceMock);

		ArgumentCaptor<AtomicInteger> captor = ArgumentCaptor.forClass(AtomicInteger.class);
		verify(servletContextMock).setAttribute(eq(ACTIVE_USERS_COUNTER_ATTRIBUTE), captor.capture());

		assertEquals(0, captor.getValue().get(), "The active user counter must be set to 0");
	}

	@Test
	public void testContextDestroyed() throws SQLException {
		servletContextListenerSpy.dataSource = dataSourceMock;

		servletContextListenerSpy.contextDestroyed(servletContextEventMock);

		verify(servletContextMock).removeAttribute(ACTIVE_USERS_COUNTER_ATTRIBUTE);
		verify(servletContextMock).removeAttribute(DATA_SOURCE_ATTRIBUTE);
		verify(dataSourceMock).close();
	}
}