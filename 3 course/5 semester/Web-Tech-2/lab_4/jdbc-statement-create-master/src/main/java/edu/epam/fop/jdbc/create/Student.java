package edu.epam.fop.jdbc.create;

import java.util.Objects;

public class Student {
	private int id;
	private String firstName;
	private String lastName;
	private Group group;

	public Student(int studentId, String studentFirstName, String studentLastName, Group studentGroup) {
		id = studentId;
		firstName = studentFirstName;
		lastName = studentLastName;
		group = studentGroup;
	}

	public Student(String studentFirstName, String studentLastName, Group studentGroup) {
		firstName = studentFirstName;
		lastName = studentLastName;
		group = studentGroup;
	}

	public int getId() {
		return id;
	}

	public void setId(int studentId) {
		id = studentId;
	}

	public String getFirstName() {
		return firstName;
	}

	public void setFirstName(String studentFirstName) {
		firstName = studentFirstName;
	}

	public String getLastName() {
		return lastName;
	}

	public void setLastName(String studentLastName) {
		lastName = studentLastName;
	}

	public Group getGroup() {
		return group;
	}

	public void setGroup(Group studentGroup) {
		group = studentGroup;
	}

	@Override
	public String toString() {
		return firstName + " " + lastName + " " + group;
	}

	@Override
	public int hashCode() {
		return Objects.hash(firstName, lastName, group);
	}

	@Override
	public boolean equals(Object object) {
		if (this == object) {
			return true;
		}
		if (object == null || getClass() != object.getClass()) {
			return false;
		}
		Student other = (Student) object;
		return Objects.equals(firstName, other.firstName) && Objects.equals(group, other.group)
				&& Objects.equals(lastName, other.lastName);
	}
}