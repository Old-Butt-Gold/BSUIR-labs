%option main

%{
    #include <stdio.h>
    #include <stdlib.h>
%}

%%

[%][0-9A-Fa-f]{1,2}       {
				int index = 0;
				char str[3] = {};
    	    			for (int i = 1; i < 3; i++)
				    if (yytext[i])
				    	str[index++] = yytext[i];
				    else
					break;
    				printf("%c", strtol(str, NULL, 16));
			  }
%%
