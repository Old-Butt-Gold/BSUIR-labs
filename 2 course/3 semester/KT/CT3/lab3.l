%option main

%%

[ ]	{
	    printf("0\n");
	}

fn	{
	    printf("1\n");
	}

main	{
	    printf("2\n");
	}

[{]	{
	    printf("3\n");
	}

[}]	{
	    printf("4\n");
	}

[(]	{
	    printf("5\n");
	}

[)]	{
	    printf("6\n");
	}

println!	{
		    printf("7\n");
		}

\".*\"	{
	    printf("8\n");
	}

\n|\r\n		{
		    printf("9\n");
		}

;		{
		    printf("10\n");
		}

[0-9]{1,10}  {
		    printf("11\n");
		}

if		{
		    printf("12\n");
		}

else		{
		    printf("13\n");
		}

loop	        {
		    printf("14\n");
		}

%%
