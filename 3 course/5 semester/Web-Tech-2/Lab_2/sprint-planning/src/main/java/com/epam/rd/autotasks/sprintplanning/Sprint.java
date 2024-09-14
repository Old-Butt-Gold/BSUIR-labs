package com.epam.rd.autotasks.sprintplanning;

import com.epam.rd.autotasks.sprintplanning.tickets.Bug;
import com.epam.rd.autotasks.sprintplanning.tickets.Ticket;
import com.epam.rd.autotasks.sprintplanning.tickets.UserStory;

import java.util.ArrayList;

public class Sprint {

    private final int capacity;
    private final int ticketsLimit;
    private int currentEstimate;
    private int ticketsCount;
    private final Ticket[] tickets;
    private int index;

    public Sprint(int capacity, int ticketsLimit) {
        this.capacity = capacity;
        this.ticketsLimit = ticketsLimit;
        this.currentEstimate = 0;
        this.ticketsCount = 0;
        this.tickets = new Ticket[ticketsLimit];
        this.index = 0;
    }

    public boolean addUserStory(UserStory userStory) {
        if (userStory == null || userStory.isCompleted() ||
                userStory.getEstimate() + currentEstimate > capacity ||
                ticketsCount >= ticketsLimit) {
            return false;
        }

        for (UserStory dependency : userStory.getDependencies()) {
            boolean dependencyFound = false;
            for (Ticket ticket : tickets) {
                if (ticket == dependency) {
                    dependencyFound = true;
                    break;
                }
            }
            if (!dependencyFound) {
                return false;
            }
        }

        tickets[index++] = userStory;
        currentEstimate += userStory.getEstimate();
        ticketsCount++;
        return true;
    }

    public boolean addBug(Bug bugReport) {
        if (bugReport == null || bugReport.isCompleted() || bugReport.getEstimate() + currentEstimate > capacity || ticketsCount >= ticketsLimit) {
            return false;
        }

        tickets[index++] = bugReport;
        currentEstimate += bugReport.getEstimate();
        ticketsCount++;
        return true;
    }

    public Ticket[] getTickets() {
        Ticket[] result = new Ticket[ticketsCount];
        System.arraycopy(tickets, 0, result, 0, ticketsCount);  // Defensive copy of the tickets array
        return result;
    }

    public int getTotalEstimate() {
        return currentEstimate;
    }
}
