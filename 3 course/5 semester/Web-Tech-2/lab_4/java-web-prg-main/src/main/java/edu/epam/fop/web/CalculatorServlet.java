package edu.epam.fop.web;

import java.io.IOException;
import java.util.ArrayList;

import jakarta.servlet.ServletException;
import jakarta.servlet.annotation.WebServlet;
import jakarta.servlet.http.HttpServlet;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import jakarta.servlet.http.HttpSession;

public class CalculatorServlet extends HttpServlet {
	private static final long serialVersionUID = 1L;

	protected void doGet(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		request.getSession().setAttribute("error", "Please use the POST method for calculations.");
		response.sendRedirect("result.jsp");
	}

	private double evaluateExpression(String expression) throws IllegalArgumentException {
		String[] operands;
		double result;

		if (expression.contains("+")) {
			operands = expression.split("\\+");
			result = Double.parseDouble(operands[0]) + Double.parseDouble(operands[1]);
		} else if (expression.contains("-")) {
			operands = expression.split("-");
			result = Double.parseDouble(operands[0]) - Double.parseDouble(operands[1]);
		} else if (expression.contains("*")) {
			operands = expression.split("\\*");
			result = Double.parseDouble(operands[0]) * Double.parseDouble(operands[1]);
		} else if (expression.contains("/")) {
			operands = expression.split("/");
			double denominator = Double.parseDouble(operands[1]);
			if (denominator == 0) {
				throw new IllegalArgumentException("Division by zero error!");
			}
			result = Double.parseDouble(operands[0]) / denominator;
		} else {
			throw new IllegalArgumentException("Invalid expression: \"" + expression + "\"");
		}

		return result;
	}

	protected void doPost(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		String action = request.getParameter("action");
		HttpSession session = request.getSession();

		if ("clearHistory".equals(action)) {
			session.removeAttribute("history");
			response.sendRedirect("result.jsp");
			return;
		}

		String expression = request.getParameter("expression");
		ArrayList<String> history = (ArrayList<String>) session.getAttribute("history");

		if (history == null) {
			history = new ArrayList<>();
		}

		try {
			double result = evaluateExpression(expression);

			session.removeAttribute("error");

			String historyEntry = expression + " = " + result;
			history.add(historyEntry);
			session.setAttribute("history", history);
		} catch (Exception ex) {
			session.setAttribute("error", "Invalid expression: " + ex.getMessage());
		}

		response.sendRedirect("result.jsp");
	}
}