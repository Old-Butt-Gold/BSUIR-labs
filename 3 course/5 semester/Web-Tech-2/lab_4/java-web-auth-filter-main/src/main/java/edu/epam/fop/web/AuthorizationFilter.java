package edu.epam.fop.web;

import java.io.IOException;
import java.util.Arrays;
import java.util.Enumeration;
import java.util.HashMap;
import java.util.HashSet;
import java.util.Map;
import java.util.Set;

import jakarta.servlet.Filter;
import jakarta.servlet.FilterChain;
import jakarta.servlet.FilterConfig;
import jakarta.servlet.ServletException;
import jakarta.servlet.ServletRequest;
import jakarta.servlet.ServletResponse;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;

public class AuthorizationFilter implements Filter {
	private static final String COMMAND_PARAMETER_NAME = "command";
	private static final String USER_ROLE_ATTRIBUTE_NAME = "role";
	private static final String LOGIN_COMMAND = "login";
	private static final String LOGOUT_COMMAND = "logout";

	Map<String, Set<String>> roleCommands;

	@Override
	public void init(FilterConfig config) throws ServletException {
		roleCommands = new HashMap<>();

		var paramNames = config.getInitParameterNames();
		while (paramNames.hasMoreElements()) {
			String role = paramNames.nextElement();
			String[] commands = config.getInitParameter(role).split(" ");
			roleCommands.put(role, new HashSet<>(Arrays.asList(commands)));
		}
	}

	@Override
	public void doFilter(ServletRequest request, ServletResponse response, FilterChain chain) throws IOException, ServletException {
		HttpServletRequest httpRequest = (HttpServletRequest) request;
		HttpServletResponse httpResponse = (HttpServletResponse) response;

		String command = httpRequest.getParameter(COMMAND_PARAMETER_NAME);
		String role = (String) httpRequest.getSession().getAttribute(USER_ROLE_ATTRIBUTE_NAME);

		if ((role == null && !LOGIN_COMMAND.equals(command)) || command == null) {
			httpResponse.sendError(HttpServletResponse.SC_FORBIDDEN);
			return;
		}

		Set<String> allowedCommands = roleCommands.get(role);

		if (LOGIN_COMMAND.equals(command)) {
			if (role != null) {
				httpResponse.sendError(HttpServletResponse.SC_FORBIDDEN);
				return;
			}
		} else if (LOGOUT_COMMAND.equals(command)) {
			if (role == null) {
				httpResponse.sendError(HttpServletResponse.SC_FORBIDDEN);
				return;
			}
		} else if (allowedCommands == null || !allowedCommands.contains(command)) {
			httpResponse.sendError(HttpServletResponse.SC_FORBIDDEN);
			return;
		}

		chain.doFilter(request, response);
	}
}