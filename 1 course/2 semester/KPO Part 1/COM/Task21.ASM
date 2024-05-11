;Дано число X, найти количество единичных бит в числе. Пример 101011 - ответ 4
        org 100h

Start:
        push    Task
        call    typeProcedure
        push    SkipLine
        call    typeProcedure
        push    [Num]
        call    outputProcedure
        push    SkipLine
        call    typeProcedure

        push    [Num]
        Call    findAnswer
        push    ax
        call    outputProcedure
        push    SkipLine
        call    typeProcedure

        mov     ax, 0C01h
        int     21h
        ret






findAnswer:
        push    bp
        mov     bp, sp
        mov     dx, [bp + 4]
        xor     ax, ax

@@:
        shr     dx, 1
        adc     ax, 0 ;carry flag содержит бит, который был сдвинут
        test    dx, dx
        jnz     @B

        pop bp
        ret 2


PrintArray:
        push    bp
        mov     bp, sp
        mov     si, [bp + 4]
        mov     cx, [si]
PrintLoop:
        add     si, 2
        push    cx
        mov     bx, [si]
        push    bx
        Call    outputProcedure
        mov     ah, 02h
        mov     dx, ' '
        int     21h
        pop     cx
        Loop    PrintLoop
        pop     bp
        ret     2

typeProcedure:
        push    bp
        mov     bp, sp
        mov     dx, [bp+4]
        mov     ah, 09h
        int     21h
        pop     bp
        ret     2

outputProcedure:
        push    bp
        mov     bp, sp
        mov     ax, [bp + 4]
        test  ax, ax   ; проверка на знак
        jns   Unsigned ;jump not signed
        mov   cx, ax
        mov   ah, 02h
        mov   dx, '-'
        int   21h
        mov   ax, cx
        neg   ax

Unsigned:
        xor     cx, cx
pushing:
        xor     dx, dx
        div     [Ten]
        add     dx, '0'
        push    dx
        inc     cx

        test    ax, ax
        jnz     pushing
 
poping:
        pop     dx
        mov     ah, 02h
        int     21h
        loop    poping
        pop     bp
        ret     2


Task    db      'Sum of even numbers$'
SkipLine        db 0Ah, 0Dh, '$'
Ten             dw 10
Num             dw 5