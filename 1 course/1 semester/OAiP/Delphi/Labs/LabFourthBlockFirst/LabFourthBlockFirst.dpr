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
        Write('������� ����� ������ � �������������� � N (N > 2): ');
        IsIncorrect := True;
        Try
            Readln(N);
        Except
            IsIncorrect := False;
            Writeln('��������� ������������ �����');
        End;
        If (IsIncorrect) and (N < MIN_NUM) Then
        Begin
            IsIncorrect := False;
            Writeln('��������� ������������ ����� ������ � ������ ���������');
        End;
    Until IsIncorrect;
    SetLength(Abscisses, N);
    SetLength(Ordinates, N);
    Perimetr := 0;
    //������ �������� � ���� (0) ����� SetLength:
    For I := 0 to (N - 1) do
    Begin
        Repeat
            IsIncorrect := True;
            Write('Abscissa[',I + 1,']= ');
            Try
                Readln(Abscisses[I]);
            Except
                IsIncorrect := False;
                Writeln('��������� ������������ �����');
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
                Writeln('��������� ������������ �����');
            End;
        Until IsIncorrect;
    End;
    //������� ���� �������� (N-1) ������, ����� ����� ������ �������, ����������� ������ � �����.
    For I := 0 to (N - 2) do
    Begin
        Perimetr := Perimetr + Sqrt((Abscisses[I] - Abscisses[I + 1]) * (Abscisses[I] - Abscisses[I + 1]) + (Ordinates[I] - Ordinates[I + 1]) * (Ordinates[I] - Ordinates[I + 1]));
    End;
    Perimetr := Perimetr + Sqrt((Abscisses[0] - Abscisses[N - 1]) * (Abscisses[0] - Abscisses[N - 1]) + (Ordinates[0] - Ordinates[N - 1]) * (Ordinates[0] - Ordinates[N - 1]));
    Write('�������� �����: ', FloatToStr(Perimetr));
    Readln;
End.

