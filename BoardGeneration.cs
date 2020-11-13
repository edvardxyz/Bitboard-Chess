using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBitboard{
    public class BoardGeneration{

        public const char bKC = '\u2654';
        public const char bQC = '\u2655';
        public const char bRC = '\u2656';
        public const char bBC = '\u2657';
        public const char bNC = '\u2658';
        public const char bPC = '\u2659';
        public const char wKC = '\u265a';
        public const char wQC = '\u265b';
        public const char wRC = '\u265c';
        public const char wBC = '\u265d';
        public const char wNC = '\u265e';
        public const char wPC = '\u265f';
        public const char eC = '\u0020';


        public static void initiateStdChess(){
            // Initialize array for board
            UInt64 bKB = 0;
            UInt64 bQB = 0;
            UInt64 bRB = 0;
            UInt64 bBB = 0;
            UInt64 bNB = 0;
            UInt64 bPB = 0;
            UInt64 wKB = 0;
            UInt64 wQB = 0;
            UInt64 wRB = 0;
            UInt64 wBB = 0;
            UInt64 wNB = 0;
            UInt64 wPB = 0;

            char[,] chessBoard = new char[,]
            {
                // few pawns
                {bRC,bNC,bBC,bQC,bKC,bBC,bNC,bRC},
                {bPC,bPC,bPC,bPC,bPC,bPC,bPC,bPC},
                {eC,eC,eC,wPC,eC,eC,eC,eC},
                {eC,eC,eC,eC,eC,eC,eC,eC},
                {eC,eC,eC,eC,eC,eC,eC,eC},
                {eC,eC,eC,eC,eC,eC,eC,eC},
                {eC,eC,eC,wPC,eC,eC,eC,eC},
                {wRC,wNC,wBC,wQC,wKC,wBC,wNC,wRC},

                /*
                // Black pawns infront of white pawns and white infront of black
                {bRC,bNC,bBC,bQC,bKC,bBC,bNC,bRC},
                {bPC,bPC,bPC,bPC,bPC,bPC,bPC,bPC},
                {eC,wPC,eC,wPC,eC,wPC,eC,wPC},
                {eC,eC,eC,eC,eC,eC,eC,eC},
                {eC,eC,eC,eC,eC,eC,eC,eC},
                {bPC,bPC,eC,bPC,eC,bPC,eC,bPC},
                {wPC,wPC,wPC,wPC,wPC,wPC,wPC,wPC},
                {wRC,wNC,wBC,wQC,wKC,wBC,wNC,wRC},
                */
               /* // Black pawns infront of white pawns
                {bRC,bNC,bBC,bQC,bKC,bBC,bNC,bRC},
                {bPC,bPC,bPC,bPC,bPC,bPC,bPC,bPC},
                {eC,eC,eC,eC,eC,eC,eC,eC},
                {eC,eC,eC,eC,eC,eC,eC,eC},
                {eC,eC,eC,eC,eC,eC,eC,eC},
                {bPC,bPC,eC,bPC,eC,bPC,eC,bPC},
                {wPC,wPC,wPC,wPC,wPC,wPC,wPC,wPC},
                {wRC,wNC,wBC,wQC,wKC,wBC,wNC,wRC},
            */
            /* // NORMAL CHESS
                {bRC,bNC,bBC,bQC,bKC,bBC,bNC,bRC},
                {bPC,bPC,bPC,bPC,bPC,bPC,bPC,bPC},
                {eC,eC,eC,eC,eC,eC,eC,eC},
                {eC,eC,eC,eC,eC,eC,eC,eC},
                {eC,eC,eC,eC,eC,eC,eC,eC},
                {eC,eC,eC,eC,eC,eC,eC,eC},
                {wPC,wPC,wPC,wPC,wPC,wPC,wPC,wPC},
                {wRC,wNC,wBC,wQC,wKC,wBC,wNC,wRC},
            */

            };
            array2Bitboard(chessBoard, bKB, bQB, bRB, bBB, bNB, bPB, wKB, wQB, wRB, wBB, wNB, wPB);
        }

        public static void array2Bitboard(char[,] chessBoard, UInt64 bKB, UInt64 bQB, UInt64 bRB, UInt64 bBB, UInt64 bNB, UInt64 bPB, UInt64 wKB, UInt64 wQB, UInt64 wRB, UInt64 wBB, UInt64 wNB, UInt64 wPB){
            string binaryString;
            // for each of the squares set to 64 zeroes
            // then it places a one at the index of i (0-64) starting from left to right
            for (int i = 0; i<64;i++){
                binaryString = "0000000000000000000000000000000000000000000000000000000000000000";
                // take a substring of the 64 char first index 1 to end then add 1 and add substring 0 to i.
                // first iteration the last substring will be substing(0, 0) which contains nothing
                binaryString = binaryString.Substring(i +1) + "1" + binaryString.Substring(0, i);

                switch(chessBoard[i/8,i%8]){
                    case bRC:
                        bRB += convertString2Bitboard(binaryString);
                        break;
                    case bNC:
                        bNB += convertString2Bitboard(binaryString);
                        break;
                    case bBC:
                        bBB += convertString2Bitboard(binaryString);
                        break;
                    case bQC:
                        bQB += convertString2Bitboard(binaryString);
                        break;
                    case bKC:
                        bKB += convertString2Bitboard(binaryString);
                        break;
                    case bPC:
                        bPB += convertString2Bitboard(binaryString);
                        break;
                        // white pieces from here
                    case wRC:
                        wRB += convertString2Bitboard(binaryString);
                        break;
                    case wNC:
                        wNB += convertString2Bitboard(binaryString);
                        break;
                    case wBC:
                        wBB += convertString2Bitboard(binaryString);
                        break;
                    case wQC:
                        wQB += convertString2Bitboard(binaryString);
                        break;
                    case wKC:
                        wKB += convertString2Bitboard(binaryString);
                        break;
                    case wPC:
                        wPB += convertString2Bitboard(binaryString);
                        break;
                }
            }
            // drawArray(bKB, bQB, bRB, bBB, bNB, bPB, wKB, wQB, wRB, wBB, wNB, wPB);
            // drawArray(0, 0, 0,0,0,0,0,0,0,0,0,(~Moves.empty));
            string print = Moves.possibleMovesW("", bKB, bQB, bRB, bBB, bNB, bPB, wKB, wQB, wRB, wBB, wNB, wPB);
            Console.WriteLine($":{print}:");


            var sw = System.Diagnostics.Stopwatch.StartNew();
            for(int index = 0; index < 500000; index++)
            {
                Moves.possibleMovesW("", bKB, bQB, bRB, bBB, bNB, bPB, wKB, wQB, wRB, wBB, wNB, wPB);
            }
            sw.Stop();
            var elapsed = sw.ElapsedMilliseconds;
            Console.WriteLine(elapsed);
        }

        public static UInt64 convertString2Bitboard(string binary){
            return Convert.ToUInt64(binary, 2);
        }

        public static void drawArray(UInt64 bKB, UInt64 bQB, UInt64 bRB, UInt64 bBB, UInt64 bNB, UInt64 bPB, UInt64 wKB, UInt64 wQB, UInt64 wRB, UInt64 wBB, UInt64 wNB, UInt64 wPB){

            char[,] chessBoard = new char[8,8];
            for (int i = 0; i < 64; i++){
                chessBoard[i/8,i%8] = eC;
            }
            for (int i = 0; i < 64; i++){
                if ((( bKB >> i ) &1 ) == 1) {
                    chessBoard[i/8,i%8] = bKC;
                }
                if ((( bQB >> i ) &1 ) == 1) {
                    chessBoard[i/8,i%8] = bQC;
                }
                if ((( bRB >> i ) &1 ) == 1) {
                    chessBoard[i/8,i%8] = bRC;
                }
                if ((( bBB >> i ) &1 ) == 1) {
                    chessBoard[i/8,i%8] = bBC;
                }
                if ((( bNB >> i ) &1 ) == 1) {
                    chessBoard[i/8,i%8] = bNC;
                }
                if ((( bPB >> i ) &1 ) == 1) {
                    chessBoard[i/8,i%8] = bPC;
                }
                if ((( wKB >> i ) &1 ) == 1) {
                    chessBoard[i/8,i%8] = wKC;
                }
                if ((( wQB >> i ) &1 ) == 1) {
                    chessBoard[i/8,i%8] = wQC;
                }
                if ((( wRB >> i ) &1 ) == 1) {
                    chessBoard[i/8,i%8] = wRC;
                }
                if ((( wBB >> i ) &1 ) == 1) {
                    chessBoard[i/8,i%8] = wBC;
                }
                if ((( wNB >> i ) &1 ) == 1) {
                    chessBoard[i/8,i%8] = wNC;
                }
                if ((( wPB >> i ) &1 ) == 1) {
                    chessBoard[i/8,i%8] = wPC;
                }
            }
            for (int i = 0; i < 8; i++){
                for (int k = 0; k < 8; k++){
                    Console.Write($"[{chessBoard[i,k]}]\u2009");
                if(k == 7) Console.WriteLine();
                }
                Console.WriteLine();
            }

        }

        public static void drawBitboard(UInt64 bitBoard){
            string[,] chessbit = new string[8,8];
            for (int i = 0; i<64; i++){
                chessbit[i/8,i%8]="";
            }
            for (int i = 0; i<64; i++){
                if (((bitBoard>>i)&1)==1){
                    chessbit[i/8,i%8] = "1";
                }
                if ("".Equals(chessbit[i/8,i%8])){
                    chessbit[i/8,i%8] = " ";
                }

            }
            for (int i = 0; i < 8; i++){
                for (int k = 0; k < 8; k++){
                    Console.Write($"[{chessbit[i,k]}]\u2009");
                if(k == 7) Console.WriteLine();
                }
                Console.WriteLine();
            }

        }

        public static string Reverse( string s ){
            char[] charArray = s.ToCharArray();
            Array.Reverse( charArray );
            return new string( charArray );
        }
    }
}
