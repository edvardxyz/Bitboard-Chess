using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;

namespace ChessBitboard{
    public class Perft{
        public static string move2Algebra(string move){
            string moveString="";
            moveString = moveString + ""+(char)(move[1]+49);
            moveString = moveString + ""+('8'- (move[0]));
            moveString = moveString + ""+(char)(move[3]+49);
            moveString = moveString + ""+('8' - (move[2]));
            return moveString;
        }
        public static string algebra2Move(string move){
            string moveString="";
            moveString = moveString + ""+('8' - (move[1]));
            moveString = moveString + ""+(char)(move[0]-49);
            moveString = moveString + ""+('8' - (move[3]));
            moveString = moveString + ""+(char)(move[2]-49);
            return moveString;
        }

        public static int perftMoveCount = 0;
        public static int perftTotalCount = 0;
        static int perftMaxDepth = 3; // used 4 for performance comparison

        public static void perftRoot(ulong bKB, ulong bQB, ulong bRB, ulong bBB, ulong bNB, ulong bPB, ulong wKB, ulong wQB, ulong wRB, ulong wBB, ulong wNB, ulong wPB, ulong EPB, bool castleWKside, bool castleWQside, bool castleBKside, bool castleBQside, bool white2Move, int depth){
            if (depth < perftMaxDepth){
                string moves;
                if(white2Move){
                    moves = Moves.possibleMovesW(bKB, bQB, bRB, bBB, bNB, bPB, wKB, wQB, wRB, wBB, wNB, wPB, EPB, castleWKside, castleWQside, castleBKside, castleBQside);
                }else{
                    moves = Moves.possibleMovesB(bKB, bQB, bRB, bBB, bNB, bPB, wKB, wQB, wRB, wBB, wNB, wPB, EPB, castleWKside, castleWQside, castleBKside, castleBQside);
            //         for (int i = 0; i < moves.Length; i+=4){
            //             Console.WriteLine(move2Algebra(moves.Substring(i, 4)));
            // }
            //             Console.ReadKey();
                }
                for (int i = 0; i < moves.Length; i+=4){
                    UInt64 bRBt = Moves.makeMove(bRB, moves.Substring(i, 4), 'r'),
                        bQBt = Moves.makeMove(bQB, moves.Substring(i, 4), 'q'),
                        bKBt = Moves.makeMove(bKB, moves.Substring(i, 4), 'k'),
                        bBBt = Moves.makeMove(bBB, moves.Substring(i, 4), 'b'),
                        bNBt = Moves.makeMove(bNB, moves.Substring(i, 4), 'n'),
                        bPBt = Moves.makeMove(bPB, moves.Substring(i, 4), 'p'),
                        wKBt = Moves.makeMove(wKB, moves.Substring(i, 4), 'K'),
                        wQBt = Moves.makeMove(wQB, moves.Substring(i, 4), 'Q'),
                        wRBt = Moves.makeMove(wRB, moves.Substring(i, 4), 'R'),
                        wBBt = Moves.makeMove(wBB, moves.Substring(i, 4), 'B'),
                        wNBt = Moves.makeMove(wNB, moves.Substring(i, 4), 'N'),
                        wPBt = Moves.makeMove(wPB, moves.Substring(i, 4), 'P'),
                        EPBt = Moves.makeMoveEP(wPB|bPB, moves.Substring(i, 4));
                    // BoardGeneration.drawBitboard(bRBt);
                    // BoardGeneration.drawBitboard(wQBt);
                    // Console.ReadKey();
                    bool castleWKsideT = castleWKside, castleWQsideT = castleWQside, castleBKsideT = castleBKside, castleBQsideT = castleBQside;
                    if(char.IsDigit(moves[i+3])){ // regular move if its a digit at last char - so not P or E
                        int start = (((moves[i] - '0') * 8) + (moves[i+1] - '0')); // set start location for each iteration of i
                        if ((((UInt64)1 << start) & wKB) != 0) {castleWKsideT = false; castleWQsideT = false;}
                        if ((((UInt64)1 << start) & bKB) != 0) {castleBKsideT = false; castleBQsideT = false;}
                        if (((((UInt64)1 << start) & wRB) & ((UInt64)1 << 63)) != 0) { castleWKsideT = false;}
                        if (((((UInt64)1 << start) & wRB) & ((UInt64)1 << 56)) != 0) { castleWQsideT = false;}
                        if (((((UInt64)1 << start) & bRB) & ((UInt64)1 << 7)) != 0) { castleBKsideT = false;}
                        if (((((UInt64)1 << start) & bRB) & (UInt64)1) != 0) { castleBQsideT = false;}
                    }
                    if (((wKBt & Moves.unsafeWhite(bKBt, bQBt, bRBt, bBBt, bNBt, bPBt, wKBt, wQBt, wRBt, wBBt, wNBt, wPBt)) == 0 && white2Move) ||
                        ((bKBt & Moves.unsafeBlack(bKBt, bQBt, bRBt, bBBt, bNBt, bPBt, wKBt, wQBt, wRBt, wBBt, wNBt, wPBt)) == 0 && !white2Move)){
                        perft(bKBt, bQBt, bRBt, bBBt, bNBt, bPBt, wKBt, wQBt, wRBt, wBBt, wNBt, wPBt, EPBt, castleWKsideT, castleWQsideT, castleBKsideT, castleBQsideT, !white2Move, depth+1);
                        // Console.WriteLine(move2Algebra(moves.Substring(i, 4)) + " " + perftMoveCount);
                        perftTotalCount += perftMoveCount;
                        perftMoveCount = 0;

                    }
                }
            }
        }

