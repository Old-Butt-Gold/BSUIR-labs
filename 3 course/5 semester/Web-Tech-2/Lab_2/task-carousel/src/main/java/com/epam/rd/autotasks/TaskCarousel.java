package com.epam.rd.autotasks;

public class TaskCarousel {

    private final Task[] tasks;
    private int currentIndex;
    private int taskCount;

    public TaskCarousel(int capacity) {
        tasks = new Task[capacity];
        currentIndex = 0;
        taskCount = 0;
    }

    public boolean addTask(Task task) {
        if (task == null || isFull() || task.isFinished()) {
            return false;
        }

        for (int i = 0; i < tasks.length; i++) {
            if (tasks[i] == null) {
                tasks[i] = task;
                taskCount++;
                return true;
            }
        }
        return false;
    }

    public boolean execute() {
        if (isEmpty()) {
            return false;
        }

        int executedTasksChecked = 0;
        while (executedTasksChecked < tasks.length) {
            if (tasks[currentIndex] != null) {
                tasks[currentIndex].execute();
                if (tasks[currentIndex].isFinished()) {
                    tasks[currentIndex] = null;
                    taskCount--;
                }
                currentIndex = (currentIndex + 1) % tasks.length;
                return true;
            }

            currentIndex = (currentIndex + 1) % tasks.length;
            executedTasksChecked++;
        }

        return false;
    }

    public boolean isFull() {
        return taskCount == tasks.length;
    }

    public boolean isEmpty() {
        return taskCount == 0;
    }

}
