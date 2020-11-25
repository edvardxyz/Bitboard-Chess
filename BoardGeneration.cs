using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;

namespace ChessBitboard{
    public class BoardGeneration{

        // unicode char for each piece
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


        // king castle check
        public static bool castleWKside = true;
        public static bool castleWQside = true;
        public static bool castleBKside = true;
        public static bool castleBQside = true;

        public static bool white2Move = true;
        public static UInt64 EPB = 0;

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

                /*//
                  {bKC,eC,eC,eC,eC,eC,eC,eC},
                  {eC,eC,eC,eC,eC,eC,eC,eC},
                  {eC,eC,eC,eC,eC,eC,eC,eC},
                  {eC,eC,eC,eC,eC,eC,eC,eC},
                  {eC,eC,eC,eC,eC,eC,eC,eC},
                  {eC,eC,eC,eC,eC,eC,eC,eC},
                  {eC,eC,eC,wKC,eC,eC,bPC,eC},
                  {eC,eC,eC,eC,eC,eC,eC,eC},

                */
                /*
                // r3k2r/p1ppqpb1/bn2pnp1/3PN3/1p2P3/2N2Q1p/PPPBBPPP/R3K2R w KQkq
                {bRC,eC,eC,eC,bKC,eC,eC,bRC},
                {bPC,eC,bPC,bPC,bQC,bPC,bBC,eC},
                {bBC,bNC,eC,eC,bPC,bNC,bPC,eC},
                {eC,eC,eC,wPC,wNC,eC,eC,eC},
                {eC,bPC,eC,eC,wPC,eC,eC,eC},
                {eC,eC,wNC,eC,eC,wQC,eC,bPC},
                {wPC,wPC,wPC,wBC,wBC,wPC,wPC,wPC},
                {wRC,eC,eC,eC,wKC,eC,eC,wRC},
                */   //
                //  NORMAL CHESS
                {bRC,bNC,bBC,bQC,bKC,bBC,bNC,bRC},
                {bPC,bPC,bPC,bPC,bPC,bPC,bPC,bPC},
                {eC,eC,eC,eC,eC,eC,eC,eC},
                {eC,eC,eC,eC,eC,eC,eC,eC},
                {eC,eC,eC,eC,eC,eC,eC,eC},
                {eC,eC,eC,eC,eC,eC,eC,eC},
                {wPC,wPC,wPC,wPC,wPC,wPC,wPC,wPC},
                {wRC,wNC,wBC,wQC,wKC,wBC,wNC,wRC},
            };

