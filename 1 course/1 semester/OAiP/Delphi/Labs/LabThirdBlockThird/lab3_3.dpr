library lab3_3;

uses
  System.SysUtils,
  System.Classes;

Type
    TArrOI = Array of Integer;

{$R *.res}

Function CheckPermission(Path: String): Boolean; StdCall;
Var
    F: TextFile;
    IsCorrect: Boolean;
Begin
    Assign(F, Path);
    IsCorrect := True;
    Try
        Rewrite(F);
        Close(F);
    Except
        Writeln('���� ������ ��� ������.');
        IsCorrect := False;
    End;
    CheckPermission := IsCorrect;
End;

Function IsCorrectSizeOfArrayInFile(Var F: TextFile; Var N: Integer): Boolean; StdCall;
Const
    MIN_NUM = 2;
Var
    IsCorrect: Boolean;
Begin
    IsCorrect := True;
    If Not Eof(F) Then
        Begin
            Try
                Readln(F, N);
            Except
                IsCorrect := False;
                Writeln('��������� ���� � ����� ����������� ������ �������');
            End;
        End
    Else
        Begin
            Writeln('��� ����������� �������');
            IsCorrect := False;
        End;
    If IsCorrect and (N < MIN_NUM) Then
        IsCorrect := False;
    IsCorrectSizeOfArrayInFile := IsCorrect;
End;

Function IsCorrectElementsOfArrayInFile(Var F: TextFile; N: Integer; Var Arr: TArrOI): Boolean;  StdCall;
Var
    I: Integer;
    IsCorrect: Boolean;
Begin
    IsCorrect := True;
    I := 0;
    SetLength(Arr, N);
    If Not Eof(F) Then
        Begin
            While Not Eof(F) and (IsCorrect) do
                Begin
                    Try
                        Read(F, Arr[I]);
                    Except
                        IsCorrect := False;
                        Writeln('������! ��������� ������� ��� �������', I + 1, '�� ������������ �����');
                    End;
                End;
            Inc(I);
        End
    Else
        Begin
            IsCorrect := False;
            Writeln('��� ������ ��� �������');
        End;
    IsCorrectElementsOfArrayInFile := IsCorrect;
End;

Function TakeFileWay(Flag: String; Var Arr: TArrOI): String;  StdCall;
Var
    Way: String;
    F: TextFile;
    N: Integer;
    IsCorrect: Boolean;
Begin
    Repeat
        IsCorrect := True;
        Writeln('������� ���� � �����');
        Readln(Way);
        AssignFile(F, Way);
        If FileExists(Way) and (ExtractFileExt(Way) = '.txt')  Then
            Begin
                If Flag = 'In' Then
                    Begin
                        Try
                            Try
                                Reset(F);
                                IsCorrect := IsCorrectSizeOfArrayInFile(F, N);
                                If (IsCorrect) Then
                                    IsCorrect := IsCorrectElementsOfArrayInFile(F, N, Arr);
                                If (IsCorrect and Not Eof(F)) Then
                                    Begin
                                        IsCorrect := False;
                                        Writeln('������� ������ ������');
                                    End;
                            Finally
                                Close(F);
                            End;
                        Except
                            Writeln('���� ������ ��� ������');
                            IsCorrect := False;
                        End;
                    End
                Else If Not(CheckPermission(Way)) Then
                    IsCorrect := False;
            End
        Else
            Begin
                Writeln('����� � �������� ���� ��� ��� ������������ ���������� �����');
                IsCorrect := False;
            End;
    Until IsCorrect;
    TakeFileWay := Way;
End;

Function ShellsSort(Arr: TArrOI): TArrOI;  StdCall;
Var
    I, J, Temp, Gap: Integer;
Begin
    Gap := Length(Arr) div 2;
    While (Gap > 0) do
        Begin
            For I := 0 to (High(Arr) - Gap) do
                Begin
                    J := I;
                    Temp := Arr[J + Gap];
                    While (J >= 0) and (Arr[J] > Temp) do
                        Begin
                            Arr[J + Gap] := Arr[J];
                            Arr[J] := Temp;
                            J := J - Gap;
                        End;
                End;
            Gap := Gap div 2;
        End;
    ShellsSort := Arr;
End;

Procedure WriteArrayOfNumbers(Arr: TArrOI);  StdCall;
Var
    I: Integer;
Begin
    Writeln('�������������� ������: ');
    For I := 0 to High(Arr) do
        Write(Arr[I]:7);
    Writeln;
    Arr := ShellsSort(Arr);
    Writeln('����� ������: ');
    For I := 0 to High(Arr) do
        Write(Arr[I]:7);
End;

Procedure WriteArrayOfNumbersInFile(Var F: TextFile; Arr: TArrOI); StdCall;
Var
    I: Integer;
Begin
    Writeln(F, '�������������� ������: ');
    For I := 0 to High(Arr) do
        Write(F, Arr[I]:7);
    Writeln(F, #13#10);
    Arr := ShellsSort(Arr);
    Writeln(F, '����� ������: ');
    For I := 0 to High(Arr) do
        Write(F, Arr[I]:7);
    Writeln('������ ������� �������� � ����');
End;

Procedure OutputAnswer(OutputChoice: String; Arr: TArrOI); StdCall;
Var
    Way: String;
    F: TextFile;
Begin
    If OutputChoice = 'console' Then
        WriteArrayOfNumbers(Arr)
    Else
        Begin
            Way := TakeFileWay('Out', Arr);
            AssignFile(F, Way);
            ReWrite(F);
            WriteArrayOfNumbersInFile(F, Arr);
            Close(F);
        End;
End;

Exports IsCorrectElementsOfArrayInFile, CheckPermission, OutputAnswer, IsCorrectSizeOfArrayInFile, OutputAnswer, WriteArrayOfNumbersInFile, WriteArrayOfNumbers, ShellsSort, TakeFileWay;

begin
end.
