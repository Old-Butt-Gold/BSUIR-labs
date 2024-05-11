Program LabFourthBlockSecond;
Uses
    System.sysUtils;
Type
    TArrArrOI = Array of Array of Integer;

Function IsConsoleChoice(): Boolean;
Var
    IsIncorrect: Boolean;
    IsTrue: Boolean;
    K: Byte;
Const
    MIN_NUM = 0;
    MAX_NUM = 1;
Begin
    Repeat
        IsIncorrect := True;
        Try
            Readln(K);
        Except
            Write('��������� ����: ');
            IsIncorrect := False;
        End;
        If (IsIncorrect and (K > MAX_NUM) or (K < MIN_NUM)) Then
            Begin
                Write('��������� ����: ');
                IsIncorrect := False;
            End;
    Until IsIncorrect;
    If K = 0 Then
        IsTrue := True
    Else
        IsTrue := False;
    IsConsoleChoice := IsTrue;
End;

Function DimensionIn(Const MIN_NUM:Integer): Integer;
Var
    Numb:Integer;
    isIncorrect: Boolean;
Begin
    Repeat
        Try
            isIncorrect := True;
            Readln(Numb);
        Except
            Writeln('��������� ������������ ��������� ������');
            isIncorrect := False;
        End;
        If (IsIncorrect and (Numb < MIN_NUM)) Then
            Begin
                isIncorrect := False;
                Writeln('��� ������ N ������ ���� ������ 1: ');
            End;
    Until isIncorrect;
    DimensionIn := Numb;
End;

Function FillMatrix(A: TArrArrOI): TArrArrOI;
Var
    I, J: Integer;
    IsIncorrect: Boolean;
Begin
    For I := 0 to High(A)  Do
        Begin
            For J := 0 to High(A[0]) Do
                Begin
                    Write('������ ', I + 1,' ������ ', J + 1, ' ������� = ');
                    Repeat
                        Try
                            Readln(A[I,J]);
                            IsIncorrect := True;
                        Except
                            IsIncorrect := False;
                            Writeln('��������� ����');
                        End;
                    Until IsIncorrect;
                End;
        End;
    FillMatrix := A;
End;

Function DimensionF(Var F: TextFile): Integer;
Var
    N: Integer;
Begin
    Readln(F,N);
    DimensionF := N;
End;

Function FillMatrixF(Var F: TextFile; A: TArrArrOI): TArrArrOI;
Var
    I, J: Integer;
Begin
    For I := 0 to High(A) Do
        Begin
            For J := 0 to High(A[0]) Do
                Read(F, A[I,J]);
        End;
    FillMatrixF := A;

End;

Procedure WriteFirstMatrix(A: TArrArrOI);
Var
    I, J: Integer;
Begin
    For I := 0 to High(A) Do
        Begin
            For J := 0 to High(A[0]) Do
                Write(A[I,J]:7);
            Writeln;
        End;
End;

Function ConvertMatrix(A: TArrArrOI; N: Integer): TArrArrOI;
Var
    I, J, L, X: Integer;
Begin
    L := N - 1;
    For I := 0 to L do
        For J := 0 to L do
            Begin
                X := A[I,J];
                A[I,J]:= A[I + N,J];
                A[I + N,J] := A[I,J+N];
                A[I,J + N] := A[I + N, J + N];
                A[I + N,J + N] := X;
            End;
    ConvertMatrix := A;
End;

Procedure WriteNewMatrix(A: TArrArrOI; N: Integer);
Var
    I, J: Integer;
Begin
    A := ConvertMatrix(A, N);
    For I := 0 to High(A) Do
        Begin
            For J := 0 to High(A[0]) Do
                Write(A[I,J]:7);          //Format('%7d', [A[I,J]])
            Writeln;
        End;
End;

Function WayToFile(P: Integer): String;
Var
    Way: String;
    F: Text;
    IsIncorrect: Boolean;
    Cell, N, L, I, J, Index1, Index2, NewN: Integer;
