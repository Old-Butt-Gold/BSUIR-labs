;�������� �������� ������� ������ � ���������, ������ � �������������...
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

        push    Answer
        Call    typeProcedure

        push    Arr1
        Call    findAnswer

        push    Arr1
        Call    PrintArray

        mov     ax, 0C01h
        int     21h
        ret






findAnswer:
        push    bp
        mov     bp, sp
        mov     si, [bp + 4]
        mov     cx, [si]

Cycle:
        add     si, 2
        dec     cx
        mov     bx, cx
        add     bx, bx

        push    Word[si + bx]
        mov     ax, [si]
        mov     [si + bx], ax
        pop     Word[si]

        cmp     cx, 0
        je      @End
@Next:
        Loop    Cycle

@End:

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
        test  ax, ax   ; �������� �� ����
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
Arr1            dw 10, 1, 2, 3, 4, 5, 6, 7, 15, 19, 5
Answer          db     'Your answer is: $'