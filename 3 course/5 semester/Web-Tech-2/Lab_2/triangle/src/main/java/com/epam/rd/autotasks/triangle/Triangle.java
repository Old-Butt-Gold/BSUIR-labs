package com.epam.rd.autotasks.triangle;

class Triangle {

    private Point a;
    private Point b;
    private Point c;

    public Triangle(Point a, Point b, Point c) {
        this.a = a;
        this.b = b;
        this.c = c;

        if (arePointsEqual(a, b) || arePointsEqual(a, c) || arePointsEqual(b, c)) {
            throw new IllegalArgumentException("The points must be distinct.");
        }

        if (area() == 0.0) {
            throw new IllegalArgumentException("The points must not be collinear.");
        }
    }

    private boolean arePointsEqual(Point p1, Point p2) {
        return p1.getX() == p2.getX() && p1.getY() == p2.getY();
    }

    public double area()
    {
        return 0.5 * Math.abs(
                a.getX() * (b.getY() - c.getY()) +
                        b.getX() * (c.getY() - a.getY()) +
                        c.getX() * (a.getY() - b.getY())
        );
    }

    public Point centroid()
    {
        double xCentroid = (a.getX() + b.getX() + c.getX()) / 3;
        double yCentroid = (a.getY() + b.getY() + c.getY()) / 3;
        return new Point(xCentroid, yCentroid);
    }

}
