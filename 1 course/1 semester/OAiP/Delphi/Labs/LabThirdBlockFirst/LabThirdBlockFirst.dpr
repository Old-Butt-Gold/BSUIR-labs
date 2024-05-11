Program LabThirdBlockFirst;
uses
  System.SysUtils,
  Math;

Var
    A, H: Real;
    IsIncorrect: Boolean;
Begin
    Writeln('������� � �� � � ����� h, ����������, ��� ����� �������� � ',
            'SIN(x) ������ ������ COS(x).');
    Repeat
        IsIncorrect := True;
        Write('������� A � ��������� �������� X: ');
        Try
            Readln(A);
        Except
            IsIncorrect := False;
            Writeln('��������� ������������ �����!');
        End;
    Until IsIncorrect;

    Repeat
        IsIncorrect := True;
        Write('������� ��� ��������� X � H: ');
        Try
            Readln(H);
        Except
            IsIncorrect := False;
            Writeln('��������� ������������ �����!');
        End;
    Until IsIncorrect;
    While (not (sin(A) > cos(A))) do
        A := A + H;
    Write('����� ������ �������� ��� X, ������: ', FloatToStr(A),#10,
          'sin(x) = ', FloatToStr(sin(A)), '; cos(x) = ', FloatToStr(cos(A)), '.');
    Readln;
End.

