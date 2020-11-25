using System;

namespace ChessBitboard{

    public class Tools{

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
            if(move.Length > 4 && move[4] == 'e'){
                moveString = "";
                moveString = moveString + ""+(char)(move[0]-49);
                moveString = moveString + ""+(char)(move[2]-49);
                moveString = moveString + "W";
                moveString = moveString + ""+char.ToUpper(move[4]);
                return moveString;
            }
            if(move.Length > 4 && move[4] == 'p'){
                moveString = "";
                moveString = moveString + ""+(char)(move[0]-49);
                moveString = moveString + ""+(char)(move[2]-49);
                moveString = moveString + ""+char.ToUpper(move[5]);
                moveString = moveString + ""+char.ToUpper(move[4]);
                return moveString;
            }
            moveString = moveString + ""+('8' - (move[1]));
            moveString = moveString + ""+(char)(move[0]-49);
            moveString = moveString + ""+('8' - (move[3]));
            moveString = moveString + ""+(char)(move[2]-49);
            return moveString;
        }

        public static int trailingZerosRight(UInt64 move){
                if (((move)&1)==1) return 0 ;
                if (((move >>1 )&1)==1) return 1 ;
                if (((move >>2 )&1)==1) return 2 ;
                if (((move >>3 )&1)==1) return 3 ;
                if (((move >>4 )&1)==1) return 4 ;
                if (((move >>5 )&1)==1) return 5 ;
                if (((move >>6 )&1)==1) return 6 ;
                if (((move >>7 )&1)==1) return 7 ;
                if (((move >>8 )&1)==1) return 8 ;
                if (((move >>9 )&1)==1) return 9 ;
                if (((move >>10)&1)==1) return 10;
                if (((move >>11)&1)==1) return 11;
                if (((move >>12)&1)==1) return 12;
                if (((move >>13)&1)==1) return 13;
                if (((move >>14)&1)==1) return 14;
                if (((move >>15)&1)==1) return 15;
                if (((move >>16)&1)==1) return 16;
                if (((move >>17)&1)==1) return 17;
                if (((move >>18)&1)==1) return 18;
                if (((move >>19)&1)==1) return 19;
                if (((move >>20)&1)==1) return 20;
                if (((move >>21)&1)==1) return 21;
                if (((move >>22)&1)==1) return 22;
                if (((move >>23)&1)==1) return 23;
                if (((move >>24)&1)==1) return 24;
                if (((move >>25)&1)==1) return 25;
                if (((move >>26)&1)==1) return 26;
                if (((move >>27)&1)==1) return 27;
                if (((move >>28)&1)==1) return 28;
                if (((move >>29)&1)==1) return 29;
                if (((move >>30)&1)==1) return 30;
                if (((move >>31)&1)==1) return 31;
                if (((move >>32)&1)==1) return 32;
                if (((move >>33)&1)==1) return 33;
                if (((move >>34)&1)==1) return 34;
                if (((move >>35)&1)==1) return 35;
                if (((move >>36)&1)==1) return 36;
                if (((move >>37)&1)==1) return 37;
                if (((move >>38)&1)==1) return 38;
                if (((move >>39)&1)==1) return 39;
                if (((move >>40)&1)==1) return 40;
                if (((move >>41)&1)==1) return 41;
                if (((move >>42)&1)==1) return 42;
                if (((move >>43)&1)==1) return 43;
                if (((move >>44)&1)==1) return 44;
                if (((move >>45)&1)==1) return 45;
                if (((move >>46)&1)==1) return 46;
                if (((move >>47)&1)==1) return 47;
                if (((move >>48)&1)==1) return 48;
                if (((move >>49)&1)==1) return 49;
                if (((move >>50)&1)==1) return 50;
                if (((move >>51)&1)==1) return 51;
                if (((move >>52)&1)==1) return 52;
                if (((move >>53)&1)==1) return 53;
                if (((move >>54)&1)==1) return 54;
                if (((move >>55)&1)==1) return 55;
                if (((move >>56)&1)==1) return 56;
                if (((move >>57)&1)==1) return 57;
                if (((move >>58)&1)==1) return 58;
                if (((move >>59)&1)==1) return 59;
                if (((move >>60)&1)==1) return 60;
                if (((move >>61)&1)==1) return 61;
                if (((move >>62)&1)==1) return 62;
                if (((move >>63)&1)==1) return 63;
            return 64;
        }

