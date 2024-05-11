Program LabSecondBlockThird;

Uses
  System.SysUtils;

Type
    TSet = Set of Char;

Function ChooseOption(): String;
Var
    Option: String;
    IsRight: Boolean;
Begin
    Writeln('������� console, ���� ������ ������������ �������, file, ���� ����');
    Repeat
        IsRight := True;
        Readln(Option);
        Option := LowerCase(Option);
        If (Option <> 'console') and (Option <> 'file') then
            Begin
                IsRight := False;
                Writeln('��������� ����!');
            End;
    Until IsRight;
    ChooseOption := Option;
End;

Function InputStrConsole(): String;
Var
    Str: String;
    IsIncorrect: Boolean;
Begin
    Repeat
        IsIncorrect := True;
        Readln(Str);
        If (Length(Str) = 0) Then
            Begin
                IsIncorrect := False;
                Writeln('��������� ����!');
            End;
    Until IsIncorrect;
    InputStrConsole := Str;
End;



Function TakeFileWay(): String;
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
        If Not(FileExists(Way)) or Not(ExtractFileExt(Way) = '.txt')  Then
            Begin
                Writeln('����� � �������� ���� ��� ��� ������������ ���������� �����');
                IsCorrect := False;
            End;
    Until IsCorrect;
    TakeFileWay := Way;
End;

Function IsCorrectStrFromFile(Var Str: String; Var F: TextFile): Boolean;
Var
    IsCorrect: Boolean;
Begin
    IsCorrect := True;
    If Not Eof(F) Then
        Begin
            Readln(F, Str);
            If (Length(Str) = 0) Then
                Begin
                    Writeln('��������� ������������ ��������� ������');
                    IsCorrect := False;
                End;
            If (IsCorrect) and Not Eof(F) Then
                Begin
                    Writeln('������� ������ ������');
                    IsCorrect := False;
                End;
        End
    Else
        Begin
            Writeln('��� ������ ��� ������');
            IsCorrect := False;
        End;
    IsCorrectStrFromFile := IsCorrect;
End;

Function InputStrFromFile(): String;
Var
    Str, Way: String;
    F: TextFile;
    IsCorrect: Boolean;
Begin
    Repeat
        IsCorrect := True;
        Way := TakeFileWay();
        AssignFile(F, Way);
        Try
            Try
                Reset(F);
                IsCorrect := IsCorrectStrFromFile(Str, F);
            Finally
                Close(F);
            End;
        Except
            IsCorrect := False;
            Writeln('���� ������ ���������');
        End;
    Until IsCorrect;
    InputStrFromFile := Str;
End;

Function FindAnswer(Str: String): TSet;
Var
    MySet, FinalSet: TSet;
    I: Integer;
Begin
    FinalSet := [];
    MySet := ['('..'>'];
    For I := 1 to Length(Str) do
        Begin
            If AnsiChar(Str[I]) In MySet Then
                Begin
                    Include(FinalSet, AnsiChar(Str[I]));
                    Exclude(MySet, AnsiChar(Str[I]));
                End;
        End;
    FindAnswer := FinalSet;
End;

Function TakeFinalInformation(Input: String): TSet;
Var
    Str: String;
    FinalSet: TSet;
Begin
    If (Input = 'console') Then
        Str := InputStrConsole()
    Else
        Str := InputStrFromFile();
    FinalSet := FindAnswer(Str);
    TakeFinalInformation := FinalSet;
End;

Procedure PrintInFile(FinalSet: TSet);
Var
    F: TextFile;
    Way: String;
    IsCorrect: Boolean;
    I: Integer;
Begin
    Repeat
        IsCorrect := True;
        Way := takeFileWay();
        AssignFile(F, Way);
        Try
            Try
                ReWrite(F);
                If FinalSet = [] Then
                  Writeln(F,'��� ������ ��������')
                Else
                  Begin
                      Writeln(F,'���� ������� � ��������� ������: ');
                      For I := 0 to 255 do
                          If Chr(I) In FinalSet Then
                              Write(F,Chr(I):4);
                  End;
            Finally
                Close(F);
            End;
        Except
            IsCorrect := False;
            Writeln('� ���� ������ �������');
        End;
    Until IsCorrect;
End;

Procedure OutputAnswer(Output: String; FinalSet: TSet);
Var
    Way: String;
    I: Integer;
Begin
      If (Output = 'console') Then
          Begin
              If FinalSet = [] Then
                  Writeln('��� ������ ��������')
              Else
                  Begin
                      Writeln('���� ������� � ��������� ������: ');
                      For I := 0 to 255 do
                          If Chr(I) In FinalSet Then
                              Write(Chr(I):4);
                  End;
          End
      Else
          PrintInFile(FinalSet);
End;

Var
    Input, Output: String;
    FinalSet: TSet;
Begin
    Input := ChooseOption();
    FinalSet := TakeFinalInformation(Input);
    Output := ChooseOption();
    OutputAnswer(Output, FinalSet);
    Readln;
End.
