using System;

namespace ChessBitboard{
    public class Perft{
        public static string move2Algebra(string move){ // convert coordinates to algebraic notation
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

        static int perftMaxDepth = 4; // set how many ply to search

        public static void perftRoot(ulong bKB, ulong bQB, ulong bRB, ulong bBB, ulong bNB, ulong bPB, ulong wKB, ulong wQB, ulong wRB, ulong wBB, ulong wNB, ulong wPB, ulong EPB, bool castleWKside, bool castleWQside, bool castleBKside, bool castleBQside, bool white2Move, int depth){
            if (depth < perftMaxDepth){
                string moves;
                char[] Cmoves = new char[4]; //create array to send to makeMove method
                if(white2Move){ // get string of all possible moves for white
                    moves = Moves.possibleMovesW(bKB, bQB, bRB, bBB, bNB, bPB, wKB, wQB, wRB, wBB, wNB, wPB, EPB, castleWKside, castleWQside, castleBKside, castleBQside);
                }else{ // get string of all possible moves for black
                    moves = Moves.possibleMovesB(bKB, bQB, bRB, bBB, bNB, bPB, wKB, wQB, wRB, wBB, wNB, wPB, EPB, castleWKside, castleWQside, castleBKside, castleBQside);
                }
                for (int i = 0; i < moves.Length; i+=4){ // every move is 4 char so length of movesString / 4 is how many moves possibleMoves method found

                    Cmoves[0] = moves[i]; Cmoves[1] = moves[i+1]; Cmoves[2] = moves[i+2]; Cmoves[3] = moves[i+3]; // put char into array for makeMove method
                    int start = (((Cmoves[0] - '0') * 8) + (Cmoves[1] - '0')); // set start location for each move i counts by 4 to get moves
                    int end = (((Cmoves[2] - '0') * 8) + (Cmoves[3] - '0')); // get end for every move

                    // send all piece bitboards to makeMove to make a move or get removed if piece is attacked
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
                        EPBt = Moves.makeMoveEP(wPB|bPB, Cmoves, start); // set the en passant mask EPBt to a file if a pawn moved 2 steps // else set it 0 so only round after double move have valid EP move

                   wRBt = Moves.CastleMove(wRBt, wKB, Cmoves, 'R', start); // checks if the king made a castle move if true then moves the rook
                   bRBt = Moves.CastleMove(bRBt, bKB, Cmoves, 'r', start);

                    // BoardGeneration.drawArray(bKBt, bQBt, bRBt, bBBt, bNBt, bPBt, wKBt, wQBt, wRBt, wBBt, wNBt, wPBt);
                    // Console.WriteLine();
                    // Console.ReadKey();
                    // Console.Clear();
                    bool castleWKsideT = castleWKside, castleWQsideT = castleWQside, castleBKsideT = castleBKside, castleBQsideT = castleBQside; // set temporary castleflags to the bool send from search before

                    if ((((UInt64)1 << start) & wKB) != 0) { // if wking moved set false flags
                        castleWKsideT = false;
                        castleWQsideT = false;
                    }
                    if ((((UInt64)1 << start) & bKB) != 0) { // if bking moved set false flags
                        castleBKsideT = false;
                        castleBQsideT = false;
                    }
                    if (((((UInt64)1 << start) & wRB) & ((UInt64)1 << 63)) != 0) { // if wrook kingside moves set kingside flag false
                        castleWKsideT = false;
                    }
                    if (((((UInt64)1 << start) & wRB) & ((UInt64)1 << 56)) != 0) { // if wrook queenside moves set queenside flag false
                        castleWQsideT = false;
                    }
                    if (((((UInt64)1 << start) & bRB) & ((UInt64)1 << 7)) != 0) { // if brook kingside moves set king side flag false
                        castleBKsideT = false;
                    }
                    if (((((UInt64)1 << start) & bRB) & (UInt64)1) != 0) { // if brook queenside moves set queensdie flag false
                        castleBQsideT = false;
                    }

//                    UInt64 occu = bKBt|bQBt|bRBt|bBBt|bNBt|bPBt|wKBt|wQBt|wRBt|wBBt|wNBt|wPBt;
                    if (((wKBt & Moves.unsafeWhite(bKBt, bQBt, bRBt, bBBt, bNBt, bPBt, wKBt, wQBt, wRBt, wBBt, wNBt, wPBt)) == 0 && white2Move) || // check if white king is in check and its white turn
                        ((bKBt & Moves.unsafeBlack(bKBt, bQBt, bRBt, bBBt, bNBt, bPBt, wKBt, wQBt, wRBt, wBBt, wNBt, wPBt)) == 0 && !white2Move)){ // check if black king is in check and its black turn
                        // start next perft search if no checkmate
                        perft(bKBt, bQBt, bRBt, bBBt, bNBt, bPBt, wKBt, wQBt, wRBt, wBBt, wNBt, wPBt, EPBt, castleWKsideT, castleWQsideT, castleBKsideT, castleBQsideT, !white2Move, depth+1);
                     //  Console.WriteLine(move2Algebra(moves.Substring(i, 4)) + " " + perftMoveCount);
                        perftTotalCount += perftMoveCount; // count moves that are valid out of the possible moves
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
                    Cmoves[0] = moves[i]; Cmoves[1] = moves[i+1]; Cmoves[2] = moves[i+2]; Cmoves[3] = moves[i+3];
                    int start = (((Cmoves[0] - '0') * 8) + (Cmoves[1] - '0'));
                    int end = (((Cmoves[2] - '0') * 8) + (Cmoves[3] - '0'));

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

                    if ((((UInt64)1 << start) & wKB) != 0) {
                        castleWKsideT = false;
                        castleWQsideT = false;
                    }
                    if ((((UInt64)1 << start) & bKB) != 0) {
                        castleBKsideT = false;
                        castleBQsideT = false;
                    }
                    if (((((UInt64)1 << start) & wRB) & ((UInt64)1 << 63)) != 0) {
                        castleWKsideT = false;
                    }
                    if (((((UInt64)1 << start) & wRB) & ((UInt64)1 << 56)) != 0) {
                        castleWQsideT = false;
                    }
                    if (((((UInt64)1 << start) & bRB) & ((UInt64)1 << 7)) != 0) {
                        castleBKsideT = false;
                    }
                    if (((((UInt64)1 << start) & bRB) & (UInt64)1) != 0) {
                        castleBQsideT = false;
                    }


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
