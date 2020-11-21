using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;

namespace ChessBitboard{
    public class Perft{
        public static string move2Algebra(string move){
            string moveString="";
            if(move[3] == 'P'){
                moveString = moveString +""+ (char)(move[0]+49);
                moveString = moveString +""+ (char)(move[1]+49);
                moveString = moveString + ""+move[2];
                moveString = moveString + ""+move[3];
                return moveString;
            }
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

        static int perftMaxDepth = 5;

        public static void perftRoot(ulong bKB, ulong bQB, ulong bRB, ulong bBB, ulong bNB, ulong bPB, ulong wKB, ulong wQB, ulong wRB, ulong wBB, ulong wNB, ulong wPB, ulong EPB, bool castleWKside, bool castleWQside, bool castleBKside, bool castleBQside, bool white2Move, int depth){
            if (depth < perftMaxDepth){
                string moves;
                char[] Cmoves = new char[4];
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
//                     string Substring = moves.Substring(i, 4);
//                    string Substring = new string (Tools.CharCombine(moves[i], moves[i+1], moves[i+2], moves[i+3]));
                    int start = (((moves[i] - '0') * 8) + (moves[i+1] - '0')); // set start location for each iteration of i
                    int end = (((moves[i+2] - '0') * 8) + (moves[i+3] - '0'));
                    Cmoves[0] = moves[i]; Cmoves[1] = moves[i+1]; Cmoves[2] = moves[i+2]; Cmoves[3] = moves[i+3];

                    UInt64 bKBt = Moves.makeMove(bKB, Cmoves, 'k', start, end),
                        bQBt = Moves.makeMove(bQB, Cmoves, 'q', start, end),
                        bRBt = Moves.makeMove(bRB, Cmoves, 'r', start, end),
                        bBBt = Moves.makeMove(bBB, Cmoves, 'b', start, end),
                        bNBt = Moves.makeMove(bNB, Cmoves, 'n', start, end),
                        bPBt = Moves.makeMove(bPB, Cmoves, 'p', start, end),
                        wKBt = Moves.makeMove(wKB, Cmoves, 'K', start, end),
                        wQBt = Moves.makeMove(wQB, Cmoves, 'Q', start, end),
                        wRBt = Moves.makeMove(wRB, Cmoves, 'R', start, end),
                        wBBt = Moves.makeMove(wBB, Cmoves, 'B', start, end),
                        wNBt = Moves.makeMove(wNB, Cmoves, 'N', start, end),
                        wPBt = Moves.makeMove(wPB, Cmoves, 'P', start, end),
                        EPBt = Moves.makeMoveEP(wPB|bPB, Cmoves, start);

                   wRBt = Moves.CastleMove(wRBt, wKB, Cmoves, 'R', start);
                   bRBt = Moves.CastleMove(bRBt, bKB, Cmoves, 'r', start);

                    // BoardGeneration.drawArray(bKBt, bQBt, bRBt, bBBt, bNBt, bPBt, wKBt, wQBt, wRBt, wBBt, wNBt, wPBt);
                    // Console.WriteLine();
                    // Console.ReadKey();
                    // Console.Clear();
                    bool castleWKsideT = castleWKside, castleWQsideT = castleWQside, castleBKsideT = castleBKside, castleBQsideT = castleBQside;

// 40 ms faster when castling is false // 40 ms slower when true // 70 ms faster if no check nothing to gain it seems

                    /*

                    if( castleWKsideT || castleWQsideT ){
                        if ((((UInt64)1 << start) & wKB) != 0) {
                            castleWKsideT = false;
                            castleWQsideT = false;
                        } else if (((((UInt64)1 << start) & wRB) & ((UInt64)1 << 63)) != 0) {
                            castleWKsideT = false;
                        } else if (((((UInt64)1 << start) & wRB) & ((UInt64)1 << 56)) != 0) {
                            castleWQsideT = false;
                        }
                    }

                    if ( castleBKsideT || castleBQsideT ){
                        if ((((UInt64)1 << start) & bKB) != 0) { // if start AND with king board is not 0 then the king is moving
                            castleBKsideT = false;
                            castleBQsideT = false;
                        } else if (((((UInt64)1 << start) & bRB) & ((UInt64)1 << 7)) != 0) {
                            castleBKsideT = false;
                        } else if (((((UInt64)1 << start) & bRB) & (UInt64)1) != 0) {
                            castleBQsideT = false;
                        }
                    }
                        */

                        if ((((UInt64)1 << start) & wKB) != 0) {castleWKsideT = false; castleWQsideT = false;}
                        if ((((UInt64)1 << start) & bKB) != 0) {castleBKsideT = false; castleBQsideT = false;}
                        if (((((UInt64)1 << start) & wRB) & ((UInt64)1 << 63)) != 0) { castleWKsideT = false;}
                        if (((((UInt64)1 << start) & wRB) & ((UInt64)1 << 56)) != 0) { castleWQsideT = false;}
                        if (((((UInt64)1 << start) & bRB) & ((UInt64)1 << 7)) != 0) { castleBKsideT = false;}
                        if (((((UInt64)1 << start) & bRB) & (UInt64)1) != 0) { castleBQsideT = false;}

//                    UInt64 occu = bKBt|bQBt|bRBt|bBBt|bNBt|bPBt|wKBt|wQBt|wRBt|wBBt|wNBt|wPBt;
                    if (((wKBt & Moves.unsafeWhite(bKBt, bQBt, bRBt, bBBt, bNBt, bPBt, wKBt, wQBt, wRBt, wBBt, wNBt, wPBt)) == 0 && white2Move) ||
                        ((bKBt & Moves.unsafeBlack(bKBt, bQBt, bRBt, bBBt, bNBt, bPBt, wKBt, wQBt, wRBt, wBBt, wNBt, wPBt)) == 0 && !white2Move)){
                        perft(bKBt, bQBt, bRBt, bBBt, bNBt, bPBt, wKBt, wQBt, wRBt, wBBt, wNBt, wPBt, EPBt, castleWKsideT, castleWQsideT, castleBKsideT, castleBQsideT, !white2Move, depth+1);
                     //  Console.WriteLine(move2Algebra(moves.Substring(i, 4)) + " " + perftMoveCount);
//                       Console.WriteLine(moves.Substring(i, 4));
                        perftTotalCount += perftMoveCount;
                        perftMoveCount = 0;

                    }
                }
            }
        }

