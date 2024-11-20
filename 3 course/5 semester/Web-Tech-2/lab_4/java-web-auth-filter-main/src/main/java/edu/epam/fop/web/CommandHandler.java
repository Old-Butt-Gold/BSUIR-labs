package edu.epam.fop.web;

import java.io.IOException;

import jakarta.servlet.ServletException;
import jakarta.servlet.http.HttpServlet;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import jakarta.servlet.http.HttpSession;

public class CommandHandler extends HttpServlet {
	private static final long serialVersionUID = -6372649884579584763L;

	private static final String LOGIN_COMMAND = "login";
	private static final String LOGOUT_COMMAND = "logout";
	private static final String COMMAND_PARAMETER_NAME = "command";
	private static final String USER_ROLE_PARAMETER_NAME = "role";
	private static final String USER_ROLE_ATTRIBUTE_NAME = "role";

	private static final String RESULT_PAGE = "/WEB-INF/jsp/result.jsp";

	protected void doPost(HttpServletRequest request, HttpServletResponse response)
			throws ServletException, IOException {

		String command = request.getParameter(COMMAND_PARAMETER_NAME);
		request.setAttribute(USER_ROLE_ATTRIBUTE_NAME, request.getSession().getAttribute(USER_ROLE_ATTRIBUTE_NAME));
		request.setAttribute(COMMAND_PARAMETER_NAME, command);

		handleCommand(command, request);

		request.getRequestDispatcher(RESULT_PAGE).forward(request, response);
	}

	private void handleCommand(String command, HttpServletRequest request) {
		switch (command) {
		case LOGIN_COMMAND:
			handleLogin(request);
			break;
		case LOGOUT_COMMAND:
			handleLogout(request);
			break;
		}
	}

	private void handleLogout(HttpServletRequest request) {
		HttpSession session = request.getSession();
		session.invalidate();
	}

	private void handleLogin(HttpServletRequest request) {
		String newRole = String.valueOf(request.getParameter(USER_ROLE_PARAMETER_NAME)).replaceAll("\\s+", "");
		if (!newRole.isEmpty()) {
			handleLogout(request);
			request.getSession().setAttribute(USER_ROLE_ATTRIBUTE_NAME, newRole);
		}
	}
}