            // makes the individual bitboard correct based on array of char
            string binaryString;
            for (int i = 0; i<64;i++){
                binaryString = "0000000000000000000000000000000000000000000000000000000000000000";
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






            bool kingsafe = true;
            bool whitewon = false;
            bool blackwon = false;
            while(true){
                string Wplay = "";
                do{
                    Console.Clear();
                    drawArray(bKB, bQB, bRB, bBB, bNB, bPB, wKB, wQB, wRB, wBB, wNB, wPB);
                    if(!kingsafe){
                        Console.WriteLine("Your move made your king in check");
                        Console.WriteLine("Try another move: ");
                        Console.WriteLine("Type 'h' for help");
                    }else{
                        Console.WriteLine("Insert play: ");
                        Console.WriteLine("Type 'h' for help");
                    }
                    Wplay = Console.ReadLine().ToLower();
                    if(!(string.IsNullOrEmpty(Wplay)) && Wplay[0] == 'h'){
                        PrintHelp();
                        Console.Write("\nPress a key to continue");
                        Console.ReadKey(true);
                    }
                }while((Wplay.Length < 4) || (string.IsNullOrEmpty(Wplay)) || (Wplay.Length > 6));

                Wplay = Tools.algebra2Move(Wplay);
                Console.WriteLine(Wplay.Length);
                Console.WriteLine("play is");
                Console.ReadKey(true);

                char[] Pmoves = new char[4]; //create array to send to makeMove method
                char[] Cmoves = new char[4]; //create array to send to makeMove method
                Pmoves[0] = Wplay[0]; Pmoves[1] = Wplay[1]; Pmoves[2] = Wplay[2]; Pmoves[3] = Wplay[3]; // put char into array for makeMove method
                string moves;
                moves = Moves.possibleMovesW(bKB, bQB, bRB, bBB, bNB, bPB, wKB, wQB, wRB, wBB, wNB, wPB, EPB, castleWKside, castleWQside, castleBKside, castleBQside);
                if( !AnyLegalMove(bKB, bQB, bRB, bBB, bNB, bPB, wKB, wQB, wRB, wBB, wNB, wPB, moves, "white")){
                    blackwon = true;
                    break;
                }
                for (int i = 0; i < moves.Length; i+=4){ // every move is 4 char so length of movesString / 4 is how many moves possibleMoves method found
                    Cmoves[0] = moves[i]; Cmoves[1] = moves[i+1]; Cmoves[2] = moves[i+2]; Cmoves[3] = moves[i+3]; // put char into array for makeMove method
                    if (Tools.ArrC(Pmoves, Cmoves)){ // the move is a possible move
                        if(checkMove(bKB, bQB, bRB, bBB, bNB, bPB, wKB, wQB, wRB, wBB, wNB, wPB, Pmoves, "white")){// check if the move causes king in check
                            kingsafe = true;
                            int start = (((Pmoves[0] - '0') * 8) + (Pmoves[1] - '0')); // set start location for each move i counts by 4 to get moves
                            int end = (((Pmoves[2] - '0') * 8) + (Pmoves[3] - '0')); // get end for every move

                            if ((((UInt64)1 << start) & wKB) != 0) { // if wking moved set false flags
                                castleWKside = false;
                                castleWQside = false;
                            }
                            if (((((UInt64)1 << start) & wRB) & ((UInt64)1 << 63)) != 0) { // if wrook kingside moves set kingside flag false
                                castleWKside = false;
                            }
                            if (((((UInt64)1 << start) & wRB) & ((UInt64)1 << 56)) != 0) { // if wrook queenside moves set queenside flag false
                                castleWQside = false;
                            }

                            // send all piece bitboards to makeMove to make a move or get removed if piece is attacked
                            EPB = Moves.makeMoveEP(wPB|bPB, Cmoves, start); // set the en passant mask EPBt to a file if a pawn moved 2 steps // else set it 0 so only round after double move have valid EP move
                            wRB = Moves.CastleMove(wRB, wKB, Cmoves, 'R', start); // checks if the king made a castle move if true then moves the rook
                            bKB = Moves.makeMove(bKB, Cmoves, 'k', start, end);
                            bQB = Moves.makeMove(bQB, Cmoves, 'q', start, end);
                            bRB = Moves.makeMove(bRB, Cmoves, 'r', start, end);
                            bBB = Moves.makeMove(bBB, Cmoves, 'b', start, end);
                            bNB = Moves.makeMove(bNB, Cmoves, 'n', start, end);
                            bPB = Moves.makeMove(bPB, Cmoves, 'p', start, end);
                            wKB = Moves.makeMove(wKB, Cmoves, 'K', start, end);
                            wQB = Moves.makeMove(wQB, Cmoves, 'Q', start, end);
                            wRB = Moves.makeMove(wRB, Cmoves, 'R', start, end);
                            wBB = Moves.makeMove(wBB, Cmoves, 'B', start, end);
                            wNB = Moves.makeMove(wNB, Cmoves, 'N', start, end);
                            wPB = Moves.makeMove(wPB, Cmoves, 'P', start, end);

                            break;
                        }
                    }
                }
                if(!Tools.ArrC(Pmoves, Cmoves)){ // continue while loop if invalid move or king check so not blacks turn
                    if(!checkMove(bKB, bQB, bRB, bBB, bNB, bPB, wKB, wQB, wRB, wBB, wNB, wPB, Pmoves, "white")){
                        kingsafe = false;
                        Console.Write("king is in check");
                        Console.ReadKey();
                        continue;
                    }
                    Console.Write("move not found in valid moves");
                    Console.ReadKey();
                    continue;
                }


                /////// BLACK COMPUTER STARTS HERE ///////////////////
                moves = Moves.possibleMovesB(bKB, bQB, bRB, bBB, bNB, bPB, wKB, wQB, wRB, wBB, wNB, wPB, EPB, castleWKside, castleWQside, castleBKside, castleBQside);
                if( !AnyLegalMove(bKB, bQB, bRB, bBB, bNB, bPB, wKB, wQB, wRB, wBB, wNB, wPB, moves, "black")){
                    whitewon = true;
                    break;
                }

                Random r = new Random();
                do{// check if the move causes king in check

                    int length = moves.Length/4;
                    int randomMoveN = r.Next(0, length)*4;
                    Cmoves[0] = moves[randomMoveN]; Cmoves[1] = moves[randomMoveN+1]; Cmoves[2] = moves[randomMoveN+2]; Cmoves[3] = moves[randomMoveN+3]; // put char into array for makeMove method

                }while(!checkMove(bKB, bQB, bRB, bBB, bNB, bPB, wKB, wQB, wRB, wBB, wNB, wPB, Cmoves, "black"));

                int startpc = (((Cmoves[0] - '0') * 8) + (Cmoves[1] - '0')); // set start location for each move i counts by 4 to get moves
                int endpc = (((Cmoves[2] - '0') * 8) + (Cmoves[3] - '0')); // get end for every move

                if ((((UInt64)1 << startpc) & bKB) != 0) { // if bking moved set false flags
                    castleBKside = false;
                    castleBQside = false;
                }
                if (((((UInt64)1 << startpc) & bRB) & ((UInt64)1 << 7)) != 0) { // if brook kingside moves set king side flag false
                    castleBKside = false;
                }
                if (((((UInt64)1 << startpc) & bRB) & (UInt64)1) != 0) { // if brook queenside moves set queensdie flag false
                    castleBQside = false;
                }

                // send all piece bitboards to makeMove to make a move or get removed if piece is attacked
                EPB = Moves.makeMoveEP(wPB|bPB, Cmoves, startpc); // set the en passant mask EPBt to a file if a pawn moved 2 steps // else set it 0 so only round after double move have valid EP move
                bRB = Moves.CastleMove(bRB, bKB, Cmoves, 'r', startpc);
                bKB = Moves.makeMove(bKB, Cmoves, 'k', startpc, endpc);
                bQB = Moves.makeMove(bQB, Cmoves, 'q', startpc, endpc);
                bRB = Moves.makeMove(bRB, Cmoves, 'r', startpc, endpc);
                bBB = Moves.makeMove(bBB, Cmoves, 'b', startpc, endpc);
                bNB = Moves.makeMove(bNB, Cmoves, 'n', startpc, endpc);
                bPB = Moves.makeMove(bPB, Cmoves, 'p', startpc, endpc);
                wKB = Moves.makeMove(wKB, Cmoves, 'K', startpc, endpc);
                wQB = Moves.makeMove(wQB, Cmoves, 'Q', startpc, endpc);
                wRB = Moves.makeMove(wRB, Cmoves, 'R', startpc, endpc);
                wBB = Moves.makeMove(wBB, Cmoves, 'B', startpc, endpc);
                wNB = Moves.makeMove(wNB, Cmoves, 'N', startpc, endpc);
                wPB = Moves.makeMove(wPB, Cmoves, 'P', startpc, endpc);


            }
            if(blackwon)
                Console.WriteLine("You lost against a stupid computer!");
            if(whitewon)
                Console.WriteLine("You won against the stupid computer!");
        }

