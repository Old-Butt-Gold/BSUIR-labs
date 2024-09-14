package com.epam.rd.autotasks.figures;

class Circle extends Figure {

    private final Point center;
    private final double radius;

    public Circle(Point center, double radius) {
        this.center = center;
        this.radius = radius;
    }

    @Override
    public double area() {
        return Math.PI * radius * radius;
    }

    @Override
    public String pointsToString() {
        return String.format("(%s,%s)%s", center.getX(), center.getY(), radius);
    }

    @Override
    public Point leftmostPoint() {
        return new Point(center.getX() - radius, center.getY());
    }
}
