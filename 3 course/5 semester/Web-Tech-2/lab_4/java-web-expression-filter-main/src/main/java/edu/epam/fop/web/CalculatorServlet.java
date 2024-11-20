package edu.epam.fop.web;

import java.io.IOException;
import java.util.ArrayList;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

import jakarta.servlet.ServletContext;
import jakarta.servlet.ServletException;
import jakarta.servlet.http.HttpServlet;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import jakarta.servlet.http.HttpSession;

public class CalculatorServlet extends HttpServlet {
	private static final long serialVersionUID = 1L;

	private Pattern pattern;

	@Override
	public void init() throws ServletException {
		var context = getServletContext();
		String regex = context.getInitParameter("expression-regex");
		this.pattern = Pattern.compile(regex);
	}

	protected void doGet(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		request.getSession().setAttribute("error", "Please use the POST method for calculations");
		response.sendRedirect("error.jsp");
	}

	protected void doPost(HttpServletRequest request, HttpServletResponse response) throws ServletException, IOException {
		String action = request.getParameter("action");

		if ("clearHistory".equals(action)) {
			request.getSession().removeAttribute("history");
			response.sendRedirect("result.jsp");
			return;
		}

		String expression = request.getParameter("expression");
		Matcher matcher = pattern.matcher(expression);

		if (!matcher.matches()) {
			request.getSession().setAttribute("error", "Invalid expression: \"" + expression + "\"");
			response.sendRedirect("error.jsp");
			return;
		}

		try {
			String operator = matcher.group("operation");
			double operand1 = Double.parseDouble(matcher.group("operand1"));
			double operand2 = Double.parseDouble(matcher.group("operand2"));

			double result = calculate(operand1, operand2, operator);

			String calculationResult = expression + "=" + result;

			HttpSession session = request.getSession();
			ArrayList<String> history = (ArrayList<String>) session.getAttribute("history");
			if (history == null) {
				history = new ArrayList<>();
			}
			history.add(calculationResult);
			session.setAttribute("history", history);
			session.removeAttribute("error");
			response.sendRedirect("result.jsp");

		} catch (ArithmeticException | IllegalArgumentException e) {
			request.getSession().setAttribute("error", e.getMessage());
			response.sendRedirect("error.jsp");
		}
	}

	private double calculate(double operand1, double operand2, String operator) throws ArithmeticException {
		switch (operator) {
			case "+":
				return operand1 + operand2;
			case "-":
				return operand1 - operand2;
			case "*":
				return operand1 * operand2;
			case "/":
				if (operand2 == 0) {
					throw new ArithmeticException("Division by zero!");
				}
				return operand1 / operand2;
			default:
				throw new IllegalArgumentException("Unknown operator: " + operator);
		}
	}

}