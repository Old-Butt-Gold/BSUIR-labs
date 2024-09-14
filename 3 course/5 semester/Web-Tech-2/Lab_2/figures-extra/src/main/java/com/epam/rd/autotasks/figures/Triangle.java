package com.epam.rd.autotasks.figures;

class Triangle extends Figure {

    private final Point a;
    private final Point b;
    private final Point c;

    public Triangle(Point a, Point b, Point c) {
        if (a == null || b == null || c == null || area(a, b, c) == 0) {
            throw new IllegalArgumentException();
        }
        this.a = a;
        this.b = b;
        this.c = c;
    }

    public double area(Point a, Point b, Point c) {
        return Math.abs(a.getX() * (b.getY() - c.getY()) +
                b.getX() * (c.getY() - a.getY()) +
                c.getX() * (a.getY() - b.getY())) / 2.0;
    }

    @Override
    public Point centroid() {
        double centroidX = (a.getX() + b.getX() + c.getX()) / 3;
        double centroidY = (a.getY() + b.getY() + c.getY()) / 3;
        return new Point(centroidX, centroidY);
    }

    @Override
    public boolean isTheSame(Figure figure) {
        if (!(figure instanceof Triangle)) {
            return false;
        }
        Triangle other = (Triangle) figure;

        return pointsMatchClockwise(this.a, this.b, this.c, other) || pointsMatchCounterClockwise(this.a, this.b, this.c, other);
    }

    private boolean pointsMatchClockwise(Point a, Point b, Point c, Triangle other) {
        return (a.equals(other.a) && b.equals(other.b) && c.equals(other.c)) ||
                (a.equals(other.b) && b.equals(other.c) && c.equals(other.a)) ||
                (a.equals(other.c) && b.equals(other.a) && c.equals(other.b));
    }

    private boolean pointsMatchCounterClockwise(Point a, Point b, Point c, Triangle other) {
        return (a.equals(other.a) && b.equals(other.c) && c.equals(other.b)) ||
                (a.equals(other.b) && b.equals(other.a) && c.equals(other.c)) ||
                (a.equals(other.c) && b.equals(other.b) && c.equals(other.a));
    }
}
