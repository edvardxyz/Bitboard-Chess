// TODO: use stringbuilder instead for the list and hist.
// strinbuilders do not have indexof and startswith method, might need?
// stringbuilder is going to be faster
// DONE: reverse bits faster
//
// Algo for sliders
// ' = reverse: o = occupied: r = rook:
// lineAttacks = ( o - 2r ) ^ reverse( o'- 2r')
// o - r removes the piece
// o -r again borrows from the next peice (attacks) and fills the traversed fields with 1s
// doing the same but reversing everything makes the slider go right (towards most least significant bit) ( board is from A8 = 1st bit H1 = 64th bit )
// https://www.chessprogramming.org/Hyperbola_Quintessence
//
using System;
// using System.Collections.Generic;
// using System.Linq;
using System.Text;
// using System.Threading.Tasks;
// using System.Globalization;

namespace ChessBitboard{

    public class Moves{

        public static UInt64 fileA = 0x101010101010101; // 1 on every field on A file
        public static UInt64 fileH = 0x8080808080808080; // 1 on H file
        public static UInt64 fileAB = 0x303030303030303; // 1 on file A & B
        public static UInt64 fileGH = 0xC0C0C0C0C0C0C0C0; // 1 on file H & H
        public static UInt64 rank1 = 0xFF00000000000000;
        public static UInt64 rank4 = 0xFF00000000;
        public static UInt64 rank5 = 0xFF000000;
        public static UInt64 rank8 = 0xFF;
        public static UInt64 kingSpan = 0xE0A0E00;
        public static UInt64 knightSpan = 0xA1100110A;
        public static UInt64 center = 0x1818000000;
        public static UInt64 centerBig = 0x3C3C3C3C0000;
        public static UInt64 kingSide = 0xF0F0F0F0F0F0F0F;
        public static UInt64 queenSide = 0xF0F0F0F0F0F0F0F0;
        public static UInt64 notMyPieces; // every piece that the side can not capture including oppesit side king to stop illegal capture
        public static UInt64 blackPieces; //black pieces except for black king // used for white pawns
        public static UInt64 whitePieces; //white pieces except for white king // used for black pawns
        public static UInt64 empty; // every field empty
        public static UInt64 occupied;
        public static UInt64[] Rankmasks8 = new UInt64[]{
            0xFF,0xFF00,0xFF0000,0xFF000000,0xFF00000000,0xFF0000000000,0xFF000000000000,0xFF00000000000000
        };

        public static UInt64[] Filemasks8 = new UInt64[]{
            0x101010101010101,0x202020202020202,0x404040404040404,0x808080808080808,0x1010101010101010,0x2020202020202020,0x4040404040404040,0x8080808080808080
        };

        public static UInt64[] Diagonalmasks8 = new UInt64[]{
            0x1, 0x102, 0x10204, 0x1020408, 0x102040810, 0x10204081020, 0x1020408102040,
            0x102040810204080, 0x204081020408000, 0x408102040800000, 0x810204080000000,
            0x1020408000000000, 0x2040800000000000, 0x4080000000000000, 0x8000000000000000
        };

        public static UInt64[] AntiDiagonalmasks8 = new UInt64[]{
            0x80, 0x8040, 0x804020, 0x80402010, 0x8040201008, 0x804020100804, 0x80402010080402,
            0x8040201008040201, 0x4020100804020100, 0x2010080402010000, 0x1008040201000000,
            0x804020100000000, 0x402010000000000, 0x201000000000000, 0x100000000000000
        };


        public static char unicode2ShortHand(char c){
            switch(c){
                case BoardGeneration.bQC:
                    return 'q';

                case BoardGeneration.wQC:
                    return 'Q';

                case BoardGeneration.bBC:
                    return 'b';

                case BoardGeneration.bRC:
                    return 'r';

                case BoardGeneration.bNC:
                    return 'n';

                case BoardGeneration.wRC:
                    return 'R';

                case BoardGeneration.wBC:
                    return 'B';

                case BoardGeneration.wNC:
                    return 'N';
                default:
                    return '0';
            }

        }

