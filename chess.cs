using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;

namespace ChessBitboard{

    public class Chess{

        public static void Main (){

             BoardGeneration.initiateStdChess();







        }
    }
}
            // assume we have a attack set of a queen and want to know if it attacks opponent pieces it may capture. You AND & the queen attacks with the set of opponent pieces
            //
            //
            // to test whether a is subset of set b
            // you AND & the two sets and if the result is eq == to a, then a is a subset of b (all a bits overlap with some of b's bits)
            //
            // to test if two sets a and b are dijoint you AND & the two sets and if result is eq == 0 then a and b are disjoint sets (no bits overlap)
            //
            // the unioin or disjunction of two bitboards is applied by bitwise or |. The union is superset of the intersection, while the intersection is
            //
            // to test if a bit is a 1 // eks. 0111 = 7. test if 3rd bit is 1. (7>>2)&1 = 1 for true. to test 4th bit (7>>3) = 0 for false
            //
            //
            //
            //

            /*
        Console.WriteLine("public static UInt64[] Knight = new UInt64[64];");

            for(int i = 1; i<19; i++){
                if((i+17) % 8 < 4){
                    UInt64 printx = Moves.kingSpan >> i;
                    printx = printx & ~Moves.fileGH;
                    Console.Write($"{printx},");
                      // BoardGeneration.drawBitboard(printx);
                      // Console.Read();
                }else{
                    UInt64 printx = Moves.kingSpan >> i;
                    printx = printx & ~Moves.fileAB;
                    Console.Write($"{printx},");
                      // BoardGeneration.drawBitboard(printx);
                      // Console.Read();

                }
            }
            Console.WriteLine();
            for(int i = 0; i<64-18;i++){
                if((i+18) % 8 < 4){
                    UInt64 printx = Moves.kingSpan << i;
                    printx = printx & ~Moves.fileGH;
                    Console.Write($"{printx},");
                     // BoardGeneration.drawBitboard(printx);
                     // Console.Read();
                }else{
                    UInt64 printx = Moves.kingSpan << i;
                    printx = printx & ~Moves.fileAB;
                    Console.Write($"{printx},");
                     // BoardGeneration.drawBitboard(printx);
                     // Console.Read();
                }
            }
            */
/*
                int count = 0;
            foreach(UInt64 n in Span.Knight ){
                BoardGeneration.drawBitboard(n);
                Console.WriteLine(n);
                Console.WriteLine(count);
                Console.ReadKey(true);
                count++;

            }
*/
