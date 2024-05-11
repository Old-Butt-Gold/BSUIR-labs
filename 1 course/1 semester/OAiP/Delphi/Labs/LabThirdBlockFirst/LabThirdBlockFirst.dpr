Program LabThirdBlockFirst;
uses
  System.SysUtils,
  Math;

Var
    A, H: Real;
    IsIncorrect: Boolean;
Begin
    Writeln('»змен€€ х от а с шагом h, определить, при каком значении х ',
            'SIN(x) станет больше COS(x).');
    Repeat
        IsIncorrect := True;
        Write('¬ведите A Ч начальное значение X: ');
        Try
            Readln(A);
        Except
            IsIncorrect := False;
            Writeln('ѕроверьте правильность ввода!');
        End;
    Until IsIncorrect;

    Repeat
        IsIncorrect := True;
        Write('¬ведите шаг изменени€ X Ч H: ');
        Try
            Readln(H);
        Except
            IsIncorrect := False;
            Writeln('ѕроверьте правильность ввода!');
        End;
    Until IsIncorrect;
    While (not (sin(A) > cos(A))) do
        A := A + H;
    Write('—инус больше косинуса при X, равном: ', FloatToStr(A),#10,
          'sin(x) = ', FloatToStr(sin(A)), '; cos(x) = ', FloatToStr(cos(A)), '.');
    Readln;
End.

