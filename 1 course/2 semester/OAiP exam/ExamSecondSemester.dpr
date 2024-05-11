program ExamSecondSemester;

{������ 1. ����� ���������� ��������� �������� � ������.

Function DifferentSymbols(Str: String): Byte;
Var
    S: Set of Char;
    I: Integer;
Begin
    S := [];
    Result := 0;
    For I := 1 to Length(Str) do
        If Not(Str[I] In S) Then
        Begin
            //Include(S, AnsiChar(Str[I]));
            S := S + [Str[I]];
            Inc(Result);
        End;
End;

Var
    Str: String;
    F: TextFile;
begin
    AssignFile(F, 'Test.txt');
    Reset(F);
    Readln(F, Str);
    CloseFile(F);
    Writeln(DifferentSymbols(Str));
    Readln;
end.}



{������ 2. ����� ������ �����.

Type
    TInfo = Record
        Index, Long, Cost: Byte
    End;
    TArr = Array of TInfo;

Const N = 5;

Function GetHighest(Arr: TArr): Byte;
Var
    I: Integer;
    Temp: Integer;
Begin
    Result := 0;
    Temp := Arr[0].Long * Arr[0].Cost;
    For I := 1 to High(Arr) do
        If Temp < Arr[I].Long * Arr[I].Cost Then
        Begin
            Result := I;
            Temp := Arr[I].Long * Arr[I].Cost;
        End;
End;

Var
    Arr: TArr;
    F: TextFile;
    Index: Byte;
    I: Integer;
begin
    SetLength(Arr, N);
    AssignFile(F, 'Test.txt');
    Reset(F);
    For I := 0 to High(Arr) do
    Begin
        Readln(F, Arr[I].Index);
        Readln(F, Arr[I].Long);
        Readln(F, Arr[I].Cost);
    End;
    CloseFile(F);
    Index := GetHighest(Arr);
    Writeln(Arr[Index].Index, ' ', Arr[Index].Long, ' ', Arr[Index].Cost, ' ');
    Readln;
end.}

{������ 3. ����� ����� ������� �����.

Type
    TInfo = Record
        Name, Author: ShortString;
        Cost, Amount, Isi: Integer;
    End;
    TArr = Array of TInfo;

Const N = 5;

Function GetLowestPrice(Arr: TArr): Integer;
Var
    I, Temp: Integer;
Begin
    Result := 0;
    Temp := Arr[0].Cost * Arr[0].Amount + Arr[0].Isi;
    For I := 1 to High(Arr) do
        If Temp > Arr[I].Cost * Arr[I].Amount + Arr[I].Isi Then
        Begin
            Temp := Arr[I].Cost * Arr[I].Amount + Arr[I].Isi;
            Result := I;
        End;
End;

Var
    F: TextFile;
    I: Integer;
    Arr: TArr;
begin
    SetLength(Arr, N);
    AssignFile(F, 'Test.txt');
    Reset(F);
    For I := 0 to High(Arr) do
    Begin
        Readln(F, Arr[I].Cost);
        Readln(F, Arr[I].Amount);
        Readln(F, Arr[I].Isi);
        Readln(F, Arr[I].Name);
        Readln(F, Arr[I].Author);
    End;
    CloseFile(F);
    Writeln(Arr[GetLowestPrice(Arr)].Name);
    Readln;
end.}

{������ 4. ����� ��������� ������� �� 1 �� K.

Const N = 8;

Type
    TArr = Array[1.. N] of Integer;

Function SumInArray(Arr: TArr; Count: Integer): Integer;
Var
    I: Integer;
Begin
    Result := 0;
    For I := 1 to Count do
        Inc(Result, Arr[I]);
End;

Var
    Arr: TArr;
    F: TextFile;
    Count, I: Integer;
begin
    AssignFile(F, 'Test.txt');
    Reset(F);
    For I := 1 to High(Arr) do
        Read(F, Arr[I]);
    Readln(F, Count);
    CloseFile(F);
    Writeln(SumInArray(Arr, Count));
    Readln;
end.}

