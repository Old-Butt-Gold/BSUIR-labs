package com.epam.rd.autotasks;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertNotNull;

import com.epam.rd.autotasks.config.AppConfig;
import org.junit.jupiter.api.Test;
import org.springframework.context.annotation.AnnotationConfigApplicationContext;

class TaskTest {

    @Test
    void testTaskConfiguration() {
        try (AnnotationConfigApplicationContext context = new AnnotationConfigApplicationContext(AppConfig.class)) {

            Task task = context.getBean(Task.class);
            assertNotNull(task, "Task should not be null");
            assertNotNull(task.getAssignee(), "Task assignee should not be null");
            assertNotNull(task.getReviewer(), "Task reviewer should not be null");
            assertEquals("New feature", task.getDescription(), "Task description should be \"New feature\"");
            assertEquals("John Doe", task.getAssignee().getName(), "Assignee name should be \"John Doe\"");
            assertEquals("Junior Software Engineer", task.getAssignee().getPosition(), "Assignee position should be \"Junior Software Engineer\"");
            assertEquals("Emily Brown", task.getReviewer().getName(), "Reviewer name should be \"Emily Brown\"");
            assertEquals("Senior Software Engineer", task.getReviewer().getPosition(), "Reviewer name should be \"Senior Software Engineer\"");
        }
    }
}
