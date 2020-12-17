using System;
// using System.Collections.Generic;
// using System.Linq;
using System.Text;
// using System.Threading.Tasks;

namespace ChessBitboard{
    public class BoardGeneration{

        // unicode karakter for hver skakbrik
        public const char bKC = '\u2654'; // sort konge unicode
        public const char bQC = '\u2655'; // sort dronning unicode
        public const char bRC = '\u2656'; // sort tarn unicode
        public const char bBC = '\u2657'; // sort lober unicde
        public const char bNC = '\u2658'; // sort hest unicode
        public const char bPC = '\u2659'; // sort bonde uni
        public const char wKC = '\u265a'; // hvid konge uni
        public const char wQC = '\u265b'; // hvid dronnign uni
        public const char wRC = '\u265c'; // hvid tarn uni
        public const char wBC = '\u265d'; // hvid lober uni
        public const char wNC = '\u265e'; // hvid hest uni
        public const char wPC = '\u265f'; // hvid bonde uni
        public const char eC = '\u0020'; // tom unicode


        // kongerokade flag
        public static bool castleWKside = true; // hvid konge side
        public static bool castleWQside = true; // hvid dronnig side
        public static bool castleBKside = true; // sort konge side
        public static bool castleBQside = true; // sort dronnig side

        public static bool white2Move = true; // hvids tur
        public static UInt64 EPB = 0; // en passant maske bliver sat af makeMoveEP()

        public static void initiateStdChess(){ // starter skakspillet og sætter bitboards til hvad positionerne i arrayet er
            // 12 bitboards 1 for hver brik
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


            char[,] chessBoard = new char[,] // 2 dimensional array for startposition
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
                {bRC,bNC,bBC,bQC,bKC,bBC,bNC,bRC}, // startposition for spillet
                {bPC,bPC,bPC,bPC,bPC,bPC,bPC,bPC},
                {eC,eC,eC,eC,eC,eC,eC,eC},
                {eC,eC,eC,eC,eC,eC,eC,eC},
                {eC,eC,eC,eC,eC,eC,eC,eC},
                {eC,eC,eC,eC,eC,eC,eC,eC},
                {wPC,wPC,wPC,wPC,wPC,wPC,wPC,wPC},
                {wRC,wNC,wBC,wQC,wKC,wBC,wNC,wRC},
            };

