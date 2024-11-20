package edu.epam.fop.web;

import java.io.IOException;
import java.nio.file.attribute.UserPrincipalNotFoundException;

import edu.epam.fop.web.exceptions.UserNotLoggedInException;
import jakarta.servlet.RequestDispatcher;
import jakarta.servlet.ServletException;
import jakarta.servlet.http.HttpServlet;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import jakarta.servlet.http.HttpSession;

public class FinesServlet extends HttpServlet {
	private static final long serialVersionUID = -4288422571113926745L;

	private static final String USER_ATTRIBUTE = "user";
	private static final String RESULT_ATTRIBUTE = "result";
	private static final String RESULT_PAGE = "result.jsp";

	public void doGet(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		HttpSession session = request.getSession(false);

		try {
			if (session != null && session.getAttribute(USER_ATTRIBUTE) != null) {
				request.setAttribute(RESULT_ATTRIBUTE, "View fines result. User: " + session.getAttribute(USER_ATTRIBUTE));
				request.getRequestDispatcher(RESULT_PAGE).forward(request, response);
			} else {
				throw new UserNotLoggedInException("You need to be logged on to view or pay fines");
			}
		} catch (UserNotLoggedInException ex) {
			response.sendError(HttpServletResponse.SC_FORBIDDEN, ex.getMessage());
		}
	}
}