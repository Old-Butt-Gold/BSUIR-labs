Program LabThirdBlockSecond;

Uses
  System.SysUtils;
Type
    TArrArrOI = Array of Array of Integer;

Function ConsoleInputChoice(): Boolean;
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
            Write('Повторите ввод: ');
            IsIncorrect := False;
        End;
        If IsIncorrect and ((K > MAX_NUM) or (K < MIN_NUM)) Then
            Begin
                Write('Повторите ввод: ');
                IsIncorrect := False;
            End;
    Until IsIncorrect;
    If K = 0 Then
        IsTrue := True
    Else
        IsTrue := False;
    ConsoleInputChoice := IsTrue;
End;

Function ParameterIn(Const MIN_NUM: Integer): Integer;
Var
    IsIncorrect: Boolean;
    N: Integer;
Begin
    Repeat
        IsIncorrect := True;
        Write('Введите количество строк: ');
        Try
            Readln(N);
        Except
            Writeln('Ошибка, введите целочисленное число');
            IsIncorrect := False;
        End;
        If IsIncorrect and (N < MIN_NUM) Then
          Begin
              Write('Введите целочисленное число, большее 1: ');
              IsIncorrect := False;
          End;
    Until IsIncorrect;
    ParameterIn := N;
End;

Function ParameterInF(Var F: TextFile): Integer;
Var
    N: Integer;
Begin
    Readln(F, N);
    ParameterInF := N;
End;

Function FillMatrix(A: TArrArrOI): TArrArrOI;
Var
    I, J: Integer;
    IsIncorrect: Boolean;
Begin
    For I := 0 to High(A) do
    begin
        For J := 0 to High(A[0]) do
        Begin
            Repeat
                Write('Введите ячейку ', I + 1, ' строки ', J + 1, ' столбца: ');
                IsIncorrect := True;
                Try
                    Readln(A[I,J]);
                Except
                    Writeln('Ошибка, введите целочисленное число');
                    IsIncorrect := False;
                End;
            Until IsIncorrect;
        End;
    End;
    FillMatrix := A;
End;

Function FillMatrixF(Var F: TextFile; A: TArrArrOI): TArrArrOI;
Var
    I, J: Integer;
Begin
    For I := 0 to High(A) do
    Begin
        For J := 0 to High(A[0]) do
            Read(F, A[I,J]);
    End;
    FillMatrixF := A;
End;

Procedure WriteMatrix(A: TArrArrOI);
Var
    I, J: Integer;
Begin
    For I := 0 to High(A) do
        Begin
            For J := 0 to High(A[0]) do
                Write(Format('%7d', [A[I,J]]));
            Writeln;
        End;
End;

Procedure WriteMatrixF(Var Out: TextFile; A: TArrArrOI);
Var
    I, J: Integer;
Begin
    For I := 0 to High(A) do
    begin
        For J := 0 to High(A[0]) do
            Write(Out, Format('%7d', [A[I,J]]));
        Writeln(Out);
    End;
End;

Function MinStroka(A: TArrArrOI; I: Integer): Integer;
Var
    J, StrokaMin: Integer;
Begin
    StrokaMin := A[I,0];
    For J := 1 to High(A[0]) Do
        Begin
            If A[I,J] < StrokaMin Then
                StrokaMin := A[I,J];
        End;
    MinStroka := StrokaMin;
End;

Function MaxStolbets(A: TArrArrOI; StrokaMin: Integer; I: Integer): Integer;
Var
    J, StolbetsMax: Integer;
Begin
    For J := 0 to High(A[0]) Do
        Begin
            If A[I,J] = StrokaMin Then
                StolbetsMax := J;
        End;
    MaxStolbets := StolbetsMax;
End;

Function SedlovayaTochka(A: TArrArrOI; StolbetsMax: Integer; StrokaMin: Integer; I: Integer): Boolean;
Var
    M: Integer;
    IsTrue: Boolean;
Begin
    M := 0;
    IsTrue := True;
    While (M <= High(A)) and (IsTrue) do
    Begin
        If StrokaMin < A[M, StolbetsMax] Then
            IsTrue := False
        Else
            Inc(M);
    End;
    SedlovayaTochka := IsTrue;
End;

Procedure WriteSedlovaya(IsTrue: Boolean; StolbetsMax: Integer; StrokaMin: Integer; I: Integer);
Begin
    If IsTrue Then
        Writeln('Седловая точка в ', I + 1, ' строке и ', StolbetsMax + 1, ' столбце = ', StrokaMin);
End;

Procedure WriteSedlovayaF(Var FileOut: TextFile; IsTrue: Boolean; StolbetsMax: Integer; StrokaMin: Integer; I: Integer);
Begin
    If IsTrue Then
        Writeln(FileOut, 'Седловая точка в ', I + 1, ' строке и ', StolbetsMax + 1, ' столбце = ', StrokaMin)