        public static UInt64 makeMove(UInt64 board, char[] moves, char type, int start){
            if ( Char.IsDigit(moves[3])){ //if last digit of move string is digit then its a normal move
                int end = (((moves[2] - '0') * 8) + (moves[3] - '0'));
                if (((board >> start) & 1) == 1) {
                    board = board & ~((UInt64)1 << start); // remove piece from start
                    board = board | ((UInt64)1 << end); // add the piece to the end
                }
                else{
                    board = board & ~((UInt64)1 << end);
                }
            }else if(moves[3] == 'P'){ // if last char in move string is P its a promotion
                int startp, endp;
                if (char.IsUpper(moves[2])){ // if 3 char is upper case then its white promoting a piece
                    startp = Tools.trailingZerosRight(Filemasks8[moves[0] - '0'] & Rankmasks8[6]); // get start position by ANDing rank and file - i only have start file and end file - i know a promotion for white can only happen from rank 7 to 8
                    endp = Tools.trailingZerosRight(Filemasks8[moves[1] - '0'] & Rankmasks8[7]);
                }else{
                    startp = Tools.trailingZerosRight(Filemasks8[moves[0] - '0'] & Rankmasks8[1]);
                    endp = Tools.trailingZerosRight(Filemasks8[moves[1] - '0'] & Rankmasks8[0]);
                }
                if(type == moves[2]) {
                    board = board & ~((UInt64)1 << startp); // remove piece from start
                    board = board | ((UInt64)1 << endp); // add the piece to the end
                }
            }
            else if (moves[3] == 'E'){ // if last char is E the move is en passant
                int starte, ende;
                if (moves[2] == 'W'){ // if 3 char is W then its white en passant
                    starte = Tools.trailingZerosRight(Filemasks8[moves[0] - '0'] & Rankmasks8[4]); // get start position by ANDing rank and file - i only have start file and end file - i know a promotion for white can only happen from rank 7 to 8
                    ende = Tools.trailingZerosRight(Filemasks8[moves[1] - '0'] & Rankmasks8[5]);
                    board = board & ~((UInt64)1 << (int)(Filemasks8[moves[1] - '0'] & Rankmasks8[4])); // remove taken pawn - file is move to and rank is the startering rank which is the rank opponent will be at
                }else{
                    starte = Tools.trailingZerosRight(Filemasks8[moves[0] - '0'] & Rankmasks8[3]);
                    ende = Tools.trailingZerosRight(Filemasks8[moves[1] - '0'] & Rankmasks8[2]);
                    board = board & ~((UInt64)1 << (int)(Filemasks8[moves[1] - '0'] & Rankmasks8[3])); // remove taken pawn
                }
                if (((board >> starte) & 1 ) == 1 ){ // if pieceBoard is equal to start location
                    board = board & ~((UInt64)1 << starte); // remove piece from start location
                    board = board | ((UInt64)1 << ende); // add pawn to end location (behind taken pawn)
                }
            } else {
                Console.WriteLine("Invalid move");
            }
            return board;
        }

        public static UInt64 CastleMove(UInt64 Rook, UInt64 King, string move, char type){
            int start = (((move[0] - '0') * 8) + (move[1] - '0'));
            if((((King >> start) & 1) == 1) && (("0402" == move) || ("0406" == move) || ("7472" == move) || ("7476" == move))){
                if(type == 'R'){
                    switch(move){
                        case "7472":
                            Rook = Rook & ~((UInt64)1 << 56); // remove piece from start
                            Rook = Rook | ((UInt64)1 << 59); // add the piece to d1 sq
                            break;
                        case "7476":
                            Rook = Rook & ~((UInt64)1 << 63); // remove piece from start
                            Rook = Rook | ((UInt64)1 << 61); // add the piece to f1 sq
                            break;
                    }
                } else if (type == 'r') {
                    switch(move){
                        case "0402":
                            Rook = Rook & ~((UInt64)1); // remove piece from start
                            Rook = Rook | ((UInt64)1 << 3); // add the piece to f8 sq
                            break;
                        case "0406":
                            Rook = Rook & ~((UInt64)1 << 7); // remove piece from start
                            Rook = Rook | ((UInt64)1 << 5); // add the piece to d1 sq
                            break;
                    }
                }
            }
            return Rook;
        }

        public static UInt64 makeMoveEP(UInt64 board, char[] moves, int start){
            if (Char.IsDigit(moves[3])){
                if ((Math.Abs(moves[0] - moves[2]) == 2) && (((board >> start) & 1) == 1)){// means it was pawn double push
                    return Filemasks8[moves[1] - '0'];
                }
            }
            return 0;
        }

        public static UInt64 HandVMoves(int square){
            UInt64 bitboardSq = (UInt64)1 << square;
            UInt64 revO = Tools.reverseBit(occupied);
            UInt64 revBitSq = Tools.reverseBitSingle(bitboardSq);
            //turn square number into binary bitboard representing piece
            // use ( o - 2r ) ^ reverse( reverse(o) - 2 * reverse(r)) for horizontal attacks
            // first part is attack toward MSB (right)
            UInt64 possibleH = (occupied - 2 * bitboardSq) ^ Tools.reverseBit(revO - 2 * revBitSq);
            // use ( o - 2r ) ^ reverse( reverse(o) - 2 * reverse(r)) same but with masksing occupied with the file the piece is on for vertical attacks
            UInt64 possibleV = ((occupied & Filemasks8[square % 8]) - (2 * bitboardSq)) ^ Tools.reverseBit(Tools.reverseBit(occupied & Filemasks8[square % 8]) - (2 * revBitSq));
            return (possibleH & Rankmasks8[square / 8]) | (possibleV & Filemasks8[square % 8]); // & with masks and | to combine vertial and horizontal
        }

