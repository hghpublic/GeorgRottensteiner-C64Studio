#RetroDevStudio.MetaData.BASIC:2049,BASIC V2,uppercase,10,10
13 X=2:ONXGOTO17,18,19
14 REM --> LISTING <-- 
15 REM --> IST OK  <--
17 PRINT"1":RUN26:PRINT"1"
18 PRINT"2":GO TO 26:PRINT"2"
19 PRINT"3":GOTO26:PRINT"3"
26 PRINT"OK":IF2=3THENRUN13:PRINT"4"
27 LIST 14 - 15:PRINT"VONBIS"
28 LIST 15 -:PRINT"VON"
29 LIST - 19:PRINT"BIS"