package edu.epam.fop.model;

import java.util.Objects;

public class FormData {
    private String name;

    private int age;

    public FormData(String name, int age) {
        this.name = name;
        this.age = age;
    }

    public FormData() {

    }

    public String getName() {
        return name;
    }

    public void setName(String name) {
        this.name = name;
    }

    public int getAge() {
        return age;
    }

    public void setAge(int age) {
        this.age = age;
    }

    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (o == null || getClass() != o.getClass()) return false;
        FormData formData = (FormData) o;
        return age == formData.age && Objects.equals(name, formData.name);
    }

    @Override
    public int hashCode() {
        return Objects.hash(name, age);
    }
}
