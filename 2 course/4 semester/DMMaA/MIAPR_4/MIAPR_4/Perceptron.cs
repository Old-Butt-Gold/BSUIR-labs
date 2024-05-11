using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace MIAPR_4;

class PerceptronAmountInfo
{
    public int ClassesCount { get; private set; }
    public int ObjectInClassCount { get; private set; }
    public int AttributesCount { get; private set; }
    public override string ToString()
    {
        return $"Classes: {ClassesCount}\nObjects: {ObjectInClassCount}\nAttributes: {AttributesCount}";
    }

    public PerceptronAmountInfo(int classes, int objects, int attributesCount)
    {
        ClassesCount = classes;
        ObjectInClassCount = objects;
        AttributesCount = attributesCount;
    }
}

class PerceptronObject
{
    public readonly List<int> Attributes = new();
    public override string ToString() => string.Join(" ", Attributes.Select(x => x));
}

class PerceptronClass
{
    public readonly List<PerceptronObject> Objects = new();
}

class Perceptron
{
    const int RandomSeed = 10;
    const int MaxIterationsCount = 10000;

    readonly PerceptronAmountInfo _info;
    readonly List<PerceptronClass> _classes = new();
    readonly List<PerceptronObject> _weights = new();

    public Perceptron(int classes, int objectsInClass, int attributes)
    {
        _info = new PerceptronAmountInfo(classes, objectsInClass, attributes);
    }

    public void Train()
    {
        InitializeRandomClasses();
        LearnDecisionFunctions();
    }

    void InitializeRandomClasses()
    {
        var rand = new Random();

        for (int i = 0; i < _info.ClassesCount; i++)
        {
            var currentClass = new PerceptronClass();

            for (int j = 0; j < _info.ObjectInClassCount; j++)
            {
                var currentObject = new PerceptronObject();

                for (int k = 0; k < _info.AttributesCount; k++)
                    currentObject.Attributes.Add(rand.Next(RandomSeed) - RandomSeed / 2);

                currentClass.Objects.Add(currentObject);
            }
            _classes.Add(currentClass);
        }

        foreach (var perceptronClass in _classes)
        {
            foreach (var perceptronObject in perceptronClass.Objects)
                perceptronObject.Attributes.Add(1);
        }

        for (int i = 0; i < _info.ClassesCount; i++)
        {
            PerceptronObject weight = new PerceptronObject();
            for (int j = 0; j < _info.ClassesCount; j++)
            {
                weight.Attributes.Add(0);
            }

            _weights.Add(weight);
        }
    }

    void LearnDecisionFunctions()
    {
        bool isClassification = true;
        int iteration = 0;

        while (isClassification && iteration < MaxIterationsCount)
        {
            for (var i = 0; i < _classes.Count; i++)
            {
                var currentClass = _classes[i];
                var currentWeight = _weights[i];

                foreach (var currentObject in currentClass.Objects)
                {
                    isClassification = CorrectWeight(currentObject, currentWeight, i);
                }
            }
            iteration++;
        }

        if (iteration == MaxIterationsCount)
            MessageBox.Show($"Количество итераций превысило {MaxIterationsCount}.{Environment.NewLine}Решаюшие функции, возможно, найдены неправильно.");

    }

    bool CorrectWeight(PerceptronObject currentObject, PerceptronObject currentWeight, int classNumber)
    {
        var result = false;
        var objectDecision = ObjectMultiplication(currentWeight, currentObject);

        for (var i = 0; i < _weights.Count; i++)
        {
            if (i == classNumber) continue;
            
            var currentDecision = ObjectMultiplication(_weights[i], currentObject);

            if (objectDecision <= currentDecision)
            {
                ChangeWeight(_weights[i], currentObject, -1);
                result = true;
            }
        }
        if (result)
            ChangeWeight(currentWeight, currentObject, 1);

        return result;
    }

    int ObjectMultiplication(PerceptronObject weight, PerceptronObject obj) =>
        weight.Attributes.Zip(obj.Attributes, (w, o) => w * o).Sum();
        
    void ChangeWeight(PerceptronObject weight, PerceptronObject perceptronObject, int sign)
    {
        for (var i = 0; i < weight.Attributes.Count; i++)
        {
            weight.Attributes[i] += sign * perceptronObject.Attributes[i];
        }
    }

    public void FillListViews(ListView listViewSelection, ListView listViewSolutions)
    {
        var indexCurrentClass = 1;

        foreach (var currentClass in _classes)
        {
            var indexCurrentObject = 1;
            listViewSelection.CreateTextBlockOnListView($"Класс {indexCurrentClass}:");
            foreach (var currentObject in currentClass.Objects)
            {
                var str = $"Объект {indexCurrentObject}: (";

                for (var j = 0; j < currentObject.Attributes.Count - 1; j++)
                {
                    var attribute = currentObject.Attributes[j];
                    str += attribute + ",";
                }
                str = str.Remove(str.Length - 1, 1);
                str += ")";
                listViewSelection.CreateTextBlockOnListView(str);
                indexCurrentObject++;
            }
            indexCurrentClass++;
        }

        for (var i = 0; i < _weights.Count; i++)
        {
            var str = $"d{i + 1}(x) = ";

            for (var j = 0; j < _weights[i].Attributes.Count; j++)
            {
                var attribute = _weights[i].Attributes[j];

                if (j < _weights[i].Attributes.Count - 1)
                    if (attribute >= 0 && j != 0)
                        str += "+" + attribute + $"·x{j + 1}";
                    else
                        str += attribute + $"·x{j + 1}";
                else if (attribute >= 0 && j != 0)
                    str += "+" + attribute;
                else
                    str += attribute;
            }
            listViewSolutions.CreateTextBlockOnListView(str);
        }
    }

    public int Classify(PerceptronObject perceptronObject)
    {
        var resultClass = 0;
        perceptronObject.Attributes.Add(1);

        var decisionMax = ObjectMultiplication(_weights[0], perceptronObject);

        for (var i = 1; i < _weights.Count; i++)
        {
            var currentDecision = ObjectMultiplication(_weights[i], perceptronObject);
            if (currentDecision > decisionMax)
            {
                decisionMax = currentDecision;
                resultClass = i;
            }
        }

        return resultClass + 1;
    }
}