        public static UInt64 DandAntiDMoves(int square){
            UInt64 bitboardSq = (UInt64)1 << square;
            UInt64 revBitSq = Tools.reverseBitSingle(bitboardSq);
            UInt64 possibleD = ((occupied & Diagonalmasks8[(square / 8) + (square % 8)]) - (2 * bitboardSq)) ^ Tools.reverseBit(Tools.reverseBit(occupied & Diagonalmasks8[(square / 8) + (square % 8)]) - (2 * revBitSq));
            UInt64 possibleAntiD = ((occupied & AntiDiagonalmasks8[(square / 8) + 7 - (square % 8)]) - (2 * bitboardSq)) ^ Tools.reverseBit(Tools.reverseBit(occupied & AntiDiagonalmasks8[(square / 8) + 7 - (square % 8)]) - (2 * revBitSq));
            return (possibleD & Diagonalmasks8[(square / 8) + (square % 8)]) | (possibleAntiD & AntiDiagonalmasks8[(square / 8) + 7 - (square % 8)]);
        }

        private static StringBuilder list = new StringBuilder();

        public static string possibleMovesW(UInt64 bKB, UInt64 bQB, UInt64 bRB, UInt64 bBB, UInt64 bNB, UInt64 bPB, UInt64 wKB, UInt64 wQB, UInt64 wRB, UInt64 wBB, UInt64 wNB, UInt64 wPB, UInt64 EPB, bool castleWKside, bool castleWQside, bool castleBKside, bool castleBQside){
            notMyPieces=~(wKB|wQB|wRB|wNB|wBB|wPB|bKB); // or'd every white piece and black king to indicate what the white pieces cant capture, also black king to avoid illegal capture
            blackPieces = bQB|bRB|bNB|bBB|bPB; // all the black pieces without king to avoid illegal capture
            occupied = bKB|bQB|bRB|bBB|bNB|bPB|wKB|wQB|wRB|wBB|wNB|wPB; // or all pieces together to get occupied
            empty = ~occupied;// indicates empty fields with a 1 with flip ~ of occupied

            list.Clear();

            possibleWP(wPB, bPB, EPB);
            possibleB(occupied, wBB);
            possibleR(occupied, wRB);
            possibleQ(occupied, wQB);
            possibleN(occupied, wNB);
            possibleK(occupied, wKB);
            possibleCW(bKB, bQB, bRB, bBB, bNB, bPB, wKB, wQB, wRB, wBB, wNB, wPB, castleWKside, castleWQside);

            return list.ToString();
        }
        public static string possibleMovesB(UInt64 bKB, UInt64 bQB, UInt64 bRB, UInt64 bBB, UInt64 bNB, UInt64 bPB, UInt64 wKB, UInt64 wQB, UInt64 wRB, UInt64 wBB, UInt64 wNB, UInt64 wPB, UInt64 EPB, bool castleWKside, bool castleWQside, bool castleBKside, bool castleBQside){
            notMyPieces=~(bKB|bQB|bRB|bNB|bBB|bPB|wKB);
            whitePieces = wQB|wRB|wNB|wBB|wPB; // all the white pieces without king to avoid illegal capture
            occupied = bKB|bQB|bRB|bBB|bNB|bPB|wKB|wQB|wRB|wBB|wNB|wPB; // or all pieces together to get occupied
            empty = ~occupied; // indicates empty fields with a 1 with flip ~ of occupied

            list.Clear();

            possibleBP(bPB, wPB, EPB);
            possibleB(occupied, bBB);
            possibleR(occupied, bRB);
            possibleQ(occupied, bQB);
            possibleN(occupied, bNB);
            possibleK(occupied, bKB);
            possibleCB(bKB, bQB, bRB, bBB, bNB, bPB, wKB, wQB, wRB, wBB, wNB, wPB, castleBKside, castleBQside);

            return list.ToString();
        }

        public static void possibleCW(UInt64 bKB, UInt64 bQB, UInt64 bRB, UInt64 bBB, UInt64 bNB, UInt64 bPB, UInt64 wKB, UInt64 wQB, UInt64 wRB, UInt64 wBB, UInt64 wNB, UInt64 wPB, bool castleWKside, bool castleWQside){
            UInt64 unsafeSq = unsafeWhite(bKB, bQB, bRB, bBB, bNB, bPB, wKB, wQB, wRB, wBB, wNB, wPB);
            if ((unsafeSq & wKB) == 0 ){
                if (castleWKside == true && ((((UInt64)1 << 63) & wRB) == 1)){
                    if ((occupied & (((UInt64)1 << 61) | ((UInt64)1 << 62))) == 0){
                        list.Append("7476");
                    }
                }

                if (castleWQside == true && ((((UInt64)1 << 56) & wRB) == 1)){
                    if ((occupied & (((UInt64)1 << 57) | ((UInt64)1 << 58) | ((UInt64)1 << 59))) == 0){
                        list.Append("7472");
                    }
                }
            }
        }

