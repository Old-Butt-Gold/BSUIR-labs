package edu.epam.fop.web;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertTrue;
import static org.mockito.ArgumentMatchers.eq;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

import java.io.IOException;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.DisplayNameGeneration;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.CsvSource;
import org.mockito.ArgumentCaptor;
import org.mockito.Mock;
import org.mockito.Mockito;
import org.mockito.MockitoAnnotations;

import jakarta.servlet.RequestDispatcher;
import jakarta.servlet.ServletConfig;
import jakarta.servlet.ServletContext;
import jakarta.servlet.ServletException;
import jakarta.servlet.http.Cookie;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;

@DisplayNameGeneration(ReplaceCamelCase.class)
public class GuessNumberCookieControllerTest {

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

	private GuessNumberCookieController guessNumberCookieController;

	@BeforeEach
	public void setUp() throws Exception {
		MockitoAnnotations.openMocks(this);

		when(servletConfigMock.getServletContext()).thenReturn(servletContextMock);

		guessNumberCookieController = new GuessNumberCookieController();
		guessNumberCookieController.logger = loggerMock;
	}

	@ParameterizedTest
	@CsvSource(value = { "2, 20, 2, 20", "5, 10, 5, 10", "null, null, 1, 50" }, nullValues = { "null" })
	public void testInit(String minNumber, String maxNumber, int expectedMinNumber, int expectedMaxNumber)
			throws ServletException {
		when(servletConfigMock.getInitParameter("minNumber")).thenReturn(minNumber);
		when(servletConfigMock.getInitParameter("maxNumber")).thenReturn(maxNumber);

		guessNumberCookieController.init(servletConfigMock);

		verify(servletContextMock).setAttribute("minNumber", expectedMinNumber);
		assertEquals(expectedMinNumber, guessNumberCookieController.minNumber,
				"The GuessNumberController.minNumber field has an incorrect value");

		verify(servletContextMock).setAttribute("maxNumber", expectedMaxNumber);
		assertEquals(expectedMaxNumber, guessNumberCookieController.maxNumber,
				"The GuessNumberController.maxNumber field has an incorrect value");

		boolean logIsCalled = Mockito.mockingDetails(loggerMock).getInvocations().stream()
				.anyMatch(invocation -> invocation.getMethod().getName().equals("log"));
		assertTrue(logIsCalled, "None of the log methods were called");
	}

	@Test
	void testHandleDefault() throws ServletException {
		guessNumberCookieController.init(servletConfigMock);
		String pathToForward = guessNumberCookieController.handleDefault();

		assertEquals("/index.html", pathToForward, "Any other request should be forwarded to the index.html page");
	}

	@ParameterizedTest
	@CsvSource({ "John, -1, 1", "Jane, -10, -5", "Anyone, 5, 10" })
	void testHandleStart(String userName, int minNumber, int maxNumber) throws ServletException {
		when(httpServletRequestMock.getParameter("name")).thenReturn(userName);

		guessNumberCookieController.init(servletConfigMock);
		guessNumberCookieController.minNumber = minNumber;
		guessNumberCookieController.maxNumber = maxNumber;
		String pathToForward = guessNumberCookieController.handleStart(httpServletRequestMock, httpServletResponseMock);

		verify(httpServletRequestMock).setAttribute("name", userName);
		verify(httpServletRequestMock).setAttribute("tryCount", 0);
		ArgumentCaptor<Integer> captor = ArgumentCaptor.forClass(Integer.class);
		verify(httpServletRequestMock).setAttribute(eq("randomNumber"), captor.capture());
		int capturedValue = captor.getValue();
		assertTrue(capturedValue >= minNumber && capturedValue <= maxNumber,
				"You should generate a random number in the range minNumber to maxNumber inclusive and save it in the randomNumber request attribute");
		assertEquals("/WEB-INF/start.jsp", pathToForward,
				"You should return the correct path to forward the request to");
	}

