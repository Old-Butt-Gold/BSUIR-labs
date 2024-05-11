Program LabFirstBlockThird;

uses
    System.SysUtils, Windows;

Function ChooseOption(): String;
Var
    Input: String;
    IsRight: Boolean;
Begin
    Writeln('������� console, ���� ������ ������������ �������, file, ���� ����');
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
    ChooseOption := Input;
End;

Function ChooseConsoleWayToFill(): String;
Var
    Input: String;
    IsRight: Boolean;
Begin
    Writeln('������� console, ���� ������ ���� ������ ������, random, ����� ������������� ��������� ������');
    Repeat
        IsRight := True;
        Readln(Input);
        Input := LowerCase(Input);
        If (Input <> 'console') and (Input <> 'random') then
            Begin
                IsRight := False;
                Writeln('��������� ����!');
            End;
    Until IsRight;
    ChooseConsoleWayToFill := Input;
End;

Function InputStrConsole(): String;
Var
    Str: String;
    IsIncorrect: Boolean;
Begin
    Repeat
        IsIncorrect := True;
        Readln(Str);
        If (Length(Str) = 0) or (Str[1] = ' ') then
            Begin
                IsIncorrect := False;
                Writeln('��������� ����!');
            End;
    Until IsIncorrect;
    InputStrConsole := Str;
End;

Function InputAmountOfOccurrences(): Integer;
Const
    MIN_NUM = 0;
    MAX_NUM = 255;
Var
    K: Integer;
    IsIncorrect: Boolean;
Begin
    Write('������� �����, ���������� ��������� ������ 1 � ������ 2, k = ');
    Repeat
        Try
            IsIncorrect := True;
            Readln(K);
        Except
            IsIncorrect := False;
            Writeln('��������� ����');
        End;
        If IsIncorrect and ((K < MIN_NUM) or (K > MAX_NUM)) Then
            Begin
                IsIncorrect := False;
                Writeln('��������� ����');
            End;
    Until IsIncorrect;
    InputAmountOfOccurrences := K;
End;

Function IsCorrectStrFromFile(Var F: TextFile): Boolean;
Var
    Str1, Str2: String;
    IsIncorrect: Boolean;
Begin
    IsIncorrect := True;
    If Not Eof(F) Then
        Begin
            Readln(F, Str1);
            If (Length(Str1) = 0) or (Str1[1] = ' ') Then
                Begin
                    IsIncorrect := False;
                    Writeln('������, ��������� ���� 1 ������ �� ������������ ������');
                End
        End
    Else
        Begin
            IsIncorrect := False;
            Writeln('� ����� ��� ������ ��� ������ ������');
        End;
    If Not Eof(F) Then
        Begin
            Readln(F, Str2);
            If (Length(Str2) = 0) or (Str2[1] = ' ') Then
                Begin
                    IsIncorrect := False;
                    Writeln('������, ��������� ���� 2 ������ �� ������������ ������');
                End
        End
    Else
        Begin
            IsIncorrect := False;
            Writeln('� ����� ��� ������ ��� ������ ������');
        End;
    IsCorrectStrFromFile := IsIncorrect;
End;

Function IsCorrectAmountFromFile(Var F: TextFile): Boolean;
Const
    MIN_NUM = 0;
    MAX_NUM = 255;
Var
    K: Integer;
    IsIncorrect: Boolean;
Begin
    IsIncorrect := True;
    If Not Eof(F) Then
        Begin
            Try
                Readln(F, K);
            Except
                IsIncorrect := False;
                Writeln('��������� ����');
            End;
        End
    Else
        Begin
            Writeln('��� ���������� ��������� �����');
            IsIncorrect := False;
        End;
    If IsIncorrect and ((K < MIN_NUM) or (K > MAX_NUM)) Then
        IsIncorrect := False;
    IsCorrectAmountFromFile := IsIncorrect;
End;

Function WayToFile(Flag: String): String;
Var
    Way, Str1, Str2: String;
    F: TextFile;
    IsIncorrect: Boolean;
Begin
    Repeat
        IsIncorrect := True;
        Writeln('������� ���� � �����');
        Readln(Way);
        AssignFile(F, Way);
        If FileExists(Way) Then
            Begin
                If Flag = 'In' Then
                    Begin
                        Reset(F);
                        IsIncorrect := IsCorrectStrFromFile(F);
                        If (IsIncorrect) Then
                            IsIncorrect := IsCorrectAmountFromFile(F);
                        If (IsIncorrect and Not Eof(F)) Then
                            Begin
                                IsIncorrect := False;
                                Writeln('������� ������ ������');
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


