package edu.epam.fop.web;

import static org.junit.jupiter.api.Assertions.assertDoesNotThrow;
import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.mockito.ArgumentMatchers.eq;
import static org.mockito.Mockito.reset;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

import java.util.stream.Stream;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.DisplayNameGeneration;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.Arguments;
import org.junit.jupiter.params.provider.CsvSource;
import org.junit.jupiter.params.provider.MethodSource;
import org.mockito.ArgumentCaptor;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.MockitoAnnotations;

import jakarta.servlet.RequestDispatcher;
import jakarta.servlet.ServletContext;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;

@DisplayNameGeneration(ReplaceCamelCase.class)
public class CalculatorServletTest {

	@Mock
	ServletContext servletContext;

	@Mock
	RequestDispatcher requestDispatcher;

	@Mock
	HttpServletRequest request;

	@Mock
	HttpServletResponse response;

	@InjectMocks
	CalculatorServlet servlet;

	@BeforeEach
	public void setUp() {
		MockitoAnnotations.openMocks(this);
	}

	@ParameterizedTest
	@CsvSource({ "5.0, 3.0, 'add', 8.0, 'result.jsp'", "5.0, 3.0, 'subtract', 2.0, 'result.jsp'",
			"5.0, 3.0, 'multiply', 15.0, 'result.jsp'", "6.0, 3.0, 'divide', 2.0, 'result.jsp'" })
	public void testCalculatorServlet(String num1, String num2, String operation, double result, String jsp)
			throws Exception {

		ArgumentCaptor<Calculation> argument = ArgumentCaptor.forClass(Calculation.class);
		for (String method : new String[] { "GET", "POST" }) {
			when(request.getParameter("num1")).thenReturn(num1);
			when(request.getParameter("num2")).thenReturn(num2);
			when(request.getParameter("operation")).thenReturn(operation);
			when(request.getRequestDispatcher(jsp)).thenReturn(requestDispatcher);
			when(servletContext.getRequestDispatcher("/" + jsp)).thenReturn(requestDispatcher);

			if ("GET".equals(method)) {
				assertDoesNotThrow(() -> servlet.doGet(request, response),
						"Incorrect implementation of the doGet method");
			} else {
				assertDoesNotThrow(() -> servlet.doPost(request, response),
						"Incorrect implementation of the doPost method");
			}

//			verify(request).getRequestDispatcher(jsp);
			verify(requestDispatcher).forward(request, response);
			verify(request).setAttribute(eq("calculation"), argument.capture());
			assertEquals(operation, argument.getValue().getOperation(),
					"Operation in Calculation object doesn't match the expected operation for HTTP " + method);
			assertEquals(Double.parseDouble(num1), argument.getValue().getNum1(),
					"num1 in Calculation object doesn't match the expected num1 for HTTP " + method);
			assertEquals(Double.parseDouble(num2), argument.getValue().getNum2(),
					"num2 in Calculation object doesn't match the expected num2 for HTTP " + method);
			assertEquals(result, argument.getValue().getResult(),
					"Result in Calculation object doesn't match the expected result for HTTP " + method);

			reset(request, requestDispatcher);
		}
	}

	@ParameterizedTest
	@MethodSource("provideFaultyParameters")
	void testErrorScenarios(String num1, String num2, String operation, int errorCode, String errorMessage)
			throws Exception {
		ArgumentCaptor<Integer> sentErrorCode = ArgumentCaptor.forClass(Integer.class);
		ArgumentCaptor<String> sentErrorMessage = ArgumentCaptor.forClass(String.class);

		for (String method : new String[] { "GET", "POST" }) {
			when(request.getParameter("num1")).thenReturn(num1);
			when(request.getParameter("num2")).thenReturn(num2);
			when(request.getParameter("operation")).thenReturn(operation);

			if ("GET".equals(method)) {
				assertDoesNotThrow(() -> servlet.doGet(request, response),
						"Incorrect implementation of the doGet method");
			} else {
				assertDoesNotThrow(() -> servlet.doPost(request, response),
						"Incorrect implementation of the doPost method");
			}

			verify(response).sendError(sentErrorCode.capture(), sentErrorMessage.capture());
			assertEquals(errorCode, sentErrorCode.getValue().intValue(),
					"Incorrect error code was sent for HTTP " + method);
			assertEquals(errorMessage, sentErrorMessage.getValue(),
					"Incorrect error message was sent for HTTP " + method);

			reset(request, response);
		}
	}

	private static Stream<Arguments> provideFaultyParameters() {
		return Stream.of(Arguments.of("5.0", "3.0", "unknown", HttpServletResponse.SC_BAD_REQUEST, "Unknown operation"),
				Arguments.of("invalid", "3.0", "add", HttpServletResponse.SC_BAD_REQUEST, "Invalid number"),
				Arguments.of("5.0", "invalid", "add", HttpServletResponse.SC_BAD_REQUEST, "Invalid number"));
	}
}