        public static void PrintHelp(){
            Console.WriteLine("To move a piece first select the piece with algebraic notation.");
            Console.WriteLine("Example: b2b4 meaning piece at b2 square, move to b4.");
            Console.WriteLine("To castle king move the king two squares left or right.");
            Console.WriteLine("To promote a pawn type the move and end with a 'p'.");
            Console.WriteLine("Example: f7g8pq meaning attacking right and promoting to queen.");
            Console.WriteLine("You can promote the pawn to a (q)queen, (r)rook, (k)knight or (b)bishop.");
            Console.WriteLine("En passant moves are made by moving behind enemy pawn and ending with a 'e'");
            Console.WriteLine("Example: a5b6e");
        }


        public static bool AnyLegalMove(UInt64 bKB, UInt64 bQB, UInt64 bRB, UInt64 bBB, UInt64 bNB, UInt64 bPB, UInt64 wKB, UInt64 wQB, UInt64 wRB, UInt64 wBB, UInt64 wNB, UInt64 wPB, string moves, string player){
            char[] Cmoves = new char[4];
            for (int i = 0; i < moves.Length; i+=4){
                Cmoves[0] = moves[i]; Cmoves[1] = moves[i+1]; Cmoves[2] = moves[i+2]; Cmoves[3] = moves[i+3];
                if(checkMove(bKB, bQB, bRB, bBB, bNB, bPB, wKB, wQB, wRB, wBB, wNB, wPB, Cmoves, player)){
                    return true;
                }
            }
            return false;
        }


