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
    Writeln('��������� ������� �� ����� �����. ', #10,
            '�������� �������� ��� ����� ����� N: 1..31');
    Repeat
        IsIncorrect := True;
        Write('������� ����� ����� N: ');
        Try
            Readln(N);
        Except
            IsIncorrect := False;
            Writeln('��������� ���� ������');
        End;
    If (IsIncorrect) and ((N < MIN_NUM) or (N > MAX_NUM)) Then
        Begin
            IsIncorrect := False;
            Writeln('��������� ���� ������');
        End;
    Until IsIncorrect;
    Sum := 0;
    Numb := 1;
    For I := 1 to N do
    Begin
        Numb := Numb * 2;
        Sum := Sum + (1 / Numb);
    End;
    Write('�����: ', FloatToStr(Sum));
    Readln;
End.

