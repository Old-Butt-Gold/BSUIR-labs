package edu.epam.fop.web;

import java.io.IOException;
import java.io.PrintWriter;
import java.util.concurrent.atomic.AtomicInteger;

import jakarta.servlet.ServletContext;
import jakarta.servlet.ServletException;
import jakarta.servlet.annotation.WebServlet;
import jakarta.servlet.http.HttpServlet;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;

@WebServlet("/VisitCounter")
public class VisitCounterServlet extends HttpServlet {
	private static final long serialVersionUID = 7485830631456155452L;

	private static final String VISIT_COUNTER_ATTRIBUTE_NAME = "visitCount";
	private static final String COUNTER_PATH = "/WEB-INF/visitCount.txt";

	// Use this object for logging.
	System.Logger logger = System.getLogger(VisitCounterServlet.class.getName());

	// Use this object to save and restore a visit counter.
	CounterFileHelper fileHelper = new CounterFileHelper();

	// Use this object to place a visit counter in a servlet context as an
	// attribute.
	AtomicInteger visitCount;

	private String counterRealPath;

	@Override
	public void init() throws ServletException {
		ServletContext context = getServletContext();
		counterRealPath = context.getRealPath(COUNTER_PATH);

		// Initialize visitCount and restore it from file.
		visitCount = new AtomicInteger();

		if (getServletContext().getAttribute(VISIT_COUNTER_ATTRIBUTE_NAME) == null) {
			int restoredCount = fileHelper.restoreCount(counterRealPath);
			visitCount.set(restoredCount);
			getServletContext().setAttribute(VISIT_COUNTER_ATTRIBUTE_NAME, visitCount);
		}

		// Log the servlet initialization event.
		logger.log(System.Logger.Level.INFO, "Servlet initialized. Visit count restored to " + visitCount.get());
	}

	@Override
	protected void service(HttpServletRequest request, HttpServletResponse response)
			throws ServletException, IOException {
		int currentCounter = visitCount.incrementAndGet();

		// Set the updated count in the servlet context as an attribute.
		getServletContext().setAttribute(VISIT_COUNTER_ATTRIBUTE_NAME, currentCounter);

		response.setContentType("text/html");
		try (PrintWriter out = response.getWriter()) {
			out.println("<!DOCTYPE html><html lang=\"en\"><head><title>Visit Counter</title></head><body>");
			out.println("<h2>Visit counter: " + currentCounter + "</h2>");
			out.println("</body></html>");
		}

		logger.log(System.Logger.Level.INFO, "Processed request, visit count: " + currentCounter);
	}

	@Override
	public void destroy() {
		try {
			fileHelper.saveCount(counterRealPath, visitCount.get());
			logger.log(System.Logger.Level.INFO, "Servlet destroyed. Visit count saved: " + visitCount.get());
		} catch (ServletException e) {
			logger.log(System.Logger.Level.ERROR, "Error while saving visit count.", e);
			throw new RuntimeException(e);
		}
	}
}