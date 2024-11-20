package edu.epam.fop.web;

import java.io.IOException;
import java.util.regex.Matcher;
import java.util.regex.Pattern;

import jakarta.servlet.Filter;
import jakarta.servlet.FilterChain;
import jakarta.servlet.FilterConfig;
import jakarta.servlet.ServletContext;
import jakarta.servlet.ServletException;
import jakarta.servlet.ServletRequest;
import jakarta.servlet.ServletResponse;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;

public class ValidationFilter implements Filter {

	private Pattern pattern;

	@Override
	public void init(FilterConfig filterConfig) throws ServletException {
		ServletContext context = filterConfig.getServletContext();
		String regex = context.getInitParameter("expression-regex");
		this.pattern = Pattern.compile(regex);
	}

	@Override
	public void doFilter(ServletRequest request, ServletResponse response, FilterChain chain) throws IOException, ServletException {
		HttpServletRequest httpRequest = (HttpServletRequest) request;
		HttpServletResponse httpResponse = (HttpServletResponse) response;

		String expression = httpRequest.getParameter("expression");

		if (expression != null) {
			Matcher matcher = pattern.matcher(expression);
			if (!matcher.matches()) {
				httpRequest.getSession().setAttribute("error", "Invalid expression: \"" + expression + "\"");
				httpResponse.sendRedirect("error.jsp");
				return;
			}
		}

		chain.doFilter(request, response);
	}
}