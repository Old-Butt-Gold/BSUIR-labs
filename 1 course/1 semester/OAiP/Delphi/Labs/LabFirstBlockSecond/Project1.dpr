Program A1;
Uses
    System.SysUtils;
Var
    TruncNumb, Sign: Integer;
    Numb, FracNumb: Real;
Begin
    Writeln('Введите число: ');
    Readln(Numb);
    TruncNumb := Trunc(Numb);
    Sign := 0;
    If TruncNumb = 0 Then
        Inc(Sign);
    While (TruncNumb > 0) Do
        Begin
            TruncNumb := TruncNumb div 10;
            Inc(Sign);
        End;
    FracNumb := Frac(Numb);
    While (FracNumb > 0) Do
        Begin
            Numb := Numb * 10;
            Inc(Sign);
            FracNumb := Frac(Numb);
        End;

    Writeln('Количество знаков числа: ', Sign);
    Readln;
End.
