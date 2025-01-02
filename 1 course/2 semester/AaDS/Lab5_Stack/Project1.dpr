program Project1;

{$APPTYPE CONSOLE}

{$R *.res}

uses
  System.SysUtils, StrUtils;

Type
    PElem=^Elem;
      Elem = Record
        Data: Char;
        Next: PElem;
      End;

    IntList = record
        FirstElem: PElem;
        Length: Integer;
        Rang: Integer;
    end;

function IsExpressionNotEmpty(Expression: string): Boolean; StdCall;
External 'libInput.dll';

function IsExpressionBracketBalanced(Expression: string): Boolean; StdCall;
External 'libInput.dll';

function IsExpressionNotContainsIllegalCharacters(Expression: string): Boolean; StdCall;
External 'libInput.dll';

function InputExpression: string; StdCall;
External 'libInput.dll';

Function IsEmptyStack(Stack: IntList): boolean;
begin
    result := Stack.FirstElem = nil;
end;

Function Pop(Var Stack: IntList): Char;
begin
    If IsEmptyStack(Stack) then
        Result := #0
    else
        begin
            Result := Stack.FirstElem^.Data;
            Dec(Stack.Length);
            Stack.FirstElem := Stack.FirstElem^.next;
        end;
end;

Function Peek(Stack: IntList): Char;
Begin
   If IsEmptyStack(Stack) Then
      Result := #0
   else
      Result := Stack.FirstElem^.Data;
End;

Procedure Push(var Stack: IntList; Data: Char);
var
    NewElem: PElem;
begin
    New(NewElem);
    NewElem^.Next := Stack.FirstElem;
    Stack.FirstElem := NewElem;
    Stack.FirstElem^.Data := Data;
    Inc(Stack.Length);
end;

function GetStackPriority(Character: Char): Integer;
begin
    case Character of
        '+', '-': Result := 2;
        '/', '*': Result := 4;
        '(': Result := 0;
        'a'..'z': Result := 8;
        '^': Result := 5;
    end;
end;

function GetPriority(Character: Char): Integer;
begin
    case Character of
        '+', '-': Result := 1;
        '/', '*': Result := 3;
        '(': Result := 9;
        'a'..'z': Result := 7;
        '^': Result := 6;
    end;
end;

Procedure СalculationOfRang(Var Stack: IntList);
Begin
    If Peek(Stack) in ['a'..'z'] then
        Inc(Stack.Rang)
    else
        Dec(Stack.Rang);
End;

function Reverse(Expression: string): string;
var
    Output: string;
begin
    Output := AnsiReverseString(Expression);
    For Var I := 1 to Length(Output) do
        If Output[I] = '(' Then Output[I] := ')'
        Else If Output[I] = ')' Then Output[I] := '(';
    Result := Output;
end;

Function ConvertToSuffix(Var Stack: IntList; Expression: String): String;
Var
    FinalAnswer: String;
Begin
    Stack.Rang := 0;
    Push(Stack, Expression[1]);
    FinalAnswer := '';
    For Var I := 2 to Length(Expression) do
        Begin
            If Expression[I] = ')' Then
                Begin
                    While Peek(Stack) <> '(' do
                        Begin
                            СalculationOfRang(Stack);
                            FinalAnswer := Concat(FinalAnswer, Pop(Stack));
                        End;
                    Pop(Stack);
                End
            Else
                Begin
                    If GetPriority(Expression[I]) > GetStackPriority(Peek(Stack)) Then
                        Push(Stack, Expression[I])
                    Else
                        Begin
                            While Not(IsEmptyStack(Stack)) and (GetPriority(Expression[I]) <= GetStackPriority(Peek(Stack))) do
                                Begin
                                    СalculationOfRang(Stack);
                                    FinalAnswer := Concat(FinalAnswer, Pop(Stack));
                                End;
                            Push(Stack, Expression[I]);
                        End;
                End;
        End;
    While Stack.FirstElem <> Nil do
        Begin
            СalculationOfRang(Stack);
            FinalAnswer := Concat(FinalAnswer, Pop(Stack));
        End;
    Result := FinalAnswer;
End;

Var
    Stack: IntList;
begin
    Var Expression := InputExpression;
    Writeln('Ваше суффиксное выражение равно = ', ConvertToSuffix(Stack, Expression));
    Writeln('Ваш ранг равен = ', Stack.Rang);
    Writeln('Ваше префиксное выражение равно = ', Reverse(ConvertToSuffix(Stack, Reverse(Expression))));
    Writeln('Ваш ранг равен = ', Stack.Rang);
    Readln;
end.
