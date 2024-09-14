package com.epam.rd.autotasks.figures;

class Quadrilateral extends Figure {

    private final Point a;
    private final Point b;
    private final Point c;
    private final Point d;

    public Quadrilateral(Point a, Point b, Point c, Point d) {
        if (a == null || b == null || c == null || d == null || area(a, b, c, d) == 0 || !isConvex(a, b, c, d)) {
            throw new IllegalArgumentException();
        }

        this.a = a;
        this.b = b;
        this.c = c;
        this.d = d;
    }

    private boolean isConvex(Point a, Point b, Point c, Point d) {
        double cross1 = crossProduct(a, b, c);
        double cross2 = crossProduct(b, c, d);
        double cross3 = crossProduct(c, d, a);
        double cross4 = crossProduct(d, a, b);
        return (cross1 > 0 && cross2 > 0 && cross3 > 0 && cross4 > 0) ||
                (cross1 < 0 && cross2 < 0 && cross3 < 0 && cross4 < 0);
    }

    private double crossProduct(Point p1, Point p2, Point p3) {
        return (p2.getX() - p1.getX()) * (p3.getY() - p1.getY()) - (p2.getY() - p1.getY()) * (p3.getX() - p1.getX());
    }

    private double area(Point a, Point b, Point c, Point d) {
        Triangle t1 = new Triangle(a, b, c);
        Triangle t2 = new Triangle(a, c, d);
        return t1.area(a, b, c) + t2.area(a, c, d);
    }

    @Override
    public Point centroid() {
        Point center1 = new Triangle(a, b, c).centroid();
        Point center2 = new Triangle(a, c, d).centroid();
        Point center3 = new Triangle(a, d, b).centroid();
        Point center4 = new Triangle(b, d, c).centroid();

        double k1 = (center2.getY() - center1.getY()) / (center2.getX() - center1.getX());
        double b1 = center1.getY() - k1 * center1.getX();

        double k2 = (center4.getY() - center3.getY()) / (center4.getX() - center3.getX());
        double b2 = center3.getY() - k2 * center3.getX();

        if (k1 == k2) {
            return null;
        }

        double x = (b2 - b1) / (k1 - k2);
        double y = k1 * x + b1;

        return new Point(x, y);
    }


    @Override
    public boolean isTheSame(Figure figure) {
        if (!(figure instanceof Quadrilateral)) {
            return false;
        }
        Quadrilateral other = (Quadrilateral) figure;

        return pointsMatchClockwise(this.a, this.b, this.c, this.d, other) || pointsMatchCounterClockwise(this.a, this.b, this.c, this.d, other);
    }

    private boolean pointsMatchClockwise(Point a, Point b, Point c, Point d, Quadrilateral other) {
        return (a.equals(other.a) && b.equals(other.b) && c.equals(other.c) && d.equals(other.d)) ||
                (a.equals(other.b) && b.equals(other.c) && c.equals(other.d) && d.equals(other.a)) ||
                (a.equals(other.c) && b.equals(other.d) && c.equals(other.a) && d.equals(other.b)) ||
                (a.equals(other.d) && b.equals(other.a) && c.equals(other.b) && d.equals(other.c));
    }

    private boolean pointsMatchCounterClockwise(Point a, Point b, Point c, Point d, Quadrilateral other) {
        return (a.equals(other.a) && b.equals(other.d) && c.equals(other.c) && d.equals(other.b)) ||
                (a.equals(other.b) && b.equals(other.a) && c.equals(other.d) && d.equals(other.c)) ||
                (a.equals(other.c) && b.equals(other.b) && c.equals(other.a) && d.equals(other.d)) ||
                (a.equals(other.d) && b.equals(other.c) && c.equals(other.b) && d.equals(other.a));
    }

}
