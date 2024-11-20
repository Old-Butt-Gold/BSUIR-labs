package edu.epam.fop.jdbc.create;

import static org.junit.jupiter.api.Assertions.assertFalse;

import java.util.HashSet;
import java.util.Set;

import org.junit.jupiter.api.DisplayNameGeneration;
import org.junit.jupiter.api.Test;

import spoon.Launcher;
import spoon.reflect.CtModel;
import spoon.reflect.code.CtInvocation;
import spoon.reflect.declaration.CtType;
import spoon.reflect.visitor.filter.TypeFilter;

@DisplayNameGeneration(ReplaceCamelCase.class)
public class ComplianceTest {
	@Test
	public void testPreparedStatement() {
		Launcher launcher = new Launcher();
		launcher.addInputResource("src/main/java");
		launcher.buildModel();
		CtModel model = launcher.getModel();

		Set<CtInvocation<?>> elementsWithCreateStatement = new HashSet<CtInvocation<?>>();
		model.getElements(new TypeFilter<>(CtInvocation.class)).stream()
				.filter(element -> "createStatement".equals(element.getExecutable().getSimpleName())
						&& "java.sql.Connection".equals(element.getExecutable().getDeclaringType().getQualifiedName())
						&& !element.getParent(CtType.class).getQualifiedName().endsWith("Demo"))
				.forEach(element -> elementsWithCreateStatement.add(element));
		assertFalse(elementsWithCreateStatement.isEmpty(),
				"Use the Connection.prepareStatement() method to create an object for sending SQL statements to the database");
	}
}