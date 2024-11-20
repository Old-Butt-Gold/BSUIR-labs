package edu.epam.fop.web;

import static org.junit.jupiter.api.Assertions.assertThrows;
import static org.mockito.ArgumentMatchers.anyString;
import static org.mockito.ArgumentMatchers.eq;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;

import java.io.IOException;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.DisplayNameGeneration;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.ValueSource;
import org.mockito.Mock;
import org.mockito.MockitoAnnotations;

import edu.epam.fop.web.exceptions.InvalidISBNException;
import jakarta.servlet.RequestDispatcher;
import jakarta.servlet.ServletConfig;
import jakarta.servlet.ServletContext;
import jakarta.servlet.ServletException;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;

@DisplayNameGeneration(ReplaceCamelCase.class)
public class BookSearchServletTest {

	private static final String ISBN_PARAMETER = "isbn";
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

	private BookSearchServlet bookSearchServlet;

	@BeforeEach
	public void setUp() throws ServletException {
		MockitoAnnotations.openMocks(this);
		bookSearchServlet = new BookSearchServlet();
		bookSearchServlet.init(servletConfigMock);
	}

	@ParameterizedTest
	@ValueSource(strings = { "1234567890123", "9876543210987" })
	public void testDoGetSuccess(String isbn) throws ServletException, IOException {

		when(servletConfigMock.getServletContext()).thenReturn(servletContextMock);
		when(request.getServletContext()).thenReturn(servletContextMock);

		when(request.getParameter(ISBN_PARAMETER)).thenReturn(isbn);
		when(request.getRequestDispatcher(RESULT_PAGE)).thenReturn(requestDispatcher);
		when(servletContextMock.getRequestDispatcher(RESULT_PAGE)).thenReturn(requestDispatcher);

		bookSearchServlet.doGet(request, response);

		verify(request).setAttribute(eq(RESULT_ATTRIBUTE), anyString());
		verify(requestDispatcher).forward(request, response);
	}

	@ParameterizedTest
	@ValueSource(strings = { "123456789012", "12345678901234", "abc" })
	public void testDoGetException(String isbn) {
		when(request.getParameter(ISBN_PARAMETER)).thenReturn(isbn);

		assertThrows(InvalidISBNException.class, () -> {
			bookSearchServlet.doGet(request, response);
		}, "You should throw InvalidISBNException");
	}
}