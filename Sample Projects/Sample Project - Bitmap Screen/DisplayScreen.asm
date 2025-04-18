USE_HIRES = 1   ;comment out this line to use multicolor

* = $0801

!if 0 {
SCREEN_CHAR     = $0400 ;$a000 - $0400;$0400
BITMAP_LOCATION = $2000 ;$a000
} else {
SCREEN_CHAR     = $9000 - $0400;$0400
BITMAP_LOCATION = $a000
}
!basic
          lda #0
          sta $D020
          sta $D021
          ;lda #$18
          ;location for bitmap and screen char
          lda #( ( BITMAP_LOCATION % $4000 ) / $400 ) + ( ( SCREEN_CHAR % $4000 ) / 1024 ) * 16 + 1
          sta $D018

          ;Enable char/bitmap multicolour
!ifdef USE_HIRES {
          lda #$08
} else {
          lda #$18
}
          sta $D016

          lda #$3B
          sta $D011

          ;VIC bank 2
          lda #$15
          sta $dd00

          ldx #0
.CopyLoop
          lda $4000 + 0 * 250,X
          sta SCREEN_CHAR + 0 * 250,X
          lda $4000 + 1 * 250,X
          sta SCREEN_CHAR + 1 * 250,X
          lda $4000 + 2 * 250,X
          sta SCREEN_CHAR + 2 * 250,X
          lda $4000 + 3 * 250,X
          sta SCREEN_CHAR + 3 * 250,X

          lda $4400 + 0 * 250,X
          sta $D800 + 0 * 250,X
          lda $4400 + 1 * 250,X
          sta $D800 + 1 * 250,X
          lda $4400 + 2 * 250,X
          sta $D800 + 2 * 250,X
          lda $4400 + 3 * 250,X
          sta $D800 + 3 * 250,X

!for ROW = 0 TO 31
          lda $2000 + ROW * 250,x
          sta BITMAP_LOCATION + ROW * 250,x
!end
          inx
          cpx #250
          beq .Done
          jmp .CopyLoop
.Done

EndlessLoop
          jmp EndlessLoop


;bitmap data
* = $2000
!ifdef USE_HIRES {
!media "hires.graphicscreen",BITMAPHIRES
} else {
!media "multicolor.graphicscreen",BITMAP
}

;charscreen colors
* = $4000
!ifdef USE_HIRES {
!media "hires.graphicscreen",SCREEN
} else {
!media "multicolor.graphicscreen",SCREEN
}

;color ram colors
* = $4400
!ifdef USE_HIRES {
!media "hires.graphicscreen",COLOR
} else {
!media "multicolor.graphicscreen",COLOR
}
