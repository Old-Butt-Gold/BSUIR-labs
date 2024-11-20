package edu.epam.fop.web;

import static org.junit.jupiter.api.Assertions.assertDoesNotThrow;
import static org.junit.jupiter.api.Assertions.assertTrue;
import static org.mockito.ArgumentMatchers.any;
import static org.mockito.ArgumentMatchers.anyString;
import static org.mockito.ArgumentMatchers.eq;
import static org.mockito.Mockito.never;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;
import static org.mockito.MockitoAnnotations.openMocks;

import java.util.ArrayList;
import java.util.List;
import java.util.stream.Collectors;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.DisplayNameGeneration;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.CsvSource;
import org.mockito.Mock;

import jakarta.servlet.ServletConfig;
import jakarta.servlet.ServletContext;
import jakarta.servlet.ServletException;
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

	@Mock
	ServletContext context;

	@Mock
	ServletConfig config;

	CalculatorServlet servlet;

	@BeforeEach
	public void setUp() throws ServletException {
		openMocks(this);
		when(config.getServletContext()).thenReturn(context);
		when(request.getServletContext()).thenReturn(context);
		when(request.getSession()).thenReturn(session);
		String regex = "^(?<operand1>[-+]?[0-9]*\\.?[0-9]+)(?<operation>[-+*/])(?<operand2>[-+]?[0-9]*\\.?[0-9]+)$";
		when(context.getInitParameter("expression-regex")).thenReturn(regex);

		servlet = new CalculatorServlet();
		servlet.init(config);
	}

	@Test
	void testDoPostWhenActionIsClearHistory() throws Exception {
		when(request.getParameter("action")).thenReturn("clearHistory");
		ArrayList<String> history = new ArrayList<>();
		history.add("10+2=12");
		when(session.getAttribute("history")).thenReturn(history);

		assertDoesNotThrow(() -> servlet.doPost(request, response), "Incorrect implementation of the doPost method");

		verify(session).removeAttribute("history");
		verify(response).sendRedirect("result.jsp");
	}

	@ParameterizedTest
	@CsvSource({ "'8/4', '8/4=2.0'", "'1+-1', '1+-1=0.0'", "'-2+.1', '-2+.1=-1.9'", "'-1.1+-0.0', '-1.1+-0.0=-1.1'",
			"'2*.0', '2*.0=0.0'", "'5--5', '5--5=10.0'", "'00.00/00.01', '00.00/00.01=0.0'", "'-.0+.0', '-.0+.0=0.0'",
			"'1++1', '1++1=2.0'", "'1.1*1', '1.1*1=1.1'", "'0.0/0.9', '0.0/0.9=0.0'" })
	public void testDoPostWithValidExpression(String expression, String expected) throws Exception {
		when(request.getParameter("expression")).thenReturn(expression);
		ArrayList<String> history = new ArrayList<>();
		when(session.getAttribute("history")).thenReturn(history);

		assertDoesNotThrow(() -> servlet.doPost(request, response), "Incorrect implementation of the doPost method");

		verify(session).setAttribute("history", history);
		verify(session, never()).setAttribute(eq("error"), anyString());
		verify(session).removeAttribute("error");
		verify(response).sendRedirect("result.jsp");

		List<String> historyWithNoSpaces = history.stream().map(s -> s.replaceAll("\\s+", ""))
				.collect(Collectors.toList());
		assertTrue(historyWithNoSpaces.contains(expected), "For the expression \"" + expression
				+ "\" you should store the following string in history: \"" + expected + "\"");
	}

	@ParameterizedTest
	@CsvSource({ "'+'", "'111'", "'.0/.0'", "'--999+999'", "'1+++1'" })
	public void testDoPostWithInvalidExpression(String expression) throws Exception {
		when(request.getParameter("expression")).thenReturn(expression);

		assertDoesNotThrow(() -> servlet.doPost(request, response), "Incorrect implementation of the doPost method");

		verify(session).setAttribute(eq("error"), anyString());
		verify(session, never()).setAttribute(eq("history"), any());
		verify(response).sendRedirect("error.jsp");
	}

	@Test
	public void testDoGet() throws Exception {
		when(request.getSession()).thenReturn(session);

		assertDoesNotThrow(() -> servlet.doGet(request, response), "Incorrect implementation of the doGet method");

		verify(session).setAttribute(eq("error"), anyString());
		verify(response).sendRedirect("error.jsp");
	}
}