package edu.epam.fop.web;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.mockito.Mockito.when;

import java.util.concurrent.atomic.AtomicInteger;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.DisplayNameGeneration;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.ValueSource;
import org.mockito.Mock;
import org.mockito.MockitoAnnotations;

import jakarta.servlet.ServletContext;
import jakarta.servlet.http.HttpSession;
import jakarta.servlet.http.HttpSessionEvent;

@DisplayNameGeneration(ReplaceCamelCase.class)
public class MyHttpSessionListenerTest {

	private static final String ACTIVE_USERS_COUNTER_ATTRIBUTE = "activeUsersCounter";

	@Mock
	private HttpSessionEvent sessionEventMock;

	@Mock
	private HttpSession sessionMock;

	@Mock
	private ServletContext servletContextMock;

	private MyHttpSessionListener listener;

	@BeforeEach
	public void setUp() {
		MockitoAnnotations.openMocks(this);

		when(sessionEventMock.getSession()).thenReturn(sessionMock);
		when(sessionMock.getServletContext()).thenReturn(servletContextMock);

		listener = new MyHttpSessionListener();
	}

	@ParameterizedTest
	@ValueSource(ints = { 0, 5, 10, 50 })
	public void testSessionCreated(int initialCount) {
		AtomicInteger counter = new AtomicInteger(initialCount);
		when(servletContextMock.getAttribute(ACTIVE_USERS_COUNTER_ATTRIBUTE)).thenReturn(counter);

		listener.sessionCreated(sessionEventMock);

		int actualCount = counter.get();
		assertEquals(initialCount + 1, actualCount, "Active users count should be incremented by 1");
	}

	@ParameterizedTest
	@ValueSource(ints = { 1, 5, 10, 50 })
	public void testSessionDestroyed(int initialCount) {
		AtomicInteger counter = new AtomicInteger(initialCount);
		when(servletContextMock.getAttribute(ACTIVE_USERS_COUNTER_ATTRIBUTE)).thenReturn(counter);

		listener.sessionDestroyed(sessionEventMock);

		int actualCount = counter.get();
		assertEquals(initialCount - 1, actualCount, "Active users count should be decremented by 1");
	}
}