program Laba1;

{$APPTYPE CONSOLE}

uses
  SysUtils,
  Windows;

Type
    Number = ^Elem;
    Elem = Record
        AbonentNumber: String[30];
    NextNumber: Number;
    PrevNumber: Number;
end;
    Number2 = ^Elem2;
    Elem2 = Record
      Data: String[30];
      Next: Number2;
end;

Function InputAmount(): Integer;
var
    Amount: integer;
    IsCorrect: Boolean;
begin
    Writeln ('Введите количество номеров телефонов:');
    Repeat
      Try
        IsCorrect := True;
        Readln(Amount);
      Except
        Writeln('Введите корректное значение');
        IsCorrect := False;
      End;
      If (IsCorrect) And ((Amount < 1) or (Amount > 9)) Then
      Begin
        IsCorrect := False;
        Writeln('Неправильное число');
      End;
    Until (IsCorrect);
  InputAmount := Amount;
End;

Procedure InputElements (Var List: Number; Amount: Integer);
Var
    X, Head, Y: Number;
    I: integer;
    IsCorrect: Boolean;
Begin
    New(X);
    List := X;
    X^.PrevNumber := nil;
    for I := 1 to Amount do
    begin
        Y := X;
        Writeln ('Введите ', I ,'-й телефонный номер:');
        Repeat
          Try
            IsCorrect := True;
            Readln (Y^.AbonentNumber);
          Except
            Writeln('Введите корректное значение');
            IsCorrect := False;
          End;
          If (IsCorrect) And ((Length(Y^.AbonentNumber) <> 3) And (Length(Y^.AbonentNumber) <> 7)) Then
          Begin
            IsCorrect := False;
            Writeln('Введено некорректное количество цифр в номере.');
          End;
        Until (IsCorrect);
        New (X);
        Y^.NextNumber := X;
        X^.PrevNumber := Y;
    end;
    Y^.NextNumber := nil;
end;

procedure OutputInputData (Y: Number);
begin
    Writeln ('Список введённых номеров телефонов:');
    While Y <> nil Do
    begin
        Write (Y^.AbonentNumber + ' ');
        Y := Y^.NextNumber;
    end;
end;

Procedure ReverseList(List: Number);
Begin
    While List^.NextNumber <> Nil do
        List := List^.NextNumber;
    While List <> Nil do
        Begin
            Write (' ', List^.AbonentNumber);
            List := List^.PrevNumber;
        End;
End;

Procedure GetNewList(Var NewList: Number2; List: Number);
Var
    Head, Current: Number2;
Begin
    New(Head);
    NewList := Head;
    While List <> Nil do
        Begin
            If Length(List^.AbonentNumber) = 7 Then
                Begin
                    Current := Head;
                    Current^.Data := List^.AbonentNumber;
                    New(Head);
                    Current^.next := Head;
                    List := List^.NextNumber;
                End
            Else
                List := List^.NextNumber;
        End;
    Current^.next := Nil;
End;

Procedure Sort(NewList: Number2);
Begin
    While NewList <> nil do
    Begin
        Var Temp := NewList^.Next;
        While Temp <> nil do
        Begin
            If Temp^.Data < NewList^.Data Then
                Begin
                    Var TempSwap := Temp^.Data;
                    Temp^.Data := NewList^.Data;
                    NewList^.Data := TempSwap;
                End;
            Temp := Temp^.Next;
        End;
        NewList := NewList^.Next;
    End;
End;

procedure WriteSortList(NewList: Number2);
begin
    Writeln;
    Writeln('Список семизначных номеров');
    While NewList <> Nil do
        Begin
            Write(NewList^.Data + ' ');
            NewList := NewList^.Next; 
        End;
end;

Var
    Amount: Integer;
    List: Number;
    NewList: Number2;
begin
    InputElements(List, InputAmount());
    Writeln('------------------------------------------');
    OutputInputData(List);
    Writeln;
    Writeln ('Список номеров телефонов (справа налево):');
    ReverseList(List);
    GetNewList(NewList, List);
    Sort(NewList);
    WriteSortList(NewList);
    Readln;
end.
