package com.epam.rd.autotasks.sprintplanning.tickets;

public class UserStory extends Ticket {

    UserStory[] userStories;

    public UserStory(int id, String name, int estimate, UserStory... dependsOn) {
        super(id, name, estimate);
        userStories = dependsOn;
    }

    @Override
    public void complete() {
        for (UserStory dependency : userStories) {
            if (!dependency.isCompleted()) {
                return;
            }
        }
        super.complete();
    }

    public UserStory[] getDependencies() {
        return userStories.clone();
    }

    @Override
    public String toString() {
        return "[US " + getId() + "] " + getName();
    }
}
