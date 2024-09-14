package com.epam.rd.autotasks;

public class CompleteByRequestTask implements Task {

    boolean isCompleted = false;
    boolean isFinished = false;
    @Override
    public void execute() {
        if (isCompleted) {
            isFinished = true;
        }
    }

    @Override
    public boolean isFinished() {
        return isFinished;
    }

    public void complete() {
        isCompleted = true;
    }
}
