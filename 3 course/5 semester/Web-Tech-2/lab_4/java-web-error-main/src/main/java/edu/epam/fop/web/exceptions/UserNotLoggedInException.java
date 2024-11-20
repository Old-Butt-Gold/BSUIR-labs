package edu.epam.fop.web.exceptions;

import java.util.concurrent.atomic.AtomicInteger;

public class UserNotLoggedInException extends Exception {
	private static final long serialVersionUID = -3767152367907337233L;

	private static AtomicInteger counter = new AtomicInteger();

	public UserNotLoggedInException(String message) {
		super(message);
		counter.incrementAndGet();
	}

	public static int getCounter() {
		return counter.get();
	}
}