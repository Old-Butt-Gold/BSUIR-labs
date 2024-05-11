;дано 2 массива 10 элементов, поменять элементы массива с равными индексами
        org 100h

Start:
        push    Task
        call    typeProcedure
        push    SkipLine
        call    typeProcedure
        push    Arr1
        Call    PrintArray
        push    SkipLine
        call    typeProcedure
        push    Arr2
        call    PrintArray
        push    SkipLine
        call    typeProcedure

        push    Arr1
        push    Arr2
        Call    findAnswer


        push    Arr1
        Call    PrintArray
        push    SkipLine
        call    typeProcedure
        push    Arr2
        call    PrintArray
        push    SkipLine
        call    typeProcedure

        mov     ax, 0C01h
        int     21h
        ret






findAnswer:
        push    bp
        mov     bp, sp
        mov     si, [bp + 6] ; 1 arr
        mov     di, [bp + 4] ; 2 arr
        mov     cx, [si]

Cycle:
        add     si, 2
        add     di, 2
        push    Word[di]
        mov     ax, [si]
        mov     [di], ax
        pop     Word[si]
        Loop    Cycle

        pop bp
        ret 4


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
Counter            dw 9
Arr1            dw 10, 1, 2, 3, 4, 5, 6, 7, 15, -19, 5
Arr2            dw 10, 10, 234, 345, 34, 56, 78, 2, 3, 4, 5
Answer          db     'Your answer is: $'