package edu.epam.fop.jdbc.callable;

import static org.junit.jupiter.api.Assertions.assertFalse;

import java.util.HashSet;
import java.util.Set;

import org.junit.jupiter.api.DisplayNameGeneration;
import org.junit.jupiter.api.Test;

import spoon.Launcher;
import spoon.reflect.CtModel;
import spoon.reflect.code.CtInvocation;
import spoon.reflect.code.CtLiteral;
import spoon.reflect.declaration.CtType;
import spoon.reflect.visitor.filter.TypeFilter;

@DisplayNameGeneration(ReplaceCamelCase.class)
public class ComplianceTest {
	@Test
	public void testCallableStatement() {
		Launcher launcher = new Launcher();
		launcher.addInputResource("src/main/java");
		launcher.buildModel();
		CtModel model = launcher.getModel();

		Set<CtInvocation<?>> elementsWithPrepareCall = new HashSet<CtInvocation<?>>();
		model.getElements(new TypeFilter<>(CtInvocation.class)).stream()
				.filter(element -> "prepareCall".equals(element.getExecutable().getSimpleName())
						&& "java.sql.Connection".equals(element.getExecutable().getDeclaringType().getQualifiedName())
						&& !element.getParent(CtType.class).getQualifiedName().endsWith("Demo"))
				.forEach(element -> elementsWithPrepareCall.add(element));
		assertFalse(elementsWithPrepareCall.isEmpty(),
				"Use the Connection.prepareCall() method to create an object for calling database stored procedures");

		for (String string : new String[] { "CALL COUNT_GROUPS", "CALL COUNT_STUDENTS",
				"CALL COUNT_STUDENTS_BY_GROUP_ID" }) {
			Set<CtLiteral<?>> elementsWithCallToStoredProcedure = new HashSet<CtLiteral<?>>();
			model.getElements(new TypeFilter<>(CtLiteral.class)).stream()
					.filter(literal -> String.valueOf(literal.getValue()).toUpperCase().contains(string))
					.forEach(element -> elementsWithCallToStoredProcedure.add(element));
			assertFalse(elementsWithCallToStoredProcedure.isEmpty(),
					"String literal containing \"" + string + "\" was not found");
		}
	}
}