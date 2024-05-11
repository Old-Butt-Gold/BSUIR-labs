Program LabSecondBlockSecond;

Uses
    System.SysUtils;
Type
    TArrOB = array of boolean;
Function NumberIn(Const MIN_NUM: Integer): Integer;
Var
    Numb: Integer;
    IsIncorrect: Boolean;
Begin
      Repeat
          IsIncorrect := True;
          Try
              Readln(Numb);
          Except
              Write('������, ������� ������������� �����!'#10#13'��������� ���� ����������� ����������: ');
              IsIncorrect := False;
          End;
          If IsIncorrect and (Numb < MIN_NUM) Then
          Begin
              Write('������� ����������� �����, ������� 1: ');
              IsIncorrect := False;
          End;
      Until IsIncorrect;
      NumberIn := Numb;
End;

Function Zanulenie(A: TArrOB; N: Integer): TArrOB;
Var
    I: Integer;
Begin
    For I := 1 to N do
        A[I] := False;
    Zanulenie := A;
End;

Function FindProst(A: TarrOB; N: Integer): TArrOB;
Var
    I, J: Integer;
Begin
    A := Zanulenie(A, N);
    I := 2;
    While I <= Sqrt(N) Do
    Begin
        J := I * I;
        While J <= N Do
        Begin
            A[J] := True;
            Inc(J,I);
        End;

        Repeat
            Inc(I);
        Until Not(A[I]);
    End;
    FindProst := A;
End;

Procedure WriteProst(A: TArrOB; N: Integer);
Var
    I: Integer;
Begin
    I := 2;
    While I <= N Do
    Begin
        If Not(A[I]) Then
            Writeln(I);
        Inc(I);
    End;
End;

Const
    MIN_NUM = 2;
Var
    P:Integer;
    Arr: TArrOB;
Begin
    Write('������ ��������� ������� ��� ������� �����, �� ������������� P'#10#13);
    Write('������� ����������� ����� P: ');
    P := NumberIn(MIN_NUM);
    SetLength(Arr, P + 1);
    Arr := FindProst(Arr, P);
    Writeln('������� �����: ');
    WriteProst(Arr, P);
    Readln;
End.