        public static bool checkMove(UInt64 bKB, UInt64 bQB, UInt64 bRB, UInt64 bBB, UInt64 bNB, UInt64 bPB, UInt64 wKB, UInt64 wQB, UInt64 wRB, UInt64 wBB, UInt64 wNB, UInt64 wPB, char[] Pmoves, string player){
            int start = (((Pmoves[0] - '0') * 8) + (Pmoves[1] - '0')); // set start location for each move i counts by 4 to get moves
            int end = (((Pmoves[2] - '0') * 8) + (Pmoves[3] - '0')); // get end for every move

            // send all piece bitboards to makeMove to make a move or get removed if piece is attacked
            UInt64 bKBt = Moves.makeMove(bKB, Pmoves, 'k', start, end),
                bQBt = Moves.makeMove(bQB, Pmoves, 'q', start, end),
                bRBt = Moves.makeMove(bRB, Pmoves, 'r', start, end),
                bBBt = Moves.makeMove(bBB, Pmoves, 'b', start, end),
                bNBt = Moves.makeMove(bNB, Pmoves, 'n', start, end),
                bPBt = Moves.makeMove(bPB, Pmoves, 'p', start, end),
                wKBt = Moves.makeMove(wKB, Pmoves, 'K', start, end),
                wQBt = Moves.makeMove(wQB, Pmoves, 'Q', start, end),
                wRBt = Moves.makeMove(wRB, Pmoves, 'R', start, end),
                wBBt = Moves.makeMove(wBB, Pmoves, 'B', start, end),
                wNBt = Moves.makeMove(wNB, Pmoves, 'N', start, end),
                wPBt = Moves.makeMove(wPB, Pmoves, 'P', start, end),
                EPBt = Moves.makeMoveEP(wPB|bPB, Pmoves, start); // set the en passant mask EPBt to a file if a pawn moved 2 steps // else set it 0 so only round after double move have valid EP move

            wRBt = Moves.CastleMove(wRBt, wKB, Pmoves, 'R', start); // checks if the king made a castle move if true then moves the rook
            bRBt = Moves.CastleMove(bRBt, bKB, Pmoves, 'r', start);

            if(player == "white"){
                if ((wKBt & Moves.unsafeWhite(bKBt, bQBt, bRBt, bBBt, bNBt, bPBt, wKBt, wQBt, wRBt, wBBt, wNBt, wPBt)) == 0){ // check if white king is in check
                    return true;
                } else {
                    return false;
                }
            } else{
                if ((bKBt & Moves.unsafeBlack(bKBt, bQBt, bRBt, bBBt, bNBt, bPBt, wKBt, wQBt, wRBt, wBBt, wNBt, wPBt)) == 0){ // check if black king is in check
                    return true;
                } else {
                    return false;
                }
            }
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
                Console.Write(8-i + " ");
                for (int k = 0; k < 8; k++){
                    Console.Write($"[{chessBoard[i,k]}]\u2009");
                    if(k == 7) Console.WriteLine();
                }
                Console.WriteLine();
            }
            Console.WriteLine("  (a) (b) (c) (d) (e) (f) (g) (h)");

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
            Console.WriteLine();

        }

        public static string Reverse( string s ){
            char[] charArray = s.ToCharArray();
            Array.Reverse( charArray );
            return new string( charArray );
        }
    }
}

// string print = Moves.possibleMovesW("1636", bKB, bQB, bRB, bBB, bNB, bPB, wKB, wQB, wRB, wBB, wNB, wPB);
// Console.WriteLine($":{print}:");
// for(int i = 0; i<8;i++){
// drawBitboard(Moves.kingSpan);
// }
// Console.Write(Moves.possibleMovesW("", bKB, bQB, bRB, bBB, bNB, bPB, wKB, wQB, wRB, wBB, wNB, wPB).Length/4);
// UInt64 One = 1;
// UInt64 number = One << 30;
// Console.WriteLine(number);
// Console.WriteLine(Moves.reverseBitSingle(number));
// Console.WriteLine(Moves.reverseBit(number));
// Moves.possibleMovesB(ulong bKB, ulong bQB, ulong bRB, ulong bBB, ulong bNB, ulong bPB, ulong wKB, ulong wQB, ulong wRB, ulong wBB, ulong wNB, ulong wPB, ulong EPB, bool castleWKside, bool castleWQside, bool castleBKside, bool castleBQside)
// string movez = Moves.possibleMovesB(bKB, bQB, bRB, bBB, bNB, bPB, wKB, wQB, wRB, wBB, wNB, wPB, EPB, castleWKside, castleWQside, castleBKside, castleBQside);
// for (int i = 0; i < movez.Length; i+=4){
//             Console.WriteLine(Perft.move2Algebra(movez.Substring(i, 4)));
// }
// Moves.makeMove((ulong)123, "6151", wPC);
//     var sw = System.Diagnostics.Stopwatch.StartNew();
//     for(int index = 0; index < 500000; index++)
//     {
//            Console.WriteLine(Moves.possibleMovesW(bKB, bQB, bRB, bBB, bNB, bPB, wKB, wQB, wRB, wBB, wNB, wPB, EPB, castleWKside, castleWQside, castleBKside, castleBQside));
//         Moves.reverseBit(number);
//     }
//     sw.Stop();
//     var elapsed = sw.ElapsedMilliseconds;
//     Console.WriteLine(elapsed);
/*
  var sw = System.Diagnostics.Stopwatch.StartNew();
  Perft.perftRoot(bKB, bQB, bRB, bBB, bNB, bPB, wKB, wQB, wRB, wBB, wNB, wPB, EPB, castleWKside, castleWQside, castleBKside, castleBQside, white2Move, 0);
  sw.Stop();
  var elapsed = sw.ElapsedMilliseconds;
  Console.WriteLine($"{elapsed} ms");
  Console.WriteLine(Perft.perftTotalCount);

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
  drawArray(bKB, bQB, bRB, bBB, bNB, bPB, wKB, wQB, wRB, wBB, wNB, wPB);
  }
*/
