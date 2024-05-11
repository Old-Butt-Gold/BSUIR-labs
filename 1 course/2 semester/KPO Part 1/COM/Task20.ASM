;Дана C-строка состоящая из пробелов и букв. Заменить после пробелов маленькие буквы на заглавные буквы
        org     100h

EntryPoint:

        push    Task
        call    outputType
        push    SkipLine
        call    outputType

        push    Str1
        Call    findAnswer



        push    Str1
        call    GetSize
        pop     di

        mov     cx, ax

@@:
        mov     ah, 02h
        mov     dx, [di]
        int     21h
        inc     di
        loop    @B

        mov     ax, 0C01h
        int     21h
        ret


findAnswer:
         push   bp
         mov    bp, sp
         mov    di, [bp + 4]
         ;cmp      cx, 0 ;Если пустая строка и пользователь ебанашка
         ;jz       @End
         push      di
         call      GetSize
         pop       di
         mov       cx, ax

         mov       ax, ' '

@@:
         repne      scasb
         jne        @Next
         cmp        Byte[di], 'a'
         jb         @Next
         cmp        Byte[di], 'z'
         ja         @Next
         add        Byte[di], 'A' - 'a'

@Next:
        cmp     cx, 0
        jnz     @B
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
Str1            db    ' str1 onoshko ne ybivay', 0
Str2            db    'str1', 0