{������ 5. ��� ������ �������� M �� N. �������� ��������� ��������� 1� ������ �� ����� ����������. ������� ���������� ����������

Const
    Row = 5;
    Cols = 5;

Type
    TMatrix = Array[1..Row, 1..Cols] of Integer;

Function AmountOfSimilar(Matrix: TMatrix): Integer;
Var
    I, J: Integer;
    IsSimilar: Boolean;
Begin
    Result := 0;
    For I := 2 to High(Matrix) do
    Begin
        IsSimilar := True;
        J := 1;
        While IsSimilar and (J <= High(Matrix[1])) do
        Begin
            IsSimilar := Matrix[1, J] = Matrix[I, J];
            Inc(J);
        End;
        If IsSimilar Then
            Inc(Result);
    End;
End;

Var
    Matrix: TMatrix;
    F: TextFile;
    I, J: Integer;
begin
    AssignFile(F, 'Test.txt');
    Reset(F);
    For I := 1 to High(Matrix) do
        For J := 1 to High(Matrix[1]) do
            Read(F, Matrix[I, J]);
    CloseFile(F);
    Writeln(AmountOfSimilar(Matrix));
    Readln;
end.}

{������ 6. �������� ����� min � max

Const N = 8;

Type
    TArr = Array[1.. N] of Integer;


Procedure GetMinAndMax(Arr: TArr; Var Min, Max: Integer);
Var
    Temp, I: Integer;
Begin
    Min := 1;
    Max := 1;
    For I := 2 to High(Arr) do
    Begin
        If Arr[Min] > Arr[I] Then
            Min := I;
        If Arr[Max] < Arr[I] Then
            Max := I;
    End;
    If Min > Max Then
    Begin
        Temp := Min;
        Min := Max;
        Max := Temp;
    End;
End;

Procedure MakeZeroInArr(Var Arr: TArr; Min, Max: Integer);
Var
    I: Integer;
Begin
    For I := Min + 1 to Max - 1 do
        Arr[I] := 0;
End;

Var
    Arr: TArr;
    F: TextFile;
    I, Min, Max: Integer;
begin
    AssignFile(F, 'Test.txt');
    Reset(F);
    For I := 1 to High(Arr) do
        Read(F, Arr[I]);
    GetMinAndMax(Arr, Min, Max);
    MakeZeroInArr(Arr, Min, Max);
    For I := 1 to High(Arr) do
        Write(Arr[I]:4);
    CloseFile(F);
    Readln;
end.}

{������ 7. ������ ������� � �� N , ����� ��������� ������ , ��� ��� �������� ���� �� �����������

Const
    Row = 5;
    Cols = 5;

Type
    TMatrix = Array[1..Row, 1..Cols] of Integer;

Function AmountOfIncreasing(Matrix: TMatrix): Integer;
Var
    I, J: Integer;
    IsSimilar: Boolean;
Begin
    Result := 0;
    For I := 1 to High(Matrix) do
    Begin
        IsSimilar := True;
        J := 1;
        While IsSimilar and (J < High(Matrix[1])) do
        Begin
            IsSimilar := Matrix[I, J] < Matrix[I, J + 1];
            Inc(J);
        End;
        If IsSimilar Then
            Inc(Result);
    End;
End;

Var
    Matrix: TMatrix;
    F: TextFile;
    I, J: Integer;
begin
    AssignFile(F, 'Test.txt');
    Reset(F);
    For I := 1 to High(Matrix) do
        For J := 1 to High(Matrix[1]) do
            Read(F, Matrix[I, J]);
    CloseFile(F);
    Writeln(AmountOfIncreasing(Matrix));
    Readln;
end.}

