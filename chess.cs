using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBitboard{

    public class Chess{

        // public const string columnLetters = "hgfedcba";

        static void Main (){

            // UInt64 bKingBoard = 0x800000000000000;
            // UInt64 bQueenBoard = 0x1000000000000000;
            // UInt64 bRookBoard = 0x8100000000000000;
            // UInt64 bBishopBoard = 0x2400000000000000;
            // UInt64 bKnightBoard = 0x4200000000000000;
            // UInt64 bPawnBoard = 0xFF000000000000;
            // UInt64 wKingBoard = 0x8;
            // UInt64 wQueenBoard = 0x10;
            // UInt64 wRookBoard = 0x81;
            // UInt64 wBishopBoard = 0x24;
            // UInt64 wKnightBoard = 0x42;
            // UInt64 wPawnBoard = 0xFF00;

            // UInt64 occupied_White = wKingBoard | wQueenBoard | wRookBoard | wBishopBoard | wKnightBoard | wPawnBoard; // OR all white bitmaps to get all squares occupied by white
            // UInt64 occupied_Black = bKingBoard | bQueenBoard | bRookBoard | bBishopBoard | bKnightBoard | bPawnBoard; // OR all black bitmaps to get all squares occupied by black
            // UInt64 occupied_Squares = occupied_Black | occupied_White;

            // UInt64 bitPattern = 0;
            // UInt64 bitCheck;
            // convertInt2Bits(wKnightBoard, "black king");

            // shifting white pawns by 7 = attack right. shitft 9 to find all attack spaces to left.
            // combining these gets where every white pawn attack in a bitboard.
            //
            //
            // convertInt2Bits(bRookBoard,  "piece");


        }

        // static void moveKnight(){
        //     //to check if posistion is clear
        //     bitCheck = 1u << 21;

        //     convertInt2Bits(occupied_Squares, "All squares");

        //     if ((bitCheck & occupied_Squares) == 0){
        //         // create posistion to move to
        //         bitPattern = 1u << 21; // 1u << (int)(value -1u)
        //         // create which knight to move
        //         bitPattern = bitPattern + (1u << 6);
        //         convertInt2Bits(bitPattern, "bitpattern");
        //         //XOR the bitpattern with occupied_Squares to move
        //         wKnightBoard = wKnightBoard ^ bitPattern;
        //         occupied_Squares = occupied_Squares | wKnightBoard;
        //     }

        //     convertInt2Bits(occupied_Squares, "All squares");
        //     convertInt2Bits(, "All squares");
        // }




        static void convertInt2Bits(UInt64 decimalNumber, string piece){
            UInt64 remainder;
            string result = string.Empty;
            char[] bing = new char[64];
            while (decimalNumber > 0)
            {
                remainder = decimalNumber % 2;
                decimalNumber /= 2;
                result = remainder.ToString() + result;
            }
            int pad = (64 - result.Length) + result.Length;
            result = result.PadLeft(pad, '0');
            Console.WriteLine(result);
            result = Reverse(result);
            Console.WriteLine(piece);


            for(int i = 56; i >= 0; i-=8){
                for(int k = i; k < 64; k++){
                    Console.Write(result[k]);
                    if(k==7 || k==15 || k==23 || k==31 || k==39 || k==47 || k==55 || k==63){
                        Console.Write("\n");
                        break;
                    }
                }
            }
            Console.Write("\n");
        }

        public static string Reverse( string s ){
            char[] charArray = s.ToCharArray();
            Array.Reverse( charArray );
            return new string( charArray );
        }
    }

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
        public const char eC = '\u265f';


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
                {bRC,bNC,bBC,bQC,bKC,bBC,bNC,bRC},
                {bPC,bPC,bPC,bPC,bPC,bPC,bPC,bPC},
                {eC,eC,eC,eC,eC,eC,eC,eC},
                {eC,eC,eC,eC,eC,eC,eC,eC},
                {eC,eC,eC,eC,eC,eC,eC,eC},
                {eC,eC,eC,eC,eC,eC,eC,eC},
                {wPC,wPC,wPC,wPC,wPC,wPC,wPC,wPC},
                {wRC,wNC,wBC,wQC,wKC,wBC,wNC,wRC},
            };
            array2Bitboard(chessBoard, bKB, bQB, bRB, bBB, bNB, bPB, wKB, wQB, wRB, wBB, wNB, wPB);
        }

        public static void array2Bitboard(char[,] chessBoard, UInt64 bKB, UInt64 bQB, UInt64 bRB, UInt64 bBB, UInt64 bNB, UInt64 bPB, UInt64 wKB, UInt64 wQB, UInt64 wRB, UInt64 wBB, UInt64 wNB, UInt64 wPB){
            string binary;
            // for each of the squares set to 64 zeroes
            // then it places a one at the index of i (0-64) starting from left to right
            for (int i = 0; i<64;i++){
                binary = "0000000000000000000000000000000000000000000000000000000000000000";
                binary = binary.Substring(int startIndex, int length);

                switch(chessBoard[i/8,i%8]){

                }
            }
        }
    }
    }