            // putter bit i korrekt position for hver briks bitboard
            string binaryString; // deklerer binarære streng
            for (int i = 0; i<64;i++){ // looper 64 gange, 1 for hver bit
                binaryString = "0000000000000000000000000000000000000000000000000000000000000000"; // start streng bitboard
                binaryString = binaryString.Substring(i +1) + "1" + binaryString.Substring(0, i); // for hver loop sæt 1 ind fra højre. første runde substring er fra index 1 til 63 + '1' plus substring 0,0 giver 0
                switch(chessBoard[i/8,i%8]){ // i/8 giver hvilken rank og i mod 8 giver hvilken file index i arrayet, som den binære string hører sammen med
                    case bRC: // hvis indexet i arrayet er sort rook char
                        bRB += convertString2Bitboard(binaryString); // adder med strengen koverteret til unsigned int 64
                        break; // break ud af case
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


            bool kingsafe = true; // bool om kongen er sikker
            bool whitewon = false; //bool om hvid har vundet
            bool blackwon = false; // bool om sort har vunder
            bool qgame = false; // bool om spilleren har valgt q for at stoppe programmet/spillet
            while(true){ // program loop, kører indtil en break
                string Wplay = ""; // spillerens play string
                do{ // do while loop for at få korrekt play af spilleren og printer boardet
                    Console.Clear(); // renser konsol vindue
                    drawArray(bKB, bQB, bRB, bBB, bNB, bPB, wKB, wQB, wRB, wBB, wNB, wPB); // kalder drawArray for at printe et skakbræt ud fra tilstanden af alle bitboards
                    if(!kingsafe){ // hvis kongen ikke er sikker informer spilleren
                        Console.WriteLine("Your move made your king in check");
                        Console.WriteLine("Try another move: ");
                        Console.WriteLine("Type 'h' for help");
                    }else{ // ellers print insert play og hvordan hjælpemenu tilgås
                        Console.WriteLine("Insert play: ");
                        Console.WriteLine("Type 'h' for help");
                    }
                    Wplay = Console.ReadLine().ToLower(); // tager spilleren input og laver lower så caps lock gælder
                    if(!string.IsNullOrEmpty(Wplay) && Char.ToLower(Wplay[0]) == 'q'){ // hvis strengen ikke er tom og index 0 af playet er q
                        qgame = true; // sæt qgame til true
                    }
                    if(!(string.IsNullOrEmpty(Wplay)) && Wplay[0] == 'h' && Wplay.Length == 1){ // hvis strengen ikke er tom og index 0 er h og længden er 1
                        PrintHelp(); //kald print hjælpemenu
                        Console.Write("\nPress a key to continue");
                        Console.ReadKey(true); // venter på keypress fra spiller
                    }
                }while(!qgame && ((Wplay.Length < 4) || (string.IsNullOrEmpty(Wplay)) || (Wplay.Length > 6))); // do while kører så længe qgame ikke er false og længden er under 4 eller tom eller mere end 6 lang.
                if(qgame){ // hvis qgame er true
                    break; // break ud af spillet
                }

                Wplay = Tools.algebra2Move(Wplay); // laver player inputtet til programmets interne koordinatsystem
                /*
                  Console.WriteLine(Wplay.Length);
                  Console.WriteLine(Wplay);
                  Console.WriteLine("play is");
                  Console.ReadKey(true);
                */
                char[] Pmoves = new char[4]; // laver char array for at holde på trækket
                char[] Cmoves = new char[4]; // laver char array for at holde på hvert muligt træk fundet af possibleMovesW
                Pmoves[0] = Wplay[0]; Pmoves[1] = Wplay[1]; Pmoves[2] = Wplay[2]; Pmoves[3] = Wplay[3]; // putter hvert char i streng ind i char array
                string moves; // streng der holder på alle mulige træk
                moves = Moves.possibleMovesW(bKB, bQB, bRB, bBB, bNB, bPB, wKB, wQB, wRB, wBB, wNB, wPB, EPB, castleWKside, castleWQside, castleBKside, castleBQside); // kalder metode der finder alle mulige træk
                if( !AnyLegalMove(bKB, bQB, bRB, bBB, bNB, bPB, wKB, wQB, wRB, wBB, wNB, wPB, moves, "white")){ // kalder metode der checker om der findes nogen træk
                    blackwon = true; // sætter blackwon til true så korret meddelse kan printes efter break
                    break; // breaker ud af program
                }
                for (int i = 0; i < moves.Length; i+=4){ // hver move er 4 chars så længden af moves divideret med 4 er antal træk(derfor i incrementer med 4)
                    Cmoves[0] = moves[i]; Cmoves[1] = moves[i+1]; Cmoves[2] = moves[i+2]; Cmoves[3] = moves[i+3]; // putter hver muligt move ind i Cmoves char arrayet
                    if (Tools.ArrC(Pmoves, Cmoves)){ // checker hvert move om spillerens move er ens med en af de mulige træk
                        if(checkMove(bKB, bQB, bRB, bBB, bNB, bPB, wKB, wQB, wRB, wBB, wNB, wPB, Pmoves, "white")){// checker om trækket for kongen til at blive i skak
                            kingsafe = true; // sætter kingsafe til true
                            int start = (((Pmoves[0] - '0') * 8) + (Pmoves[1] - '0')); // sætter start index ud fra trækket(minus '0' char konvertere char tallet til int automatisk)
                            int end = (((Pmoves[2] - '0') * 8) + (Pmoves[3] - '0'));   // sætter slut index ud fra trækket(minus '0' char konvertere char tallet til int automatisk)

                            if ((((UInt64)1 << start) & wKB) != 0) { // hvis hvid konge bevæger sig set kongerokademuligheder til false
                                castleWKside = false;
                                castleWQside = false;
                            }
                            if (((((UInt64)1 << start) & wRB) & ((UInt64)1 << 63)) != 0) { // hvis hvid tårn på index 63(højre) bevæger sig set kongesiden til false
                                castleWKside = false;
                            }
                            if (((((UInt64)1 << start) & wRB) & ((UInt64)1 << 56)) != 0) { // hvis dronnigside tårnet bevæger sig set hvid dronning kongerokade til false
                                castleWQside = false;
                            }

                            // sender alle brikkers bitboard til makeMove metoden for at ændre deres bitboard afhængig af hvad trækket er
                            EPB = Moves.makeMoveEP(wPB|bPB, Cmoves, start); // sætter en passant masken EPB til den til den fil hvor en bonde bevæger sig 2 ranks op, ellers sætter den 0 så den kun masker en runde
                            wRB = Moves.CastleMove(wRB, wKB, Cmoves, 'R', start); // checker om kongen bevæger sig 2 træk og sætter tårnets position ud fra det
                            bKB = Moves.makeMove(bKB, Cmoves, 'k', start, end); // sætter sort konge bitboard til retur værdi af makeMove
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

                            break; // breaker ud af for loop da trækket blev fundet og resten af moves er ubetydelig
                        }
                    }
                }

                // Det her er noget rod
                if(!Tools.ArrC(Pmoves, Cmoves)){ // hvis trækket ikke er i Cmoves       continue while loop if invalid move or king check so not blacks turn
                    if(!checkMove(bKB, bQB, bRB, bBB, bNB, bPB, wKB, wQB, wRB, wBB, wNB, wPB, Pmoves, "white")){ // hvis trækket får kongen i check
                        kingsafe = false; // sæt kingsafe false
                        Console.Write("king is in check");
                        Console.ReadKey();
                        continue; // continue loop så turen ikke går til sort/computer hvis trækket er invalid
                    }
                    Console.Write("move not found in valid moves"); // trækket er invalid
                    Console.ReadKey();
                    continue;// continue loop så turen ikke går til sort/computer hvis trækket er invalid
                }


                /////// BLACK COMPUTER STARTS HERE ///////////////////
                moves = Moves.possibleMovesB(bKB, bQB, bRB, bBB, bNB, bPB, wKB, wQB, wRB, wBB, wNB, wPB, EPB, castleWKside, castleWQside, castleBKside, castleBQside); // kalder possibleMovesB som finder alle mulige træk og sætter i strengen moves
                if( !AnyLegalMove(bKB, bQB, bRB, bBB, bNB, bPB, wKB, wQB, wRB, wBB, wNB, wPB, moves, "black")){ // hvis computeren ikke har nogen lovlige træk break ud og sæt whitewon true
                    whitewon = true;
                    break;
                }

                Random r = new Random(); // laver random object
                do{// do while loop der prøver tilfælde træk indtil en der er valid

                    int length = moves.Length/4; // hvor mange moves der er
                    int randomMoveN = r.Next(0, length)*4; // tager random tal fra 0 til antal moves og ganger med 4 for at få start index på trækket
                    Cmoves[0] = moves[randomMoveN]; Cmoves[1] = moves[randomMoveN+1]; Cmoves[2] = moves[randomMoveN+2]; Cmoves[3] = moves[randomMoveN+3]; // putter det random move ind i Cmoves arrayet

                }while(!checkMove(bKB, bQB, bRB, bBB, bNB, bPB, wKB, wQB, wRB, wBB, wNB, wPB, Cmoves, "black")); // checker trækket


                //
                // det næste er kommenteret når hvid laver et træk
                //

                int startpc = (((Cmoves[0] - '0') * 8) + (Cmoves[1] - '0'));
                int endpc = (((Cmoves[2] - '0') * 8) + (Cmoves[3] - '0'));

                if ((((UInt64)1 << startpc) & bKB) != 0) {
                    castleBKside = false;
                    castleBQside = false;
                }
                if (((((UInt64)1 << startpc) & bRB) & ((UInt64)1 << 7)) != 0) {
                    castleBKside = false;
                }
                if (((((UInt64)1 << startpc) & bRB) & (UInt64)1) != 0) {
                    castleBQside = false;
                }


                EPB = Moves.makeMoveEP(wPB|bPB, Cmoves, startpc);
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


            } // while loop slutter her

            if(blackwon){ // hvis blackwon er true
                Console.WriteLine("You lost against a stupid computer!");
                Console.Read(true);
            }
            else if(whitewon){ // hvis whitewon er true
                Console.WriteLine("You won against a stupid computer!");
                Console.Read(true);
            }
            else if (qgame){ // hvis qgame er true
                Console.WriteLine("The game ended before a winner was found!");
                Console.Read(true);
            }
        } // initiatestdchess metode slutter her

        public static void PrintHelp(){
            Console.WriteLine("To move a piece first select the piece with algebraic notation.");
            Console.WriteLine("Example: b2b4 meaning piece at b2 square, move to b4.");
            Console.WriteLine("To castle king move the king two squares left or right.");
            Console.WriteLine("To promote a pawn type the move and end with a 'p'.");
            Console.WriteLine("Example: f7g8pq meaning attacking right and promoting to queen.");
            Console.WriteLine("You can promote the pawn to a (q)queen, (r)rook, (k)knight or (b)bishop.");
            Console.WriteLine("En passant moves are made by moving behind enemy pawn and ending with a 'e'");
            Console.WriteLine("Example: a5b6e");
            Console.WriteLine("\nType 'q' to quit");
        }


        // tager en masse argumenter og checker en string af moves om nogen af dem er lovlige
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

        public static UInt64 convertString2Bitboard(string binary){ // metode konveterer binær tal skrevet i streng til tal
            return Convert.ToUInt64(binary, 2);
        }

        // printer skakbræt ud fra bitboards
        public static void drawArray(UInt64 bKB, UInt64 bQB, UInt64 bRB, UInt64 bBB, UInt64 bNB, UInt64 bPB, UInt64 wKB, UInt64 wQB, UInt64 wRB, UInt64 wBB, UInt64 wNB, UInt64 wPB){
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            char[,] chessBoard = new char[8,8]; // 2d array som fyldes ud med chars ud fra positionen af bits
            for (int i = 0; i < 64; i++){ // fylder array med tom char
                chessBoard[i/8,i%8] = eC;
            }
// kører 64 gange, 1 for hvert bit, hvert if statement shifter bitboardet med i og ANDer med 1 og ser om det er 1 - hvilket vil sige at på den plads er der en forekomst af i bitboardet på det index
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
            // her starter loopet der printer skakbrætet
            for (int i = 0; i < 8; i++){ // i loopet er for hver rank
                Console.Write(8-i + " "); // printer rank nummer
                for (int k = 0; k < 8; k++){ // k loopet er for hver file inden i rank i
                    Console.Write($"[{chessBoard[i,k]}]\u2009"); // udskriver char i chessBoard arrayet i index i,k (rank,file) og laver et halvt mellemrum som er \u2009 unicode
                    if(k == 7) Console.WriteLine(); // hvis k er 7 lav linje
                }
                Console.WriteLine(); // lav linje
            }
            Console.WriteLine("  (a) (b) (c) (d) (e) (f) (g) (h)"); // print file bogstaver

        }

// metode ikke brugt, men blev brugt nogen gange til debug og visuelt se forskellige bitboard under programforløb
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

    }
}

/*
        public static string Reverse( string s ){
            char[] charArray = s.ToCharArray();
            Array.Reverse( charArray );
            return new string( charArray );
        }
*/