Function CalculateAnswer(Str1: String; Str2: String; K: Integer): Integer;
Var
    CountN, N, I: Integer;
Begin
    CountN := 0;
    N := 0;
    I := 1;
    For I := 1 to Length(Str2) Do
        Begin
            If Copy(Str2,I,Length(Str1)) = Str1 Then
                Begin
                    Inc(CountN);
                    If CountN = K Then
                        N := I;
                End;

        End;
    CalculateAnswer := N;
End;

Function TakeStrFromFile(Var F: TextFile): String;
Var
    Str: String;
Begin
    Readln(F, Str);
    TakeStrFromFile := Str;
End;

Function InputAmountOfOccurrencesFromFile(Var F: TextFile): Integer;
Var
    K: Integer;
    IsIncorrect: Boolean;
Begin
    Readln(F, K);
    InputAmountOfOccurrencesFromFile := K;
End;

Function RandomStrConsole(): String;
Var
    I, N: Integer;
    Str: String;
Begin
    Randomize;
    N := random(255) + 1;
    For I := 1 to N Do
        Str := Str + Chr(Random((Ord('z') - Ord('A'))) + Ord('A')); //Chr(random(96) + 32);
    RandomStrConsole := Str;
End;

Function ConsoleInput(InputChoice: String): Integer;
Var
    IsLengthRight: Boolean;
    Str1, Str2: String;
    K, N: Integer;
Begin
    If InputChoice = 'console' Then
        Begin
            Repeat
                IsLengthRight := True;
                Writeln('������� ������ 1(������ ������ �� ������ ���� ��������)');
                Str1 := InputStrConsole();
                Writeln('������� ������ 2(������ ������ �� ������ ���� ��������)');
                Str2 := InputStrConsole();
                If Length(Str1) > Length(Str2) Then
                  Begin
                      Writeln('��������� ���� �����, ����� ������ ����� ������ ������ ��� ������ ������� ������ ������');
                      IsLengthRight := False;
                  End;
            Until IsLengthRight;
        End
    Else
        Begin
            Repeat
                IsLengthRight := True;
                Str1 := RandomStrConsole();
                Str2 := RandomStrConsole();
                If Length(Str1) > Length(Str2) Then
                    IsLengthRight := False;
            Until IsLengthRight;
            Writeln('������ �������������.');
            Writeln(Str1);
            Writeln(Str2);
        End;
    K := InputAmountOfOccurrences();
    N := CalculateAnswer(Str1, Str2, K);
    ConsoleInput := N;
End;

Function FileInput(): Integer;
Var
    Way, Str1, Str2: String;
    F: TextFile;
    N, K: Integer;
Begin
    Way := WayToFile('In');
    AssignFile(F, Way);
    Reset(F);
    Str1 := TakeStrFromFile(F);
    Str2 := TakeStrFromFile(F);
    K := InputAmountOfOccurrencesFromFile(F);
    Close(F);
    N := CalculateAnswer(Str1, Str2, K);
    FileInput := N;
End;

Function TakeFinalInformation(InputChoice: String): Integer;
Var
    Way: String;
    N: Integer;
Begin
    If (InputChoice = 'console') Then
        Begin
            InputChoice := ChooseConsoleWayToFill();
            N := ConsoleInput(InputChoice);
        End
    Else
        Begin
            N := FileInput;
            Writeln('������ ������� �������');
        End;
    TakeFinalInformation := N;
End;

Procedure WriteAnswerConsole(N: Integer);
Begin
    Writeln('��������� ����������� �������.',#10,'����� ������� k-�� ���������: ', N);
End;

Procedure WriteAnswerFile(N: Integer);
Var
    Way: String;
    F: TextFile;
Begin
    Way := WayToFile('Out');
    AssignFile(F, Way);
    ReWrite(F);
    Writeln(F, '����� ������� k-�� ���������: ', N);
    Close(F);
End;

Procedure OutputAnswer(OutputChoice: String; N: Integer);
Begin
    If (OutputChoice = 'console') Then
        WriteAnswerConsole(N)
    Else
        WriteAnswerFile(N);
End;

Var
    str, InputChoice, OutputChoice: String;
    N: Integer;
Begin
    Writeln('������ ��������� ������� ���������� ���������(����������� �������������) ������ ������ �� ������');
    InputChoice := ChooseOption();
    N := TakeFinalInformation(InputChoice);
    OutputChoice := ChooseOption();
    OutputAnswer(OutputChoice, N);
    Readln;
End.
