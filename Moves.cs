using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBitboard{

    public class Moves{

        public static UInt64 fileA = 0x8080808080808080;
        public static UInt64 fileH = 0x101010101010101;
        public static UInt64 fileAB = 0xC0C0C0C0C0C0C0C0;
        public static UInt64 fileGH = 0x303030303030303;
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

        public static string possibleMovesW(string hist, UInt64 bKB, UInt64 bQB, UInt64 bRB, UInt64 bBB, UInt64 bNB, UInt64 bPB, UInt64 wKB, UInt64 wQB, UInt64 wRB, UInt64 wBB, UInt64 wNB, UInt64 wPB){
            notWhitePieces=~(wKB|wQB|wRB|wNB|wBB|wPB|bKB); // or'd every white piece and black king to indicate what the white pieces cant capture, also black king to avoid illegal capture
            blackPieces = bQB|bRB|bNB|bBB|bPB; // all the black pieces without king to avoid illegal capture
            empty = ~(bKB|bQB|bRB|bBB|bNB|bPB|wKB|wQB|wRB|wBB|wNB|wPB); // indicates empty fields with a 1 with flip ~
            string list=possiblewP(hist,wPB); // add possible white every other piece

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


        public static string possiblewP(string hist, UInt64 wPB){
            string list = "";

            //x1,y1,x2,y2
            UInt64 pMoves = (wPB>>7) & blackPieces & ~rank8 & ~fileH; // shift everything to the right by 7 to indicate capture right, and there is black piece and not on rank8 and not file H to stop capture one the other side of board
            UInt64 possibleMoves=pMoves&~(pMoves-1);
            while (possibleMoves != 0){
                int index = trailingZerosRight(possibleMoves);
                list += "" + (index/8+1) + (index%8-1) + (index/8) + (index%8); //add move cords to list
                pMoves &= ~(possibleMoves);
                possibleMoves = pMoves & ~(pMoves-1);
            }

        /*
          pMoves = (wPB>>9) & blackPieces & ~rank8 & ~fileA; // shift everything to the right by 9 to indicate capture left, and there is black piece and not on rank8 and not file A to stop capture on the other side of board
          for (int i=0;i<64;i++){
          if (((pMoves>>i)&1)==1){ // search every field on board, every time it finds a bit it adds the position to the list of valid moves
          list+=""+(i/8+1)+(i%8+1)+(i/8)+(i%8); //add move cords to list
          }
          }

          pMoves = (wPB>>8) & empty & ~rank8; // shift everything to the right by 8 to indicate move forward by one, and there is empty field and not on rank8
          for (int i=0;i<64;i++){
          if (((pMoves>>i)&1)==1){ // search every field on board, every time it finds a bit it adds the position to the list of valid moves
          list+=""+(i/8+1)+(i%8)+(i/8)+(i%8); //add move cords to list
          }
          }

          pMoves = (wPB>>16) & empty & (empty>>8) & rank4; // shift everything to the right by 16 to indicate move forward and field is empty and field infront is empty and it is rank 4(meaning only when rank2 posistion can it move this way)
          for (int i=0;i<64;i++){
          if (((pMoves>>i)&1)==1){ // search every field on board, every time it finds a bit it adds the position to the list of valid moves
          list+=""+(i/8+2)+(i%8)+(i/8)+(i%8); //add move cords to list
          }
          }
        */
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
        return list;
    }

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
