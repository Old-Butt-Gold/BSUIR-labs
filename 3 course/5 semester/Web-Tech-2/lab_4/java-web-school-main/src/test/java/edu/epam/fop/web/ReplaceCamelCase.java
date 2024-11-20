package edu.epam.fop.web;

import java.lang.reflect.Method;

import org.junit.jupiter.api.DisplayNameGenerator;

public class ReplaceCamelCase extends DisplayNameGenerator.Standard {
	@Override
	public String generateDisplayNameForClass(Class<?> testClass) {
		return replaceCamelCase(super.generateDisplayNameForClass(testClass));
	}

	@Override
	public String generateDisplayNameForNestedClass(Class<?> nestedClass) {
		return replaceCamelCase(super.generateDisplayNameForNestedClass(nestedClass));
	}

	@Override
	public String generateDisplayNameForMethod(Class<?> testClass, Method testMethod) {
		return this.replaceCamelCase(testMethod.getName());
	}

	private String replaceCamelCase(String camelCase) {
		StringBuilder result = new StringBuilder();
		result.append(Character.toUpperCase(camelCase.charAt(0)));
		for (int i = 1, len = camelCase.length(); i < len; i++) {
			if (Character.isUpperCase(camelCase.charAt(i))) {
				result.append(' ');
				result.append(Character.toLowerCase(camelCase.charAt(i)));
			} else {
				result.append(camelCase.charAt(i));
			}
		}
		return result.toString();
	}
}