package edu.epam.fop.web;

import static org.mockito.ArgumentMatchers.any;
import static org.mockito.ArgumentMatchers.anyInt;
import static org.mockito.Mockito.doReturn;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

import java.io.IOException;
import java.util.Arrays;
import java.util.List;

import javax.sql.DataSource;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.DisplayNameGeneration;
import org.junit.jupiter.api.Test;
import org.mockito.Mock;
import org.mockito.MockitoAnnotations;
import org.mockito.Spy;

import jakarta.servlet.RequestDispatcher;
import jakarta.servlet.ServletConfig;
import jakarta.servlet.ServletContext;
import jakarta.servlet.ServletException;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;

@DisplayNameGeneration(ReplaceCamelCase.class)
public class MyServletTest {

	private static final String DATA_SOURCE_ATTRIBUTE = "dataSource";
	private static final String STUDENT_LIST_ATTRIBUTE = "studentList";
	private static final String TEACHER_LIST_ATTRIBUTE = "teacherList";
	private static final String RESULT_PAGE = "/WEB-INF/result.jsp";

	@Mock
	private HttpServletRequest requestMock;

	@Mock
	private HttpServletResponse responseMock;

	@Mock
	private RequestDispatcher requestDispatcherMock;

	@Mock
	private ServletConfig servletConfigMock;

	@Mock
	private ServletContext servletContextMock;

	@Mock
	private DataSource dataSourceMock;

	@Mock
	DbManager dbManagerMock;

	@Spy
	private MyServlet myServletSpy;

	@BeforeEach
	public void setUp() throws ServletException {
		MockitoAnnotations.openMocks(this);
	}

	@Test
	public void testDoGet() throws ServletException, IOException {
		when(servletConfigMock.getServletContext()).thenReturn(servletContextMock);
		when(requestMock.getServletContext()).thenReturn(servletContextMock);
		when(servletContextMock.getAttribute(DATA_SOURCE_ATTRIBUTE)).thenReturn(dataSourceMock);
		when(requestMock.getRequestDispatcher(RESULT_PAGE)).thenReturn(requestDispatcherMock);
		when(servletContextMock.getRequestDispatcher(RESULT_PAGE)).thenReturn(requestDispatcherMock);

		List<String> list = Arrays.asList("John", "Alice");
		when(dbManagerMock.getUsersByRole(anyInt())).thenReturn(list);

		doReturn(dbManagerMock).when(myServletSpy).createDbManager(any());

		myServletSpy.init(servletConfigMock);
		myServletSpy.doGet(requestMock, responseMock);

		verify(servletContextMock).getAttribute(DATA_SOURCE_ATTRIBUTE);
		verify(myServletSpy).createDbManager(dataSourceMock);
		verify(requestMock).setAttribute(STUDENT_LIST_ATTRIBUTE, list);
		verify(requestMock).setAttribute(TEACHER_LIST_ATTRIBUTE, list);
		verify(requestDispatcherMock).forward(requestMock, responseMock);
	}
}