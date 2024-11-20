package edu.epam.fop.web.exceptions;

import jakarta.servlet.ServletException;

public class InvalidISBNException extends ServletException {
	private static final long serialVersionUID = 8565485869991102838L;

	public InvalidISBNException(String message) {
		super(message);
	}
}