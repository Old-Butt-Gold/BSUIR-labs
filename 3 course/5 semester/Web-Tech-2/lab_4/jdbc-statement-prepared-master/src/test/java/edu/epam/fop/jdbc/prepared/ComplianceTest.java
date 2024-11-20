package edu.epam.fop.jdbc.prepared;

import static org.junit.jupiter.api.Assertions.assertFalse;

import java.util.HashSet;
import java.util.Set;

import org.junit.jupiter.api.DisplayNameGeneration;
import org.junit.jupiter.api.Test;

import spoon.Launcher;
import spoon.reflect.CtModel;
import spoon.reflect.code.CtFieldAccess;
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

		Set<CtInvocation<?>> elementsWithPrepareStatement = new HashSet<CtInvocation<?>>();
		model.getElements(new TypeFilter<>(CtInvocation.class)).stream()
				.filter(element -> "prepareStatement".equals(element.getExecutable().getSimpleName())
						&& "java.sql.Connection".equals(element.getExecutable().getDeclaringType().getQualifiedName())
						&& !element.getParent(CtType.class).getQualifiedName().endsWith("Demo"))
				.forEach(element -> elementsWithPrepareStatement.add(element));
		assertFalse(elementsWithPrepareStatement.isEmpty(),
				"Use the Connection.prepareStatement() method that has the capability to retrieve auto-generated keys");

		Set<CtFieldAccess<?>> elementsWithGeneratedKeys = new HashSet<CtFieldAccess<?>>();
		model.getElements(new TypeFilter<>(CtFieldAccess.class)).stream()
				.filter(element -> "RETURN_GENERATED_KEYS".equals(element.getVariable().getSimpleName())
						&& "java.sql.Statement".equals(element.getVariable().getDeclaringType().getQualifiedName())
						&& !element.getParent(CtType.class).getQualifiedName().endsWith("Demo"))
				.forEach(element -> elementsWithGeneratedKeys.add(element));
		assertFalse(elementsWithGeneratedKeys.isEmpty(),
				"Use the Statement.RETURN_GENERATED_KEYS constant, which indicates that generated keys should be made available for retrieval");
	}
}