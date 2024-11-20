package edu.epam.fop.web;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertTrue;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

import java.io.IOException;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.DisplayNameGeneration;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.CsvSource;
import org.mockito.Mock;
import org.mockito.Mockito;
import org.mockito.MockitoAnnotations;

import jakarta.servlet.RequestDispatcher;
import jakarta.servlet.ServletConfig;
import jakarta.servlet.ServletContext;
import jakarta.servlet.ServletException;
import jakarta.servlet.annotation.WebServlet;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;

@DisplayNameGeneration(ReplaceCamelCase.class)
public class GuessNumberControllerTest {

	@Mock
	private ServletConfig servletConfigMock;

	@Mock
	private ServletContext servletContextMock;

	@Mock
	RequestDispatcher requestDispatcherMock;

	@Mock
	private HttpServletRequest httpServletRequestMock;

	@Mock
	private HttpServletResponse httpServletResponseMock;

	@Mock
	System.Logger loggerMock;

	private GuessNumberController guessNumberController;

	@BeforeEach
	public void setUp() throws Exception {
		MockitoAnnotations.openMocks(this);

		when(servletConfigMock.getServletContext()).thenReturn(servletContextMock);

		guessNumberController = new GuessNumberController();
		guessNumberController.logger = loggerMock;
	}

	@Test
	void testAnnotation() {
		WebServlet annotation = GuessNumberController.class.getAnnotation(WebServlet.class);
		assertEquals("/GuessNumber", annotation.value()[0]);
	}

	@ParameterizedTest
	@CsvSource(value = { "2, 20, 2, 20", "5, 10, 5, 10", "null, null, 1, 50" }, nullValues = { "null" })
	public void testInit(String minNumber, String maxNumber, int expectedMinNumber, int expectedMaxNumber)
			throws ServletException {
		when(servletConfigMock.getInitParameter("minNumber")).thenReturn(minNumber);
		when(servletConfigMock.getInitParameter("maxNumber")).thenReturn(maxNumber);

		guessNumberController.init(servletConfigMock);

		verify(servletContextMock).setAttribute("minNumber", expectedMinNumber);
		assertEquals(expectedMinNumber, guessNumberController.minNumber,
				"The GuessNumberController.minNumber field has an incorrect value");

		verify(servletContextMock).setAttribute("maxNumber", expectedMaxNumber);
		assertEquals(expectedMaxNumber, guessNumberController.maxNumber,
				"The GuessNumberController.maxNumber field has an incorrect value");

		boolean logIsCalled = Mockito.mockingDetails(loggerMock).getInvocations().stream()
				.anyMatch(invocation -> invocation.getMethod().getName().equals("log"));
		assertTrue(logIsCalled, "None of the log methods were called");
	}

	@Test
	void testDestroy() throws ServletException {
		guessNumberController.init(servletConfigMock);
		guessNumberController.destroy();

		boolean logIsCalled = Mockito.mockingDetails(loggerMock).getInvocations().stream()
				.anyMatch(invocation -> invocation.getMethod().getName().equals("log"));
		assertTrue(logIsCalled, "None of the log methods were called");
	}

	@Test
	void testHandleDefault() throws ServletException {
		guessNumberController.init(servletConfigMock);
		String pathToForward = guessNumberController.handleDefault();

		assertEquals("/index.html", pathToForward, "Any other request should be forwarded to the index.html page");
	}

