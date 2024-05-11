using System.Windows;

namespace MIAPR_5;

public class Function(double xCoef, double yCoef, double xyCoef, double freeCoef)
{
    double XCoef { get; set; } = xCoef;
    double YCoef { get; set; } = yCoef;
    double XYCoef { get; set; } = xyCoef;
    double FreeCoef { get; set; } = freeCoef;
    
    public double GetValue(Point point) => 
        FreeCoef + XCoef * point.X + YCoef * 
        point.Y + XYCoef * point.X * point.Y;

    public double GetY(double x) => -(XCoef * x + FreeCoef) / (XYCoef * x + YCoef);

    public override string ToString()
    {
        if (XYCoef != 0)
        {
            return $"y=({-XCoef}·x{(-FreeCoef < 0 ? "" : "+")}{-FreeCoef})/({XYCoef}·x{(YCoef < 0 ? "" : "+")}{YCoef})";
        }
        if (YCoef != 0)
        {
            return $"y={-XCoef / YCoef}·x{(-FreeCoef / YCoef < 0 ? "" : "+")}{-FreeCoef / YCoef}";
        }
        return $"x={-FreeCoef / XCoef}";
    }

    public static Function operator +(Function first, Function second) => 
        new (first.XCoef + second.XCoef, first.YCoef + second.YCoef,
            first.XYCoef + second.XYCoef, first.FreeCoef + second.FreeCoef);

    public static Function operator *(int koeff, Function function) => 
        new(koeff * function.XCoef, koeff * function.YCoef, 
            koeff * function.XYCoef,koeff * function.FreeCoef);
}