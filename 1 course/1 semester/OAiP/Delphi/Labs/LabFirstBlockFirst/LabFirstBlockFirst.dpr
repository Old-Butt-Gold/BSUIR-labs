Program LabFirstBlockFirst;
uses
  System.SysUtils;

Var
    Abscissa, Ordinate: Real;
    IsIncorrect: Boolean;
Begin
    Writeln('���� ���������� ����� �(�,�). ����������, ����������� ',
            '�� ������ ����� ���������� ��������� D.',#10);
    Writeln(5 - 12 and 5);
    Repeat
        IsIncorrect := True;
        Write('������� ���������� ��� �������: ');
        Try
            Readln(Abscissa);
        Except
            IsIncorrect := False;
            Writeln('��������� ������������ �����!');
        End;
    Until IsIncorrect;

    Repeat
        IsIncorrect := True;
        Write('������� ���������� ��� �������: ');
        Try
            Readln(Ordinate);
        Except
            IsIncorrect := False;
            Writeln('��������� ������������ �����!');
        End;
    Until IsIncorrect;
    If (Ordinate > (-(Abscissa / 2) + 1)) or (Ordinate < 0) or (Abscissa < 0) or (Abscissa > 2) Then
        Writeln('����� M �� ����������� ��������� D ')
    Else
        Writeln('����� M ����������� ��������� D ');

    Readln;
End.