        public static void possibleCB(UInt64 bKB, UInt64 bQB, UInt64 bRB, UInt64 bBB, UInt64 bNB, UInt64 bPB, UInt64 wKB, UInt64 wQB, UInt64 wRB, UInt64 wBB, UInt64 wNB, UInt64 wPB, bool castleBKside, bool castleBQside){
            UInt64 unsafeSq = unsafeBlack(bKB, bQB, bRB, bBB, bNB, bPB, wKB, wQB, wRB, wBB, wNB, wPB);
            if ((unsafeSq & bKB) == 0){
                if (castleBKside == true && ((((UInt64)1 << 7) & bRB) == 1)){
                    if ((occupied & (((UInt64)1 << 5) | ((UInt64)1 << 6))) == 0){
                        list.Append("0406");
                    }
                }

                if (castleBQside == true && (((UInt64)1 & bRB) == 1)){
                    if ((occupied & (((UInt64)1 << 1) | ((UInt64)1 << 2) | ((UInt64)1 << 3))) == 0){
                        list.Append("0402");
                    }
                }
            }
        }

        public static void possibleN(UInt64 occupied, UInt64 N){
            UInt64 i = N & ~(N-1); // get first knight to check for moves
            UInt64 possible;

            while(i != 0){
                int iLocation = Tools.trailingZerosRight(i); // get number of zeroes until first 1 from left to right - the number is equal to the index on board of knight

                if(iLocation > 18){
                    possible = knightSpan << (iLocation-18);
                }else {
                    possible = knightSpan >> (18-iLocation);
                }

                if(iLocation % 8<4){
                    possible = possible & ~fileGH & notMyPieces;
                }else{
                    possible = possible & ~fileAB & notMyPieces;
                }

                UInt64 k = possible & ~(possible-1); // get one of the possiblities
                while (k != 0){ // goes trough each of the possibilies
                    int index = Tools.trailingZerosRight(k); // get index of move to
                    list.Append(iLocation / 8).Append(iLocation % 8).Append(index / 8).Append(index % 8); // iLocation where the knight is and index where the knight can move to
                    possible = possible & ~k; // remove the move from possibiliies that was just listed
                    k = possible & ~(possible-1); // get the next move possible
                }
                N = N & ~i; // remove the knight that was checked for all moves
                i = N & ~(N-1); // get the next knight to check for all moves
            }
            // int temp = list.Length/4;
            // Console.WriteLine(temp);
        }

        public static void possibleK(UInt64 occupied, UInt64 K){
            UInt64 possible;

            int iLocation = Tools.trailingZerosRight(K); // get number of zeroes until first 1 from left to right - the number is equal to the index on board of knight

            if(iLocation > 18){
                possible = kingSpan << (iLocation-18);
            }else {
                possible = kingSpan >> (18-iLocation);
            }

            if(iLocation % 8 < 4){
                possible = possible & ~fileGH & notMyPieces;
            }else{
                possible = possible & ~fileAB & notMyPieces;
            }
            // BoardGeneration.drawBitboard(possible);

            UInt64 i = possible & ~(possible-1); // get one of the possiblities
            while (i != 0){ // goes trough each of the possibilies
                int index = Tools.trailingZerosRight(i); // get index of move to
                list.Append(iLocation / 8).Append(iLocation % 8).Append(index / 8).Append(index % 8); // iLocation where the knight is and index where the knight can move to
                possible = possible & ~i; // remove the move from possibiliies that was just listed
                i = possible & ~(possible-1); // get the next move possible
            }
            // int temp = list.Length/4;
            // Console.WriteLine(temp);
        }

        public static void possibleQ(UInt64 occupied, UInt64 Q){
            UInt64 i = Q & ~(Q-1); // get first queen to check for moves
            UInt64 possible;

            while(i != 0){
                int iLocation = Tools.trailingZerosRight(i); // get number of zeroes until first 1 from left to right - the number is equal to the index on board of queen
                possible = (DandAntiDMoves(iLocation) | HandVMoves(iLocation)) & notMyPieces; // get diagonal and antidiagonal moves for the selected piece and with not my pieces
                UInt64 k = possible & ~(possible-1); // get one of the possiblities
                while (k != 0){ // goes trough each of the possibilies
                    int index = Tools.trailingZerosRight(k); // get index of move to
                    list.Append(iLocation / 8).Append(iLocation % 8).Append(index / 8).Append(index % 8); // iLocation where the Queen is and index where the Queen can move to
                    possible = possible & ~k; // remove the move from possibiliies that was just listed
                    k = possible & ~(possible-1); // get the next move possible
                }
                Q = Q & ~i; // remove the Queen that was checked for all moves
                i = Q & ~(Q-1); // get the next Queen to check for all moves
            }
            // int temp = list.Length/4;
            // Console.WriteLine(temp);
        }

