package edu.epam.fop.web;

import jakarta.servlet.ServletContext;
import jakarta.servlet.http.HttpSessionEvent;
import jakarta.servlet.http.HttpSessionListener;

import java.util.concurrent.atomic.AtomicInteger;

public class MyHttpSessionListener implements HttpSessionListener {

	private static final String ACTIVE_USERS_COUNTER_ATTRIBUTE = "activeUsersCounter";

	@Override
	public void sessionCreated(HttpSessionEvent sessionEvent) {
		ServletContext context = sessionEvent.getSession().getServletContext();
		AtomicInteger activeUsersCounter = (AtomicInteger) context.getAttribute(ACTIVE_USERS_COUNTER_ATTRIBUTE);
		if (activeUsersCounter != null) {
			activeUsersCounter.incrementAndGet();
		}
	}

	@Override
	public void sessionDestroyed(HttpSessionEvent sessionEvent) {
		ServletContext context = sessionEvent.getSession().getServletContext();
		AtomicInteger activeUsersCounter = (AtomicInteger) context.getAttribute(ACTIVE_USERS_COUNTER_ATTRIBUTE);
		if (activeUsersCounter != null) {
			activeUsersCounter.decrementAndGet();
		}
	}
}