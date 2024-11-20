package edu.epam.fop.jdbc.transactions;

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
	public void testConnectionForTransaction() {
		Launcher launcher = new Launcher();
		launcher.addInputResource("src/main/java");
		launcher.buildModel();
		CtModel model = launcher.getModel();

		Set<CtInvocation<?>> elementsWithSetAutoCommit = new HashSet<CtInvocation<?>>();
		model.getElements(new TypeFilter<>(CtInvocation.class)).stream()
				.filter(element -> "setAutoCommit".equals(element.getExecutable().getSimpleName())
						&& "java.sql.Connection".equals(element.getExecutable().getDeclaringType().getQualifiedName())
						&& !element.getParent(CtType.class).getQualifiedName().endsWith("Demo"))
				.forEach(element -> elementsWithSetAutoCommit.add(element));
		assertFalse(elementsWithSetAutoCommit.isEmpty(),
				"You should use the Connection.setAutoCommit() method to set connection's auto-commit mode");

		Set<CtInvocation<?>> elementsWithCommit = new HashSet<CtInvocation<?>>();
		model.getElements(new TypeFilter<>(CtInvocation.class)).stream()
				.filter(element -> "commit".equals(element.getExecutable().getSimpleName())
						&& "java.sql.Connection".equals(element.getExecutable().getDeclaringType().getQualifiedName())
						&& !element.getParent(CtType.class).getQualifiedName().endsWith("Demo"))
				.forEach(element -> elementsWithCommit.add(element));
		assertFalse(elementsWithCommit.isEmpty(),
				"You should use the Connection.commit() method to save all changes made in the current transaction");

		Set<CtInvocation<?>> elementsWithRollback = new HashSet<CtInvocation<?>>();
		model.getElements(new TypeFilter<>(CtInvocation.class)).stream()
				.filter(element -> "rollback".equals(element.getExecutable().getSimpleName())
						&& "java.sql.Connection".equals(element.getExecutable().getDeclaringType().getQualifiedName())
						&& !element.getParent(CtType.class).getQualifiedName().endsWith("Demo"))
				.forEach(element -> elementsWithRollback.add(element));
		assertFalse(elementsWithRollback.isEmpty(),
				"You should use the Connection.rollback() method to undo all changes made in the current transaction");
	}
}