        public static void perft(ulong bKB, ulong bQB, ulong bRB, ulong bBB, ulong bNB, ulong bPB, ulong wKB, ulong wQB, ulong wRB, ulong wBB, ulong wNB, ulong wPB, ulong EPB, bool castleWKside, bool castleWQside, bool castleBKside, bool castleBQside, bool white2Move, int depth){
            if (depth < perftMaxDepth){
                string moves;
                if(white2Move){
                    moves = Moves.possibleMovesW(bKB, bQB, bRB, bBB, bNB, bPB, wKB, wQB, wRB, wBB, wNB, wPB, EPB, castleWKside, castleWQside, castleBKside, castleBQside);
                }else{
                    moves = Moves.possibleMovesB(bKB, bQB, bRB, bBB, bNB, bPB, wKB, wQB, wRB, wBB, wNB, wPB, EPB, castleWKside, castleWQside, castleBKside, castleBQside);
                }
                for (int i = 0; i < moves.Length; i+=4){
                    UInt64 bKBt = Moves.makeMove(bKB, moves.Substring(i, 4), 'k'),
                        bQBt = Moves.makeMove(bQB, moves.Substring(i, 4), 'q'),
                        bRBt = Moves.makeMove(bRB, moves.Substring(i, 4), 'r'),
                        bBBt = Moves.makeMove(bBB, moves.Substring(i, 4), 'b'),
                        bNBt = Moves.makeMove(bNB, moves.Substring(i, 4), 'n'),
                        bPBt = Moves.makeMove(bPB, moves.Substring(i, 4), 'p'),
                        wKBt = Moves.makeMove(wKB, moves.Substring(i, 4), 'K'),
                        wQBt = Moves.makeMove(wQB, moves.Substring(i, 4), 'Q'),
                        wRBt = Moves.makeMove(wRB, moves.Substring(i, 4), 'R'),
                        wBBt = Moves.makeMove(wBB, moves.Substring(i, 4), 'B'),
                        wNBt = Moves.makeMove(wNB, moves.Substring(i, 4), 'N'),
                        wPBt = Moves.makeMove(wPB, moves.Substring(i, 4), 'P'),
                        EPBt = Moves.makeMoveEP(wPB|bPB, moves.Substring(i, 4));
                    bool castleWKsideT = castleWKside, castleWQsideT = castleWQside, castleBKsideT = castleBKside, castleBQsideT = castleBQside;
                    if(char.IsDigit(moves[i+3])){ // regular move if its a digit at last char - so not P or E
                        int start = (((moves[i] - '0') * 8) + (moves[i+1] - '0')); // set start location for each iteration of i
                        if ((((UInt64)1 << start) & wKB) != 0) {castleWKsideT = false; castleWQsideT = false;}
                        if ((((UInt64)1 << start) & bKB) != 0) {castleBKsideT = false; castleBQsideT = false;}
                        if (((((UInt64)1 << start) & wRB) & ((UInt64)1 << 63)) != 0) { castleWKsideT = false;}
                        if (((((UInt64)1 << start) & wRB) & ((UInt64)1 << 56)) != 0) { castleWQsideT = false;}
                        if (((((UInt64)1 << start) & bRB) & ((UInt64)1 << 7)) != 0) { castleBKsideT = false;}
                        if (((((UInt64)1 << start) & bRB) & (UInt64)1) != 0) { castleBQsideT = false;}
                    }
                    if (((wKBt & Moves.unsafeWhite(bKBt, bQBt, bRBt, bBBt, bNBt, bPBt, wKBt, wQBt, wRBt, wBBt, wNBt, wPBt)) == 0 && white2Move) ||
                        ((bKBt & Moves.unsafeBlack(bKBt, bQBt, bRBt, bBBt, bNBt, bPBt, wKBt, wQBt, wRBt, wBBt, wNBt, wPBt)) == 0 && !white2Move)){
                        if (depth+1 == perftMaxDepth) {perftMoveCount++;}
                        perft(bKBt, bQBt, bRBt, bBBt, bNBt, bPBt, wKBt, wQBt, wRBt, wBBt, wNBt, wPBt, EPBt, castleWKsideT, castleWQsideT, castleBKsideT, castleBQsideT, !white2Move, depth+1);
                    }
                }
            }
        }
    }
}