        public static void possibleB(UInt64 occupied, UInt64 B){
            UInt64 i = B & ~(B-1); // get first bishop to check for moves
            UInt64 possible;

            while(i != 0){
                int iLocation = Tools.trailingZerosRight(i); // get number of zeroes until first 1 from left to right - the number is equal to the index on board of bishop
                possible = DandAntiDMoves(iLocation)&notMyPieces; // get diagonal and antidiagonal moves for the selected piece and with not my pieces
                UInt64 k = possible & ~(possible-1); // get one of the possiblities
                while (k != 0){ // goes trough each of the possibilies
                    int index = Tools.trailingZerosRight(k); // get index of move to
                    list.Append(iLocation / 8).Append(iLocation % 8).Append(index / 8).Append(index % 8); // iLocation where the bishop is and index where the bishop can move to
                    possible = possible & ~k; // remove the move from possibiliies that was just listed
                    k = possible & ~(possible-1); // get the next move possible
                }
                B = B & ~i; // remove the bishop that was checked for all moves
                i = B & ~(B-1); // get the next bishop to check for all moves
            }
            // int temp = list.Length/4;
            // Console.WriteLine(temp);
        }

        public static void possibleR(UInt64 occupied, UInt64 R){
            UInt64 i = R & ~(R-1); // get first rook to check for moves
            UInt64 possible;

            while(i != 0){
                int iLocation = Tools.trailingZerosRight(i); // get number of zeroes until first 1 from left to right - the number is equal to the index on board of rook
                possible = HandVMoves(iLocation)&notMyPieces; // get hori and verti moves for the selected piece and with not my pieces
                UInt64 k = possible & ~(possible-1); // get one of the possiblities
                while (k != 0){ // goes trough each of the possibilies
                    int index = Tools.trailingZerosRight(k); // get index of move to
                    list.Append(iLocation / 8).Append(iLocation % 8).Append(index / 8).Append(index % 8); // iLocation where the rook is and index where the rook can move to
                    possible = possible & ~k; // remove the move from possibiliies that was just listed
                    k = possible & ~(possible-1); // get the next move possible
                }
                R = R & ~i; // remove the rook that was checked for all moves
                i = R & ~(R-1); // get the next rook to check for all moves
            }
            // int temp = list.Length/4;
            // Console.WriteLine(temp);
        }

