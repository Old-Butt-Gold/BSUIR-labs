library libInput;

{ Important note about DLL memory management: ShareMem must be the
  first unit in your library's USES clause AND your project's (select
  Project-View Source) USES clause if your DLL exports any procedures or
  functions that pass strings as parameters or function results. This
  applies to all strings passed to and from your DLL--even those that
  are nested in records and classes. ShareMem is the interface unit to
  the BORLNDMM.DLL shared memory manager, which must be deployed along
  with your DLL. To avoid using BORLNDMM.DLL, pass string information
  using PChar or ShortString parameters. }

uses
  System.SysUtils,
  System.Classes;

{$R *.res}

Function ChooseConversationType: Integer; StdCall;
var
    IsCorrect: Boolean;
    ConversionType: Integer;
begin
    ConversionType := 0;
    Writeln('Выберите вид преобразования.');
    Writeln('1. Из инфиксной формы записи в постфиксную форму представления.');
    Writeln('2. Из инфискной формы записи в префиксную форму представления');
    repeat
        Write('Вид преобразования: ');
        IsCorrect := True;
        try
            Readln(ConversionType);
            case ConversionType of
                1: Writeln('Вы выбрали преобразование из инфиксной формы записи в ',
                    'постфиксную форму представления.');
                2: Writeln('Вы выбрали преобразование из инфиксной формы записи в ',
                    'префиксную форму представления.');
                else
                begin
                    IsCorrect := False;
                    Writeln('Ошибка: проверьте правильность ввода данных.');
                end;
            end;
        except
            IsCorrect := False;
            Writeln('Ошибка: проверьте правильность ввода данных.');
        end;
    until IsCorrect;
    Result := ConversionType;
end;

function IsExpressionNotEmpty(Expression: string): Boolean;
begin
    Result := Length(Expression) > 0;
end;

function IsExpressionBracketBalanced(Expression: string): Boolean; StdCall;
var
    Counter: Integer;
begin
    Counter := 0;
    for Var I := 1 to Length(Expression) do
    begin
        case Expression[I] of
            '(': Inc(Counter);
            ')': Dec(Counter);
        end;
    end;
    Result := Counter = 0;
end;

function IsExpressionNotContainsIllegalCharacters(Expression: string): Boolean;  StdCall;
var
    I: Integer;
    IsCorrect: Boolean;
begin
    IsCorrect := True;
    for I := 1 to Length(Expression) do
        begin
            if not (Expression[I] in ['a'..'z', '+', '-', '*', '/', '(', ')', '^']) then
            begin
                IsCorrect := False;
                Break;
            end;
        end;
    Result := IsCorrect;
end;

function InputExpression: string;      StdCall;
var
    IsCorrect: Boolean;
    Expression: string;
begin
    Writeln('Введите выражение, которые необходимо преобразовать.');
    Writeln('Допустимые символы: латинские символы в нижнем регистре,',
     ' +, -, *, /, ^, (, ). Выражение вводится без пробелов. Между буквенными ',
     'символами должен находиться знак операции. В выражении должен ',
     'выполняться скобочный баланс.');
    repeat
        Write('Выражение: ');
        Readln(Expression);
        IsCorrect := IsExpressionBracketBalanced(Expression) and IsExpressionNotContainsIllegalCharacters(Expression) and IsExpressionNotEmpty(Expression);
        if not IsCorrect then
            Writeln('Ошибка: проверьте правильность ввода данных.');
    until IsCorrect;
    Result := Expression;
end;

exports InputExpression, IsExpressionNotContainsIllegalCharacters, IsExpressionBracketBalanced, IsExpressionNotEmpty, ChooseConversationType;

begin
end.
