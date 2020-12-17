using System;
// using System.Collections.Generic;
// using System.Linq;
// using System.Text;
// using System.Threading.Tasks;

namespace ChessBitboard{

    public class Chess{
        public static void Main (){
             BoardGeneration.initiateStdChess();

             //
             // Følgende er brugt til at metaprogrammere BitSpan.cs
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
        }
    }
}
