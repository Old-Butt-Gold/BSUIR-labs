;ƒана C-строка. ќпределить, €вл€етс€ значение этой строки записью целого беззнакового дес€тичного числа (могут быть пробелы до и после него).
;¬ернуть 1, если €вл€етс€. ¬ернуть 0, если не €вл€етс€
        org     100h

EntryPoint:

        push    Task
        call    outputType
        push    SkipLine
        call    outputType

        push    Str1
        Call    findAnswer

        push    ax
        call    outputProcedure

        mov     ax, 0C01h
        int     21h
        ret


findAnswer:
         push   bp
         mov    bp, sp
         mov    di, [bp + 4]
         push      di
         call      GetSize
         pop       di
         mov       cx, ax
         xor       ax, ax

@Start1:
         cmp    cx, 0
         je     @End

         cmp    Byte[di], ' '
         jne    @Start2
         inc    di
         loop    @Start1

@Start2:

         cmp    Byte[di], ' '
         je     @Start3

         cmp    Byte[di], '0'
         jb     @End
         cmp    Byte[di], '9'
         ja     @End
         inc    di
         Loop   @Start2


@Start3:
         cmp    cx, 0
         je     @F
         cmp    Byte[di], ' '
         jne    @End
         inc    di
         loop    @Start3

@@:
         inc     ax



@End:
         pop     bp
         ret     2




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
Str1            db    '   100   ', 0
Str2            db    'str1', 0
