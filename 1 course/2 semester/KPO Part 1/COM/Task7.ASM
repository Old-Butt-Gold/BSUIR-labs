;����� ����� ��������� � �������
;����� ��������� � �������, ������� ������� � ������� 4
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
        push    [Arr1]
        push    [Elem]
        Call    findAnswer

        push    Arr1
        Call    PrintArray

        mov     ax, 0C01h
        int     21h
        ret






findAnswer:
        push    bp
        mov     bp, sp
        xor     dx, dx
        xor     bx, bx
        mov     si, [bp + 8]  ;����� ������
        mov     cx, [bp + 6]
        mov     ax, [bp + 4]

Cycle:
        add     si, 2
        inc     bx
        cmp     [si], ax
        jge     @Next
        add     [si], bx

@Next:
        Loop    Cycle

        pop bp
        ret 6


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


Task    db      'This program will find all similar elements in two arrays$'
SkipLine        db 0Ah, 0Dh, '$'
Ten             dw 10
Elem            dw 8
Arr1            dw 10, 1, 2, 3, 4, 5, 6, 7, 8, 5, 5
Answer          db     'Your answer is: $'