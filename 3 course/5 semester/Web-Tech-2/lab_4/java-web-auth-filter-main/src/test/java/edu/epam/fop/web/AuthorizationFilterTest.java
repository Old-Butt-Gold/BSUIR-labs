package edu.epam.fop.web;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.mockito.Mockito.never;
import static org.mockito.Mockito.verify;
import static org.mockito.Mockito.when;
import static org.mockito.MockitoAnnotations.openMocks;

import java.util.Collections;
import java.util.HashMap;
import java.util.HashSet;
import java.util.List;
import java.util.Map;
import java.util.Set;
import java.util.stream.Stream;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.DisplayNameGeneration;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.Arguments;
import org.junit.jupiter.params.provider.MethodSource;
import org.mockito.Mock;

import jakarta.servlet.FilterChain;
import jakarta.servlet.FilterConfig;
import jakarta.servlet.ServletException;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import jakarta.servlet.http.HttpSession;

@DisplayNameGeneration(ReplaceCamelCase.class)
public class AuthorizationFilterTest {

	@Mock
	HttpServletRequest request;

	@Mock
	HttpServletResponse response;

	@Mock
	HttpSession session;

	@Mock
	FilterChain chain;

	@Mock
	FilterConfig config;

	AuthorizationFilter authorizationFilter;

	@BeforeEach
	public void setUp() throws ServletException {
		openMocks(this);

		when(config.getInitParameterNames()).thenReturn(Collections.enumeration(List.of("student", "teacher")));
		when(config.getInitParameter("student")).thenReturn("logout viewSettings");
		when(config.getInitParameter("teacher")).thenReturn("logout viewSettings updateSettings");

		authorizationFilter = new AuthorizationFilter();
		authorizationFilter.init(config);
	}

	@Test
	public void testInit() {
		Map<String, Set<String>> expectedMap = new HashMap<>();
		expectedMap.put("student", new HashSet<>(Set.of("logout", "viewSettings")));
		expectedMap.put("teacher", new HashSet<>(Set.of("logout", "viewSettings", "updateSettings")));
		Map<String, Set<String>> actualMap = authorizationFilter.roleCommands;

		assertEquals(expectedMap.keySet(), actualMap.keySet(), "All roles are not present in the actual map");

		expectedMap.forEach((role, commands) -> {
			Set<String> expectedCommands = expectedMap.get(role);
			Set<String> actualCommands = actualMap.get(role);
			assertEquals(expectedCommands, actualCommands,
					"Commands for role \"" + role + "\" do not match expected commands");
		});
	}

	@ParameterizedTest
	@MethodSource("provideCommandsAndRolesForTesting")
	public void testDoFilter(String role, String command, boolean shouldContinueFilterChain) throws Exception {
		when(request.getSession()).thenReturn(session);
		when(session.getAttribute("role")).thenReturn(role);
		when(request.getParameter("command")).thenReturn(command);

		authorizationFilter.doFilter(request, response, chain);

		if (shouldContinueFilterChain) {
			verify(chain).doFilter(request, response);
		} else {
			verify(response).sendError(HttpServletResponse.SC_FORBIDDEN);
			verify(chain, never()).doFilter(request, response);
		}
	}

	private static Stream<Arguments> provideCommandsAndRolesForTesting() {
		return Stream.of(Arguments.of("student", "logout", true), Arguments.of("teacher", "logout", true),
				Arguments.of("guest", "logout", true), Arguments.of(null, "logout", false),
				Arguments.of("student", "viewSettings", true), Arguments.of("teacher", "viewSettings", true),
				Arguments.of("guest", "viewSettings", false), Arguments.of(null, "viewSettings", false),
				Arguments.of("teacher", "updateSettings", true), Arguments.of("student", "updateSettings", false),
				Arguments.of("guest", "updateSettings", false), Arguments.of(null, "updateSettings", false),
				Arguments.of("student", "login", false), Arguments.of("teacher", "login", false),
				Arguments.of("guest", "login", false), Arguments.of(null, "login", true),
				Arguments.of("student", null, false), Arguments.of("teacher", null, false),
				Arguments.of("guest", null, false), Arguments.of(null, null, false),
				Arguments.of(null, "wrongCommand", false), Arguments.of("teacher", "wrongCommand", false),
				Arguments.of("student", "wrongCommand", false), Arguments.of("guest", "wrongCommand", false));
	}
}