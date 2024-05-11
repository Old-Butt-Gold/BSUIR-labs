;Сравнить две C-строки. Вернуть 1 если равны, 0 если нет
        org     100h

EntryPoint:

        push    Task
        call    outputType
        push    SkipLine
        call    outputType

        push    Str1
        push    Str2
        Call    findAnswer

        push    ax
        call    outputProcedure

        mov     ax, 0C01h
        int     21h
        ret


findAnswer:
         push   bp
         mov    bp, sp
         mov    di, [bp + 6]
         mov    si, [bp + 4]
         ;cmp      cx, 0 ;Если пустая строка и пользователь ебанашка
         ;jz       @End
         push      di
         call      GetSize
         pop       di
         mov       cx, ax
         push      si
         call      GetSize
         pop       di
         cmp       cx, ax
         je       @Start
         xor      ax, ax
         jmp      @End

@Start:
        repe    cmpsb
        je      Equals
        xor     ax, ax
        jmp     @End


Equals:
        mov     ax, 1

@End:


        pop     bp
        ret     4




GetSize:
        push    bp
        mov     bp, sp
        xor     ax, ax
        mov     di, [bp + 4]

@@:
        cmp     Byte[di], 0
        je      @F
        inc     ax
        inc     di
        jmp     @B

@@:

        pop     bp
        ret

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



Task    db      'This programm will compare two C-strings$'
WaitForInput     db 'Enter your string $'
Ten             db 10
SkipLine        db    0Ah, 0Dh, '$'
Str1            db    'str1', 0
Str2            db    'str1', 0
