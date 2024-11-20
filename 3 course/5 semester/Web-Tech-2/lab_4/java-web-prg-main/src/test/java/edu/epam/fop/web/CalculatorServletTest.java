package edu.epam.fop.web;

import static org.junit.jupiter.api.Assertions.assertDoesNotThrow;
import static org.junit.jupiter.api.Assertions.assertTrue;
import static org.mockito.ArgumentMatchers.any;
import static org.mockito.ArgumentMatchers.anyString;
import static org.mockito.ArgumentMatchers.eq;
import static org.mockito.Mockito.never;
import static org.mockito.Mockito.when;

import java.util.ArrayList;
import java.util.List;
import java.util.stream.Collectors;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.DisplayNameGeneration;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.CsvSource;
import org.mockito.Mock;
import org.mockito.Mockito;
import org.mockito.MockitoAnnotations;

import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import jakarta.servlet.http.HttpSession;

@DisplayNameGeneration(ReplaceCamelCase.class)
public class CalculatorServletTest {

	@Mock
	HttpServletRequest request;

	@Mock
	HttpServletResponse response;

	@Mock
	HttpSession session;

	CalculatorServlet servlet;

	@BeforeEach
	public void setUp() {
		MockitoAnnotations.openMocks(this);
		when(request.getSession()).thenReturn(session);
		servlet = new CalculatorServlet();
	}

	@Test
	void testDoPostWhenActionIsClearHistory() throws Exception {
		Mockito.when(request.getParameter("action")).thenReturn("clearHistory");

		ArrayList<String> history = new ArrayList<>();
		history.add("10 + 2 = 12");
		Mockito.when(session.getAttribute("history")).thenReturn(history);

		assertDoesNotThrow(() -> servlet.doPost(request, response), "Incorrect implementation of the doPost method");

		Mockito.verify(session).removeAttribute("history");
		Mockito.verify(response).sendRedirect("result.jsp");
	}

	@ParameterizedTest
	@CsvSource({ "'3+2', '3+2=5.0'", "'10-2', '10-2=8.0'", "'5*5', '5*5=25.0'", "'10/2', '10/2=5.0'",
			"'0.5*2', '0.5*2=1.0'", "'5*0.2', '5*0.2=1.0'", "'0.5/2', '0.5/2=0.25'", "'20.5-0.5', '20.5-0.5=20.0'",
			"'99.5+0.5', '99.5+0.5=100.0'", "'1.1*1', '1.1*1=1.1'", "'0.0/0.9', '0.0/0.9=0.0'" })
	public void testDoPostWithValidExpression(String expression, String expected) throws Exception {
		Mockito.when(request.getParameter("expression")).thenReturn(expression);

		ArrayList<String> history = new ArrayList<>();
		Mockito.when(session.getAttribute("history")).thenReturn(history);

		assertDoesNotThrow(() -> servlet.doPost(request, response), "Incorrect implementation of the doPost method");

		Mockito.verify(session).setAttribute("history", history);
		Mockito.verify(session, never()).setAttribute(eq("error"), anyString());
		Mockito.verify(session).removeAttribute("error");
		Mockito.verify(response).sendRedirect("result.jsp");

		List<String> historyWithNoSpaces = history.stream().map(s -> s.replaceAll("\\s+", ""))
				.collect(Collectors.toList());
		assertTrue(historyWithNoSpaces.contains(expected), "For the expression \"" + expression
				+ "\" you should store the following string in history: \"" + expected + "\"");
	}

	@ParameterizedTest
	@CsvSource({ "'10/0'", "'5**'", "'++10'", "'2/*5'", "'1'" })
	public void testDoPostWithInvalidExpression(String expression) throws Exception {
		Mockito.when(request.getParameter("expression")).thenReturn(expression);

		assertDoesNotThrow(() -> servlet.doPost(request, response), "Incorrect implementation of the doPost method");

		Mockito.verify(session).setAttribute(eq("error"), anyString());
		Mockito.verify(session, never()).setAttribute(eq("history"), any());
		Mockito.verify(response).sendRedirect("result.jsp");
	}

	@Test
	public void testDoGet() throws Exception {
		Mockito.when(request.getSession()).thenReturn(session);

		assertDoesNotThrow(() -> servlet.doGet(request, response), "Incorrect implementation of the doGet method");

		Mockito.verify(session).setAttribute(eq("error"), anyString());
		Mockito.verify(response).sendRedirect("result.jsp");
	}
}