Begin
    Repeat
        IsIncorrect := True;
        Writeln('������� ���� � �����');
        Readln(Way);
        AssignFile(F, Way);
        If FileExists(Way) Then
            Begin
                If P = 1 Then
                Begin
                    Reset(F);
                    Try
                        Read(F, N);
                    Except
                        Writeln('��������� ������������ ����� ����������� ����� �������');
                        IsIncorrect := False;
                    End;
                    If N < 2 Then
                        Begin
                            Writeln('������, ���� ������������� ����� ���������� ������ ���� ������ 1');
                            IsIncorrect := False;
                        End;
                    NewN := 2*N;
                    For I := 1 to NewN Do
                        Begin
                            For J := 1 to NewN do
                                Begin
                                    Try
                                        Read(F, Cell);
                                    Except
                                        IsIncorrect := False;
                                        Index2 := J;
                                        Index1 := I;
                                        Writeln('������, ��������� �������� ��������� ����� ������� ', Index1,' ������ ', Index2, ' ������� �� ������������ ������ �����');
                                    End;
                                End;
                        End;
                    Close(F);
                End;
            End
        Else
            Begin
                Writeln('����� � �������� ���� ���');
                IsIncorrect := False;
            End;
    Until IsIncorrect;
    WayToFile := Way;
End;

Procedure WriteFirstMatrixF(Var FileOut: TextFile; A: TArrArrOI);
Var
    I, J: Integer;
Begin
    Writeln(FileOut,'���� �������:');
    For I := 0 to High(A) Do
        Begin
            For J := 0 to High(A[0]) Do
                Write(FileOut, A[I,J]:7);//Format('%7d', [A[I,J]]) ��� FloatToStr; :x:y(x ��� ������ ���� ��� ����� + �����)(y ��� ���������� ���� ��� �����);
            Writeln(FileOut);
        End;
End;

Procedure WriteNewMatrixF(Var FileOut: TextFile; A: TArrArrOI; N: Integer);
Var
    I, J: Integer;
Begin
    Writeln(FileOut, '��������������� ������� ����� ���������:');
    A := ConvertMatrix(A, N);
    For I := 0 to High(A) Do
        Begin
            For J := 0 to High(A[0]) Do
                Write(FileOut, A[I,J]:7);
            Writeln(FileOut);
        End;
End;

Function MatrixIn(IsInput: Boolean; Const MIN_NUM: Integer; Var N: Integer): TArrArrOI;
Var
    Way: String;
    F: TextFile;
    A: TArrArrOI;
Begin
    If IsInput Then
        Begin
            Write('������� N � ������ ���������� ������� 2N: ');
            N := DimensionIn(MIN_NUM);
            SetLength(A, 2*N, 2*N);
            A := FillMatrix(A);
        End
    Else
        Begin
            Way := WayToFile(1);
            AssignFile(F, Way);
            Reset(F);
            N := DimensionF(F);
            SetLength(A, 2*N, 2*N);
            A := FillMatrixF(F, A);
            Close(F);
            Writeln('� ����� ��� ������� �������');
        End;

    MatrixIn := A;
End;

Procedure MatrixOut(IsOutput: Boolean; N: Integer; A: TArrArrOI);
Var
    Way: String;
    FileOut: TextFile;
Begin
    If IsOutput Then
        Begin
            Writeln('���� �������:');
            WriteFirstMatrix(A);
            Writeln('��������������� ������� ����� ���������:');
            WriteNewMatrix(A, N);
        End
    Else
        Begin
            Way := WayToFile(11);
            Assign(FileOut, Way);
            ReWrite(FileOut);
            WriteFirstMatrixF(FileOut, A);
            WriteNewMatrixF(FileOut, A, N);
            Close(FileOut);
        End;
End;

Var
    A: TArrArrOI;
    N: Integer;
    IsInput, IsOutput: Boolean;
Const
    MIN_NUM = 2;
Begin
    Writeln('������ ��������� ������ ���������� N ����� ������� 2N �������');
    Write('������� 0, ���� ������ ���� �� �������, 1 � ���� �� �����: ');
    IsInput := IsConsoleChoice();
    A := MatrixIn(IsInput, MIN_NUM, N);
    Write('������� 0, ���� ������ ����� �� �������, 1 � ����� � ����: ');
    IsOutput := IsConsoleChoice();
    MatrixOut(IsOutput, N, A);
    Readln;
End.
