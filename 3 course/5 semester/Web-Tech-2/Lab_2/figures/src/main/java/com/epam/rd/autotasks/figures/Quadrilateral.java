package com.epam.rd.autotasks.figures;

class Quadrilateral extends Figure {

    private final Point a;
    private final Point b;
    private final Point c;
    private final Point d;

    public Quadrilateral(Point a, Point b, Point c, Point d) {
        this.a = a;
        this.b = b;
        this.c = c;
        this.d = d;
    }

    @Override
    public double area() {
        Triangle t1 = new Triangle(a, b, c);
        Triangle t2 = new Triangle(a, c, d);
        return t1.area() + t2.area();
    }

    @Override
    public String pointsToString() {
        return String.format("(%s,%s)(%s,%s)(%s,%s)(%s,%s)",
                a.getX(), a.getY(),
                b.getX(), b.getY(),
                c.getX(), c.getY(),
                d.getX(), d.getY());
    }

    @Override
    public Point leftmostPoint() {
        Point leftmost = a;
        if (b.getX() < leftmost.getX()) leftmost = b;
        if (c.getX() < leftmost.getX()) leftmost = c;
        if (d.getX() < leftmost.getX()) leftmost = d;
        return leftmost;
    }
}
