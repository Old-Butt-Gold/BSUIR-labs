program project1_1;

{$APPTYPE CONSOLE}

uses
  SysUtils,
  Math,
  Windows;

type
    Pt = ^TElement;
    TElement = record
        Coefficient: Integer;
        Degree: Integer;
        Next: Pt;
    end;

function Meaning(P: Pt; X: Integer): Integer;
Var
    Sum: Integer;
begin
    Sum := 0;
    while P^.Next <> nil do  //P
    begin
        Inc(Sum, P^.Coefficient * Round(Power(X, P^.Degree)));
        P := P^.Next;
    end;
    Meaning := Sum;
end;

function Equality(P, Q: Pt): Boolean;
var
    IsEquality: Boolean;
begin
    IsEquality := True;
    while ((P^.Next <> nil) or (Q^.Next <> nil)) and IsEquality do   //P <> Nil or Q <> Nil
    begin
        if (P^.Next = nil) or (Q^.Next = nil) then  //P = Nil or Q = Nil
            IsEquality := False
        else
        begin
            if (P^.Degree <> Q^.Degree) or (P^.Coefficient <> Q^.Coefficient) then
                IsEquality := False;
            P := P^.Next;
            Q := Q^.Next;
        end;
    end;
    Equality := IsEquality;
end;

Function Add(P, Q: Pt): Pt;
var
    X, R: Pt;
    //Y: Pt;
begin
    New(X);
    R := X;
    while (P^.Next <> nil) and (Q^.Next <> nil) do  //P <> nil and Q <> Nil
    begin
        //If (P^.Degree = 0) or (Q^.Degree = 0) Then
          //  Y := X;
        if P^.Degree > Q^.Degree then
        begin
            X^.Degree := P^.Degree;
            X^.Coefficient := P^.Coefficient;
            P := P^.Next;
        end
        else If P^.Degree < Q^.Degree then
            begin
                X^.Degree := Q^.Degree;
                X^.Coefficient := Q^.Coefficient;
                Q := Q^.Next;
            end
            else
            begin
                X^.Degree := P^.Degree;
                X^.Coefficient := p^.Coefficient + Q^.Coefficient;
                P := P^.Next;
                Q := Q^.Next;
            end;
        New(X^.Next);
        X := X^.Next;
    end;
    X^.Next := nil; //Y^.Next := Nil;
    Result := R;
end;

Function InputCoefficient(): Integer;
Var
    Coef: Integer;
    IsCorrect: Boolean;
Begin
    Repeat
        IsCorrect := True;
        Try
            Readln(Coef);
        Except
            IsCorrect := False;
        End;
    Until IsCorrect;
    Result := Coef;
End;

function InputList(Degree: Integer): Pt;
var
    Element, X: Pt;
    //Y: Pt;
    I, Coefficient: Integer;
begin
    New(X);
    //Y := X;
    Element := X;
    for I := Degree downto 0 do
    begin
        If I <> 0 then
        begin
            Write('x^', I, ' * ');
            Coefficient := InputCoefficient();
            Write(' + ');
        end
        else
            Coefficient := InputCoefficient();
        If (Coefficient <> 0) or (I = 0) then
        begin
            X^.Coefficient := Coefficient;
            X^.Degree := I;
            New(X^.Next);
            X := X^.Next;
        end;
        //If I = 1 Then
          //Y := X;
    end;
    //Y^.Next := nil;
    X^.Next := nil;
    InputList := Element;
end;


procedure OutputList(List: Pt);
begin
    If List^.Degree <> 0 Then
        Write(List^.Coefficient, 'x^', List^.Degree)
    else
        Write(List^.Coefficient);
    List := List^.Next;
    while List^.Next <> nil do   //List
    begin
        Write(' + ');
        if List^.Degree <> 0 then
            Write(List^.Coefficient, 'x^', List^.Degree)
        else
            Write(List^.Coefficient);
        List := List^.Next;
    end;
    Writeln;
end;

Function InputDegree(): Integer;
Var
    Degree: Integer;
Begin
    Repeat
        Try
            Readln(Degree);
        Except

        End;
    Until (Degree > -1) and (Degree < 16);
    Result := Degree;
End;

Function InputPoint(): Integer;
Var
    X: Integer;
    IsCorrect: Boolean;
Begin
    Repeat
        IsCorrect := true;
        Try
            Readln(X);
        Except
            IsCorrect := False;
        End;
    Until IsCorrect;
    Result := X;
End;

var
    P, Q, R: Pt;
    Degree, X: integer;
begin
    Write('Введите степень многочлена P(x): ');
    Degree := InputDegree;
    P := InputList(Degree);

    Write('Введите степень многочлена Q(x): ');
    Degree := InputDegree;
    Q := InputList(Degree);

    Write('Многочлен P(x) = ');
    OutputList(P);

    Write('Многочлен Q(x) = ');
    OutputList(Q);

    R := Add(P, Q);
    Write('R(x) = P(X) + Q(X) = ');
    OutputList(R);

    Writeln('Q(X) = P(X)? ', Equality(P, Q));
    Write('Введите значение X: ');
    X := InputPoint();
    Writeln('Q(X) = ', Meaning(Q, X));
    Writeln('P(X) = ', Meaning(P, X));
    Writeln('R(X) = ', Meaning(R, X));
    Readln;
end.