        // 6.3 seconds faster on 5 ply deep search than doing with for loop
        public static UInt64 reverseBit(UInt64 bitboard){
            UInt64 reverse = 0;
                if (((bitboard)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>1 )&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>2 )&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>3 )&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>4 )&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>5 )&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>6 )&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>7 )&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>8 )&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>9 )&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>10)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>11)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>12)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>13)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>14)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>15)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>16)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>17)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>18)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>19)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>20)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>21)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>22)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>23)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>24)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>25)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>26)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>27)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>28)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>29)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>30)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>31)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>32)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>33)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>34)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>35)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>36)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>37)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>38)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>39)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>40)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>41)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>42)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>43)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>44)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>45)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>46)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>47)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>48)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>49)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>50)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>51)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>52)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>53)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>54)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>55)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>56)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>57)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>58)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>59)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>60)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>61)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>62)&1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if (((bitboard >>63)&1)==1) { reverse = reverse ^ 1; }
            return reverse;
        }
        public static UInt64 reverseBitx(UInt64 bitboard){
            UInt64 reverse = 0;
                if ((bitboard & 1)==1) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 2)==2) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 4)==4) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 8)==8) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 16)==16) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 32)==32) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 64)==64) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 128)==128) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 256)==256) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 512)==512) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 1024)==1024) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 2048)==2048) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 4096)==4096) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard &0x2000)==0x2000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard &0x4000)==0x4000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard &0x8000)==0x8000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard &0x10000)==0x10000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard &0x20000)==0x20000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard &0x40000)==0x40000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard &0x80000)==0x80000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 0x100000)==0x100000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 0x200000)==0x200000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 0x400000)==0x400000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 0x800000)==0x800000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard& 0x1000000)==0x1000000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 0x2000000)==0x2000000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 0x4000000)==0x4000000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 0x8000000)==0x8000000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 0x10000000)==0x10000000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 0x20000000)==0x20000000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 0x40000000)==0x40000000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 0x80000000)==0x80000000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 0x100000000)==0x100000000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 0x200000000)==0x200000000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard& 0x400000000)==0x400000000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 0x800000000)==0x800000000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard& 0x1000000000)==0x1000000000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 0x2000000000)==0x2000000000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 0x4000000000)==0x4000000000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 0x8000000000)==0x8000000000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 0x10000000000)==0x10000000000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 0x20000000000)==0x20000000000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 0x40000000000)==0x40000000000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 0x80000000000)==0x80000000000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 0x100000000000)==0x100000000000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 0x200000000000)==0x200000000000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 0x400000000000)==0x400000000000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 0x800000000000)==0x800000000000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 0x1000000000000)==0x1000000000000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 0x2000000000000)==0x2000000000000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 0x4000000000000)==0x4000000000000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 0x8000000000000)==0x8000000000000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 0x10000000000000)==0x10000000000000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 0x20000000000000)==0x20000000000000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 0x40000000000000)==0x40000000000000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 0x80000000000000)==0x80000000000000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 0x100000000000000)==0x100000000000000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 0x200000000000000)==0x200000000000000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 0x400000000000000)==0x400000000000000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 0x800000000000000)==0x800000000000000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 0x1000000000000000)==0x1000000000000000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 0x2000000000000000)==0x2000000000000000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 0x4000000000000000)==0x4000000000000000) { reverse = reverse ^ 1; }
                reverse = reverse << 1;
                if ((bitboard & 0x8000000000000000)==0x8000000000000000) { reverse = reverse ^ 1; }
            return reverse;
        }

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

        public static bool ArrC(char[] arr1, char[] arr2){

            if( arr1[0] != arr2[0] ) { return false;}
            if( arr1[1] != arr2[1] ) { return false;}
            if( arr1[2] != arr2[2] ) { return false;}
            if( arr1[3] != arr2[3] ) { return false;}

            return true;
        }

        public static bool IsCharDigit(char c){
            return ((c >= '0') && (c <= '9'));
        }

        public static int Abs(int i){
            return ((i + (i >> 31)) ^ (i >> 31));
        }

        public static char[] CharCombine(char c0, char c1, char c2, char c3){
            char[] c = new char[4];
            c[0] = c0;
            c[1] = c1;
            c[2] = c2;
            c[3] = c3;
            return c;
        }
        public static UInt64 reverseBitSingle(UInt64 bitboard){
        // almost twice as fast as other method but only for single bit
        // almost 3 seconds faster at perft search depth of 5 almost 10% faster
            UInt64 reverse = 1;
            int shift = Tools.trailingZerosRight(bitboard);
            reverse = reverse << (63-shift);
            return reverse;
        }

        public static UInt64 reverseBitxx(UInt64 bitboard){
            // is doing XOR on every last 0 faster checking a flag and skip the XOR and just shift the rest?
            UInt64 reverse = 0;
            for(int i = 0; i < 64; i++){
                reverse = reverse << 1;
                if(((bitboard & 1) == 1)){
                    reverse = reverse ^ 1;
                }
                bitboard = bitboard >> 1;
            }
            return reverse;
        }

        public static int trailingZerosLeft(UInt64 move){
            for(int i = 0; i<64; i++){
                if (((move <<i)&0x8000000000000000)==0x8000000000000000) return i;
            }
            return 0;
        }

        public static int trailingZerosRightLoop(UInt64 move){
            for(int i = 0; i<64; i++){
                if (((move >>i)&1)==1) return i;
            }
            return 64;
        }
    }
}
