Program LabFourthBlockFirst;
uses
  System.SysUtils,
  Math;

Const
    MIN_NUM = 3;
Var
    N, I: Integer;
    Abscisses, Ordinates: Array of Real;
    Perimetr: Real;
    IsIncorrect: Boolean;
Begin
    Repeat
        Write('Введите число сторон у многоугольника — N (N > 2): ');
        IsIncorrect := True;
        Try
            Readln(N);
        Except
            IsIncorrect := False;
            Writeln('Проверьте правильность ввода');
        End;
        If (IsIncorrect) and (N < MIN_NUM) Then
        Begin
            IsIncorrect := False;
            Writeln('Проверьте правильность ввода данных в нужном диапазоне');
        End;
    Until IsIncorrect;
    SetLength(Abscisses, N);
    SetLength(Ordinates, N);
    Perimetr := 0;
    //Массив начнется с нуля (0) после SetLength:
    For I := 0 to (N - 1) do
    Begin
        Repeat
            IsIncorrect := True;
            Write('Abscissa[',I + 1,']= ');
            Try
                Readln(Abscisses[I]);
            Except
                IsIncorrect := False;
                Writeln('Проверьте правильность ввода');
            End;
        Until IsIncorrect;
    End;
    For I := 0 to (N - 1) do
    Begin
        Repeat
            IsIncorrect := True;
            Write('Ordinate[',I + 1,']= ');
            Try
                Readln(Ordinates[I]);
            Except
                IsIncorrect := False;
                Writeln('Проверьте правильность ввода');
            End;
        Until IsIncorrect;
    End;
    //сначала ищем периметр (N-1) сторон, после цикла найдем сторону, соединяющую начало и конец.
    For I := 0 to (N - 2) do
    Begin
        Perimetr := Perimetr + Sqrt((Abscisses[I] - Abscisses[I + 1]) * (Abscisses[I] - Abscisses[I + 1]) + (Ordinates[I] - Ordinates[I + 1]) * (Ordinates[I] - Ordinates[I + 1]));
    End;
    Perimetr := Perimetr + Sqrt((Abscisses[0] - Abscisses[N - 1]) * (Abscisses[0] - Abscisses[N - 1]) + (Ordinates[0] - Ordinates[N - 1]) * (Ordinates[0] - Ordinates[N - 1]));
    Write('Периметр равен: ', FloatToStr(Perimetr));
    Readln;
End.

