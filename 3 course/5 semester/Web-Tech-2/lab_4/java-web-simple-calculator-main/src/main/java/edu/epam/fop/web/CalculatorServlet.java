package edu.epam.fop.web;

import java.io.IOException;
import jakarta.servlet.ServletException;
import jakarta.servlet.annotation.WebServlet;
import jakarta.servlet.http.HttpServlet;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;

public class CalculatorServlet extends HttpServlet {
	private static final long serialVersionUID = 1L;

	private double performOperation(double num1, double num2, String operation) {
		switch (operation) {
			case "add": {
				return num1 + num2;
			}

			case "subtract": {
				return num1 - num2;
			}

			case "multiply": {
				return num1 * num2;
			}

			case "divide": {
				if (num2 == 0) {
					throw new IllegalArgumentException("Division by zero");
				}
				return num1 / num2;
			}

			default: {
				throw new IllegalArgumentException("Unknown operation");
			}
		}
	}

	private void processRequest(HttpServletRequest request, HttpServletResponse response)
			throws ServletException, IOException {
		try {
			double num1 = Double.parseDouble(request.getParameter("num1"));
			double num2 = Double.parseDouble(request.getParameter("num2"));
			String operation = request.getParameter("operation");

			double result = performOperation(num1, num2, operation);

			Calculation calculation = new Calculation();
			calculation.setNum1(num1);
			calculation.setNum2(num2);
			calculation.setOperation(operation);
			calculation.setResult(result);

			request.setAttribute("calculation", calculation);

			request.getRequestDispatcher("result.jsp").forward(request, response);
		} catch (NumberFormatException e) {
			response.sendError(HttpServletResponse.SC_BAD_REQUEST, "Invalid number");
		} catch (IllegalArgumentException e) {
			response.sendError(HttpServletResponse.SC_BAD_REQUEST, e.getMessage());
		}
	}

	@Override
	protected void doGet(HttpServletRequest request, HttpServletResponse response)
			throws ServletException, IOException {
		processRequest(request, response);
	}

	@Override
	protected void doPost(HttpServletRequest request, HttpServletResponse response)
			throws ServletException, IOException {
		processRequest(request, response);
	}
}