Program ExamFirstSemester;

uses
  System.SysUtils;

//Задача 1.(15).
    //Отсортировать массив [1..40] по неубыванию.
    var
    I, J, Tmp: Integer;
    TheArray: Array [1..40] of Integer;
    F_on: Boolean;
Begin
    Randomize;
    For I := 1 to 40 do
    begin
        TheArray[I] := Random(100);
        Write(TheArray[I]:4);
    end;
    Writeln;
    I := 1;
    F_on := True;
    While (F_on) and (I < 40) do
    Begin
        F_on := False;
        For J := 2 to 41 - I do
        Begin
            If (TheArray[J - 1] < TheArray[J]) Then
            Begin
                Tmp := TheArray[J - 1];
                TheArray[J - 1] := TheArray[J];
                TheArray[J] := Tmp;
                F_on := True;
            End;
        End;
        Inc(I);
    End;
    For I := 1 to 40 do
        Write(TheArray[I]:4);
    Readln;
End.
    {//Задача 2.(43).
    //Преобразовать строку, состоящую из букв латинского алфавита, в строку, состоящую из порядковых номеров этих букв в алфавите. Пример: 'ABZ' - '1226'.
    Var
    A, B: String;
    I: Integer;
    Readln(A);
    A := LowerCase(A);
    B := '';
    For I := 1 to Length(A) do
        B := B + IntToStr(Ord(A[I]) - Ord('a') + 1) + ' ';
    Writeln(B);}
    {//Задача 3.(44).
    //Дана строка из слов и пробелов. Составить строку, состоящую из первых букв слов исходной строки
    Var
    S, W: String;
    I: Integer;
    Readln(S);
    S := Trim(S);
    W := S[1];
    For I := 2 to Length(S) - 1 do
        If (S[I] = ' ') and (S[I + 1] <> ' ') Then
            W := W + S[I + 1];
    Writeln(W);}
    {//Задача 4.(22).
    //Дана матрица М 100х100. Определить, равна ли сумма элементов над главной диагональю сумме элементов под главной диагональю. Возможные ответы "Да" или "нет".
    Var
    Arr: Array[1..100, 1..100] of Integer;
    I, J, SumUnder, SumTop: Integer;
    Randomize;
    For I := 1 to 100 do
        For J := 1 to 100 do
            Arr[I,J] := Random(300) - Random(300);
    SumUnder := 0;
    SumTop := 0;
    For I := 1 to 99 do
        For J := I + 1 to 100 do
        Begin
            SumTop := Arr[I,J];
            SumUnder := Arr[J, I];
        End;
    If SumUnder = SumTop Then
        Writeln('Yes')
    Else
        Writeln('No');}
    {//Задача 5.(?).
    //Дана матрица 10х10. Найти минимальный элемент побочной диагонали и заменить все нулевые элементы матрицы на него.
    var
    matrix: Array[1..10, 1..10] of Integer;
    I, J, Min: integer;
    Randomize;
    For I := 1 to 10 do
    Begin
        For J := 1 to 10 do
        Begin
            Matrix[I,J] := Random(20) - Random(20);
            Write(Matrix[I,J]:4);
        End;
        Writeln;
    End;
    Min := Matrix[10,1];
    For I := 9 Downto 1 do
        If (Matrix[I][11 - I] < Min) then
          Min := Matrix[I][11-I];
    For I := 1 to 10 do
        For J := 1 to 10 do
            If (Matrix[I,J] = 0) Then
                Matrix[I,J] := Min;
    For I := 1 to 10 do
    Begin
        For J := 1 to 10 do
            Write(Matrix[I,J]:4);
        Writeln;
    End;}
    {//Задача 6.(?).
    //Дан массив [1..100]. Определить являются элементы массива отсортированными строго по возрастанию.
    var
    A: array[1..100] of Integer;
    I: Integer;
    Flag: Boolean;
    For I := 1 to 100 do
        A[I] := Random(20);
    I := 1;
    Flag := True;
    While Flag and (I < 100) do
    Begin
        If A[I] >= A[I + 1] Then
            Flag := False;
        Inc(I);
    End;
    If Flag Then
        Writeln('Sorted')
    Else
        Writeln('Not Sorted');
    Readln;}

    {//Задача 7.(31).
    //Дана матрица 10х10, поменять местами столбцы с максимальной и минимальной суммами элементов.
    Var
    A: Array[1..10, 1..10] of Integer;
    Max, Min, I, J, IndexMin, IndexMax, Tmp: Integer;
    Randomize;
    For I := 1 to 10 do
        For J := 1 to 10 do
            A[I,J] := Random(30) - Random(30);
    For I := 1 to 10 do
    Begin
        For J := 1 to 10 do
            Write(A[I,J]:4);
        Writeln;
    End;
    Writeln;
    Min := 0;
    For I := 1 to 10 do
        Min := Min + A[I, 1];
    Max := Min;
    IndexMin := 1;
    IndexMax := 1;
    For I := 2 to 10 do
    begin
        Tmp := 0;
        For J := 1 to 10 do
            Tmp := Tmp + A[I,J];
        If (Tmp < Min) Then
        Begin
            IndexMin := I;
            Min := Tmp;
        End;
        If (Tmp > Max) Then
        Begin
            IndexMax := I;
            Max := Tmp;
        End;
    end;
    For I := 1 to 10 do
    Begin
        Tmp := A[I, IndexMin];
        A[I, IndexMin] := A[I, IndexMax];
        A[I, IndexMax] := Tmp;
    End;
    For I := 1 to 10 do
    Begin
        For J := 1 to 10 do
            Write(A[I,J]:4);
        Writeln;
    End;}
    {//Задача 8.(21).
    //Матрица 100х100 найти строку с минимальной суммой ,если таких строк несколько вывести с наименьшим номером.
    Var
    Arr: Array [1..100, 1..100] of Integer;
    TempIndex, I, J, Min, Sum: Integer;
    Randomize;
    For I := 1 to 100 do
        For J := 1 to 100 do
            Arr[I,J] := Random(100) - Random(100);
    Sum := 0;
    For J := 1 to 100 do
        Sum := Sum + Arr[1,J];
    Min := Sum;
    TempIndex := 1;
    For I := 2 to 100 do
    Begin
        Sum := 0;
        For J := 1 to 100 do
            Sum := Sum + Arr[I,J];
        If (Sum < Min) Then
        Begin
            Min := Sum;
            TempIndex := I;
        End;
    End;
    Writeln(Min);
    Writeln(TempIndex);
    For I := 1 to 100 do
        Write(Arr[TempIndex, I]:4);}
    {//Задача 9.(?).
    //найти НОД, найти НОК ,разложить на простые множители.
    Var
    I, A, B, Nod, Nok: Integer;
    Readln(A);
    Readln(B);
    I := 1;
    Nok := 1;
    While (Nok mod B) <> 0 do
    Begin
        Nok := A * I;
        Inc(I);
    End;
    Writeln(Nok);
    Nod := A * B div Nok;
    Writeln(Nod);
    Writeln;
    I := 2;
    Write('A = 1');
    While (A > 1) do
    Begin
        If (A mod I = 0) Then
        Begin
            Write('*', I);
            A := A div I;
        End
        Else
        Inc(I);
    End;
    Writeln;
    I := 2;
    Write('B = 1');
    While (B > 1) do
    Begin
        If (B mod I = 0) Then
        Begin
            Write('*', I);
            B := B div I;
        End
        Else
        Inc(I);
    End;}
{    //Задача 10.(29).
    //Дана матрица 8х8 и её шахматная раскраска. Элементы на белых клетках взять по модулю, элементы на черных взять с противоположным знаком и вывести матрицу.
Var
    A: Array [1..8, 1..8] of Integer;
    I, J: Integer;
Begin
    Randomize;
    For I := 1 to 8 do
    Begin
        For J := 1 to 8 do
        Begin
            A[I,J] := Random(20) - Random(20);
            Write(A[I,J]:4);
        End;
        Writeln;
    End;
    Writeln;
    For I := 1 to 8 do
        For J := 1 to 8 do
            If (I + J) mod 2 = 0 Then
                A[I,J] := -A[I,J]
            Else
                A[I,J] := Abs(A[I,J]);
    For I := 1 to 8 do
    Begin
        For J := 1 to 8 do
            Write(A[I,J]:4);
        Writeln;
    End;}
    {Задача 11.(?).
    Дана запись десятичного числа, убрать в нем четные цифры.
    Var
    I, Numb: Integer;
    Str: String;
    Readln(Numb);
    Str := IntToStr(Numb);
    For I := Length(Str) downto 1 do
        If StrToInt(Str[I]) mod 2 = 0 Then
            Delete(Str, I, 1);
    Writeln(Str);}


    {//Задача 12.(42).
    //Есть строка S состоящая из латинских символов. Нужно зашифровать строку по
    //типу A - D, B - E, C - F, ... W - Z, X - A, Y - B, Z - C. Вывести зашифрованную строку.
    Var
    Str: String;
    I: Integer;
    Readln(Str);
    Str := UpperCase(Str);
    For I := 1 to Length(Str) do
        Str[I] := Chr((Ord(Str[I]) - Ord('A') + 3) Mod 26 + Ord('A'));
    Writeln(Str); }
    {//Задача 13.(?).
    //Дана строка S , содержащая имя файла(например,
    //C:\WebServers\home\testsite\www\myfile.txt) выделить из этой строки имя файла
    //без расширения и вывести его.
    Var
    I, J, Temp: Integer;
    Str, Name: String;
    Name := '';
    Readln(Str);
    J := 0;
    For I := 1 to Length(Str) do
        If Str[I] = '\' Then
            Inc(J);
    I := 0;
    Temp := 1;
    While (I <> J) do
    Begin
        If Str[Temp] = '\' Then
            Inc(I);
        Inc(Temp);
    End;
    While (Str[Temp] <> '.') do
    Begin
        Name := Name + Str[Temp];
        Inc(Temp);
    End;
    Writeln(Name);}
    {//Задача 14.(26).
    //Дана матрица 10×10. Заменить последние элементы строк суммой предыдущих элементов строки.
    Var
    I, J, Sum: Integer;
    Arr: Array [1..10, 1..10] of Integer;
    Randomize;
    For I := 1 to 10 do
    Begin
        For J := 1 to 10 do
        Begin
              Arr[I,J] := Random(50) - Random(50);
              Write(Arr[I,J]:4);
        End;
        Writeln;
    End;
    Writeln;
    For I := 1 to 10 do
    Begin
        Sum := 0;
        For J := 1 to 9 do
            Sum := Sum + Arr[I,J];
        Arr[I,10] := Sum;
    End;
    For I := 1 to 10 do
    Begin
        For J := 1 to 10 do
            Write(Arr[I,J]:4);
        Writeln;
    End;}
    {Дано натуральное число N. Нужно вывести количество повторений цифр от 0 до
    9, которые присутствуют в числе N.
    (то есть, если дано число 12134712, то вывод такой:
    цифра 0 повторяется 0 раз
    цифра 1 повторяется 3 раза
    цифра 2 повторяется 2 раза
    цифра 3 повторяется 1 раз
    ...
    цифра 9 повторяется 0 раз).
    Var
    I, Numb, Counter: Integer;
    Str: String;
    Readln(Str);
    Counter := 0;
    While (Counter < 10) do
    Begin
        Numb := 0;
        For I := 1 to Length(Str) do
            If (StrToInt(Str[I]) = Counter) Then
                Inc(Numb);
        Writeln('Цифра ', Counter, ' встречается ', Numb, ' раз');
        Inc(Counter);
    End;}


    {//Задача 16.(6).
    //Даны два числа M и N. Определить являются ли эти числа взаимно простыми.
    //Ответ вывести в виде ДА или НЕТ.
    Var
    M, N, Coef: Integer;
    Readln(N);
    Readln(M);
    While N <> M do
        If N > M Then
            Dec(N, M)
        Else
            Dec(M, N);
    Coef := N;
    If Coef = 1 Then
        Writeln('Yes')
    Else
        Writeln('No');}

        //Задача 17.(?).
        //Дана строка из пробелов и последовательности латинских букв. найти все слова
        //начинающиеся и заканчивающиеся с одной и той же буквы, если таких нет,
        //вывести, что нет.
{Procedure Find(Str: String);
Var
    Finder, I: Integer;
    Temp: String;
Begin
    Finder := 0;
    I := 1;
    While I < Length(Str) do
    Begin
        Temp := '';
        While (Str[I] <> ' ') do
        Begin
            Temp := Temp + Str[I];
            Inc(I);
        End;
        If LowerCase(Temp[1]) = Temp[Length(Temp)] Then
        Begin
            Writeln(Temp);
            Inc(Finder);
        End;
        Inc(I);
    End;
    If Finder = 0 Then
        Writeln('No');
End;

Var
    Str, Temp: String;
    Finder, I: Integer;

Begin
    Readln(Str);
    Str := Trim(Str);
    Str := Str + ' ';
    Find(Str);}

    {//Задача 18.(?).
    //Дано натуральное число N. вывести это число тройками через пробелы, начиная с конца. Например: 1234567 ->1 234 567.
Var
    N, I: Integer;
    Str: String;
Begin
    Readln(N);
    Str := IntToStr(N);
    I := Length(Str);
    While (I > 3) do
    Begin
        Dec(I, 3);
        Insert(' ', Str, I + 1);
    End;
    Writeln(Str);}
{    //Задача 19.(34).
    //Дана последовательность слов разделенная одним или несколькими пробелами,
    //вывести слово максимальной длины.
Procedure New(Str: String);
Var
    Temp, I, Counter: Integer;
    Word, TempWord: String;
Begin
    Temp := 0;
    I := 1;
    While (I <= Length(Str)) do
    Begin
        TempWord := '';
        Counter := 0;
        While (Str[I] <> ' ') and (I <= Length(Str)) do
        Begin
            TempWord := TempWord + Str[I];
            Inc(I);
            Inc(Counter);
        End;
        If (Temp < Counter) Then
        Begin
              Temp := Counter;
              Word := TempWord;
        End;
        Inc(I);
    End;
    Writeln(Word);
End;
Var
    I, Temp, Counter: Integer;
    Str, TempWord, Word: String;
Begin
    Readln(Str);
    Str := Trim(Str);
    New(Str);
    Readln;}

    {//Задача 20.(?).
    //Преобразовать строку с буквами в строку из номера их в алфавите
    //ABCDE->12345.

Var
    Str, StrNumb: String;
    I, Temp: Integer;
Begin
    Readln(Str);
    StrNumb := '';
    For I := 1 to Length(Str) do
    Begin
        If (Ord(Str[I]) < Ord('a')) Then
            Temp := 32
        Else
            Temp := 0;
        StrNumb := StrNumb + IntToStr(Ord(Str[I]) - Ord('a') + 1 + Temp);
        // Или просто LowerCase всю строку и IntToStr(Ord(Str[I]) - Ord('a') + 1);
    End;
    Writeln(StrNumb);}

    {//Задача 21.(?).
    //Даны числа D , M , Y - номер дня в месяце, номер месяца в году и год.
    //определить порядковый номер дня, учитывая високосность

Var
    I, D, M, Y, DaySum, Err: Integer;
    Date, Current: String;
Begin
    Readln(Date);
    DaySum := 0;
    Current := Copy(Date,1,Pos('.',Date)-1);
    Delete(Date, 1, Pos('.', Date));
    Val(Current, D, Err);
    Current := Copy(Date,1,Pos('.',Date)-1);
    Delete(Date, 1, Pos('.', Date));
    Val(Current,M,err);
    Val(Date,Y,err);
    If Y mod 4 = 0 Then
        Inc(DaySum);
    For I := 1 to M - 1 do //M - 1, чтобы не учитывать еще весь не пройденный месяц выбранного дня
    Begin
        If (I = 2) Then
            Inc(DaySum, 28);
        If (I <= 7) Then
            If (I mod 2 = 0) and (I <> 2) Then
                Inc(DaySum, 30)
            Else
                Inc(DaySum, 31)
        Else
            If (I mod 2 = 0) Then
                Inc(DaySum, 31)
            Else
                Inc(DaySum, 30);

    End;
    Inc(DaySum, D);
    Writeln(DaySum);}
{Var
    Str: String;


Begin
    //Задача 22.(?).
    //Пользователь вводит цифры в строку в формате 'hh:mm:ss', это время. Надо
    //проверить правильность ввода и вывести на консоль: да, нет. Там тип нельзя,
    //чтобы было 24 часа, 63 секунды и т.д.
    Try
        Readln(Str);
        If (StrToInt(Copy(Str,1,2)) > 23) or (StrToInt(Copy(Str,4,2)) > 59) or (StrToInt(Copy(Str,7,2)) > 59) Then
            Writeln('No')
        Else
            Writeln('Yes')
    Except
        Writeln('No');
    End;}
    {//Задача 23 (28).
    //Дана матрица 10*10
    //Найти в строке нулевой элемент и все элементы после нулевого сложить по
    //модулю
Var
    Arr: Array[1..10, 1..10] of Integer;
    I, J, Sum: Integer;
    IsZero: Boolean;
Begin
    Randomize;
    For I := 1 to 10 do
    Begin
        For J := 1 to 10 do
        Begin
            Arr[I,J] := Random(20) - Random(20);
            Write(Arr[I,J]:4);
        End;
        Writeln;
    End;
    For I := 1 to 10 do
    Begin
        Sum := 0;
        IsZero := False;
        For J := 1 to 10 do
        Begin
            If (Arr[I,J] = 0) Then
                IsZero := True;
            If (IsZero) Then
                Inc(Sum, Abs(Arr[I,J]));
        End;
        If (isZero) then
        Writeln('Row ', I, '. Sum : ', Sum);
    End;}
    {//Задача 24(37.)
    //проверить правильно определён идентификатор, первый символ должен быть латинской
    //буквой, все остальное цифра или буква
Var
    Str: String;
    I: Integer;
    IsCorrect: Boolean;
Begin
    Readln(Str);
    IsCorrect := True;
    Str := UpperCase(Str);
    If (Ord(Str[1]) - Ord('A') < 0) or (Ord(Str[1]) - Ord('A') > 25) Then
        IsCorrect := False;
        I := 2;
    While (I < Length(Str)) and IsCorrect do
    Begin
        If ((Ord(Str[I]) - Ord('A') < 0) or (Ord(Str[I]) - Ord('A') > 25)) and ((Ord(Str[I]) - Ord('0') < 0) or (Ord(Str[I]) - Ord('0') > 9)) Then
            IsCorrect := False;
        Inc(I);
    End;
    If (IsCorrect) Then
        Writeln('Good')
    Else
        Writeln('Bad');
    Readln;
End.}
    {//Задача 25(?)
    //дано натуральное число, найти сумму четных цифр
Var
    N, I, Sum: Integer;
    Str: String;
Begin
    Readln(N);
    Sum := 0;
    Str := IntToStr(N);
    For I := 1 to Length(Str) do
        If Not(Odd(StrToInt(Str[I]))) Then
            Inc(Sum, StrToInt(Str[I]));
    Writeln(Sum);}
    {//Задача 28(33).
    //Дана строка с пробелами. Первую букву каждого слова сделать заглавной
Var
    Str: String;
    I: Integer;
Begin
    Readln(Str);
    Str := Trim(Str);
    Str[1] := UpCase(Str[1]);
    For I := 2 to Length(Str) do
        If (Str[I - 1] = ' ') and (Str[I] <> ' ') Then
            Str[I] := UpCase(Str[I]);
    Writeln(Str);}

    {//+1 задачка с номером 4 у Оношко:
    //Дано натуральное число N. Определить,является ли оно простым. Вывести "Да" или "Нет".
Var
    Counter, I, N: Integer;
Begin
    Readln(N);
    Counter := 0;
    I := 1;
    While (I < N div 2) and (Counter <> 2) do
    Begin
        If N Mod I = 0 Then
            Inc(Counter);
        Inc(I);
    End;
    If (Counter = 2) Then //2, потому что, когда 1, то она инкрементирует Counter
        Writeln('Ne Prostoe')
    Else
        Writeln('Prostoe');}

{Begin

    Readln;
End.}

