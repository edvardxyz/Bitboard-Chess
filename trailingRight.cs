using System;

namespace ChessBitboard{

    public class Zero{

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
