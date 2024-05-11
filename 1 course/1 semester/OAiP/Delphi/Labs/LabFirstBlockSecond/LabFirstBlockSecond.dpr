Program LabFirstBlockSecond;
Uses
    System.SysUtils, Math;
Var
    N, I, J: Integer;
    CrossLine1, CrossLine2: Real;
    Abscisses, Ordinates: Array of Real;
    IsIncorrect, IsTrue: Boolean;
Const
    MIN_NUM = 3;
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
            Writeln('Проверьте правильность ввода');
        End;
    Until IsIncorrect;
    SetLength(Abscisses, N + 3);
    SetLength(Ordinates, N + 3);
    For I := 1 to N do
    Begin
        Repeat
            IsIncorrect := True;
            Write('Abscissa[',I,']= ');
            Try
                Readln(Abscisses[I]);
            Except
                IsIncorrect := False;
                Writeln('Проверьте правильность ввода');
            End;
        Until IsIncorrect;
    End;
    For I := 1 to N do
    Begin
        Repeat
            IsIncorrect := True;
            Write('Ordinate[',I,']= ');
            Try
                Readln(Ordinates[I]);
            Except
                IsIncorrect := False;
                Writeln('Проверьте правильность ввода');
            End;
        Until IsIncorrect;
    End;
    Abscisses[0] := Abscisses[N];
    Ordinates[0] := Ordinates[N];
    Abscisses[N + 1] := Abscisses[1];
    Abscisses[N + 2] := Abscisses[2];
    Ordinates[N + 1] := Ordinates[1];
    Ordinates[N + 2] := Ordinates[2];
    IsTrue := True;
    I := 1;
    While (I <= N) and (IsTrue) Do
        Begin
            CrossLine1 := (Abscisses[I - 1] - Abscisses[I]) * (Ordinates[I + 1] - Ordinates[I]) - (Ordinates[I - 1] - Ordinates[I]) * (Abscisses[I + 1] - Abscisses[I]);
            CrossLine2 := (Abscisses[I] - Abscisses[I + 1]) * (Ordinates[I + 2] - Ordinates[I + 1]) - (Ordinates[I] - Ordinates[I + 1]) * (Abscisses[I + 2] - Abscisses[I + 1]);
            If CrossLine1 * CrossLine2 < 0 Then
                IsTrue := False
            Else
                Inc(I);
        End;
    If IsTrue Then
        Write('Выпуклый')
    Else
        Write('Не выпуклый');
    Readln;
End.
