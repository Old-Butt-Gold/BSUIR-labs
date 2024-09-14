package com.epam.rd.autotasks.figures;

class Point {
    private double x;
    private double y;

    private final double delta = 1e-6;

    public Point(final double x, final double y) {
        this.x = x;
        this.y = y;
    }

    public double getX() {
        return x;
    }

    public double getY() {
        return y;
    }

    public boolean equals(Point other) {
        return Math.abs(this.x - other.x) <= delta && Math.abs(this.y - other.y) <= delta;
    }
}
