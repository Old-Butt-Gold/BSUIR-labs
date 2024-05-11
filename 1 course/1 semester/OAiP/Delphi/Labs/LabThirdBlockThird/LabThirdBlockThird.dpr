program LabThirdBlockThird;

uses
  System.SysUtils;

Type
    TArrOI = Array of Integer;

Function ChooseOptionForInput(): String;
Var
    Input: String;
    IsRight: Boolean;
Begin
    Writeln('������� console, ���� ������ ������������ �������, file, ���� ����, random, ����� ������������� ��������� ������');
    Repeat
        IsRight := True;
        Readln(Input);
        Input := LowerCase(Input);
        If (Input <> 'console') and (Input <> 'file') and (Input <> 'random') then
            Begin
                IsRight := False;
                Writeln('��������� ����!');
            End;
    Until IsRight;
    ChooseOptionForInput := Input;
End;

Function ChooseOptionForOutput(): String;
Var
    Input: String;
    IsRight: Boolean;
Begin
    Writeln('������� console, ���� ������ ������������ �������, file, ���� ����: ��� ������ ������');
    Repeat
        IsRight := True;
        Readln(Input);
        Input := LowerCase(Input);
        If (Input <> 'console') and (Input <> 'file') then
            Begin
                IsRight := False;
                Writeln('��������� ����!');
            End;
    Until IsRight;
    ChooseOptionForOutput := Input;
End;

Function InputSizeOfArray(): Integer;
Const
    MIN_NUM = 2;
Var
    N: Integer;
    IsCorrect: Boolean;
Begin
    Write('������� ����������� �������: ');
    Repeat
        Try
            IsCorrect := True;
            Readln(N);
        Except
            IsCorrect := False;
            Writeln('��������� ����');
        End;
        If IsCorrect and (N < MIN_NUM) Then
            Begin
                IsCorrect := False;
                Writeln('��������� ����');
            End;
    Until IsCorrect;
    InputSizeOfArray := N;
End;

Function TakeCellIntoArray(): Integer;
Var
    N: Integer;
    IsCorrect: Boolean;
Begin
    Repeat
        Try
            IsCorrect := True;
            Readln(N);
        Except
            IsCorrect := False;
            Writeln('��������� ����');
        End;
    Until IsCorrect;
    TakeCellIntoArray := N;
End;

Function FillArray(): TArrOI;
Var
    Arr: TArrOI;
    I: Integer;
    N: Integer;
Begin
    N := InputSizeOfArray();
    SetLength(Arr, N);
    For I := 0 to High(Arr) do
        Begin
            Write('������� �������: ', I + 1, ' ');
            Arr[I] := TakeCellIntoArray();
        End;
    FillArray := Arr;
End;

Function ShellsSort(Arr: TArrOI): TArrOI;
StdCall;
External 'lab3_3.dll';

Procedure WriteArrayOfNumbers(Arr: TArrOI);
StdCall;
External 'lab3_3.dll';


Function TakeRandomSizeOfArray(): Integer;
Var
    N: Integer;
Begin
    Randomize;
    N := Random(100) + 2;
    TakeRandomSizeOfArray := N;
End;

Function TakeRandomElementsOfArray(): TArrOI;
Var
    Arr: TArrOI;
    N: Integer;
    I: Integer;
Begin
    N := TakeRandomSizeOfArray();
    SetLength(Arr, N);
    For I := 0 to High(Arr) do
        Arr[I] := Random(1000) - Random(1000);
    TakeRandomElementsOfArray := Arr;
End;

Function IsCorrectSizeOfArrayInFile(Var F: TextFile; Var N: Integer): Boolean;
StdCall;
External 'lab3_3.dll';

Function IsCorrectElementsOfArrayInFile(Var F: TextFile; N: Integer; Var Arr: TArrOI): Boolean;
StdCall;
External 'lab3_3.dll';

Function CheckPermission(Path: String): Boolean;
StdCall;
External 'lab3_3.dll';

Function TakeFileWay(Flag: String; Var Arr: TArrOI): String;
StdCall;
External 'lab3_3.dll';

Function TakeFinalArrInformation(InputChoice: String): TArrOI;
Var
    N: Integer;
    Way: String;
    F: TextFile;
    Arr: TArrOI;
Begin
    If InputChoice = 'console' Then
        Arr := FillArray()
    else If InputChoice = 'random' Then
        Arr := TakeRandomElementsOfArray()
    else
        Way := TakeFileWay('In', Arr);
    TakeFinalArrInformation := Arr;
End;

Procedure WriteArrayOfNumbersInFile(Var F: TextFile; Arr: TArrOI);
StdCall;
External 'lab3_3.dll';


Procedure OutputAnswer(OutputChoice: String; Arr: TArrOI); StdCall;
External 'lab3_3.dll';

Var
    Arr: TArrOI;
    InputChoice, OutputChoice: String;
Begin
    Writeln('������ ��������� ��������� ������ ������� �����');
    InputChoice := ChooseOptionForInput();
    Arr := TakeFinalArrInformation(InputChoice);
    OutputChoice := ChooseOptionForOutput();
    OutputAnswer(OutputChoice, Arr);
    Readln;
End.
