using System;

namespace ChessBitboard{
    public class Perft{

        public static int perftMoveCount = 0;
        public static int perftTotalCount = 0;

        static int perftMaxDepth = 2; // set how many ply to search

        public static void perftRoot(UInt64[] bitboards, ulong EPB, bool castleWKside, bool castleWQside, bool castleBKside, bool castleBQside, bool white2Move, int depth){
            if (depth < perftMaxDepth){
                string moves;
                char[] Cmoves = new char[4]; //create array to send to makeMove method
                if(white2Move){ // get string of all possible moves for white
                    moves = Moves.possibleMovesW(bitboards, EPB, castleWKside, castleWQside, castleBKside, castleBQside);
                }else{ // get string of all possible moves for black
                    moves = Moves.possibleMovesB(bitboards, EPB, castleWKside, castleWQside, castleBKside, castleBQside);
                }
                for (int i = 0; i < moves.Length; i+=4){ // every move is 4 char so length of movesString / 4 is how many moves possibleMoves method found

                    Cmoves[0] = moves[i]; Cmoves[1] = moves[i+1]; Cmoves[2] = moves[i+2]; Cmoves[3] = moves[i+3]; // put char into array for makeMove method
                    int start = (((Cmoves[0] - '0') * 8) + (Cmoves[1] - '0')); // set start location for each move i counts by 4 to get moves
                    int end = (((Cmoves[2] - '0') * 8) + (Cmoves[3] - '0')); // get end for every move

                    BoardGeneration.drawArray(bitboards);
                    Console.Read();
                    // send all piece bitboards to makeMove to make a move or get removed if piece is attacked
            UInt64[] Tbitboards = new UInt64[] {
                Moves.makeMove(bitboards[0], Cmoves, 'k', start, end),
                Moves.makeMove(bitboards[1], Cmoves, 'q', start, end),
                Moves.makeMove(bitboards[2], Cmoves, 'r', start, end),
                Moves.makeMove(bitboards[3], Cmoves, 'b', start, end),
                Moves.makeMove(bitboards[4], Cmoves, 'n', start, end),
                Moves.makeMove(bitboards[5], Cmoves, 'p', start, end),
                Moves.makeMove(bitboards[6], Cmoves, 'K', start, end),
                Moves.makeMove(bitboards[7], Cmoves, 'Q', start, end),
                Moves.makeMove(bitboards[8], Cmoves, 'R', start, end),
                Moves.makeMove(bitboards[9], Cmoves, 'B', start, end),
                Moves.makeMove(bitboards[10], Cmoves, 'N', start, end),
                Moves.makeMove(bitboards[11], Cmoves, 'P', start, end)
            };

            UInt64 EPBt = Moves.makeMoveEP(bitboards[11]|bitboards[5], Cmoves, start); // sætter en passant masken EPB til den til den fil hvor en bonde bevæger sig 2 ranks op, ellers sætter den 0 så den kun masker en runde
            Tbitboards[8] = Moves.CastleMove(Tbitboards[8], bitboards[6], Cmoves, 'R', start); // checks if the king made a castle move if true then moves the rook
            Tbitboards[2] = Moves.CastleMove(Tbitboards[2], bitboards[0], Cmoves, 'r', start);

                    bool castleWKsideT = castleWKside, castleWQsideT = castleWQside, castleBKsideT = castleBKside, castleBQsideT = castleBQside; // set temporary castleflags to the bool send from search before

                    if ((((UInt64)1 << start) & bitboards[6]) != 0) { // if wking moved set false flags
                        castleWKsideT = false;
                        castleWQsideT = false;
                    }
                    if ((((UInt64)1 << start) & bitboards[0]) != 0) { // if bking moved set false flags
                        castleBKsideT = false;
                        castleBQsideT = false;
                    }
                    if (((((UInt64)1 << start) & bitboards[8]) & ((UInt64)1 << 63)) != 0) { // if wrook kingside moves set kingside flag false
                        castleWKsideT = false;
                    }
                    if (((((UInt64)1 << start) & bitboards[8]) & ((UInt64)1 << 56)) != 0) { // if wrook queenside moves set queenside flag false
                        castleWQsideT = false;
                    }
                    if (((((UInt64)1 << start) & bitboards[2]) & ((UInt64)1 << 7)) != 0) { // if brook kingside moves set king side flag false
                        castleBKsideT = false;
                    }
                    if (((((UInt64)1 << start) & bitboards[2]) & (UInt64)1) != 0) { // if brook queenside moves set queensdie flag false
                        castleBQsideT = false;
                    }

                    BoardGeneration.drawArray(Tbitboards);
                    Console.Read();

                    if (((Tbitboards[6] & Moves.unsafeWhite(Tbitboards)) == 0 && white2Move) || // check if white king is in check and its white turn
                        ((Tbitboards[0] & Moves.unsafeBlack(Tbitboards)) == 0 && !white2Move)){ // check if black king is in check and its black turn
                        // start next perft search if no checkmate
                        perft(Tbitboards, EPBt, castleWKsideT, castleWQsideT, castleBKsideT, castleBQsideT, !white2Move, depth+1);
                        Console.WriteLine(Tools.move2Algebra(moves.Substring(i, 4)) + " " + perftMoveCount);
                        perftTotalCount += perftMoveCount; // count moves that are valid out of the possible moves
                        perftMoveCount = 0;

                    }
                }
            }
        }

