;Найти первое вхождение символа в строку
        org     100h

EntryPoint:

        push    Task
        call    outputType
        push    SkipLine
        call    outputType

        push    WaitForInput
        Call    outputType



        mov     dx, BufferInput
        mov     ah, 0Ah
        int     21h

        push    SkipLine
        call    outputType

        push    InputChar
        call    outputType

        mov     dx, Char
        mov     ah, 0Ah   ;через строку
        int     21h

        ;mov     ah, 01h
        ;int     21h
        ;через встроенную функцию
        ; возвращает в al, однако сразу на новую строку

        push    SkipLine
        Call    outputType

        push    Char
        push    BufferInput

        Call    calculateCounters

        push    bx
        call    outputProcedure
        mov     ax, 0C01h
        int     21h
        ret








calculateCounters:
         push   bp
         mov    bp, sp
         mov    di, [bp + 6]
         movzx    ax, [di + 2]

         xor    bx, bx

         mov    di, [bp + 4]
         movzx    cx, [di + 1]
         add      di, 2

@@:
        repne   scasb
        je      @Found
        jmp     @End

@Found:
        mov     si, [bp + 4]
        add     si, 2
        sub     di, si
        mov     bx, di

@End:
        pop     bp
        ret     4




outputType:
        push    bp
        mov     bp, sp
        mov     dx, [bp + 4]
        mov     ah, 09h
        int     21h
        pop     bp
        ret     2


outputProcedure:
        push    bp
        mov     bp, sp
        movzx     ax, [bp+4]
        test  al, al   ; проверка на знак
        jns   Unsigned ;jump not signed
        mov   cl, al
        mov   ah, 02h
        mov   dx, '-'
        int   21h
        mov   al, cl
        neg   ax

Unsigned:
        xor     cx, cx
pushing:
        xor     ah, ah
        xor     bx, bx
        div     [Ten]
        mov     bl, ah
        add     bl, '0'
        push    bx
        inc     cx

        test    al, al
        jnz     pushing
 
poping:
        pop     dx
        mov     ah, 02h
        int     21h
        loop    poping
        pop     bp
        ret     2



Task    db      'This programm will find how many times symbol is in input string$'
WaitForInput     db 'Enter your string $'
InputChar        db 'Enter your char $'

SkipLine        db    0Ah, 0Dh, '$'
BufferInput     db   100, 0, 100 dup(?)
; + 1 будет длина текущая, остальные 4 все символы
Char            db   2, 0, 2 dup(?) ;или же так задавать ввод char или через 01h
Ten             db   10