        public static void possibleWP(UInt64 wPB, UInt64 bPB, UInt64 EPB){

            //x1,y1,x2,y2
            UInt64 pMoves = (wPB>>7) & blackPieces & ~rank8 & ~fileA; // shift everything to the right by 7 to indicate capture right, and there is black piece and not on rank8 and not file H to stop capture one the other side of board
            /*  gets the first pawn-bit-move and puts into possibleMoves (if any) in a bitboard alone to put into moves list:
             *  possibleMoves =
             *  pMove = 10100011
             * -1 ~   = 01011101
             *  &     = 00000001
             * then later the tested pawn-bit is removed like so:
             *  possibleMoves =
             *  pMove = 10100011
             *   & ~  = 11111110
             *        = 10100010 == moved that was put into list is removed others remain
             *        */
            UInt64 possibleMoves=pMoves&~(pMoves-1);
            while (possibleMoves != 0){
                int index = Tools.trailingZerosRight(possibleMoves); // puts the index to the first pawn by counting number of 0's to the right
                list.Append(index/8+1).Append(index%8-1).Append(index/8).Append(index%8); //add move cords to list // first index +1 to get back to startlocation in the internal rank representation
                pMoves &= ~(possibleMoves); // the listed move is removed from pMoves
                possibleMoves = pMoves & ~(pMoves-1); // gets the next move alone in a bitboard
            }

            pMoves = (wPB>>9) & blackPieces & ~rank8 & ~fileH; // shift everything to the right by 9 to indicate capture left, and there is black piece and not on rank8 and not file A to stop capture on the other side of board
            possibleMoves=pMoves&~(pMoves-1);
            while (possibleMoves != 0){
                int index = Tools.trailingZerosRight(possibleMoves);
                list.Append(index/8+1).Append(index%8+1).Append(index/8).Append(index%8); //add move cords to list
                pMoves &= ~(possibleMoves);
                possibleMoves = pMoves & ~(pMoves-1);
            }

            pMoves = (wPB>>8) & empty & ~rank8; // shift everything to the right by 8 to indicate move forward by one, and there is empty field and not on rank8
            possibleMoves=pMoves&~(pMoves-1);
            while (possibleMoves != 0){
                int index = Tools.trailingZerosRight(possibleMoves);
                list.Append(index/8+1).Append(index%8).Append(index/8).Append(index%8); //add move cords to list
                pMoves &= ~(possibleMoves);
                possibleMoves = pMoves & ~(pMoves-1);
            }

            pMoves = (wPB>>16) & empty & (empty>>8) & rank4; // shift everything to the right by 16 to indicate move forward and field is empty and field infront is empty and it is rank 4(meaning only when rank2 posistion can it move this way)
            possibleMoves=pMoves&~(pMoves-1);
            while (possibleMoves != 0){
                int index = Tools.trailingZerosRight(possibleMoves);
                list.Append(index/8+2).Append(index%8).Append(index/8).Append(index%8); //add move cords to list
                pMoves &= ~(possibleMoves);
                possibleMoves = pMoves & ~(pMoves-1);
            }
            // y1, y2, promotype, "P"
            pMoves = (wPB>>7) & blackPieces & rank8 & ~fileA; // shift everything to the right by 7 to indicate capture right, and there is black piece and not on rank8 and not file H to stop capture one the other side of board
            possibleMoves=pMoves&~(pMoves-1);
            while (possibleMoves != 0){
                int index = Tools.trailingZerosRight(possibleMoves);
                list.Append(index%8-1).Append(index%8).Append("QP").Append(index%8-1).Append(index%8).Append("RP").Append(index%8-1).Append(index%8).Append("NP").Append(index%8-1).Append(index%8).Append("BP");
                pMoves &= ~(possibleMoves);
                possibleMoves = pMoves & ~(pMoves-1);
            }

            pMoves = (wPB>>9) & blackPieces & rank8 & ~fileH; // shift everything to the right by 9 to indicate capture left, and there is black piece and not on rank8 and not file A to stop capture on the other side of board
            possibleMoves=pMoves&~(pMoves-1);
            while (possibleMoves != 0){
                int index = Tools.trailingZerosRight(possibleMoves);
                list.Append(index%8+1).Append(index%8).Append("QP").Append(index%8+1).Append(index%8).Append("RP").Append(index%8+1).Append(index%8).Append("NP").Append(index%8+1).Append(index%8).Append("BP");
                pMoves &= ~(possibleMoves);
                possibleMoves = pMoves & ~(pMoves-1);
            }

            pMoves = (wPB>>8) & empty & rank8; // shift everything to the right by 8 to indicate move forward by one, and there is empty field and not on rank8
            possibleMoves=pMoves&~(pMoves-1);
            while (possibleMoves != 0){
                int index = Tools.trailingZerosRight(possibleMoves);
                list.Append(index%8).Append(index%8).Append("QP").Append(index%8).Append(index%8).Append("RP").Append(index%8).Append(index%8).Append("NP").Append(index%8).Append(index%8).Append("BP");
                pMoves &= ~(possibleMoves);
                possibleMoves = pMoves & ~(pMoves-1);
            }
            // y1, y2 E
            // en passant to the right
            possibleMoves = (wPB << 1) & bPB & rank5 & ~fileA & EPB; // piece location to move, no destination put in hist - does not try and grab moves one by one because there should only be one possible per round
            if (possibleMoves != 0){
                int index = Tools.trailingZerosRight(possibleMoves);
                list.Append(index%8-1).Append(index%8).Append("WE");
            }
            // en passant to the left
            possibleMoves = (wPB >> 1) & bPB & rank5 & ~fileH & EPB;// piece to move no destination put in hist
            if (possibleMoves != 0){
                int index = Tools.trailingZerosRight(possibleMoves);
                list.Append(index%8+1).Append(index%8).Append("WE");
            }
        }

