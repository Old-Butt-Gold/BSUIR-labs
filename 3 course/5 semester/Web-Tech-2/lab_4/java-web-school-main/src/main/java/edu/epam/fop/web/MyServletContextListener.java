package edu.epam.fop.web;

import java.util.concurrent.atomic.AtomicInteger;

import org.apache.commons.dbcp2.BasicDataSource;

import jakarta.servlet.ServletContext;
import jakarta.servlet.ServletContextEvent;
import jakarta.servlet.ServletContextListener;

public class MyServletContextListener implements ServletContextListener {

	private static final String JDBC_DRIVER_CLASS_NAME_PARAMETER = "jdbcDriverClassName";
	private static final String JDBC_URL_PARAMETER = "jdbcUrl";
	private static final String USERNAME_PARAMETER = "username";
	private static final String PASSWORD_PARAMETER = "password";
	private static final String DATA_SOURCE_ATTRIBUTE = "dataSource";
	private static final String ACTIVE_USERS_COUNTER_ATTRIBUTE = "activeUsersCounter";

	BasicDataSource dataSource;

	protected BasicDataSource createDataSource() {
		return new BasicDataSource();
	}

	@Override
	public void contextInitialized(ServletContextEvent contextEvent) {
		ServletContext context = contextEvent.getServletContext();

		dataSource = createDataSource();

		dataSource.setDriverClassName(context.getInitParameter(JDBC_DRIVER_CLASS_NAME_PARAMETER));
		dataSource.setUrl(context.getInitParameter(JDBC_URL_PARAMETER));
		dataSource.setUsername(context.getInitParameter(USERNAME_PARAMETER));
		dataSource.setPassword(context.getInitParameter(PASSWORD_PARAMETER));

		context.setAttribute(DATA_SOURCE_ATTRIBUTE, dataSource);

		AtomicInteger activeUsersCounter = new AtomicInteger(0);
		context.setAttribute(ACTIVE_USERS_COUNTER_ATTRIBUTE, activeUsersCounter);
	}

	@Override
	public void contextDestroyed(ServletContextEvent contextEvent) {
		ServletContext context = contextEvent.getServletContext();

		context.removeAttribute(DATA_SOURCE_ATTRIBUTE);
		context.removeAttribute(ACTIVE_USERS_COUNTER_ATTRIBUTE);

		try {
			if (dataSource != null) {
				dataSource.close();
			}
		} catch (Exception e) {
			e.printStackTrace();
		}
	}

}