	@ParameterizedTest
	@CsvSource({ "John, -1, 1", "Jane, -10, -5", "Anyone, 5, 10" })
	void testHandleStart(String userName, int minNumber, int maxNumber) throws ServletException {
		when(httpServletRequestMock.getParameter("name")).thenReturn(userName);

		guessNumberController.init(servletConfigMock);
		guessNumberController.minNumber = minNumber;
		guessNumberController.maxNumber = maxNumber;
		String pathToForward = guessNumberController.handleStart(httpServletRequestMock);

		verify(httpServletRequestMock).setAttribute("name", userName);

		assertEquals("/WEB-INF/start.jsp", pathToForward,
				"You should return the correct path to forward the request to");
		assertEquals(userName, guessNumberController.name,
				"The GuessNumberController.name field has an incorrect value");
		assertTrue(
				(guessNumberController.randomNumber <= guessNumberController.maxNumber)
						&& (guessNumberController.randomNumber >= guessNumberController.minNumber),
				"It is necessary to generate a random number in the range GuessNumberController.minNumber to GuessNumberController.maxNumber inclusive and write it into the GuessNumberController.randomNumber variable");
		assertEquals(0, guessNumberController.tryCount, "The guessNumberController.tryCount field must be set to 0");
	}

	@ParameterizedTest
	@CsvSource(value = { "null, 0, 0, 1, MISSING, Please enter a number, /WEB-INF/tryIt.jsp",
			"abc, 0, 1, 2, INVALID, Invalid value entered, /WEB-INF/tryIt.jsp",
			"3, 5, 2, 3, LESS, Your number is less than the number being guessed, /WEB-INF/tryIt.jsp",
			"5, 4, 3, 4, GREATER, Your number is greater than the number being guessed, /WEB-INF/tryIt.jsp",
			"4, 4, 4, 5, WIN, You win!, /WEB-INF/finish.jsp", "4, 4, 5, 6, WIN, You win!, /WEB-INF/finish.jsp",
			"4, 4, 6, 7, WIN, You win!, /WEB-INF/finish.jsp", "4, 4, 7, 8, WIN, You win!, /WEB-INF/finish.jsp",
			"4, 4, 8, 9, WIN, You win!, /WEB-INF/finish.jsp", "4, 4, 9, 10, WIN, You win!, /WEB-INF/finish.jsp",
			"4, 6, 10, 11, LOSE, You lose, /WEB-INF/finish.jsp" }, nullValues = { "null" })
	void testHandleTry(String number, int randomNumber, int tryCount, int expectedTryCount, String status,
			String expectedMessage, String expectedPage) throws ServletException {

		when(httpServletRequestMock.getParameter("number")).thenReturn(number);

		guessNumberController.init(servletConfigMock);
		guessNumberController.randomNumber = randomNumber;
		guessNumberController.tryCount = tryCount;

		String pathToForward = guessNumberController.handleTry(httpServletRequestMock);

		assertEquals(expectedTryCount, guessNumberController.tryCount,
				"The GuessNumberController.tryCount field has an incorrect value");
		verify(httpServletRequestMock).setAttribute("tryCount", expectedTryCount);
		verify(httpServletRequestMock).setAttribute("result", expectedMessage);
		assertEquals(expectedPage, pathToForward, "You should return the correct path to forward the request to");
	}

	@ParameterizedTest
	@CsvSource(value = { "start, /WEB-INF/start.jsp", "tryIt, /WEB-INF/tryIt.jsp", "anyCommand, /index.html",
			"null, /index.html" }, nullValues = { "null" })
	public void testService(String command, String expectedPage) throws IOException, ServletException {
		when(httpServletRequestMock.getParameter("command")).thenReturn(command);
		when(httpServletRequestMock.getRequestDispatcher(expectedPage)).thenReturn(requestDispatcherMock);
		when(servletContextMock.getRequestDispatcher(expectedPage)).thenReturn(requestDispatcherMock);

		guessNumberController.init(servletConfigMock);
		guessNumberController.service(httpServletRequestMock, httpServletResponseMock);

		verify(requestDispatcherMock).forward(httpServletRequestMock, httpServletResponseMock);

		boolean logIsCalled = Mockito.mockingDetails(loggerMock).getInvocations().stream()
				.anyMatch(invocation -> invocation.getMethod().getName().equals("log"));
		assertTrue(logIsCalled, "None of the log methods were called");
	}
}