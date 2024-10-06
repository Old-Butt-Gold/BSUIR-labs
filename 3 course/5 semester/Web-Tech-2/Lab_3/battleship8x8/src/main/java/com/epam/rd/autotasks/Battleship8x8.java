package com.epam.rd.autotasks;

public class Battleship8x8 {
    private final long ships;
    private long shots = 0L;

    public Battleship8x8(final long ships) {
        this.ships = ships;
    }

    private int convertShotToIndex(String shot) {
        if (shot == null || shot.length() != 2) {
            throw new IllegalArgumentException("Invalid shot format");
        }

        char row = shot.charAt(0);
        char column = shot.charAt(1);

        int rowIndex = 7 - (row - 'A'); //инверсия
        int colIndex = 7 - (column - '1'); //инверсия

        if (rowIndex < 0 || rowIndex > 7 || colIndex < 0 || colIndex > 7) {
            throw new IllegalArgumentException("Invalid shot format");
        }

        return rowIndex + colIndex * 8;
    }

    public boolean shoot(String shot) {
        int index = convertShotToIndex(shot);

        long shotBit = 1L << index;
        shots |= shotBit;

        return (ships & shotBit) != 0;
    }

    private char getCellState(int index) {
        long mask = 1L << (63 - index); //инверсия

        boolean hasShip = (ships & mask) != 0;
        boolean hasShot = (shots & mask) != 0;

        if (hasShip && hasShot) {
            return '☒';
        } else if (hasShip) {
            return '☐';
        } else if (hasShot) {
            return '×';
        } else {
            return '.';
        }
    }

    public String state() {
        StringBuilder result = new StringBuilder();

        for (int i = 0; i < 64; i++) {
            char cell = getCellState(i);

            result.append(cell);

            if (i % 8 == 7) {
                result.append('\n');
            }
        }

        return result.toString();
    }
}
