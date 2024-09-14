package com.epam.rd.autotasks;

class Spiral {
    static int[][] spiral(int rows, int columns) {
        int rowCount = rows;
        int colCount = columns;
        int[][] matrix = new int[rows][columns];
        int left = 0;
        int right = colCount - 1;
        int top = 0;
        int bottom = rowCount - 1;

        int counter = 1;

        while (left <= right && top <= bottom)
        {
            for (int i = left; i <= right; i++)
            {
                matrix[top][i] = counter++;
            }
            top++;

            for (int i = top; i <= bottom; i++)
            {
                matrix[i][right] = counter++;
            }
            right--;

            if (top <= bottom)
            {
                for (int i = right; i >= left; i--)
                {
                    matrix[bottom][i] = counter++;
                }
                bottom--;
            }

            if (left <= right)
            {
                for (int i = bottom; i >= top; i--)
                {
                    matrix[i][left] = counter++;
                }
                left++;
            }
        }

        return matrix;
    }
}
