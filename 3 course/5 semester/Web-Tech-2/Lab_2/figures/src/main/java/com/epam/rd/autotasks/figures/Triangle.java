package com.epam.rd.autotasks.figures;

class Triangle extends Figure {

    private final Point a;
    private final Point b;
    private final Point c;

    public Triangle(Point a, Point b, Point c) {
        this.a = a;
        this.b = b;
        this.c = c;
    }
    @Override
    public double area() {
        return 0.5 * Math.abs(
                a.getX() * (b.getY() - c.getY()) +
                        b.getX() * (c.getY() - a.getY()) +
                        c.getX() * (a.getY() - b.getY())
        );

    }

    @Override
    public String pointsToString() {
        return String.format("(%s,%s)(%s,%s)(%s,%s)",
                a.getX(), a.getY(),
                b.getX(), b.getY(),
                c.getX(), c.getY());
    }

    @Override
    public Point leftmostPoint() {
        Point leftmost = a;
        if (b.getX() < leftmost.getX()) leftmost = b;
        if (c.getX() < leftmost.getX()) leftmost = c;
        return leftmost;
    }
}
