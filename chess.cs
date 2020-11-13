using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessBitboard{

    public class Chess{

        // public const string columnLetters = "hgfedcba";

        public static void Main (){

            Console.WriteLine(Moves.trailingZerosLeft(0x100000000000));
            Console.WriteLine(Moves.trailingZerosRight(0x100000000000));
            BoardGeneration.initiateStdChess();

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
            // assume we have a attack set of a queen and want to know if it attacks opponent pieces it may capture. You AND & the queen attacks with the set of opponent pieces
            //
            //
            // to test whether a is subset of set b
            // you AND & the two sets and if the result is eq == to a, then a is a subset of b (all a bits overlap with some of b's bits)
            //
            // to test if two sets a and b are dijoint you AND & the two sets and if result is eq == 0 then a and b are disjoint sets (no bits overlap)
            //
            // the unioin or disjunction of two bitboards is applied by bitwise or |. The union is superset of the intersection, while the intersection is
            //
            // to test if a bit is a 1 // eks. 0111 = 7. test if 3rd bit is 1. (7>>2)&1 = 1 for true. to test 4th bit (7>>3) = 0 for false
            //
            //
            //
            //

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




    }
}