{������ 8. �������� �������� �������, ������ ������� � ��������� ���������.

Const N = 10;

Type
    TArr = Array[1..N] of Integer;

Function SwapElements(Var Arr: TArr): Integer;
Var
    I: Integer;
    Temp: Integer;
Begin
    I := 1;
    While I < High(Arr) do
    Begin
        Temp := Arr[I];
        Arr[I] := Arr[I + 1];
        Arr[I + 1] := Temp;
        Inc(I, 2);
    End;
End;

Var
    Arr: TArr;
    F: TextFile;
    Count, I: Integer;
begin
    AssignFile(F, 'Test.txt');
    Reset(F);
    For I := 1 to High(Arr) do
        Read(F, Arr[I]);
    CloseFile(F);
    SwapElements(Arr);
    For I := 1 to High(Arr) do
        Write(Arr[I]:4);
    Readln;
end.}


{������ 9. ��� ������ ������� N. � ��� ������� ����: <������� ��������>, <����� ������>, <��� �����������>.
����� � ������� ������ � ���������� ����������� ���������. ���� ����� ����� ���������, �� ������� ��������� �� ���.

Const N = 5;

Type
    TInfo = Record
        Name: ShortString;
        Group, Year: Integer;
    End;
    TArr = Array[1..N] of TInfo;

Function GetHighestIndexGroup(Arr: TArr): Integer;
Var
    Count, Temp, I, J: Integer;
Begin
    Count := 0;
    For I := 1 to High(Arr) do
    Begin
        Temp := 0;
        For J := 1 to High(Arr) do
            If Arr[I].Group = Arr[J].Group Then
                Inc(Temp);
        If Temp >= Count Then //>= ��� ���������, > ��� ������
        Begin
            Count := Temp;
            Result := Arr[I].Group;
        End;
    End;
End;

Var
    Arr: TArr;
    F: TextFile;
    I: Integer;
begin
    AssignFile(F, 'Test.txt');
    Reset(F);
    For I := 1 to High(Arr) do
    Begin
        Readln(F, Arr[I].Name);
        Readln(F, Arr[I].Group);
        Readln(F, Arr[I].Year);
    End;
    CloseFile(F);
    Writeln(GetHighestIndexGroup(Arr));
    Readln;
end.}

{������ 10. ��� ������ �� n ���������, n > 5. � ������ ���������� ������� �������� ��� ��������, ���-�� ������ � ��������� � ������.
//������� 5 �������, � ������� ���������� ���������� ������

Type
    TInfo = Record
        Name: ShortString;
        Likes, Messages, Friends: Integer;
    End;
    TArr = Array of TInfo;

Const N = 7;


Procedure Swap(Arr: TArr; I, J: Integer);
Var
    Temp: TInfo;
Begin
    Temp := Arr[I];
    Arr[I] := Arr[J];
    Arr[J] := Temp;
End;

Function Partition(Arr: TArr; Left, Right: Integer): Integer;
Var
    Pivot, I, J: Integer;
Begin
    Pivot := Arr[Left].Likes;
    J := Left;
    For I := J + 1 to Right do
        If Arr[I].Likes >= Pivot Then  //���������� �� �������������
        Begin
            Inc(J);
            Swap(Arr, I, J);
        End;
    Swap(Arr, Left, J);
    Result := J;
End;

Procedure QSort(Arr: TArr; Left, Right: Integer);
Var
    Temp: Integer;
Begin
    While Left < Right do
    Begin
        Temp := Partition(Arr, Left, Right);
        QSort(Arr, Left, Temp - 1);
        Left := Temp + 1;
    End;
End;

Var
    F: TextFile;
    Arr: TArr;
    I: Integer;
begin
    AssignFile(F, 'Test.txt');
    Reset(F);
    SetLength(Arr, N);
    For I := 0 to High(Arr) do
    Begin
        Readln(F, Arr[I].Name);
        Readln(F, Arr[I].Likes);
        Readln(F, Arr[I].Messages);
        Readln(F, Arr[I].Friends);
    End;
    CloseFile(F);
    QSort(Arr, Low(Arr), High(Arr));
    For I := 0 to 4 do
        Write(Arr[I].Name, ' ', Arr[I].Likes, ' ', Arr[I].Messages, ' ', Arr[I].Friends);
    Readln;
end.}


