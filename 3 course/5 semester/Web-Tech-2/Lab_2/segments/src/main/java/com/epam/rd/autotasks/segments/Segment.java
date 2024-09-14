package com.epam.rd.autotasks.segments;

import static java.lang.Math.abs;
import static java.lang.Math.sqrt;
import static java.lang.StrictMath.pow;

class Segment {

    private Point start;
    private Point end;

    public Segment(Point start, Point end) {
        if (start.getX() == end.getX() && start.getY() == end.getY()) {
            throw new IllegalArgumentException();
        }

        this.start = start;
        this.end = end;
    }

    double length() {
        double xLen = this.end.getX() - this.start.getX();
        double yLen = this.end.getY() - this.start.getY();
        return Math.sqrt(xLen * xLen + yLen * yLen);
    }

    Point middle() {
        return new Point(
                (this.end.getX() + this.start.getX()) / 2,
                (this.end.getY() + this.start.getY()) / 2
        );
    }

    Point intersection(Segment another) {
        double k1 = (this.end.getY() - this.start.getY()) / (this.end.getX() - this.start.getX());
        double b1 = this.start.getY() - k1 * this.start.getX();

        double k2 = (another.end.getY() - another.start.getY()) / (another.end.getX() - another.start.getX());
        double b2 = another.start.getY() - k2 * another.start.getX();

        if (k1 == k2) {
            return null;
        }

        double x = (b2 - b1) / (k1 - k2);
        double y = k1 * x + b1;
        Point intersectionPoint = new Point(x, y);

        if (isBetween(this.start, this.end, intersectionPoint) &&
                isBetween(another.start, another.end, intersectionPoint)) {
            return intersectionPoint;
        } else {
            return null;
        }
    }

    private boolean isBetween(Point start, Point end, Point point) {
        return Math.min(start.getX(), end.getX()) <= point.getX() && point.getX() <= Math.max(start.getX(), end.getX()) &&
                Math.min(start.getY(), end.getY()) <= point.getY() && point.getY() <= Math.max(start.getY(), end.getY());
    }


}