        public static void possibleBP(UInt64 bPB, UInt64 wPB, UInt64 EPB){
            //x1,y1,x2,y2
            UInt64 pMoves = (bPB<<7) & whitePieces & ~rank1 & ~fileH;// shift everything to the right by 7 to indicate capture right, and there is black piece and not on rank8 and not file H to stop capture one the other side of board
            UInt64 possibleMoves=pMoves&~(pMoves-1);
            while (possibleMoves != 0){
                int index = Tools.trailingZerosRight(possibleMoves); // puts the index to the first pawn by counting number of 0's to the right
                list.Append(index/8-1).Append(index%8+1).Append(index/8).Append(index%8); //add move cords to list // first index +1 to get back to startlocation in the internal rank representation
                pMoves &= ~(possibleMoves); // the listed move is removed from pMoves
                possibleMoves = pMoves & ~(pMoves-1); // gets the next move alone in a bitboard
            }

            pMoves = (bPB<<9) & whitePieces & ~rank1 & ~fileA; // shift everything to the right by 9 to indicate capture left, and there is black piece and not on rank8 and not file A to stop capture on the other side of board
            possibleMoves=pMoves&~(pMoves-1);
            while (possibleMoves != 0){
                int index = Tools.trailingZerosRight(possibleMoves);
                list.Append(index/8-1).Append(index%8-1).Append(index/8).Append(index%8); //add move cords to list
                pMoves &= ~(possibleMoves);
                possibleMoves = pMoves & ~(pMoves-1);
            }

            pMoves = (bPB<<8) & empty & ~rank1; // shift everything to the right by 8 to indicate move forward by one, and there is empty field and not on rank8
            possibleMoves=pMoves&~(pMoves-1);
            while (possibleMoves != 0){
                int index = Tools.trailingZerosRight(possibleMoves);
                list.Append(index/8-1).Append(index%8).Append(index/8).Append(index%8); //add move cords to list
                pMoves &= ~(possibleMoves);
                possibleMoves = pMoves & ~(pMoves-1);
            }

            pMoves = (bPB<<16) & empty & (empty<<8) & rank5; // shift everything to the right by 16 to indicate move forward and field is empty and field infront is empty and it is rank 4(meaning only when rank2 posistion can it move this way)
            possibleMoves=pMoves&~(pMoves-1);
            while (possibleMoves != 0){
                int index = Tools.trailingZerosRight(possibleMoves);
                list.Append(index/8-2).Append(index%8).Append(index/8).Append(index%8); //add move cords to list
                pMoves &= ~(possibleMoves);
                possibleMoves = pMoves & ~(pMoves-1);
            }
            // y1, y2, promotype, "P"
            pMoves = (bPB<<7) & whitePieces & rank1 & ~fileH; // shift everything to the right by 7 to indicate capture right, and there is black piece and not on rank8 and not file H to stop capture one the other side of board
            possibleMoves=pMoves&~(pMoves-1);
            while (possibleMoves != 0){
                int index = Tools.trailingZerosRight(possibleMoves);
                list.Append(index%8+1).Append(index%8).Append("qP").Append(index%8+1).Append(index%8).Append("rP").Append(index%8+1).Append(index%8).Append("nP").Append(index%8+1).Append(index%8).Append("bP");
                pMoves &= ~(possibleMoves);
                possibleMoves = pMoves & ~(pMoves-1);
            }

            pMoves = (bPB<<9) & whitePieces & rank1 & ~fileA; // shift everything to the right by 9 to indicate capture left, and there is black piece and not on rank8 and not file A to stop capture on the other side of board
            possibleMoves=pMoves&~(pMoves-1);
            while (possibleMoves != 0){
                int index = Tools.trailingZerosRight(possibleMoves);
                list.Append(index%8-1).Append(index%8).Append("qP").Append(index%8-1).Append(index%8).Append("rP").Append(index%8-1).Append(index%8).Append("nP").Append(index%8-1).Append(index%8).Append("bP");
                pMoves &= ~(possibleMoves);
                possibleMoves = pMoves & ~(pMoves-1);
            }

            pMoves = (bPB<<8) & empty & rank1; // shift everything to the right by 8 to indicate move forward by one, and there is empty field and not on rank8
            possibleMoves=pMoves&~(pMoves-1);
            while (possibleMoves != 0){
                int index = Tools.trailingZerosRight(possibleMoves);
                list.Append(index%8).Append(index%8).Append("qP").Append(index%8).Append(index%8).Append("rP").Append(index%8).Append(index%8).Append("nP").Append(index%8).Append(index%8).Append("bP");
                pMoves &= ~(possibleMoves);
                possibleMoves = pMoves & ~(pMoves-1);
            }
            // y1, y2 BE
            // en passant to the right
            possibleMoves = (bPB >> 1) & wPB & rank4 & ~fileH & EPB; // piece location to move, no destination put in hist - does not try and grab moves one by one because there should only be one possible per round
            if (possibleMoves != 0){
                int index = Tools.trailingZerosRight(possibleMoves);
                list.Append(index%8+1).Append(index%8).Append("BE");
            }
            // en passant to the left
            possibleMoves = (bPB << 1) & wPB & rank4 & ~fileA & EPB;// piece to move no destination put in hist
            if (possibleMoves != 0){
                int index = Tools.trailingZerosRight(possibleMoves);
                list.Append(index%8-1).Append(index%8).Append("BE");
            }
        }