{������ 11. ��� ������ ����� � , ����� �, ����� ����� � ����������� ������ �� ����� �. ����� ������ ������������ X � Y.
//������ ��������� �� ���������� �����, ��� ����� ������ � ������������

Const N = 10;

Type
    TCoord = Record
        X, Y: Real;
    End;
    TArr = Array[1..N] of TCoord;

Function FindTheClosest(ArrA: TArr; DotB: TCoord): Integer;
Var
    Index, I: Integer;
    XDiffPrev, YDiffPrev: Real;
    XDiffCurr, YDiffCurr: Real;
Begin
    Index := 1;
    For I := 2 to High(ArrA) do
    Begin
        XDiffPrev := ArrA[Index].X - DotB.X;
        YDiffPrev := ArrA[Index].Y - DotB.Y;
        XDiffCurr := ArrA[I].X - DotB.X;
        YDiffCurr := ArrA[I].Y - DotB.Y;
        If Sqrt(XdiffPrev * XDiffPrev + YDiffPrev * YDiffPrev) > Sqrt(XdiffCurr * XDiffCurr + YDiffCurr * YDiffCurr) Then
            Index := I;
    End;
    Result := Index;
End;

Var
    ArrA: TArr;
    DotB: TCoord;
    F: TextFile;
    Index, I: Integer;
begin
    AssignFile(F, 'Test.txt');
    Reset(F);
    For I := 1 to High(ArrA) do
    Begin
        Read(F, ArrA[I].X);
        Read(F, ArrA[I].Y);
    End;
    Read(F, DotB.X);
    Read(F, DotB.Y);
    CloseFile(F);
    Index := FindTheClosest(ArrA, DotB);
    Writeln(ArrA[Index].X, ' ', ArrA[Index].Y);
    Readln;
end.}

{������ 12. ��� ������ � ������� N, ������������ � ������� ����� ������ ���� �� �������
//��� ������ ������� B[k] ����� ����� ���� ��������� ��������� ������� � �������� �� 1 �� k.

Const N = 10;

Type

    TArr = Array[1..N] of Integer;

Function GetNewArr(Arr: TArr): TArr;
Var
    ArrNew: TArr;
    I, J: Integer;
Begin
    For I := 1 to High(Arr) do
    Begin
        ArrNew[I] := 0;
        For J := 1 to I do
            Inc(ArrNew[I], Arr[J]);
    End;
    Result := ArrNew;
End;

Var
    Arr, ArrNew: TArr;
    F: TextFile;
    Index, I: Integer;
begin
    AssignFile(F, 'Test.txt');
    Reset(F);
    For I := 1 to High(Arr) do
        Read(F, Arr[I]);
    CloseFile(F);
    ArrNew := GetNewArr(Arr);
    For I := 1 to High(ArrNew) do
        Write(ArrNew[I]:4);
    Readln;
end.}

{������ 13. ��� ������ ������� N. ����� ��� �������� ��������, ����� ������� �����������, � ������� ��� �������� � �������
//����������� �� ��������

Const N = 10;

Type
    TArr = Array[1..N] of Integer;

Function MaxSumIndex(Arr: TArr): Integer;
Var
    I, Sum: Integer;
Begin
    Sum := Arr[1] + Arr[2];
    Result := 1;
    For I := 2 to High(Arr) - 1 do
        If Arr[I] + Arr[I + 1] > Sum Then
        Begin
            Sum := Arr[I] * Arr[I + 1];
            Result := I;
        End;
End;

Var
    Arr: TArr;
    F: TextFile;
    Index, I: Integer;
begin
    AssignFile(F, 'Test.txt');
    Reset(F);
    For I := 1 to High(Arr) do
        Read(F, Arr[I]);
    CloseFile(F);
    Index := MaxSumIndex(Arr);
    Write(Arr[Index]:4, ' ', Arr[Index + 1]:4);
    Readln;
end. }

