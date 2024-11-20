package edu.epam.fop.jdbc.transactions;

public class Group {
	private int id;
	private String name;

	public Group(int groupId, String groupName) {
		id = groupId;
		name = groupName;
	}

	public Group(String groupName) {
		name = groupName;
	}

	public int getId() {
		return id;
	}

	public void setId(int groupId) {
		id = groupId;
	}

	public String getName() {
		return name;
	}

	public void setName(String groupName) {
		name = groupName;
	}

	@Override
	public String toString() {
		return name;
	}

	@Override
	public int hashCode() {
		final int prime = 31;
		int result = 1;
		result = prime * result + ((name == null) ? 0 : name.hashCode());
		return result;
	}

	@Override
	public boolean equals(Object object) {
		if (this == object) {
			return true;
		}
		if (object == null || getClass() != object.getClass()) {
			return false;
		}
		Group other = (Group) object;
		if (name == null) {
			if (other.name != null)
				return false;
		} else if (!name.equals(other.name))
			return false;
		return true;
	}
}