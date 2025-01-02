program Project1;

{$APPTYPE CONSOLE}

{$R *.res}

uses
  System.SysUtils;

type
    TString = String[20];
    User = ^TUser;  {Указатель на элемент типа TList}
    TUser = Record {А это наименование нашего типа "запись" обычно динамические структуры описываются через запись}
        UserName: TString;
        PhoneNumber: Integer;  {данные, хранимые в элементе}
        Next: User;   {указатель на следующий элемент списка}
    end;

function InputList(Count: Integer): User;
var
    Element, Head, Start: User;
    I: Integer;
    IsCorrect: Boolean;
    Temp: String;
    LowerTemp: String[20];
Begin
    New(Element);
    Head := Element;
    Start := Element;
    I := 0;
    While I < Count do
    Begin
        IsCorrect := True;
        Start := Head;
        Writeln('UserName: ', I + 1);
        Repeat
            Readln(Element^.UserName);
        Until Length(Element^.UserName) < 21;

        Element^.UserName[1] := UpCase(Element^.UserName[1]);
        LowerTemp := Copy(Element^.UserName, 2, Length(Element^.UserName));
        Delete(Element^.UserName, 2, Length(Element^.UserName));
        Element^.UserName := Element^.UserName + LowerCase(LowerTemp);

        Writeln('Phone Number: ', I + 1);
        Repeat
            IsCorrect := True;
            Try
                Readln(Temp);
                Element^.PhoneNumber := StrToInt(Temp);
            Except
                IsCorrect := False;
                Writeln('Проверьте ваши данные');
            End;
            If IsCorrect and (Length(Temp) <> 7) Then
                Begin
                    IsCorrect := False;
                    Writeln('Введите семизначное число');
                End;
        Until IsCorrect;

        While (Start^.Next <> Element^.Next) and IsCorrect do
        Begin
            If (Start^.UserName = Element^.UserName) and (Start^.PhoneNumber = Element^.PhoneNumber) Then
                IsCorrect := False;
            Start := Start^.Next;
        End;
        If IsCorrect Then
            Begin
                  New(Element^.Next);
                  Element := Element^.Next;
                  Inc(I);
            End
        Else
            Writeln('Проверьте ваши данные');
    End;
    Element^.Next := nil;
    Result := Head;
End;


procedure SortList(Count: User);
Var
    Temp, Start: User;
    TempSwap: String[20];
    PhoneSwap: Integer;
begin
    Start := Count;
    While Start^.Next <> nil do   //Почему Start^.Next
    Begin
        Temp := Start^.next;
        While Temp^.Next <> nil do
        Begin
            If Temp^.UserName < Start^.UserName Then
                Begin
                    TempSwap := Temp^.UserName;
                    Temp^.UserName := Start^.UserName;
                    Start^.UserName := TempSwap;

                    PhoneSwap := Temp^.PhoneNumber;
                    Temp^.PhoneNumber := Start^.PhoneNumber;
                    Start^.PhoneNumber := PhoneSwap;

                End;
            Temp := Temp^.Next;
        End;
        Start := Start^.Next;
    End;
end;

procedure OutputList(Count: User);
var
    Start: User;
begin
    Start := Count;
    While Start^.Next <> nil do
    begin
        Write(Start^.UserName + '–' + IntToStr(Start^.PhoneNumber),#13#10);
        Start := Start^.Next;
    end;
end;

Procedure FindFromNumber(Count: User);
Var
    Start: User;
    Temp: String;
    TempInt: Integer;
    IsCorrect: Boolean;
Begin
    Repeat
        IsCorrect := True;
        Try
            Readln(Temp);
            TempInt := StrToInt(Temp);
        Except
            IsCorrect := False;
            Writeln('Проверьте ваши данные');
        End;
        If IsCorrect and (Length(Temp) <> 7) Then
            Begin
                IsCorrect := False;
                Writeln('Введите семизначное число');
            End;
    Until IsCorrect;
    Start := Count;
    While Start^.Next <> nil do
    Begin
        If (Start^.PhoneNumber = TempInt) Then
            Writeln(Start^.UserName);
        Start := Start^.Next;
    End;
End;

Procedure FindFromUserName(Count: User);
Var
    Start: User;
    Temp: TString;
Begin
    Writeln('Введите имя пользователя');
    Repeat
        Readln(Temp);
    Until Length(Temp) < 21;
    Start := Count;
    While Start^.Next <> nil do
    Begin
        If (Start^.UserName = Temp) Then
            Writeln(Start^.PhoneNumber);
        Start := Start^.Next;
    End;
End;

Function InputChoice(): String;
Var
    Str: String;
    IsCorrect: Boolean;
    Numb: Integer;
Begin
    Repeat
        IsCorrect := True;
        Try
            Readln(Str);
            Numb := StrToInt(Str);
        Except
            IsCorrect := False;
            Writeln('Повторите ввод');
        End;
        If IsCorrect and ((Numb > 3) or (Numb < 1)) Then
    Until IsCorrect;
    Result := Str;
End;

Var
    IsIncorrect: Boolean;
    Count: Integer;
    Head: User;
    Number: Integer;
    Str: TString;
begin
    Write('Введите количество абонентов от 2 до 6: ');
    Repeat
        IsInCorrect := True;
        Try
            Readln(Count);
        Except
            IsInCorrect := False;
            Writeln('Проверьте данные');
        End;
        If IsIncorrect and ((Count > 6) or (Count < 2)) Then
            Begin
                IsIncorrect := False;
                Writeln('Проверьте ваши даннные');
            End;
    Until IsIncorrect;

    Head := InputList(Count);
    SortList(Head);
    OutputList(Head);

    Repeat
        Writeln('Нажмите 1 для поиска фамилии по номеру телефона'#13#10'2 для поиска телефона по фамилии'#13#10'3 чтобы выйти');
        Str := InputChoice();
        If Str = '1' Then
            FindFromNumber(Head)
        Else If Str = '2' Then
            FindFromUserName(Head);
    Until StrToInt(Str) = 3;
end.

