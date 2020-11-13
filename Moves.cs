using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBitboard{

    public class Moves{

        public static UInt64 fileA = 0x101010101010101; // 1 on every field on A file
        public static UInt64 fileH = 0x8080808080808080; // 1 on H file
        public static UInt64 fileAB = 0x303030303030303; // 1 on file A & B
        public static UInt64 fileGH = 0xC0C0C0C0C0C0C0C0; // 1 on file H & H
        public static UInt64 rank1 = 0xFF00000000000000;
        public static UInt64 rank4 = 0xFF00000000;
        public static UInt64 rank5 = 0xFF000000;
        public static UInt64 rank8 = 0xFF;
        public static UInt64 center = 0x1818000000;
        public static UInt64 centerBig = 0x3C3C3C3C0000;
        public static UInt64 kingSide = 0xF0F0F0F0F0F0F0F;
        public static UInt64 queenSide = 0xF0F0F0F0F0F0F0F0;
        public static UInt64 notWhitePieces; // every piece that white can capture// every black piece except for black king
        public static UInt64 blackPieces; //black pieces except for black king
        public static UInt64 empty; // every field empty
        public static UInt64 test = 0x8001000000000000;
        public static UInt64[] Rankmasks8 = new UInt64[] { 0xFF,0xFF00,0xFF0000,0xFF000000,0xFF00000000,0xFF0000000000,0xFF000000000000,0xFF00000000000000};
        public static UInt64[] Filemasks8 = new UInt64[] { 0x101010101010101,0x202020202020202,0x404040404040404,0x808080808080808,0x1010101010101010,0x2020202020202020,0x4040404040404040,0x8080808080808080};

        public static string possibleMovesW(string hist, UInt64 bKB, UInt64 bQB, UInt64 bRB, UInt64 bBB, UInt64 bNB, UInt64 bPB, UInt64 wKB, UInt64 wQB, UInt64 wRB, UInt64 wBB, UInt64 wNB, UInt64 wPB){
            notWhitePieces=~(wKB|wQB|wRB|wNB|wBB|wPB|bKB); // or'd every white piece and black king to indicate what the white pieces cant capture, also black king to avoid illegal capture
            blackPieces = bQB|bRB|bNB|bBB|bPB; // all the black pieces without king to avoid illegal capture
            empty = ~(bKB|bQB|bRB|bBB|bNB|bPB|wKB|wQB|wRB|wBB|wNB|wPB); // indicates empty fields with a 1 with flip ~
            string list=possiblewP(hist,wPB,bPB); // add possible white every other piece

            return list;
        }

        public static int trailingZerosLeft(UInt64 move){
            for(int i = 0; i<64; i++){
                if (((move <<i)&0x8000000000000000)==0x8000000000000000) return i;
            }
            return 0;
        }
        public static int trailingZerosRight(UInt64 move){
            for(int i = 0; i<64; i++){
                if (((move >>i)&1)==1) return i;
            }
            return 0;
        }


        public static string possiblewP(string hist, UInt64 wPB, UInt64 bPB){
            string list = "";

            //x1,y1,x2,y2
            UInt64 pMoves = (wPB>>7) & blackPieces & ~rank8 & ~fileA; // shift everything to the right by 7 to indicate capture right, and there is black piece and not on rank8 and not file H to stop capture one the other side of board
            /*  gets the first pawn-bit-move and puts into possibleMoves (if any) in a bitboard alone to put into moves list:
             *  possibleMoves =
             *  pMove = 10100011
             * -1 ~   = 01011101
             *  &     = 00000001
             * then later the tested pawn-bit is removed like so:
             *  possibleMoves =
             *  pMove = 10100011
             *   & ~  = 11111110
             *        = 10100010 == moved that was put into list is removed others remain
             *        */
            UInt64 possibleMoves=pMoves&~(pMoves-1);
            while (possibleMoves != 0){
                int index = trailingZerosRight(possibleMoves); // puts the index to the first pawn by counting number of 0's to the right
                list += "" + (index/8+1) + (index%8-1) + (index/8) + (index%8); //add move cords to list // first index +1 to get back to startlocation in the internal rank representation
                pMoves &= ~(possibleMoves); // the listed move is removed from pMoves
                possibleMoves = pMoves & ~(pMoves-1); // gets the next move alone in a bitboard
            }

            pMoves = (wPB>>9) & blackPieces & ~rank8 & ~fileH; // shift everything to the right by 9 to indicate capture left, and there is black piece and not on rank8 and not file A to stop capture on the other side of board
            possibleMoves=pMoves&~(pMoves-1);
            while (possibleMoves != 0){
                int index = trailingZerosRight(possibleMoves);
                list += "" + (index/8+1) + (index%8+1) + (index/8) + (index%8); //add move cords to list
                pMoves &= ~(possibleMoves);
                possibleMoves = pMoves & ~(pMoves-1);
            }

            pMoves = (wPB>>8) & empty & ~rank8; // shift everything to the right by 8 to indicate move forward by one, and there is empty field and not on rank8
            possibleMoves=pMoves&~(pMoves-1);
            while (possibleMoves != 0){
                int index = trailingZerosRight(possibleMoves);
                list += "" + (index/8+1) + (index%8) + (index/8) + (index%8); //add move cords to list
                pMoves &= ~(possibleMoves);
                possibleMoves = pMoves & ~(pMoves-1);
            }

            pMoves = (wPB>>16) & empty & (empty>>8) & rank4; // shift everything to the right by 16 to indicate move forward and field is empty and field infront is empty and it is rank 4(meaning only when rank2 posistion can it move this way)
            possibleMoves=pMoves&~(pMoves-1);
            while (possibleMoves != 0){
                int index = trailingZerosRight(possibleMoves);
                list += "" + (index/8+2) + (index%8) + (index/8) + (index%8); //add move cords to list
                pMoves &= ~(possibleMoves);
                possibleMoves = pMoves & ~(pMoves-1);
            }
            // y1, y2, promotype, "P"
            pMoves = (wPB>>7) & blackPieces & rank8 & ~fileA; // shift everything to the right by 7 to indicate capture right, and there is black piece and not on rank8 and not file H to stop capture one the other side of board
            possibleMoves=pMoves&~(pMoves-1);
            while (possibleMoves != 0){
                int index = trailingZerosRight(possibleMoves);
                list += "" + (index%8-1) + (index%8) + "QP" + (index%8-1) + (index%8) + "RP" + (index%8-1) + (index%8) + "NP" + (index%8-1) + (index%8) + "BP";
                pMoves &= ~(possibleMoves);
                possibleMoves = pMoves & ~(pMoves-1);
            }

            pMoves = (wPB>>9) & blackPieces & rank8 & ~fileH; // shift everything to the right by 9 to indicate capture left, and there is black piece and not on rank8 and not file A to stop capture on the other side of board
            possibleMoves=pMoves&~(pMoves-1);
            while (possibleMoves != 0){
                int index = trailingZerosRight(possibleMoves);
                list += "" + (index%8+1) + (index%8) + "QP" + (index%8+1) + (index%8) + "RP" + (index%8+1) + (index%8) + "NP" + (index%8+1) + (index%8) + "BP";
                pMoves &= ~(possibleMoves);
                possibleMoves = pMoves & ~(pMoves-1);
            }

            pMoves = (wPB>>8) & empty & rank8; // shift everything to the right by 8 to indicate move forward by one, and there is empty field and not on rank8
            possibleMoves=pMoves&~(pMoves-1);
            while (possibleMoves != 0){
                int index = trailingZerosRight(possibleMoves);
                list += "" + (index%8) + (index%8) + "QP" + (index%8) + (index%8) + "RP" + (index%8) + (index%8) + "NP" + (index%8) + (index%8) + "BP";
                pMoves &= ~(possibleMoves);
                possibleMoves = pMoves & ~(pMoves-1);
            }
            // y1, y2 E
            // DONE: maybe for later test if hist length in var is faster?
            // hist = "1636";
            /*
            if (hist.Length >=4) //1636
            { // last digit is equal to 3rd last digit, meaning same file. And that 2nd last digit and 4th last digit are 2 apart
                if ((hist[hist.Length-1] == hist[hist.Length-3]) && (hist[hist.Length-2] - hist[hist.Length-4] == 2)){
                    int enpassantFile = hist[hist.Length-1] - '0'; // convert last digit in hist - faster convert by subtracting with ascii code // works up to decimal 9 - I need to 7
                    // en passant to the right
                    possibleMoves = (wPB << 1) & bPB & rank5 & ~fileA & Filemasks8[enpassantFile]; // piece location to move, no destination put in hist - does not try and grab moves one by one because there should only be one possible per round
                    if (possibleMoves != 0){
                        int index = trailingZerosRight(possibleMoves);
                        list += "" + (index%8-1) + (index%8) + " E";
                    }
                    // en passant to the left
                    possibleMoves = (wPB >> 1) & bPB & rank5 & ~fileH & Filemasks8[enpassantFile];// piece to move no destination put in hist
                    if (possibleMoves != 0){
                        int index = trailingZerosRight(possibleMoves);
                        list += "" + (index%8+1) + (index%8) + " E";
                    }
                }
            }
*/

            if (hist.Length >=4) //1636
            { // last digit is equal to 3rd last digit, meaning same file. And that 2nd last digit and 4th last digit are 2 apart
            int histLen = hist.Length; // trying to replace hist.Length because its called 6 times
                if ((hist[histLen-1] == hist[histLen-3]) && (hist[histLen-2] - hist[histLen-4] == 2)){
                    int enpassantFile = hist[histLen-1] - '0'; // convert last digit in hist - faster convert by subtracting with ascii code // works up to decimal 9 - I need to 7
                    // en passant to the right
                    possibleMoves = (wPB << 1) & bPB & rank5 & ~fileA & Filemasks8[enpassantFile]; // piece location to move, no destination put in hist - does not try and grab moves one by one because there should only be one possible per round
                    if (possibleMoves != 0){
                        int index = trailingZerosRight(possibleMoves);
                        list += "" + (index%8-1) + (index%8) + " E";
                    }
                    // en passant to the left
                    possibleMoves = (wPB >> 1) & bPB & rank5 & ~fileH & Filemasks8[enpassantFile];// piece to move no destination put in hist
                    if (possibleMoves != 0){
                        int index = trailingZerosRight(possibleMoves);
                        list += "" + (index%8+1) + (index%8) + " E";
                    }
                }
            }


            return list;
        }


        /*
        // y1,y2 promotion type, p
        pMoves = (wPB>>7) & blackPieces & rank8 & ~fileA; // capture right and promote pawn at rank 8
        for (int i=0;i<64;i++){
        if (((pMoves>>i)&1)==1){ // search every field on board, every time it finds a bit it adds the position to the list of valid moves
        list+=""+(i/8-1)+(i%8)+"QP"+(i/8)+(i%8); //add move cords to list
        }
        }

        pMoves = (wPB>>8) & empty & rank8; //  move forward and promote
        for (int i=0;i<64;i++){
        if (((pMoves>>i)&1)==1){ // search every field on board, every time it finds a bit it adds the position to the list of valid moves
        list+=""+(i/8+1)+(i%8)+(i/8)+(i%8); //add move cords to list
        }
        }

        pMoves = (wPB>>9) & empty & blackPieces & rank8 & ~fileH; // capture left and promote pawn at rank8
        for (int i=0;i<64;i++){
        if (((pMoves>>i)&1)==1){ // search every field on board, every time it finds a bit it adds the position to the list of valid moves
        list+=""+(i/8+2)+(i%8)+(i/8)+(i%8); //add move cords to list

        }
        }
        */

        // public static void possibleMovesB(string hist, UInt64 bKB, UInt64 bQB, UInt64 bRB, UInt64 bBB, UInt64 bNB, UInt64 bPB, UInt64 wKB, UInt64 wQB, UInt64 wRB, UInt64 wBB, UInt64 wNB, UInt64 wPB){

        // }



        // public static void newGame(){


        //     BoardGeneration.initiateStdChess();

        // }


    }

}


