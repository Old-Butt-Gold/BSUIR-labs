package com.epam.rd.autotasks.figures;

abstract class Figure {

    protected final double Delta = 1e-6;

    public abstract Point centroid();
    public abstract boolean isTheSame(Figure figure);
}