{������ 14. ���� ������� ������� M � N � ����� ����� K (1 <= K <= M). ����� ����� � ������������ ��������� K-�� ������

Const
    Row = 5;
    Cols = 5;

Type
    TMatrix = Array[1..Row, 1..Cols] of Integer;

Function MultiplicationInRow(MAtrix: TMatrix; K: Integer): Integer;
Var
    I: Integer;
Begin
    Result := 1;
    For I := 1 to High(Matrix) do
        Result := Result * Matrix[K, I];
End;

Var
    Matrix: TMatrix;
    F: TextFile;
    I, J, K: Integer;
begin
    AssignFile(F, 'Test.txt');
    Reset(F);
    For I := 1 to High(Matrix) do
        For J := 1 to High(Matrix[1]) do
            Read(F, Matrix[I, J]);
    Read(F, K);
    CloseFile(F);
    Writeln(MultiplicationInRow(Matrix, K));
    Readln;
end.}

{������ 15. ���� ����� N, L, K (1 < L <= K <= N) � ������ (N ���������), ����� ����� ��������� �������, ������� ������� �� ������ � ���������� �� L �� K ������������

Const N = 10;

Type
    TArr = Array[1..N] of Integer;

Function GetSum(Arr: TArr; L, K: Integer): Integer;
Var
    I: Integer;
Begin
    Result := 0;
    For I := 1 to L - 1 do
        Inc(Result, Arr[I]);
    For I := K + 1 to High(Arr) do
        Inc(Result, Arr[I]);
End;

Var
    Arr: TArr;
    F: TextFile;
    L, K, I: Integer;
begin
    AssignFile(F, 'Test.txt');
    Reset(F);
    For I := 1 to High(Arr) do
        Read(F, Arr[I]);
    Read(F, L);
    Read(F, K);
    CloseFile(F);
    Write(GetSum(Arr, L, K));
    Readln;
end.}

{������ 16. ��� ������ �� N ���������. �������� ������ ������� ������� �������� �������� ��������������� ��� ������ � ��� �������.
//��� ��������

Const N = 10;

Type
    TArr = Array of Real;

Procedure ChangeArr(Var Arr: TArr);
Var
    I: Integer;
    ArrFinal: TArr;
Begin
    SetLength(ArrFinal, N);
    For I := 0 to High(ArrFinal) do
        ArrFinal[I] := Arr[I];
    ArrFinal[0] := (ArrFinal[0] + ArrFinal[1]) / 2;
    ArrFinal[High(ArrFinal)] := (ArrFinal[High(ArrFinal)] + ArrFinal[High(ArrFinal) - 1]) / 2;
    For I := 1 to High(ArrFinal) - 1 do
        ArrFinal[I] := (Arr[I - 1] + Arr[I] + Arr[I + 1]) / 3;
    Arr := ArrFinal;
End;

Var
    Arr: TArr;
    F: TextFile;
    I: Integer;
begin
    AssignFile(F, 'Test.txt');
    Reset(F);
    SetLength(Arr, N);
    For I := 0 to High(Arr) do
        Read(F, Arr[I]);
    CloseFile(F);
    ChangeArr(Arr);
    For I := 0 to High(Arr) do
        Write(Arr[I]:4);
    Readln;
end.}

{������ 17. ���� ������� M*N, ����� ������ � ������������ ������ ���������, ������� ��� ����� � ����� ������.

Const
    Row = 5;
    Cols = 5;

Type
    TMatrix = Array[1..Row, 1..Cols] of Integer;

Function GetSumInRow(Matrix: TMatrix; Index: Integer): Integer;
Var
    I: Integer;
Begin
    Result := 0;
    For I := 1 to High(Matrix[1]) do
        Inc(Result, Matrix[Index, I]);
End;

Function FindRowWithMaxSum(Matrix: TMatrix): Integer;
Var
    I: Integer;
Begin
    Result := 1;
    For I := 2 to High(Matrix) do
        If GetSumInRow(Matrix, I) > GetSumInRow(Matrix, Result) Then
            Result := I;
End;

Var
    Matrix: TMatrix;
    F: TextFile;
    I, J, Index: Integer;
begin
    AssignFile(F, 'Test.txt');
    Reset(F);
    For I := 1 to High(Matrix) do
        For J := 1 to High(Matrix[1]) do
            Read(F, Matrix[I, J]);
    CloseFile(F);
    Index := FindRowWithMaxSum(Matrix);
    Writeln(GetSumInRow(Matrix, Index));
    Writeln(Index);
    Readln;
end.}

