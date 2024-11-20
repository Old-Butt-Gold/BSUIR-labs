package edu.epam.fop.web;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.mockito.ArgumentMatchers.anyString;
import static org.mockito.ArgumentMatchers.eq;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

import java.io.IOException;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.DisplayNameGeneration;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.ValueSource;
import org.mockito.Mock;
import org.mockito.MockitoAnnotations;

import edu.epam.fop.web.exceptions.UserNotLoggedInException;
import jakarta.servlet.RequestDispatcher;
import jakarta.servlet.ServletConfig;
import jakarta.servlet.ServletContext;
import jakarta.servlet.ServletException;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import jakarta.servlet.http.HttpSession;

@DisplayNameGeneration(ReplaceCamelCase.class)
public class FinesServletTest {

	private static final String USER_ATTRIBUTE = "user";
	private static final String RESULT_ATTRIBUTE = "result";
	private static final String RESULT_PAGE = "result.jsp";

	@Mock
	private ServletConfig servletConfigMock;

	@Mock
	private ServletContext servletContextMock;

	@Mock
	private HttpServletRequest request;

	@Mock
	private HttpServletResponse response;

	@Mock
	private RequestDispatcher requestDispatcher;

	@Mock
	private HttpSession session;

	private FinesServlet finesServlet;

	@BeforeEach
	public void setUp() throws ServletException {
		MockitoAnnotations.openMocks(this);

		when(servletConfigMock.getServletContext()).thenReturn(servletContextMock);
		when(request.getServletContext()).thenReturn(servletContextMock);
		when(request.getSession(false)).thenReturn(session);
		when(request.getRequestDispatcher(RESULT_PAGE)).thenReturn(requestDispatcher);
		when(servletContextMock.getRequestDispatcher(RESULT_PAGE)).thenReturn(requestDispatcher);

		finesServlet = new FinesServlet();
		finesServlet.init(servletConfigMock);
	}

	@ParameterizedTest
	@ValueSource(strings = { "user1", "user2" })
	public void testDoGetWhenUserIsLoggedIn(String userAttribute) throws ServletException, IOException {
		when(session.getAttribute(USER_ATTRIBUTE)).thenReturn(userAttribute);

		finesServlet.doGet(request, response);

		verify(request).setAttribute(eq(RESULT_ATTRIBUTE), anyString());
		verify(requestDispatcher).forward(request, response);
	}

	@Test
	public void testDoGetWhenUserIsNotLoggedIn() throws ServletException, IOException {
		when(session.getAttribute(USER_ATTRIBUTE)).thenReturn(null);

		int oldValue = UserNotLoggedInException.getCounter();
		finesServlet.doGet(request, response);
		int newValue = UserNotLoggedInException.getCounter();

		assertEquals(oldValue + 1, newValue, "You should throw and catch UserNotLoggedInException");
		verify(response).sendError(eq(HttpServletResponse.SC_FORBIDDEN), anyString());
	}
}