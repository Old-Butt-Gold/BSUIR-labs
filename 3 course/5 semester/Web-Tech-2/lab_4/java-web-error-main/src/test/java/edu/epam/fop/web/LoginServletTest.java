package edu.epam.fop.web;

import static org.mockito.ArgumentMatchers.anyString;
import static org.mockito.ArgumentMatchers.eq;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

import java.io.IOException;
import java.util.stream.Stream;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.DisplayNameGeneration;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.Arguments;
import org.junit.jupiter.params.provider.MethodSource;
import org.mockito.Mock;
import org.mockito.MockitoAnnotations;

import jakarta.servlet.RequestDispatcher;
import jakarta.servlet.ServletConfig;
import jakarta.servlet.ServletContext;
import jakarta.servlet.ServletException;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import jakarta.servlet.http.HttpSession;

@DisplayNameGeneration(ReplaceCamelCase.class)
public class LoginServletTest {

	private static final String USERNAME_PARAMETER = "username";
	private static final String PASSWORD_PARAMETER = "password";
	private static final String ERROR_MESSAGE_ATTRIBUTE = "errorMessage";
	private static final String USER_ATTRIBUTE = "user";
	private static final String HOME_PAGE = "home.jsp";
	private static final String LOGIN_PAGE = "login.jsp";

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

	private LoginServlet loginServlet;

	@BeforeEach
	public void setUp() throws ServletException {
		MockitoAnnotations.openMocks(this);

		when(servletConfigMock.getServletContext()).thenReturn(servletContextMock);
		when(request.getServletContext()).thenReturn(servletContextMock);

		loginServlet = new LoginServlet();
		loginServlet.init(servletConfigMock);
	}

	@ParameterizedTest
	@MethodSource("validCredentialsProvider")
	public void testDoPostWhenSuccessfulLogin(String username, String password) throws ServletException, IOException {
		when(request.getSession()).thenReturn(session);
		when(request.getParameter(USERNAME_PARAMETER)).thenReturn(username);
		when(request.getParameter(PASSWORD_PARAMETER)).thenReturn(password);

		loginServlet.doPost(request, response);

		verify(session).setAttribute(USER_ATTRIBUTE, username);
		verify(response).sendRedirect(HOME_PAGE);
	}

	@ParameterizedTest
	@MethodSource("invalidCredentialsProvider")
	public void testDoPostWhenUnsuccessfulLogin(String username, String password) throws ServletException, IOException {
		when(request.getRequestDispatcher(LOGIN_PAGE)).thenReturn(requestDispatcher);
		when(servletContextMock.getRequestDispatcher(LOGIN_PAGE)).thenReturn(requestDispatcher);
		when(request.getParameter(USERNAME_PARAMETER)).thenReturn(username);
		when(request.getParameter(PASSWORD_PARAMETER)).thenReturn(password);

		loginServlet.doPost(request, response);

		verify(request).setAttribute(eq(ERROR_MESSAGE_ATTRIBUTE), anyString());
		verify(requestDispatcher).forward(request, response);
	}

	private static Stream<Arguments> validCredentialsProvider() {
		return Stream.of(Arguments.of("user1", "password1"), Arguments.of("user2", "password2"));
	}

	private static Stream<Arguments> invalidCredentialsProvider() {
		return Stream.of(Arguments.of(null, "password1"), Arguments.of("", "password2"), Arguments.of("user1", null),
				Arguments.of("user2", ""));
	}
}