{������ 18. ���� ������ ������ ���������. ������ ������ ����� �������, ����, ��������� ����.
//����� �������� � ����� ������� ���-��� ������, ���� ���������, ������� ������ ��������.

Const N = 10;

Type
    TInfo = Record
        Surname, Language: ShortString;
        Year: Integer;
    End;
    TArr = Array[1..N] of TInfo;

Function FindStudent(Arr: TArr): TInfo;
Var
    Index, I, J, Temp: Integer;

Begin
    Index := 0;
    For I := 1 to High(Arr) do
    Begin
        Temp := 0;
        For J := 1 to High(Arr) do
            If Arr[I].Surname = Arr[J].Surname Then
                Inc(Temp);
        If Temp > Index Then
            Index := I;
    End;
    Result := Arr[Index];
End;

Var
    Arr: TArr;
    F: TextFile;
    I: Integer;
    Temp: TInfo;
begin
    AssignFile(F, 'Test.txt');
    Reset(F);
    For I := 1 to High(Arr) do
    Begin
        Readln(F, Arr[I].Surname);
        Readln(F, Arr[I].Language);
        Readln(F, Arr[I].Year);
    End;
    CloseFile(F);
    Temp := FindStudent(Arr);
    Write(Temp.Surname, ' ', Temp.Language, ' ', Temp.Year);
    Readln;
end.}

//������ 19. ���� ������. � ��� �������, ����� ������, ��� �������� �� ������, ��������. ������� ������ ����� �������. ����� ������������ �������� �����. �.�
//��� ������� �������� ����� �������. ������� ���� ����� � ������� ��������.

Const N = 7;

Type
    TInfo = Record
        Surname, Department: ShortString;
        Year, Salary: Integer;
    End;
    TArr = Array[1..N] of TInfo;

Function GetDepartmentName(Arr: TArr): ShortString;
Var
    Index, I, J, Count: Integer;
    Sum, Temp: Real;
Begin
    Sum := 0;
    Index := 1;
    For I := 1 to High(Arr) do
    Begin
        Temp := 0;
        Count := 0;
        For J := 1 to High(Arr) do
            If Arr[J].Department = Arr[I].Department Then
            Begin
                Temp := Temp + Arr[J].Salary;
                Inc(Count);
            End;
        Temp := Temp / Count;
        If Temp > Sum Then
        Begin
            Index := I;
            Sum := Temp;
        End;
    End;
    Result := Arr[Index].Department;
End;

Function GetAverageSalary(Arr: TArr; Department: ShortString): Real;
Var
    I, Count: Integer;
Begin
    Result := 0;
    Count := 0;
    For I := 1 to High(Arr) do
        If Arr[I].Department = Department Then
        Begin
            Inc(Count);
            Result := Result + Arr[I].Salary;
        End;
    Result := Result / Count;
End;

Var
    Arr: TArr;
    F: TextFile;
    I: Integer;
    Department: ShortString;
begin
    AssignFile(F, 'Test.txt');
    Reset(F);
    For I := 1 to High(Arr) do
    Begin
        Readln(F, Arr[I].Surname);
        Readln(F, Arr[I].Department);
        Readln(F, Arr[I].Year);
        Readln(F, Arr[I].Salary);
    End;
    CloseFile(F);
    Department := GetDepartmentName(Arr);
    Writeln(Department);
    Writeln(GetAverageSalary(Arr, Department));
    Readln;
end.


