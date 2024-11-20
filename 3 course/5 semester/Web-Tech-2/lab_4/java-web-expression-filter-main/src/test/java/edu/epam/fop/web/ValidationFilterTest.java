package edu.epam.fop.web;

import static org.junit.jupiter.api.Assertions.assertDoesNotThrow;
import static org.mockito.ArgumentMatchers.anyString;
import static org.mockito.ArgumentMatchers.eq;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;
import static org.mockito.MockitoAnnotations.openMocks;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.DisplayNameGeneration;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.CsvSource;
import org.mockito.Mock;

import jakarta.servlet.FilterChain;
import jakarta.servlet.FilterConfig;
import jakarta.servlet.ServletContext;
import jakarta.servlet.ServletException;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import jakarta.servlet.http.HttpSession;

@DisplayNameGeneration(ReplaceCamelCase.class)
public class ValidationFilterTest {

	@Mock
	HttpServletRequest request;

	@Mock
	HttpServletResponse response;

	@Mock
	ServletContext context;

	@Mock
	HttpSession session;

	@Mock
	FilterConfig config;

	@Mock
	FilterChain chain;

	ValidationFilter filter;

	@BeforeEach
	public void setup() throws ServletException {
		openMocks(this);
		when(config.getServletContext()).thenReturn(context);
		when(request.getServletContext()).thenReturn(context);
		when(request.getSession()).thenReturn(session);
		String regex = "^(?<operand1>[-+]?[0-9]*\\.?[0-9]+)(?<operation>[-+*/])(?<operand2>[-+]?[0-9]*\\.?[0-9]+)$";
		when(context.getInitParameter("expression-regex")).thenReturn(regex);

		filter = new ValidationFilter();
		filter.init(config);
	}

	@ParameterizedTest
	@CsvSource({ "'+'", "'111'", "'--999+999'", "'1+++1'" })
	public void testDoFilterWithInvalidExpression(String expression) throws Exception {
		when(request.getParameter("expression")).thenReturn(expression);

		assertDoesNotThrow(() -> filter.doFilter(request, response, chain),
				"Incorrect implementation of the doFilter method");

		verify(session).setAttribute(eq("error"), anyString());
		verify(response).sendRedirect("error.jsp");
	}

	@ParameterizedTest
	@CsvSource({ "'8/4', '8/4=2.0'", "'1+-1', '1+-1=0.0'", "'-2+.1', '-2+.1=-1.9'", "'-1.1+-0.0', '-1.1+-0.0=-1.1'",
			"'2*.0', '2*.0=0.0'", "'5--5', '5--5=10.0'", "'00.00/00.01', '00.00/00.01=0.0'", "'-.0+.0', '-.0+.0=0.0'",
			"'1++1', '1++1=2.0'", "'1.1*1', '1.1*1=1.1'", "'0.0/0.9', '0.0/0.9=0.0'" })
	public void testDoFilterWithValidExpression(String expression, String ignoreExpected) throws Exception {
		when(request.getParameter("expression")).thenReturn(expression);

		assertDoesNotThrow(() -> filter.doFilter(request, response, chain),
				"Incorrect implementation of the doFilter method");

		verify(chain).doFilter(request, response);
	}
}