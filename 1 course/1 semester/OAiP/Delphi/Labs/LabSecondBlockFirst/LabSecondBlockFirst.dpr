Program LabSecondBlockFirst;
uses
  SysUtils;

Const
    MIN_NUM = 1;
    MAX_NUM = 31;
Var
    I, N: Byte;
    Sum: Real;
    Numb: Integer;
    IsIncorrect: Boolean;
Begin
    Writeln('Программа выводит на экран сумму. ', #10,
            'Диапазон значений для ввода числа N: 1..31');
    Repeat
        IsIncorrect := True;
        Write('Введите целое число N: ');
        Try
            Readln(N);
        Except
            IsIncorrect := False;
            Writeln('Проверьте ваши данные');
        End;
    If (IsIncorrect) and ((N < MIN_NUM) or (N > MAX_NUM)) Then
        Begin
            IsIncorrect := False;
            Writeln('Проверьте ваши данные');
        End;
    Until IsIncorrect;
    Sum := 0;
    Numb := 1;
    For I := 1 to N do
    Begin
        Numb := Numb * 2;
        Sum := Sum + (1 / Numb);
    End;
    Write('Сумма: ', FloatToStr(Sum));
    Readln;
End.

