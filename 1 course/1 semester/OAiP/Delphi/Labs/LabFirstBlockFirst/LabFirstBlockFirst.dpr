Program LabFirstBlockFirst;
uses
  System.SysUtils;

Var
    Abscissa, Ordinate: Real;
    IsIncorrect: Boolean;
Begin
    Writeln('Даны координаты точки М(х,у). Определите, принадлежит ',
            'ли данная точка замкнутому множеству D.',#10);
    Writeln(5 - 12 and 5);
    Repeat
        IsIncorrect := True;
        Write('Введите координату оси абсцисс: ');
        Try
            Readln(Abscissa);
        Except
            IsIncorrect := False;
            Writeln('Проверьте правильность ввода!');
        End;
    Until IsIncorrect;

    Repeat
        IsIncorrect := True;
        Write('Введите координату оси ординат: ');
        Try
            Readln(Ordinate);
        Except
            IsIncorrect := False;
            Writeln('Проверьте правильность ввода!');
        End;
    Until IsIncorrect;
    If (Ordinate > (-(Abscissa / 2) + 1)) or (Ordinate < 0) or (Abscissa < 0) or (Abscissa > 2) Then
        Writeln('Точка M не принадлежит плоскости D ')
    Else
        Writeln('Точка M принадлежит плоскости D ');

    Readln;
End.

