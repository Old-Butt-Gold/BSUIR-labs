;«аменить все строчные буквы на заглавные
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

        push    BufferInput
        Call    calculateCounters

        push    BufferInput
        Call    WriteString


        mov     ax, 0C01h
        int     21h
        ret



WriteString:
        push    bp
        mov     bp, sp
        xor     dx, dx
        mov     di, [bp + 4]
        inc     di
        movzx     cx, [di]
@Loop:
        inc     di
        mov     ah, 02h
        mov     dl, Byte[di]
        int     21h
        loop    @Loop

        pop     bp
        ret     2




calculateCounters:
         push   bp
         mov    bp, sp
         mov    di, [bp + 4]
         movzx    cx, [di + 1]
         ;cmp      cx, 0 ;≈сли пуста€ строка и пользователь ебанашка
         ;jz       @End
         add    di, 2


@Cycle:
        cmp     Byte[di], 'a'
        jb      @F
        cmp     Byte[di], 'z'
        ja      @F

        add     Byte[di], 'A' - 'a'
@@:
        inc     di
        loop    @Cycle
@End:
        pop     bp
        ret     2




outputType:
        push    bp
        mov     bp, sp
        mov     dx, [bp + 4]
        mov     ah, 09h
        int     21h
        pop     bp
        ret     2



Task    db      'This programm will find how many times symbol is in input string$'
WaitForInput     db 'Enter your string $'

SkipLine        db    0Ah, 0Dh, '$'
BufferInput     db   100, 0, 100 dup(?)
; + 1 будет длина текуща€, остальные 4 все символы