	@ParameterizedTest
	@CsvSource(value = { "John, null, 0, 0, 1, MISSING, Please enter a number, /WEB-INF/tryIt.jsp", // #1
			"Jane, abc, 0, 1, 2, INVALID, Invalid value entered, /WEB-INF/tryIt.jsp", // #2
			"Anyone, 3, 5, 2, 3, LESS, Your number is less than the number being guessed, /WEB-INF/tryIt.jsp", // #3
			"John, 5, 4, 3, 4, GREATER, Your number is greater than the number being guessed, /WEB-INF/tryIt.jsp", // #4
			"John, 4, 4, 4, 5, WIN, You win!, /WEB-INF/finish.jsp", // #5
			"John, 4, 4, 5, 6, WIN, You win!, /WEB-INF/finish.jsp", // #6
			"Jane, 4, 4, 6, 7, WIN, You win!, /WEB-INF/finish.jsp", // #7
			"John, 4, 4, 7, 8, WIN, You win!, /WEB-INF/finish.jsp", // #8
			"John, 4, 4, 8, 9, WIN, You win!, /WEB-INF/finish.jsp", // #9
			"John, 4, 4, 9, 10, WIN, You win!, /WEB-INF/finish.jsp", // #10
			"John, 4, 6, 10, 11, LOSE, You lose, /WEB-INF/finish.jsp", // #11
			"John, 4, 4, 11, 12, LOSE, You lose, /WEB-INF/finish.jsp", // #12
			"Jane, 4, 4, 12, 13, LOSE, You lose, /WEB-INF/finish.jsp" // #13
	}, nullValues = { "null" })
	void testHandleTry(String name, String number, int randomNumber, int tryCount, int expectedTryCount, String status,
			String expectedMessage, String expectedPage) throws ServletException {

		when(httpServletRequestMock.getParameter("number")).thenReturn(number);
		Cookie[] cookies = { new Cookie("name", name), new Cookie("tryCount", String.valueOf(tryCount)),
				new Cookie("randomNumber", String.valueOf(randomNumber)) };
		when(httpServletRequestMock.getCookies()).thenReturn(cookies);

		guessNumberCookieController.init(servletConfigMock);
		String pathToForward = guessNumberCookieController.handleTry(httpServletRequestMock, httpServletResponseMock);

		verify(httpServletRequestMock).setAttribute("tryCount", expectedTryCount);
		verify(httpServletRequestMock).setAttribute("result", expectedMessage);
		assertEquals(expectedPage, pathToForward, "You should return the correct path to forward the request to");
	}

	@ParameterizedTest
	@CsvSource(value = { "abc, 1, 2, start, /WEB-INF/start.jsp", "defg, 3, 4, tryIt, /WEB-INF/tryIt.jsp",
			"hijkl, 5, 6, anyCommand, /index.html", "mnopqr, 7, 8, null, /index.html" }, nullValues = { "null" })
	public void testService(String name, String tryCount, String randomNumber, String command, String expectedPage)
			throws IOException, ServletException {
		when(httpServletRequestMock.getParameter("command")).thenReturn(command);
		when(httpServletRequestMock.getRequestDispatcher(expectedPage)).thenReturn(requestDispatcherMock);
		when(servletContextMock.getRequestDispatcher(expectedPage)).thenReturn(requestDispatcherMock);

		when(httpServletRequestMock.getAttribute("name")).thenReturn(name);
		when(httpServletRequestMock.getAttribute("tryCount")).thenReturn(tryCount);
		when(httpServletRequestMock.getAttribute("randomNumber")).thenReturn(randomNumber);

		guessNumberCookieController.init(servletConfigMock);
		guessNumberCookieController.service(httpServletRequestMock, httpServletResponseMock);
		verify(requestDispatcherMock).forward(httpServletRequestMock, httpServletResponseMock);
		if ("start".equals(command) || "tryIt".equals(command)) {
			verify(httpServletResponseMock).addCookie(new Cookie("name", name));
			verify(httpServletResponseMock).addCookie(new Cookie("tryCount", tryCount));
			verify(httpServletResponseMock).addCookie(new Cookie("randomNumber", randomNumber));
		}
		boolean logIsCalled = Mockito.mockingDetails(loggerMock).getInvocations().stream()
				.anyMatch(invocation -> invocation.getMethod().getName().equals("log"));
		assertTrue(logIsCalled, "None of the log methods were called");
	}
}