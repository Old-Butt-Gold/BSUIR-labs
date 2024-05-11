program Project1;

{$APPTYPE CONSOLE}

{$R *.res}

uses
  System.SysUtils;

Type
  TPElem = ^TElem;
  TElem = Record
      Data: Integer;
      Next: TPElem;
  End;

Function CreateRoundList(N: Integer): TPElem;
Var
    Head, Tp, X: TPElem;
Begin
    Var I := 1;
    New(Tp);
    Head := Tp;
    While I <= N Do
        Begin
            X := Tp;
            Tp^.Data := I;
            New(Tp);
            X^.Next := Tp;
            Inc(I);
        End;
    X^.Next := Head;
    Result := Head;
End;

Procedure DeleteData(Var Head: TPElem);
Begin
    Write(Head^.Next^.Data:3);
	  Head^.Next := Head^.Next^.Next;
End;

Function GetNumberOfGap(): Integer;
Var
    IsCorrect: Boolean;
    Gap: Integer;
Begin
    Repeat
        IsCorrect := True;
        Try
            Gap := 0;
            Readln(Gap);
        Except
            IsCorrect := False;
        End;
        If IsCorrect and (Gap < 2) Then
    Until IsCorrect;
    Result := Gap;
End;

Procedure CountDown(Var Head: TPElem; Gap: Integer);
Begin
    Write('Номера выбывших: ');
    While Head <> Head^.Next do
        Begin
            For Var I := 1 To Gap - 2 do
                Head := Head^.Next;
            DeleteData(Head);
            Head := Head^.Next;
        End;
    Writeln;
End;

Procedure WriteFirstList(Head: TPElem; Data: Integer);
Begin
    Write('Первоначальный список: ');
    For Var I := 1 to Data do
        Begin
            Write(Head^.Data:3);
            Head := Head^.Next;
        End;
    Writeln;
End;

Var
    Head: TPElem;
begin
    Var Gap := GetNumberOfGap();
    For Var Data := 1 to 64 do
        Begin
            Head := CreateRoundList(Data);
            WriteFirstList(Head, Data);
            CountDown(Head, Gap);
            Writeln('Количество игроков: ', Data:4, '':4, 'Победитель: ', Head^.Data:4);
            Writeln;
        End;

    Readln;
end.