        public static UInt64 unsafeWhite(UInt64 bKB, UInt64 bQB, UInt64 bRB, UInt64 bBB, UInt64 bNB, UInt64 bPB, UInt64 wKB, UInt64 wQB, UInt64 wRB, UInt64 wBB, UInt64 wNB, UInt64 wPB){
            UInt64 unsafeSq;
            occupied = bKB|bQB|bRB|bBB|bNB|bPB|wKB|wQB|wRB|wBB|wNB|wPB; // or all pieces together to get occupied

            //pawn
            unsafeSq = ((bPB << 7) & ~fileH); // pawn capture right
            unsafeSq = unsafeSq | ((bPB << 9) & ~fileA); // pawn capture left

            UInt64 possible;
            //knight
            UInt64 i = bNB & ~(bNB-1);
            while(i != 0){
                int iLocation = Tools.trailingZerosRight(i); // get number of zeroes until first 1 from left to right - the number is equal to the index on board of knight

                if(iLocation > 18){
                    possible = knightSpan << (iLocation-18);
                }else {
                    possible = knightSpan >> (18-iLocation);
                }

                if(iLocation % 8<4){
                    possible = possible & ~fileGH;
                }else{
                    possible = possible & ~fileAB;
                }
                unsafeSq = unsafeSq | possible; // add knight attack  squares to unsafeSq var
                bNB = bNB & ~i;
                i = bNB & ~(bNB-1);
            }
            //bishop and queen
            UInt64 QandB = bQB|bBB;
            i = QandB & ~(QandB-1);
            while(i !=0){
                int iLocation = Tools.trailingZerosRight(i);
                possible = DandAntiDMoves(iLocation);
                unsafeSq = unsafeSq | possible;
                QandB = QandB & ~i;
                i = QandB & ~(QandB-1);
            }
            //rook and queen
            UInt64 QandR = bQB|bRB;
            i = QandR & ~(QandR-1);
            while(i !=0){
                int iLocation = Tools.trailingZerosRight(i);
                possible = HandVMoves(iLocation);
                unsafeSq = unsafeSq | possible;
                QandR = QandR & ~i;
                i = QandR & ~(QandR-1);
            }
            //king
            int kLocation = Tools.trailingZerosRight(bKB); // get number of zeroes until first 1 from left to right - the number is equal to the index on board of knight

            if(kLocation > 54){
                possible = kingSpan << (kLocation-54);
            }else {
                possible = kingSpan >> (54-kLocation);
            }

            if(kLocation % 8<4){
                possible = possible & ~fileGH;
            }else{
                possible = possible & ~fileAB;
            }
            unsafeSq = unsafeSq | possible; // add knight attack  squares to unsafeSq var
            return unsafeSq;
        }

        public static UInt64 unsafeBlack(UInt64 bKB, UInt64 bQB, UInt64 bRB, UInt64 bBB, UInt64 bNB, UInt64 bPB, UInt64 wKB, UInt64 wQB, UInt64 wRB, UInt64 wBB, UInt64 wNB, UInt64 wPB){
            UInt64 unsafeSq;
            occupied = bKB|bQB|bRB|bBB|bNB|bPB|wKB|wQB|wRB|wBB|wNB|wPB; // or all pieces together to get occupied

            //pawn
            unsafeSq = ((wPB >> 7) & ~fileA); // pawn capture right
            unsafeSq = unsafeSq | ((wPB >> 9) & ~fileH); // pawn capture left

            UInt64 possible;
            //knight
            UInt64 i = wNB & ~(wNB-1);
            while(i != 0){
                int iLocation = Tools.trailingZerosRight(i); // get number of zeroes until first 1 from left to right - the number is equal to the index on board of knight

                if(iLocation > 18){
                    possible = knightSpan << (iLocation-18);
                }else {
                    possible = knightSpan >> (18-iLocation);
                }

                if(iLocation % 8<4){
                    possible = possible & ~fileGH;
                }else{
                    possible = possible & ~fileAB;
                }
                unsafeSq = unsafeSq | possible; // add knight attack  squares to unsafeSq var
                wNB = wNB & ~i;
                i = wNB & ~(wNB-1);
            }
            //bishop and queen
            UInt64 QandB = wQB|wBB;
            i = QandB & ~(QandB-1);
            while(i !=0){
                int iLocation = Tools.trailingZerosRight(i);
                possible = DandAntiDMoves(iLocation);
                unsafeSq = unsafeSq | possible;
                QandB = QandB & ~i;
                i = QandB & ~(QandB-1);
            }
            //rook and queen
            UInt64 QandR = wQB|wRB;
            i = QandR & ~(QandR-1);
            while(i !=0){
                int iLocation = Tools.trailingZerosRight(i);
                possible = HandVMoves(iLocation);
                unsafeSq = unsafeSq | possible;
                QandR = QandR & ~i;
                i = QandR & ~(QandR-1);
            }
            //king
            int kLocation = Tools.trailingZerosRight(wKB); // get number of zeroes until first 1 from left to right - the number is equal to the index on board of knight

            if(kLocation > 54){
                possible = kingSpan << (kLocation-54);
            }else {
                possible = kingSpan >> (54-kLocation);
            }

            if(kLocation % 8<4){
                possible = possible & ~fileGH;
            }else{
                possible = possible & ~fileAB;
            }
            unsafeSq = unsafeSq | possible; // add knight attack  squares to unsafeSq var
            return unsafeSq;
        }

    }
}

