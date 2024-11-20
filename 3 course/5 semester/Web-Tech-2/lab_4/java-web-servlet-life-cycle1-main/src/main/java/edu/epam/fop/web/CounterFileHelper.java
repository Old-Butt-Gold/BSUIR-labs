package edu.epam.fop.web;

import java.io.BufferedReader;
import java.io.BufferedWriter;
import java.io.File;
import java.io.FileReader;
import java.io.FileWriter;

import jakarta.servlet.ServletException;

public class CounterFileHelper {
	public int restoreCount(String fileName) throws ServletException {
		int counter = 0;

		File file = new File(fileName);
		if (file.exists()) {
			try (BufferedReader reader = new BufferedReader(new FileReader(file))) {
				String countString = reader.readLine();
				if (countString != null && !countString.isEmpty()) {
					counter = Integer.parseInt(countString);
				}
			} catch (Exception e) {
				throw new ServletException(e);
			}
		}
		return counter;
	}

	public void saveCount(String fileName, int counter) throws ServletException {
		File file = new File(fileName);
		try (BufferedWriter writer = new BufferedWriter(new FileWriter(file))) {
			writer.write(String.valueOf(counter));
		} catch (Exception e) {
			throw new ServletException(e);
		}
	}
}