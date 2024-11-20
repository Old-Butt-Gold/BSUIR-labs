package edu.epam.fop.web;

import java.io.IOException;

import jakarta.servlet.RequestDispatcher;
import jakarta.servlet.ServletException;
import jakarta.servlet.http.HttpServlet;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import jakarta.servlet.http.HttpSession;

public class LoginServlet extends HttpServlet {
	private static final long serialVersionUID = -8352359866302205183L;

	private static final String USERNAME_PARAMETER = "username";
	private static final String PASSWORD_PARAMETER = "password";
	private static final String ERROR_MESSAGE_ATTRIBUTE = "errorMessage";
	private static final String USER_ATTRIBUTE = "user";
	private static final String HOME_PAGE = "home.jsp";
	private static final String LOGIN_PAGE = "login.jsp";

	public void doPost(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		String username = request.getParameter(USERNAME_PARAMETER);
		String password = request.getParameter(PASSWORD_PARAMETER);

		try {
			if (username != null && !username.trim().isEmpty() && password != null && !password.trim().isEmpty()) {
				HttpSession httpSession = request.getSession();
				httpSession.setAttribute(USER_ATTRIBUTE, username);
				response.sendRedirect(HOME_PAGE);
			} else {
				throw new IllegalArgumentException();
			}
		} catch (IllegalArgumentException e) {
			request.setAttribute(ERROR_MESSAGE_ATTRIBUTE, "Invalid username or password");
			request.getRequestDispatcher(LOGIN_PAGE).forward(request, response);
		}
	}
}