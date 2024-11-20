package edu.epam.fop.web;

import static org.junit.jupiter.api.Assertions.assertDoesNotThrow;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;
import static org.mockito.MockitoAnnotations.openMocks;

import java.io.IOException;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.DisplayNameGeneration;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.CsvSource;
import org.mockito.Mock;

import jakarta.servlet.FilterChain;
import jakarta.servlet.FilterConfig;
import jakarta.servlet.ServletException;
import jakarta.servlet.ServletRequest;
import jakarta.servlet.ServletResponse;

@DisplayNameGeneration(ReplaceCamelCase.class)
public class EncodingFilterTest {

	@Mock
	ServletRequest request;

	@Mock
	ServletResponse response;

	@Mock
	FilterChain chain;

	@Mock
	FilterConfig config;

	EncodingFilter filter;

	@BeforeEach
	public void setUp() throws ServletException {
		openMocks(this);
		filter = new EncodingFilter();
	}

	@ParameterizedTest
	@CsvSource({ "'UTF-8'", "'ISO-8859-1'" })
	public void testDoFilter(String encoding) throws ServletException, IOException {
		when(config.getInitParameter("encoding")).thenReturn(encoding);
		filter.init(config);

		assertDoesNotThrow(() -> filter.doFilter(request, response, chain),
				"Incorrect implementation of the doFilter method");

		verify(request).setCharacterEncoding(encoding);
		verify(response).setCharacterEncoding(encoding);
		verify(chain).doFilter(request, response);
	}
}