        public static void perft(UInt64[] bitboards, ulong EPB, bool castleWKside, bool castleWQside, bool castleBKside, bool castleBQside, bool white2Move, int depth){
            if (depth < perftMaxDepth){
                string moves;
                char[] Cmoves = new char[4];
                if(white2Move){
                    moves = Moves.possibleMovesW(bitboards, EPB, castleWKside, castleWQside, castleBKside, castleBQside);
                }else{
                    moves = Moves.possibleMovesB(bitboards, EPB, castleWKside, castleWQside, castleBKside, castleBQside);
                }
                for (int i = 0; i < moves.Length; i+=4){
                    Cmoves[0] = moves[i]; Cmoves[1] = moves[i+1]; Cmoves[2] = moves[i+2]; Cmoves[3] = moves[i+3];
                    int start = (((Cmoves[0] - '0') * 8) + (Cmoves[1] - '0'));
                    int end = (((Cmoves[2] - '0') * 8) + (Cmoves[3] - '0'));

                    BoardGeneration.drawArray(bitboards);
                    Console.Read();

            UInt64[] Tbitboards = new UInt64[] {
                Moves.makeMove(bitboards[0], Cmoves, 'k', start, end),
                Moves.makeMove(bitboards[1], Cmoves, 'q', start, end),
                Moves.makeMove(bitboards[2], Cmoves, 'r', start, end),
                Moves.makeMove(bitboards[3], Cmoves, 'b', start, end),
                Moves.makeMove(bitboards[4], Cmoves, 'n', start, end),
                Moves.makeMove(bitboards[5], Cmoves, 'p', start, end),
                Moves.makeMove(bitboards[6], Cmoves, 'K', start, end),
                Moves.makeMove(bitboards[7], Cmoves, 'Q', start, end),
                Moves.makeMove(bitboards[8], Cmoves, 'R', start, end),
                Moves.makeMove(bitboards[9], Cmoves, 'B', start, end),
                Moves.makeMove(bitboards[10], Cmoves, 'N', start, end),
                Moves.makeMove(bitboards[11], Cmoves, 'P', start, end)
            };

            UInt64 EPBt = Moves.makeMoveEP(bitboards[11]|bitboards[5], Cmoves, start); // sætter en passant masken EPB til den til den fil hvor en bonde bevæger sig 2 ranks op, ellers sætter den 0 så den kun masker en runde
            Tbitboards[8] = Moves.CastleMove(Tbitboards[8], bitboards[6], Cmoves, 'R', start); // checks if the king made a castle move if true then moves the rook
            Tbitboards[2] = Moves.CastleMove(Tbitboards[2], bitboards[0], Cmoves, 'r', start);

                    bool castleWKsideT = castleWKside, castleWQsideT = castleWQside, castleBKsideT = castleBKside, castleBQsideT = castleBQside;

                    if ((((UInt64)1 << start) & bitboards[6]) != 0) { // if wking moved set false flags
                        castleWKsideT = false;
                        castleWQsideT = false;
                    }
                    if ((((UInt64)1 << start) & bitboards[0]) != 0) { // if bking moved set false flags
                        castleBKsideT = false;
                        castleBQsideT = false;
                    }
                    if (((((UInt64)1 << start) & bitboards[8]) & ((UInt64)1 << 63)) != 0) { // if wrook kingside moves set kingside flag false
                        castleWKsideT = false;
                    }
                    if (((((UInt64)1 << start) & bitboards[8]) & ((UInt64)1 << 56)) != 0) { // if wrook queenside moves set queenside flag false
                        castleWQsideT = false;
                    }
                    if (((((UInt64)1 << start) & bitboards[2]) & ((UInt64)1 << 7)) != 0) { // if brook kingside moves set king side flag false
                        castleBKsideT = false;
                    }
                    if (((((UInt64)1 << start) & bitboards[2]) & (UInt64)1) != 0) { // if brook queenside moves set queensdie flag false
                        castleBQsideT = false;
                    }

                    BoardGeneration.drawArray(Tbitboards);
                    Console.Read();

                    if (((Tbitboards[6] & Moves.unsafeWhite(Tbitboards)) == 0 && white2Move) || // check if white king is in check and its white turn
                        ((Tbitboards[0] & Moves.unsafeBlack(Tbitboards)) == 0 && !white2Move)){ // check if black king is in check and its black turn
                        if (depth+1 == perftMaxDepth) {perftMoveCount++;}
                        perft(Tbitboards, EPBt, castleWKsideT, castleWQsideT, castleBKsideT, castleBQsideT, !white2Move, depth+1);
                    }
                }
            }
        }
    }
}