        public static void perft(ulong bKB, ulong bQB, ulong bRB, ulong bBB, ulong bNB, ulong bPB, ulong wKB, ulong wQB, ulong wRB, ulong wBB, ulong wNB, ulong wPB, ulong EPB, bool castleWKside, bool castleWQside, bool castleBKside, bool castleBQside, bool white2Move, int depth){
            if (depth < perftMaxDepth){
                string moves;
                char[] Cmoves = new char[4];
                if(white2Move){
                    moves = Moves.possibleMovesW(bKB, bQB, bRB, bBB, bNB, bPB, wKB, wQB, wRB, wBB, wNB, wPB, EPB, castleWKside, castleWQside, castleBKside, castleBQside);
                }else{
                    moves = Moves.possibleMovesB(bKB, bQB, bRB, bBB, bNB, bPB, wKB, wQB, wRB, wBB, wNB, wPB, EPB, castleWKside, castleWQside, castleBKside, castleBQside);
                }
                for (int i = 0; i < moves.Length; i+=4){
                     // string Substring = moves.Substring(i, 4);
//                     string Substring = new string (Tools.CharCombine(moves[i], moves[i+1], moves[i+2], moves[i+3]));
                    int start = (((moves[i] - '0') * 8) + (moves[i+1] - '0')); // set start location for each iteration of i
                    int end = (((moves[i+2] - '0') * 8) + (moves[i+3] - '0'));
                    Cmoves[0] = moves[i]; Cmoves[1] = moves[i+1]; Cmoves[2] = moves[i+2]; Cmoves[3] = moves[i+3];

                    UInt64 bKBt = Moves.makeMove(bKB, Cmoves, 'k', start, end),
                        bQBt = Moves.makeMove(bQB, Cmoves, 'q', start, end),
                        bRBt = Moves.makeMove(bRB, Cmoves, 'r', start, end),
                        bBBt = Moves.makeMove(bBB, Cmoves, 'b', start, end),
                        bNBt = Moves.makeMove(bNB, Cmoves, 'n', start, end),
                        bPBt = Moves.makeMove(bPB, Cmoves, 'p', start, end),
                        wKBt = Moves.makeMove(wKB, Cmoves, 'K', start, end),
                        wQBt = Moves.makeMove(wQB, Cmoves, 'Q', start, end),
                        wRBt = Moves.makeMove(wRB, Cmoves, 'R', start, end),
                        wBBt = Moves.makeMove(wBB, Cmoves, 'B', start, end),
                        wNBt = Moves.makeMove(wNB, Cmoves, 'N', start, end),
                        wPBt = Moves.makeMove(wPB, Cmoves, 'P', start, end),
                        EPBt = Moves.makeMoveEP(wPB|bPB, Cmoves, start);

                    wRBt = Moves.CastleMove(wRBt, wKB, Cmoves, 'R', start);
                    bRBt = Moves.CastleMove(bRBt, bKB, Cmoves, 'r', start);

                    // BoardGeneration.drawArray(bKBt, bQBt, bRBt, bBBt, bNBt, bPBt, wKBt, wQBt, wRBt, wBBt, wNBt, wPBt);
                    // Console.WriteLine();
                    // Console.ReadKey();
                    // Console.Clear();

                    bool castleWKsideT = castleWKside, castleWQsideT = castleWQside, castleBKsideT = castleBKside, castleBQsideT = castleBQside;

    /*
                    if( castleWKsideT || castleWQsideT ){
                        if ((((UInt64)1 << start) & wKB) != 0) {
                            castleWKsideT = false;
                            castleWQsideT = false;
                        } else if (((((UInt64)1 << start) & wRB) & ((UInt64)1 << 63)) != 0) {
                            castleWKsideT = false;
                        } else if (((((UInt64)1 << start) & wRB) & ((UInt64)1 << 56)) != 0) {
                            castleWQsideT = false;
                        }
                    }

                    if ( castleBKsideT || castleBQsideT ){
                        if ((((UInt64)1 << start) & bKB) != 0) {
                            castleBKsideT = false;
                            castleBQsideT = false;
                        } else if (((((UInt64)1 << start) & bRB) & ((UInt64)1 << 7)) != 0) {
                            castleBKsideT = false;
                        } else if (((((UInt64)1 << start) & bRB) & (UInt64)1) != 0) {
                            castleBQsideT = false;
                        }
                    }
    */
                        if ((((UInt64)1 << start) & wKB) != 0) {castleWKsideT = false; castleWQsideT = false;}
                        if ((((UInt64)1 << start) & bKB) != 0) {castleBKsideT = false; castleBQsideT = false;}
                        if (((((UInt64)1 << start) & wRB) & ((UInt64)1 << 63)) != 0) { castleWKsideT = false;}
                        if (((((UInt64)1 << start) & wRB) & ((UInt64)1 << 56)) != 0) { castleWQsideT = false;}
                        if (((((UInt64)1 << start) & bRB) & ((UInt64)1 << 7)) != 0) { castleBKsideT = false;}
                        if (((((UInt64)1 << start) & bRB) & (UInt64)1) != 0) { castleBQsideT = false;}


 //                    UInt64 occu = bKBt|bQBt|bRBt|bBBt|bNBt|bPBt|wKBt|wQBt|wRBt|wBBt|wNBt|wPBt;
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