/* Very bad search
   public static string possiblewPOptimize1(string hist, UInt64 wPB){
   string list = "";

   //x1,y1,x2,y2
   UInt64 pMoves = (wPB>>7) & blackPieces & ~rank8 & ~fileH; // shift everything to the right by 7 to indicate capture right, and there is black piece and not on rank8 and not file H to stop capture one the other side of board
   for (int i = trailingZerosRight(pMoves); i < 64-trailingZerosLeft(pMoves); i++){
   if (((pMoves>>i)&1)==1){ // search every field on board, every time it finds a bit it adds the position to the list of valid moves
   list+=""+(i/8+1)+(i%8-1)+(i/8)+(i%8); //add move cords to list
   }
   }
   return list;
   }
*/

/* Goes through every field on board - seems to not beat current pawn moves method
   public static string possiblewPnoOptimize(string hist, UInt64 wPB){
   string list = "";

   //x1,y1,x2,y2
   UInt64 pMoves = (wPB>>7) & blackPieces & ~rank8 & ~fileH; // shift everything to the right by 7 to indicate capture right, and there is black piece and not on rank8 and not file H to stop capture one the other side of board
   for (int i=0;i<64;i++){
   if (((pMoves>>i)&1)==1){ // search every field on board, every time it finds a bit it adds the position to the list of valid moves
   list+=""+(i/8+1)+(i%8-1)+(i/8)+(i%8); //add move cords to list
   }
   }
   return list;
   }
*/