End;

Function WayToFile(P: Integer): String;
Var
    Way: String;
    F: Text;
    IsIncorrect: Boolean;
    Arr: TArrArrOI;
    Cell, N, L, I, J, Index1, Index2: Integer;
Begin
    Repeat
        IsIncorrect := true;
        Writeln('Введите путь к файлу');
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
                        Writeln('Проверьте правильность ввода вашей строки в файле на наличие целого числа');
                        IsIncorrect := False;
                    End;
                    If N < 2 Then
                        Begin
                            Writeln('Ошибка, ваше целочисленное число для строк должно быть больше 1');
                            IsIncorrect := False;
                        End;
                    Try
                        Read(F, L);
                    Except
                        Writeln('Проверьте правильность ввода вашего столбца в файле на наличие целого числа');
                        IsIncorrect := False;
                    End;
                    If L < 2 Then
                        Begin
                            Writeln('Ошибка, ваше целочисленное число для столбца должно быть больше 1');
                            IsIncorrect := False;
                        End;
                    SetLength(Arr, N, L);
                    For I := 0 to High(Arr) Do
                        Begin
                            For J := 0 to High(Arr[0]) do
                                Begin
                                    Try
                                        Read(F, Cell);
                                    Except
                                        IsIncorrect := False;
                                        Index2 := J;//?
                                        Index1 := I;//?
                                        Writeln('Ошибка, проверьте параметр документа ввода матрицы ', Index1+1,' строки ', Index2+1, ' столбца на соответствие целого числа');
                                    End;
                                End;
                        End;
                    Close(F);
                End;
            End
        Else
            Begin
                Writeln('Файла в заданном пути нет');
                IsIncorrect := False;
            End;
    Until IsIncorrect;
    WayToFile := Way;
End;

Function MatrixIn(IsInput: Boolean; Const MIN_NUM: Integer): TArrArrOI;
Var
    N, L: Integer;
    A: TArrArrOI;
    Way: String;
    F: TextFile;
Begin
    If IsInput Then
        Begin
            N := ParameterIn(MIN_NUM);
            L := ParameterIn(MIN_NUM);
            SetLength(A, N, L);
            A := FillMatrix(A);
        End
    Else
        Begin
            Way := WayToFile(1);
            AssignFile(F, Way);
            Reset(F);
            N := ParameterInF(F);
            L := ParameterInF(F);
            SetLength(A, N, L);
            A := FillMatrixF(F, A);
            Close(F);
            Writeln('С файла все считано успешно');
        End;
    MatrixIn := A;
End;

Procedure MatrixOut(isOutput: Boolean; A: TArrArrOI);
Var
    I, J, StrokaMin, StolbetsMax: Integer;
    IsTrue: Boolean;
    Way: String;
    FileOut: TextFile;
Begin
    If IsOutput Then
        Begin
            WriteMatrix(A);
            For I := 0 to High(A) do
                Begin
                    StrokaMin := MinStroka(A, I);
                    StolbetsMax := MaxStolbets(A, StrokaMin, I);
                    IsTrue := SedlovayaTochka(A, StolbetsMax, StrokaMin, I);
                    WriteSedlovaya(IsTrue, StolbetsMax, StrokaMin, I);
                End;
            If Not(IsTrue) Then
                Writeln('Седловых точек нет');
        End
    Else
        Begin
            Way := WayToFile(11);
            Assign(FileOut, Way);
            ReWrite(FileOut);
            WriteMatrixF(FileOut, A);
            For I := 0 to High(A) do
                Begin
                    StrokaMin := MinStroka(A, I);
                    StolbetsMax := MaxStolbets(A, StrokaMin, I);
                    IsTrue := SedlovayaTochka(A, StolbetsMax, StrokaMin, I);
                    WriteSedlovayaF(FileOut, IsTrue, StolbetsMax, StrokaMin, I);
                End;
            If Not(IsTrue) Then
                Writeln(FileOut, 'Седловых точек нет');
            Close(FileOut);
        End;
End;

Var
    A: TArrArrOI;
    IsInput, IsOutput: Boolean;
Const
    MIN_NUM = 2;
Begin
    Writeln('Данная программа находит седловую точку в матрице');
    Write('Введите 0, если хотите ввод на консоль, 1 — ввод из файла: ');
    IsInput := ConsoleInputChoice();
    A := MatrixIn(IsInput, MIN_NUM);
    Write('Введите 0, если хотите вывод на консоль, 1 — вывод в файл: ');
    IsOutput := ConsoleInputChoice();
    MatrixOut(IsOutput, A);
    Readln;
End.
