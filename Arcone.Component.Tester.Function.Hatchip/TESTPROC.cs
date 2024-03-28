using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Arcone.Component.Tester.Function.Hatchip
{
    public partial class TESTPROC : TESTPROC_BASE
    {
        //1#1,Cp-D;2,Cp-Q;3,Cp-G;4,Cp-Rp;5,Cs-D;6,Cs-Q;7,Cs-Rs
        //8#8,Lp-Q;9,Lp-D;10,Lp-G;11,Lp-Rp;12,Ls-D;13,Ls-Q;14,Ls-Rs
        //15#15,R-X;16,Z-deg;17,Z-Rad;18,G_B;19,Y-deg;20,Y-rad
        internal string getHP4284MeasureFunction(int functionIndex)
        {
            string functionName = "CPD";
            switch (functionIndex)
            {
                case 1:
                    functionName = "CPD";
                    break;
                case 2:
                    functionName = "CPQ";
                    break;
                case 3:
                    functionName = "CPG";
                    break;
                case 4:
                    functionName = "CPRP";
                    break;
                case 5:
                    functionName = "CSD";
                    break;
                case 6:
                    functionName = "CSQ";
                    break;
                case 7:
                    functionName = "CSRS";
                    break;
                case 8:
                    functionName = "LPQ";
                    break;
                case 9:
                    functionName = "LPD";
                    break;
                case 10:
                    functionName = "LPG";
                    break;
                case 11:
                    functionName = "LPRP";
                    break;
                case 12:
                    functionName = "LSD";
                    break;
                case 13:
                    functionName = "LSQ";
                    break;
                case 14:
                    functionName = "LSRS";
                    break;
                case 15:
                    functionName = "RX";
                    break;
                case 16:
                    functionName = "ZTD";
                    break;
                case 17:
                    functionName = "ZTR";
                    break;
                case 18:
                    functionName = "GB";
                    break;
                case 19:
                    functionName = "YTD";
                    break;
                case 20:
                    functionName = "YTR";
                    break;
                default:
                    functionName = "CPD";
                    break;
            }

            return functionName;
        }

    }
}
