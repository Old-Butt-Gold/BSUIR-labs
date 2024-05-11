using System;
using System.Collections.Generic;
using System.Windows;

namespace MIAPR_5;

public class Potentials
{
    const int ClassCount = 2;

    const int IterationsCount = 50;

    int _correction;

    Function _result;

    public bool Warning { get; private set; }

    public Function GetFunction(List<Point>[] teachingPoints)
    {
        _result = new Function(0, 0, 0, 0);
        _correction = 1;
        var nextIteration = true;
        var iterationNumber = 0;
        while (nextIteration && iterationNumber < IterationsCount)
        {
            nextIteration = DoOneIteration(teachingPoints);
            iterationNumber++;
        }

        Warning = iterationNumber == IterationsCount;
        return _result;
    }

    bool DoOneIteration(List<Point>[] teachingPoints)
    {
        var nextIteration = false;

        if (teachingPoints.Length != ClassCount) throw new Exception();

        for (var classNumber = 0; classNumber < ClassCount; classNumber++)
        {
            int pointInClass = teachingPoints[classNumber].Count;
            for (var i = 0; i < pointInClass; i++)
            {
                _result += _correction * PartFunction(teachingPoints[classNumber][i]);
                
                var index = (i + 1) % pointInClass;
                var nextClassNumber = index == 0 
                    ? (classNumber + 1) % ClassCount 
                    : classNumber;
                
                var nextPoint = teachingPoints[nextClassNumber][index];
                _correction = GetNewCorrection(nextPoint, nextClassNumber);
                if (_correction != 0) 
                    nextIteration = true;
            }
        }

        return nextIteration;
    }

    int GetNewCorrection(Point nextPoint, int nextClassNumber)
    {
        var functionValue = _result.GetValue(nextPoint);
        
        if (functionValue <= 0 && nextClassNumber == 0)
            return 1;

        if (functionValue > 0 && nextClassNumber == 1)
            return -1;

        return 0;
    }
    
    Function PartFunction(Point point) => new(4 * point.X, 4 * point.Y, 16 * point.X * point.Y, 1);
}