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
using System.Threading.Tasks;
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
        public static UInt64 center = 0x1818000000;
        public static UInt64 centerBig = 0x3C3C3C3C0000;
        public static UInt64 kingSide = 0xF0F0F0F0F0F0F0F;
        public static UInt64 queenSide = 0xF0F0F0F0F0F0F0F0;
        public static UInt64 notMyPieces; // every piece that the side can not capture including oppesit side king to stop illegal capture
        public static UInt64 empty; // every field empty
        public static UInt64 occupied;
        public static UInt64 notMyPiecesAndOccu;

        public static char[] Zero2 = new char[4] {'0', '4', '0', '2'};
        public static char[] Zero6 = new char[4] {'0', '4', '0', '6'};
        public static char[] Seven2 = new char[4] {'7', '4', '7', '2'};
        public static char[] Seven6 = new char[4] {'7', '4', '7', '6'};

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



        public static UInt64 makeMove(UInt64 board, char[] moves, char type, int start, int end){
            if ( Tools.IsCharDigit(moves[3])){ //if last digit of move string is digit then its a normal move
                //int end = (((moves[2] - '0') * 8) + (moves[3] - '0'));
                if (((board >> start) & 1) == 1) {
                    board = board & ~((UInt64)1 << start); // remove piece from start
                    board = board | ((UInt64)1 << end); // add the piece to the end
                }
                else{
                    board = board & ~((UInt64)1 << end); // remove piece if it got attacked
                }
            }else if(moves[3] == 'P'){ // if last char in move string is P its a promotion
                int startp, endp;
                if (char.IsUpper(moves[2])){ // if 3 char is upper case then its white promoting a piece
                    startp = Tools.trailingZerosRight(Filemasks8[moves[0] - '0'] & Rankmasks8[1]); // get start position by ANDing rank and file - i only have start file and end file - i know a promotion for white can only happen from rank 7 to 8
                    endp = Tools.trailingZerosRight(Filemasks8[moves[1] - '0'] & Rankmasks8[0]);
                }else{
                    startp = Tools.trailingZerosRight(Filemasks8[moves[0] - '0'] & Rankmasks8[6]);
                    endp = Tools.trailingZerosRight(Filemasks8[moves[1] - '0'] & Rankmasks8[7]);
                }
                if(type == moves[2]) { // if the promote is the same as the pieceboard making moves, add the promoted piece to board on endp location
                    board = board | ((UInt64)1 << endp); // add the piece to the end
                }else {
                    board = board & ~((UInt64)1 << startp); // remove piece from start
                    board = board & ~((UInt64)1 <<endp);
                }
            }
            else if (moves[3] == 'E'){ // if last char is E the move is en passant // moves[0] and moves[1] is file from and file to
                int starte, ende;
                if (moves[2] == 'W'){ // if 3 char is W then its white en passant
                    starte = Tools.trailingZerosRight(Filemasks8[moves[0] - '0'] & Rankmasks8[3]); // get start position
                    ende = Tools.trailingZerosRight(Filemasks8[moves[1] - '0'] & Rankmasks8[2]); //
                    board = board & ~(Filemasks8[moves[1] - '0'] & Rankmasks8[3]); // remove taken pawn - file is move to and rank is the startering rank which is the rank opponent will be at
                }else{
                    starte = Tools.trailingZerosRight(Filemasks8[moves[0] - '0'] & Rankmasks8[4]);
                    ende = Tools.trailingZerosRight(Filemasks8[moves[1] - '0'] & Rankmasks8[5]);
                    board = board & ~(Filemasks8[moves[1] - '0'] & Rankmasks8[4]); // remove taken pawn
                }
                if (((board >> starte) & 1 ) == 1 ){ // if pieceBoard is equal to start location
                    board = board & ~((UInt64)1 << starte); // remove piece from start location
                    board = board | ((UInt64)1 << ende); // add pawn to end location (behind taken pawn)
                }
            }
            return board;
        }

        public static UInt64 CastleMove(UInt64 Rook, UInt64 King, char[] Cmoves, char type, int start){
            if (((King >> start) & 1) == 1){
                if (Tools.ArrC(Cmoves, Zero2) || Tools.ArrC(Cmoves, Zero6) || Tools.ArrC(Cmoves, Seven2) || Tools.ArrC(Cmoves, Seven6)){
                    string move = new string(Cmoves);
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
            }
            return Rook;
        }

        public static UInt64 makeMoveEP(UInt64 board, char[] moves, int start){
                if ((Tools.Abs(moves[0] - moves[2]) == 2) && (((board >> start) & 1) == 1)){ // if moved 2 forward on file and pawnboard shifted with start location is 1 then its a pawn
                    return Filemasks8[moves[1] - '0'];
                }
            return (UInt64)0;
        }

        public static UInt64 HandVMoves(int square){
            UInt64 bitboardSq = (UInt64)1 << square;
            UInt64 revO = Tools.reverseBit(occupied);
            UInt64 revBitSq = Tools.reverseBitSingle(bitboardSq) * 2;
            int rank = square / 8;
            int file = square % 8;
            //turn square number into binary bitboard representing piece
            // use ( o - 2r ) ^ reverse( reverse(o) - 2 * reverse(r)) for horizontal attacks
            // first part is attack toward MSB (right)
            UInt64 possibleH = (occupied - 2 * bitboardSq) ^ Tools.reverseBit(revO - revBitSq);
            // use ( o - 2r ) ^ reverse( reverse(o) - 2 * reverse(r)) same but with masksing occupied with the file the piece is on for vertical attacks
            UInt64 possibleV = ((occupied & Filemasks8[file]) - (2 * bitboardSq)) ^ Tools.reverseBit(Tools.reverseBit(occupied & Filemasks8[file]) - revBitSq);
            return (possibleH & Rankmasks8[rank]) | (possibleV & Filemasks8[file]); // & with masks and | to combine vertial and horizontal
        }

        public static UInt64 DandAntiDMoves(int square){
            int rank = square / 8;
            int file = square % 8;
            UInt64 bitboardSq = (UInt64)1 << square;
            UInt64 revBitSq = Tools.reverseBitSingle(bitboardSq);
            UInt64 possibleD = ((occupied & Diagonalmasks8[rank + file]) - (2 * bitboardSq)) ^ Tools.reverseBit(Tools.reverseBit(occupied & Diagonalmasks8[rank + file]) - (2 * revBitSq));
            UInt64 possibleAntiD = ((occupied & AntiDiagonalmasks8[rank + 7 - file]) - (2 * bitboardSq)) ^ Tools.reverseBit(Tools.reverseBit(occupied & AntiDiagonalmasks8[rank + 7 - file]) - (2 * revBitSq));
            return (possibleD & Diagonalmasks8[rank + file]) | (possibleAntiD & AntiDiagonalmasks8[rank + 7 - file]);
        }

        private static StringBuilder list = new StringBuilder();

        public static string possibleMovesW(UInt64[] bitboards, UInt64 EPB, bool castleWKside, bool castleWQside, bool castleBKside, bool castleBQside){
            notMyPieces=~(bitboards[6]|bitboards[7]|bitboards[8]|bitboards[9]|bitboards[10]|bitboards[11]|bitboards[0]); // or'd every white piece and black king to indicate what the white pieces cant capture, also black king to avoid illegal capture

            occupied =
                bitboards[0]|bitboards[1]|bitboards[2]|bitboards[3]|bitboards[4]|bitboards[5]|
                bitboards[6]|bitboards[7]|bitboards[8]|bitboards[9]|bitboards[10]|bitboards[11]; // or all pieces together to get occupied

            empty = ~occupied;// indicates empty fields with a 1 with flip ~ of occupied

            notMyPiecesAndOccu = notMyPieces & occupied;

            list.Clear();

            possibleWP(bitboards[11], bitboards[5], EPB);
            possibleB(occupied, bitboards[9]);
            possibleR(occupied, bitboards[8]);
            possibleQ(occupied, bitboards[7]);
            possibleN(occupied, bitboards[10]);
            possibleK(occupied, bitboards[6]);
            possibleCW(bitboards, castleWKside, castleWQside);

            return list.ToString();
        }
        public static string possibleMovesB(UInt64[] bitboards, UInt64 EPB, bool castleWKside, bool castleWQside, bool castleBKside, bool castleBQside){
            notMyPieces=~(bitboards[0]|bitboards[1]|bitboards[2]|bitboards[3]|bitboards[4]|bitboards[5]|bitboards[6]); // or'd every white piece and black king to indicate what the white pieces cant capture, also black king to avoid illegal capture
            occupied =
                bitboards[0]|bitboards[1]|bitboards[2]|bitboards[3]|bitboards[4]|bitboards[5]|
                bitboards[6]|bitboards[7]|bitboards[8]|bitboards[9]|bitboards[10]|bitboards[11]; // or all pieces together to get occupied
            empty = ~occupied; // indicates empty fields with a 1 with flip ~ of occupied
            notMyPiecesAndOccu = notMyPieces & occupied;

            list.Clear();

            possibleBP(bitboards[5], bitboards[11], EPB);
            possibleB(occupied, bitboards[3]);
            possibleR(occupied, bitboards[2]);
            possibleQ(occupied, bitboards[1]);
            possibleN(occupied, bitboards[4]);
            possibleK(occupied, bitboards[0]);
            possibleCB(bitboards, castleBKside, castleBQside);

            return list.ToString();
        }

        public static void possibleCW(UInt64[] bitboards, bool castleWKside, bool castleWQside){
            if (castleWKside){ // check if king has moved or rook on king side moved
                if ((occupied & (((UInt64)1 << 61) | ((UInt64)1 << 62))) == 0 && ((bitboards[8] & ((UInt64)1 << 63)) != 0)){ // sq in between are empty
                    UInt64 unsafeSq = unsafeWhite(bitboards);
                    if((unsafeSq & ((bitboards[6] | ((UInt64)1 << 61)) | ((UInt64)1 << 62))) == 0){ // king is not in check and field it moves over are not under attack
                        list.Append("7476");
                    }
                }
            }

            if (castleWQside){ // check if king has moved or rook on queen side moved
                if ((occupied & (((UInt64)1 << 57) | ((UInt64)1 << 58) | ((UInt64)1 << 59))) == 0 && ((bitboards[8] & ((UInt64)1 << 56)) != 0)){ // sq in between are empty
                    UInt64 unsafeSqWQ = unsafeWhite(bitboards);
                    if((unsafeSqWQ & ((bitboards[6] | ((UInt64)1 << 58)) | ((UInt64)1 << 59))) == 0){ // king is not in check and field it moves over are not under attack
                        list.Append("7472");
                    }
                }
            }
        }

        public static void possibleCB(UInt64[] bitboards, bool castleBKside, bool castleBQside){
            if (castleBKside){ // check if king has moved or rook on king side moved
                if ((occupied & (((UInt64)1 << 5) | ((UInt64)1 << 6))) == 0 && ((bitboards[2] & ((UInt64)1 << 7)) != 0)){ // sq in between are empty
                    UInt64 unsafeSq = unsafeBlack(bitboards);
                    if((unsafeSq & ((bitboards[0] | ((UInt64)1 << 5)) | ((UInt64)1 << 6))) == 0){ // king is not in check and field it moves over are not under attack
                        list.Append("0406");
                    }
                }
            }

            if (castleBQside){ // check if king has moved or rook on queen side moved
                if ((occupied & (((UInt64)1 << 1) | ((UInt64)1 << 2) | ((UInt64)1 << 3))) == 0 && ((bitboards[2] & (UInt64)1) != 0)){ // sq in between are empty
                    UInt64 unsafeSqBQ = unsafeBlack(bitboards);
                    if((unsafeSqBQ & ((bitboards[0] | ((UInt64)1 << 2)) | ((UInt64)1 << 3))) == 0){ // king is not in check and field it moves over are not under attack
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
                possible = Span.Knight[iLocation] & notMyPieces; // put location in index of array with all 64 possible knight locations // turns out to not be faster than calculating possible location on the spot
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
        }

        public static void possibleK(UInt64 occupied, UInt64 K){

            int iLocation = Tools.trailingZerosRight(K); // get number of zeroes until first 1 from left to right - the number is equal to the index on board of knight
            UInt64 possible = Span.King[iLocation] & notMyPieces; // put location into index of array with all possible kingspans

            UInt64 i = possible & ~(possible-1); // get one of the possiblities
            while (i != 0){ // goes trough each of the possibilies
                int index = Tools.trailingZerosRight(i); // get index of move to
                list.Append(iLocation / 8).Append(iLocation % 8).Append(index / 8).Append(index % 8); // iLocation where the knight is and index where the knight can move to
                possible = possible & ~i; // remove the move from possibiliies that was just listed
                i = possible & ~(possible-1); // get the next move possible
            }
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
        }

        public static void possibleWP(UInt64 wPB, UInt64 bPB, UInt64 EPB){

            //x1,y1,x2,y2
            UInt64 pMoves = (wPB>>7) & notMyPiecesAndOccu & ~rank8 & ~fileA; // shift everything to the right by 7 to indicate capture right, and there is black piece and not on rank8 and not file H to stop capture one the other side of board
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

            pMoves = (wPB>>9) & notMyPiecesAndOccu & ~rank8 & ~fileH; // shift everything to the right by 9 to indicate capture left, and there is black piece and not on rank8 and not file A to stop capture on the other side of board
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
            pMoves = (wPB>>7) & notMyPiecesAndOccu & rank8 & ~fileA; // shift everything to the right by 7 to indicate capture right, and there is black piece and not on rank8 and not file H to stop capture one the other side of board
            possibleMoves=pMoves&~(pMoves-1);
            while (possibleMoves != 0){
                int index = Tools.trailingZerosRight(possibleMoves);
                list.Append(index%8-1).Append(index%8).Append("QP").Append(index%8-1).Append(index%8).Append("RP").Append(index%8-1).Append(index%8).Append("NP").Append(index%8-1).Append(index%8).Append("BP");
                pMoves &= ~(possibleMoves);
                possibleMoves = pMoves & ~(pMoves-1);
            }

            pMoves = (wPB>>9) & notMyPiecesAndOccu & rank8 & ~fileH; // shift everything to the right by 9 to indicate capture left, and there is black piece and not on rank8 and not file A to stop capture on the other side of board
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
            possibleMoves = (wPB << 1) & bPB & rank5 & ~fileA & EPB; // location of enemy pawn that is inside the en passant mask(EPB)
            if (possibleMoves != 0){
                int index = Tools.trailingZerosRight(possibleMoves);
                list.Append(index%8-1).Append(index%8).Append("WE"); // file from and to
            }
            // en passant to the left
            possibleMoves = (wPB >> 1) & bPB & rank5 & ~fileH & EPB;//
            if (possibleMoves != 0){
                int index = Tools.trailingZerosRight(possibleMoves);
                list.Append(index%8+1).Append(index%8).Append("WE"); // file from and to
            }
        }

        public static void possibleBP(UInt64 bPB, UInt64 wPB, UInt64 EPB){
            //x1,y1,x2,y2
            UInt64 pMoves = (bPB<<7) & notMyPiecesAndOccu & ~rank1 & ~fileH;// shift everything to the right by 7 to indicate capture right, and there is black piece and not on rank8 and not file H to stop capture one the other side of board
            UInt64 possibleMoves=pMoves&~(pMoves-1);
            while (possibleMoves != 0){
                int index = Tools.trailingZerosRight(possibleMoves); // puts the index to the first pawn by counting number of 0's to the right
                list.Append(index/8-1).Append(index%8+1).Append(index/8).Append(index%8); //add move cords to list // first index +1 to get back to startlocation in the internal rank representation
                pMoves &= ~(possibleMoves); // the listed move is removed from pMoves
                possibleMoves = pMoves & ~(pMoves-1); // gets the next move alone in a bitboard
            }

            pMoves = (bPB<<9) & notMyPiecesAndOccu & ~rank1 & ~fileA; // shift everything to the right by 9 to indicate capture left, and there is black piece and not on rank8 and not file A to stop capture on the other side of board
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
            pMoves = (bPB<<7) & notMyPiecesAndOccu & rank1 & ~fileH; // shift everything to the right by 7 to indicate capture right, and there is black piece and not on rank8 and not file H to stop capture one the other side of board
            possibleMoves=pMoves&~(pMoves-1);
            while (possibleMoves != 0){
                int index = Tools.trailingZerosRight(possibleMoves);
                list.Append(index%8+1).Append(index%8).Append("qP").Append(index%8+1).Append(index%8).Append("rP").Append(index%8+1).Append(index%8).Append("nP").Append(index%8+1).Append(index%8).Append("bP");
                pMoves &= ~(possibleMoves);
                possibleMoves = pMoves & ~(pMoves-1);
            }

            pMoves = (bPB<<9) & notMyPiecesAndOccu & rank1 & ~fileA; // shift everything to the right by 9 to indicate capture left, and there is black piece and not on rank8 and not file A to stop capture on the other side of board
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

        public static UInt64 unsafeWhite(UInt64[] bitboards){
            UInt64 unsafeSq;
            occupied =
                bitboards[0]|bitboards[1]|bitboards[2]|bitboards[3]|bitboards[4]|bitboards[5]|
                bitboards[6]|bitboards[7]|bitboards[8]|bitboards[9]|bitboards[10]|bitboards[11]; // or all pieces together to get occupied

            //pawn
            unsafeSq = ((bitboards[5] << 7) & ~fileH); // pawn capture right
            unsafeSq = unsafeSq | ((bitboards[5] << 9) & ~fileA); // pawn capture left

            UInt64 possible;
            //knight
            UInt64 i = bitboards[4] & ~(bitboards[4]-1);
            while(i != 0){
                possible = Span.Knight[Tools.trailingZerosRight(i)]; // put location in index of array with all 64 possible knight locations // turns out to not be faster than calculating possible location on the spot

                unsafeSq = unsafeSq | possible; // add knight attack  squares to unsafeSq var
                bitboards[4] = bitboards[4] & ~i;
                i = bitboards[4] & ~(bitboards[4]-1);
            }
            //bishop and queen
            UInt64 QandB = bitboards[1]|bitboards[3];
            i = QandB & ~(QandB-1);
            while(i !=0){
                int iLocation = Tools.trailingZerosRight(i);
                possible = DandAntiDMoves(iLocation);
                unsafeSq = unsafeSq | possible;
                QandB = QandB & ~i;
                i = QandB & ~(QandB-1);
            }
            //rook and queen
            UInt64 QandR = bitboards[1]|bitboards[2];
            i = QandR & ~(QandR-1);
            while(i !=0){
                int iLocation = Tools.trailingZerosRight(i);
                possible = HandVMoves(iLocation);
                unsafeSq = unsafeSq | possible;
                QandR = QandR & ~i;
                i = QandR & ~(QandR-1);
            }
            //king
            possible = Span.King[Tools.trailingZerosRight(bitboards[0])]; // put location into index of array with all possible kingspans
            unsafeSq = unsafeSq | possible; // add king attack  squares to unsafeSq var
            return unsafeSq;
        }

        public static UInt64 unsafeBlack(UInt64[] bitboards){
            UInt64 unsafeSq;
            occupied =
                bitboards[0]|bitboards[1]|bitboards[2]|bitboards[3]|bitboards[4]|bitboards[5]|
                bitboards[6]|bitboards[7]|bitboards[8]|bitboards[9]|bitboards[10]|bitboards[11]; // or all pieces together to get occupied

            //pawn
            unsafeSq = ((bitboards[11] >> 7) & ~fileA); // pawn capture right
            unsafeSq = unsafeSq | ((bitboards[11] >> 9) & ~fileH); // pawn capture left

            UInt64 possible;
            //knight
            UInt64 i = bitboards[10] & ~(bitboards[10]-1);
            while(i != 0){
                possible = Span.Knight[Tools.trailingZerosRight(i)]; // put location in index of array with all 64 possible knight locations // turns out to not be faster than calculating possible location on the spot
                unsafeSq = unsafeSq | possible; // add knight attack  squares to unsafeSq var
                bitboards[10] = bitboards[10] & ~i;
                i = bitboards[10] & ~(bitboards[10]-1);
            }
            //bishop and queen
            UInt64 QandB = bitboards[7]|bitboards[9];
            i = QandB & ~(QandB-1);
            while(i !=0){
                int iLocation = Tools.trailingZerosRight(i);
                possible = DandAntiDMoves(iLocation);
                unsafeSq = unsafeSq | possible;
                QandB = QandB & ~i;
                i = QandB & ~(QandB-1);
            }
            //rook and queen
            UInt64 QandR = bitboards[7]|bitboards[8];
            i = QandR & ~(QandR-1);
            while(i !=0){
                int iLocation = Tools.trailingZerosRight(i);
                possible = HandVMoves(iLocation);
                unsafeSq = unsafeSq | possible;
                QandR = QandR & ~i;
                i = QandR & ~(QandR-1);
            }
            //king
            possible = Span.King[Tools.trailingZerosRight(bitboards[6])]; // put location into index of array with all possible kingspans
            unsafeSq = unsafeSq | possible; // add knight attack  squares to unsafeSq var
            return unsafeSq;
        }
    }
}

