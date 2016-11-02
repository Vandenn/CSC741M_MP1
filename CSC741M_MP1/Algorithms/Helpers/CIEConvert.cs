using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColorMine.ColorSpaces;

namespace CSC741M_MP1.Algorithms.Helpers
{
    /* ---------------------------------------------------------------------------------
     * CIE Color Space Conversion Class
     * This calss has methods that can convert RGB values to Luv
     * It also contains methods to get the LUV indexed color(quantized into 159 colors) 
     * based on programs by:
     *    Greg Thoenen and the University of Utah,
     *    Mohan Kankahalli, NUS
     * ---------------------------------------------------------------------------------
     * Converted from code provided by: Sir Conrado Ruiz
     * ---------------------------------------------------------------------------------
     */
    public class CIEConvert
    {
        private static bool initialized = false;
        public static Luv[] LuvIndex;
        private static List<int> LuvIndexLPositions;
        public static double[,] LuvSimilarityMatrix;
        public static double MaxLuvDistance;
        public static double PerceptualDifferenceThreshold;

        protected CIEConvert() {}

        public static void initialize()
        {
            if (!initialized)
            {
                LuvIndex = new Luv[159];
                LuvIndexLPositions = new List<int>();
                LuvSimilarityMatrix = new double[159, 159];
                MaxLuvDistance = 0;
                PerceptualDifferenceThreshold = 0;
                for (int i = 0; i < 159; i++)
                {
                    LuvIndex[i] = new Luv();
                }
                initLuvIndex();
                initLuvSimilarityMatrix();
                initialized = true;
            }
        }

        public static Luv RGBtoLUV(Color color)
        {
            return RGBtoLUV(new Rgb { R = color.R, G = color.G, B = color.B });
        }

        public static Luv RGBtoLUV(Rgb rgb)
        {
            return rgb.To<Luv>();
        }

        public static Rgb LUVtoRGB(Luv luv)
        {
            return luv.To<Rgb>();
        }

        public static void initLuvIndex()
        {
            LuvIndexLPositions.Add(0);
            LuvIndex[0].L = 0.0; LuvIndex[0].U = 0.0; LuvIndex[0].V = 0.0;
            LuvIndexLPositions.Add(1);
            LuvIndex[1].L = 11.111111; LuvIndex[1].U = -14.363326; LuvIndex[1].V = 5.685223;
            LuvIndex[2].L = 11.111111; LuvIndex[2].U = 24.831306; LuvIndex[2].V = 5.685223;
            LuvIndexLPositions.Add(3);
            LuvIndex[3].L = 22.222222; LuvIndex[3].U = -14.363326; LuvIndex[3].V = -52.209036;
            LuvIndex[4].L = 22.222222; LuvIndex[4].U = -14.363326; LuvIndex[4].V = -23.261906;
            LuvIndex[5].L = 22.222222; LuvIndex[5].U = -14.363326; LuvIndex[5].V = 5.685223;
            LuvIndex[6].L = 22.222222; LuvIndex[6].U = 24.831306; LuvIndex[6].V = -23.261906;
            LuvIndex[7].L = 22.222222; LuvIndex[7].U = 24.831306; LuvIndex[7].V = 5.685223;
            LuvIndex[8].L = 22.222222; LuvIndex[8].U = 64.025939; LuvIndex[8].V = 5.685223;
            LuvIndexLPositions.Add(9);
            LuvIndex[9].L = 33.333333; LuvIndex[9].U = -14.363326; LuvIndex[9].V = -81.156165;
            LuvIndex[10].L = 33.333333; LuvIndex[10].U = -14.363326; LuvIndex[10].V = -52.209036;
            LuvIndex[11].L = 33.333333; LuvIndex[11].U = -14.363326; LuvIndex[11].V = -23.261906;
            LuvIndex[12].L = 33.333333; LuvIndex[12].U = -14.363326; LuvIndex[12].V = 5.685223;
            LuvIndex[13].L = 33.333333; LuvIndex[13].U = -14.363326; LuvIndex[13].V = 34.632352;
            LuvIndex[14].L = 33.333333; LuvIndex[14].U = 24.831306; LuvIndex[14].V = -52.209036;
            LuvIndex[15].L = 33.333333; LuvIndex[15].U = 24.831306; LuvIndex[15].V = -23.261906;
            LuvIndex[16].L = 33.333333; LuvIndex[16].U = 24.831306; LuvIndex[16].V = 5.685223;
            LuvIndex[17].L = 33.333333; LuvIndex[17].U = 24.831306; LuvIndex[17].V = 34.632352;
            LuvIndex[18].L = 33.333333; LuvIndex[18].U = 64.025939; LuvIndex[18].V = -23.261906;
            LuvIndex[19].L = 33.333333; LuvIndex[19].U = 64.025939; LuvIndex[19].V = 5.685223;
            LuvIndex[20].L = 33.333333; LuvIndex[20].U = 64.025939; LuvIndex[20].V = 34.632352;
            LuvIndexLPositions.Add(21);
            LuvIndex[21].L = 44.444444; LuvIndex[21].U = -53.557959; LuvIndex[21].V = -23.261906;
            LuvIndex[22].L = 44.444444; LuvIndex[22].U = -53.557959; LuvIndex[22].V = 5.685223;
            LuvIndex[23].L = 44.444444; LuvIndex[23].U = -53.557959; LuvIndex[23].V = 34.632352;
            LuvIndex[24].L = 44.444444; LuvIndex[24].U = -53.557959; LuvIndex[24].V = 63.579482;
            LuvIndex[25].L = 44.444444; LuvIndex[25].U = -14.363326; LuvIndex[25].V = -110.103295;
            LuvIndex[26].L = 44.444444; LuvIndex[26].U = -14.363326; LuvIndex[26].V = -81.156165;
            LuvIndex[27].L = 44.444444; LuvIndex[27].U = -14.363326; LuvIndex[27].V = -52.209036;
            LuvIndex[28].L = 44.444444; LuvIndex[28].U = -14.363326; LuvIndex[28].V = -23.261906;
            LuvIndex[29].L = 44.444444; LuvIndex[29].U = -14.363326; LuvIndex[29].V = 5.685223;
            LuvIndex[30].L = 44.444444; LuvIndex[30].U = -14.363326; LuvIndex[30].V = 34.632352;
            LuvIndex[31].L = 44.444444; LuvIndex[31].U = 24.831306; LuvIndex[31].V = -81.156165;
            LuvIndex[32].L = 44.444444; LuvIndex[32].U = 24.831306; LuvIndex[32].V = -52.209036;
            LuvIndex[33].L = 44.444444; LuvIndex[33].U = 24.831306; LuvIndex[33].V = -23.261906;
            LuvIndex[34].L = 44.444444; LuvIndex[34].U = 24.831306; LuvIndex[34].V = 5.685223;
            LuvIndex[35].L = 44.444444; LuvIndex[35].U = 24.831306; LuvIndex[35].V = 34.632352;
            LuvIndex[36].L = 44.444444; LuvIndex[36].U = 64.025939; LuvIndex[36].V = -52.209036;
            LuvIndex[37].L = 44.444444; LuvIndex[37].U = 64.025939; LuvIndex[37].V = -23.261906;
            LuvIndex[38].L = 44.444444; LuvIndex[38].U = 64.025939; LuvIndex[38].V = 5.685223;
            LuvIndex[39].L = 44.444444; LuvIndex[39].U = 64.025939; LuvIndex[39].V = 34.632352;
            LuvIndex[40].L = 44.444444; LuvIndex[40].U = 103.220572; LuvIndex[40].V = 5.685223;
            LuvIndex[41].L = 44.444444; LuvIndex[41].U = 103.220572; LuvIndex[41].V = 34.632352;
            LuvIndex[42].L = 44.444444; LuvIndex[42].U = 142.415204; LuvIndex[42].V = 34.632352;
            LuvIndexLPositions.Add(43);
            LuvIndex[43].L = 55.555556; LuvIndex[43].U = -53.557959; LuvIndex[43].V = -81.156165;
            LuvIndex[44].L = 55.555556; LuvIndex[44].U = -53.557959; LuvIndex[44].V = -52.209036;
            LuvIndex[45].L = 55.555556; LuvIndex[45].U = -53.557959; LuvIndex[45].V = -23.261906;
            LuvIndex[46].L = 55.555556; LuvIndex[46].U = -53.557959; LuvIndex[46].V = 5.685223;
            LuvIndex[47].L = 55.555556; LuvIndex[47].U = -53.557959; LuvIndex[47].V = 34.632352;
            LuvIndex[48].L = 55.555556; LuvIndex[48].U = -53.557959; LuvIndex[48].V = 63.579482;
            LuvIndex[49].L = 55.555556; LuvIndex[49].U = -14.363326; LuvIndex[49].V = -110.103295;
            LuvIndex[50].L = 55.555556; LuvIndex[50].U = -14.363326; LuvIndex[50].V = -81.156165;
            LuvIndex[51].L = 55.555556; LuvIndex[51].U = -14.363326; LuvIndex[51].V = -52.209036;
            LuvIndex[52].L = 55.555556; LuvIndex[52].U = -14.363326; LuvIndex[52].V = -23.261906;
            LuvIndex[53].L = 55.555556; LuvIndex[53].U = -14.363326; LuvIndex[53].V = 5.685223;
            LuvIndex[54].L = 55.555556; LuvIndex[54].U = -14.363326; LuvIndex[54].V = 34.632352;
            LuvIndex[55].L = 55.555556; LuvIndex[55].U = -14.363326; LuvIndex[55].V = 63.579482;
            LuvIndex[56].L = 55.555556; LuvIndex[56].U = 24.831306; LuvIndex[56].V = -110.103295;
            LuvIndex[57].L = 55.555556; LuvIndex[57].U = 24.831306; LuvIndex[57].V = -81.156165;
            LuvIndex[58].L = 55.555556; LuvIndex[58].U = 24.831306; LuvIndex[58].V = -52.209036;
            LuvIndex[59].L = 55.555556; LuvIndex[59].U = 24.831306; LuvIndex[59].V = -23.261906;
            LuvIndex[60].L = 55.555556; LuvIndex[60].U = 24.831306; LuvIndex[60].V = 5.685223;
            LuvIndex[61].L = 55.555556; LuvIndex[61].U = 24.831306; LuvIndex[61].V = 34.632352;
            LuvIndex[62].L = 55.555556; LuvIndex[62].U = 24.831306; LuvIndex[62].V = 63.579482;
            LuvIndex[63].L = 55.555556; LuvIndex[63].U = 64.025939; LuvIndex[63].V = -81.156165;
            LuvIndex[64].L = 55.555556; LuvIndex[64].U = 64.025939; LuvIndex[64].V = -52.209036;
            LuvIndex[65].L = 55.555556; LuvIndex[65].U = 64.025939; LuvIndex[65].V = -23.261906;
            LuvIndex[66].L = 55.555556; LuvIndex[66].U = 64.025939; LuvIndex[66].V = 5.685223;
            LuvIndex[67].L = 55.555556; LuvIndex[67].U = 64.025939; LuvIndex[67].V = 34.632352;
            LuvIndex[68].L = 55.555556; LuvIndex[68].U = 64.025939; LuvIndex[68].V = 63.579482;
            LuvIndex[69].L = 55.555556; LuvIndex[69].U = 103.220572; LuvIndex[69].V = -23.261906;
            LuvIndex[70].L = 55.555556; LuvIndex[70].U = 103.220572; LuvIndex[70].V = 5.685223;
            LuvIndex[71].L = 55.555556; LuvIndex[71].U = 103.220572; LuvIndex[71].V = 34.632352;
            LuvIndex[72].L = 55.555556; LuvIndex[72].U = 142.415204; LuvIndex[72].V = 5.685223;
            LuvIndex[73].L = 55.555556; LuvIndex[73].U = 142.415204; LuvIndex[73].V = 34.632352;
            LuvIndex[74].L = 55.555556; LuvIndex[74].U = 181.609837; LuvIndex[74].V = 34.632352;
            LuvIndexLPositions.Add(75);
            LuvIndex[75].L = 66.666667; LuvIndex[75].U = -92.752592; LuvIndex[75].V = 34.632352;
            LuvIndex[76].L = 66.666667; LuvIndex[76].U = -92.752592; LuvIndex[76].V = 63.579482;
            LuvIndex[77].L = 66.666667; LuvIndex[77].U = -92.752592; LuvIndex[77].V = 92.526611;
            LuvIndex[78].L = 66.666667; LuvIndex[78].U = -53.557959; LuvIndex[78].V = -81.156165;
            LuvIndex[79].L = 66.666667; LuvIndex[79].U = -53.557959; LuvIndex[79].V = -52.209036;
            LuvIndex[80].L = 66.666667; LuvIndex[80].U = -53.557959; LuvIndex[80].V = -23.261906;
            LuvIndex[81].L = 66.666667; LuvIndex[81].U = -53.557959; LuvIndex[81].V = 5.685223;
            LuvIndex[82].L = 66.666667; LuvIndex[82].U = -53.557959; LuvIndex[82].V = 34.632352;
            LuvIndex[83].L = 66.666667; LuvIndex[83].U = -53.557959; LuvIndex[83].V = 63.579482;
            LuvIndex[84].L = 66.666667; LuvIndex[84].U = -53.557959; LuvIndex[84].V = 92.526611;
            LuvIndex[85].L = 66.666667; LuvIndex[85].U = -14.363326; LuvIndex[85].V = -81.156165;
            LuvIndex[86].L = 66.666667; LuvIndex[86].U = -14.363326; LuvIndex[86].V = -52.209036;
            LuvIndex[87].L = 66.666667; LuvIndex[87].U = -14.363326; LuvIndex[87].V = -23.261906;
            LuvIndex[88].L = 66.666667; LuvIndex[88].U = -14.363326; LuvIndex[88].V = 5.685223;
            LuvIndex[89].L = 66.666667; LuvIndex[89].U = -14.363326; LuvIndex[89].V = 34.632352;
            LuvIndex[90].L = 66.666667; LuvIndex[90].U = -14.363326; LuvIndex[90].V = 63.579482;
            LuvIndex[91].L = 66.666667; LuvIndex[91].U = 24.831306; LuvIndex[91].V = -81.156165;
            LuvIndex[92].L = 66.666667; LuvIndex[92].U = 24.831306; LuvIndex[92].V = -52.209036;
            LuvIndex[93].L = 66.666667; LuvIndex[93].U = 24.831306; LuvIndex[93].V = -23.261906;
            LuvIndex[94].L = 66.666667; LuvIndex[94].U = 24.831306; LuvIndex[94].V = 5.685223;
            LuvIndex[95].L = 66.666667; LuvIndex[95].U = 24.831306; LuvIndex[95].V = 34.632352;
            LuvIndex[96].L = 66.666667; LuvIndex[96].U = 24.831306; LuvIndex[96].V = 63.579482;
            LuvIndex[97].L = 66.666667; LuvIndex[97].U = 64.025939; LuvIndex[97].V = -81.156165;
            LuvIndex[98].L = 66.666667; LuvIndex[98].U = 64.025939; LuvIndex[98].V = -52.209036;
            LuvIndex[99].L = 66.666667; LuvIndex[99].U = 64.025939; LuvIndex[99].V = -23.261906;
            LuvIndex[100].L = 66.666667; LuvIndex[100].U = 64.025939; LuvIndex[100].V = 5.685223;
            LuvIndex[101].L = 66.666667; LuvIndex[101].U = 64.025939; LuvIndex[101].V = 34.632352;
            LuvIndex[102].L = 66.666667; LuvIndex[102].U = 64.025939; LuvIndex[102].V = 63.579482;
            LuvIndex[103].L = 66.666667; LuvIndex[103].U = 103.220572; LuvIndex[103].V = -52.209036;
            LuvIndex[104].L = 66.666667; LuvIndex[104].U = 103.220572; LuvIndex[104].V = -23.261906;
            LuvIndex[105].L = 66.666667; LuvIndex[105].U = 103.220572; LuvIndex[105].V = 5.685223;
            LuvIndex[106].L = 66.666667; LuvIndex[106].U = 103.220572; LuvIndex[106].V = 34.632352;
            LuvIndex[107].L = 66.666667; LuvIndex[107].U = 103.220572; LuvIndex[107].V = 63.579482;
            LuvIndex[108].L = 66.666667; LuvIndex[108].U = 142.415204; LuvIndex[108].V = -23.261906;
            LuvIndex[109].L = 66.666667; LuvIndex[109].U = 142.415204; LuvIndex[109].V = 5.685223;
            LuvIndex[110].L = 66.666667; LuvIndex[110].U = 142.415204; LuvIndex[110].V = 34.632352;
            LuvIndex[111].L = 66.666667; LuvIndex[111].U = 142.415204; LuvIndex[111].V = 63.579482;
            LuvIndex[112].L = 66.666667; LuvIndex[112].U = 181.609837; LuvIndex[112].V = 63.579482;
            LuvIndexLPositions.Add(113);
            LuvIndex[113].L = 77.777778; LuvIndex[113].U = -92.752592; LuvIndex[113].V = -52.209036;
            LuvIndex[114].L = 77.777778; LuvIndex[114].U = -92.752592; LuvIndex[114].V = -23.261906;
            LuvIndex[115].L = 77.777778; LuvIndex[115].U = -92.752592; LuvIndex[115].V = 5.685223;
            LuvIndex[116].L = 77.777778; LuvIndex[116].U = -92.752592; LuvIndex[116].V = 34.632352;
            LuvIndex[117].L = 77.777778; LuvIndex[117].U = -92.752592; LuvIndex[117].V = 63.579482;
            LuvIndex[118].L = 77.777778; LuvIndex[118].U = -92.752592; LuvIndex[118].V = 92.526611;
            LuvIndex[119].L = 77.777778; LuvIndex[119].U = -53.557959; LuvIndex[119].V = -52.209036;
            LuvIndex[120].L = 77.777778; LuvIndex[120].U = -53.557959; LuvIndex[120].V = -23.261906;
            LuvIndex[121].L = 77.777778; LuvIndex[121].U = -53.557959; LuvIndex[121].V = 5.685223;
            LuvIndex[122].L = 77.777778; LuvIndex[122].U = -53.557959; LuvIndex[122].V = 34.632352;
            LuvIndex[123].L = 77.777778; LuvIndex[123].U = -53.557959; LuvIndex[123].V = 63.579482;
            LuvIndex[124].L = 77.777778; LuvIndex[124].U = -53.557959; LuvIndex[124].V = 92.526611;
            LuvIndex[125].L = 77.777778; LuvIndex[125].U = -14.363326; LuvIndex[125].V = -52.209036;
            LuvIndex[126].L = 77.777778; LuvIndex[126].U = -14.363326; LuvIndex[126].V = -23.261906;
            LuvIndex[127].L = 77.777778; LuvIndex[127].U = -14.363326; LuvIndex[127].V = 5.685223;
            LuvIndex[128].L = 77.777778; LuvIndex[128].U = -14.363326; LuvIndex[128].V = 34.632352;
            LuvIndex[129].L = 77.777778; LuvIndex[129].U = -14.363326; LuvIndex[129].V = 63.579482;
            LuvIndex[130].L = 77.777778; LuvIndex[130].U = -14.363326; LuvIndex[130].V = 92.526611;
            LuvIndex[131].L = 77.777778; LuvIndex[131].U = 24.831306; LuvIndex[131].V = -52.209036;
            LuvIndex[132].L = 77.777778; LuvIndex[132].U = 24.831306; LuvIndex[132].V = -23.261906;
            LuvIndex[133].L = 77.777778; LuvIndex[133].U = 24.831306; LuvIndex[133].V = 5.685223;
            LuvIndex[134].L = 77.777778; LuvIndex[134].U = 24.831306; LuvIndex[134].V = 34.632352;
            LuvIndex[135].L = 77.777778; LuvIndex[135].U = 24.831306; LuvIndex[135].V = 63.579482;
            LuvIndex[136].L = 77.777778; LuvIndex[136].U = 24.831306; LuvIndex[136].V = 92.526611;
            LuvIndex[137].L = 77.777778; LuvIndex[137].U = 64.025939; LuvIndex[137].V = -52.209036;
            LuvIndex[138].L = 77.777778; LuvIndex[138].U = 64.025939; LuvIndex[138].V = -23.261906;
            LuvIndex[139].L = 77.777778; LuvIndex[139].U = 64.025939; LuvIndex[139].V = 5.685223;
            LuvIndex[140].L = 77.777778; LuvIndex[140].U = 64.025939; LuvIndex[140].V = 34.632352;
            LuvIndex[141].L = 77.777778; LuvIndex[141].U = 64.025939; LuvIndex[141].V = 63.579482;
            LuvIndex[142].L = 77.777778; LuvIndex[142].U = 64.025939; LuvIndex[142].V = 92.526611;
            LuvIndex[143].L = 77.777778; LuvIndex[143].U = 103.220572; LuvIndex[143].V = 63.579482;
            LuvIndexLPositions.Add(144);
            LuvIndex[144].L = 88.888889; LuvIndex[144].U = -53.557959; LuvIndex[144].V = -23.261906;
            LuvIndex[145].L = 88.888889; LuvIndex[145].U = -53.557959; LuvIndex[145].V = 5.685223;
            LuvIndex[146].L = 88.888889; LuvIndex[146].U = -53.557959; LuvIndex[146].V = 34.632352;
            LuvIndex[147].L = 88.888889; LuvIndex[147].U = -53.557959; LuvIndex[147].V = 63.579482;
            LuvIndex[148].L = 88.888889; LuvIndex[148].U = -14.363326; LuvIndex[148].V = -23.261906;
            LuvIndex[149].L = 88.888889; LuvIndex[149].U = -14.363326; LuvIndex[149].V = 5.685223;
            LuvIndex[150].L = 88.888889; LuvIndex[150].U = -14.363326; LuvIndex[150].V = 34.632352;
            LuvIndex[151].L = 88.888889; LuvIndex[151].U = -14.363326; LuvIndex[151].V = 63.579482;
            LuvIndex[152].L = 88.888889; LuvIndex[152].U = -14.363326; LuvIndex[152].V = 92.526611;
            LuvIndex[153].L = 88.888889; LuvIndex[153].U = 24.831306; LuvIndex[153].V = -23.261906;
            LuvIndex[154].L = 88.888889; LuvIndex[154].U = 24.831306; LuvIndex[154].V = 5.685223;
            LuvIndex[155].L = 88.888889; LuvIndex[155].U = 24.831306; LuvIndex[155].V = 34.632352;
            LuvIndex[156].L = 88.888889; LuvIndex[156].U = 24.831306; LuvIndex[156].V = 63.579482;
            LuvIndex[157].L = 88.888889; LuvIndex[157].U = 24.831306; LuvIndex[157].V = 92.526611;
            LuvIndexLPositions.Add(158);
            LuvIndex[158].L = 100; LuvIndex[158].U = 0; LuvIndex[158].V = 0;
        }
        public static void initLuvSimilarityMatrix()
        {
            for (int i = 0; i < LuvSimilarityMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < LuvSimilarityMatrix.GetLength(1); j++)
                {
                    LuvSimilarityMatrix[i, j] = getLuvDistance(LuvIndex[i], LuvIndex[j]);
                    MaxLuvDistance = Math.Max(MaxLuvDistance, LuvSimilarityMatrix[i, j]);
                }
            }
            PerceptualDifferenceThreshold = 0.2 * MaxLuvDistance;
            for (int i = 0; i < LuvSimilarityMatrix.GetLength(0); i++)
            {
                for (int j = 0; j < LuvSimilarityMatrix.GetLength(1); j++)
                {
                    if (LuvSimilarityMatrix[i, j] > PerceptualDifferenceThreshold)
                    {
                        LuvSimilarityMatrix[i, j] = 0;
                    }
                    else
                    {
                        LuvSimilarityMatrix[i, j] = 1 - (LuvSimilarityMatrix[i, j]/PerceptualDifferenceThreshold);
                    }
                }
            }
        }
        private static double getLuvDistance(Luv luv1, Luv luv2)
        {
            return Math.Sqrt(Math.Pow(luv1.L - luv2.L, 2) + Math.Pow(luv1.U - luv2.U, 2) + Math.Pow(luv1.V - luv2.V, 2));
        }
        private static double round(double value, int decimalPlace)
        {
            double powerOfTen = 1;
            while (decimalPlace-- > 0)
            {
                powerOfTen *= 10.0;
            }
            return Math.Round(value * powerOfTen) / powerOfTen;
        }

        public static int LuvIndexOf(Luv luv)
        {
            int nL = 0;
            int nHi = 0;

            /*//Old Code
            while (nHi < 159 && luv.L > LuvIndex[nHi].L)
            {
                nHi++;
            }*/
            foreach (int index in LuvIndexLPositions)
            {
                nHi = index;
                if (luv.L > LuvIndex[nHi].L) continue;
                else break;
            }

            if (luv.L != LuvIndex[nHi].L)
            {
                nHi--;
            }

            /*//Old Code
            while (nL < 159 && LuvIndex[nL].L != LuvIndex[nHi].L)
            {
                nL++;
            }*/
            foreach (int index in LuvIndexLPositions)
            {
                nL = index;
                if (LuvIndex[nL].L != LuvIndex[nHi].L) continue;
                else break;
            }

            for (int i = nL; i < nHi; i++)
            {
                if (luv.U <= LuvIndex[i].U)
                {
                    if (luv.V <= LuvIndex[i].V)
                        return i;
                }
            }

            return nHi;
        }
    }

    /*public class CIEConvert2
    {
        public LUVClass finalLUV;
        public double r, g, b, Index;
        private double X, Y, Z, uPrime, vPrime, un, vn;
        private LUVClass[] LuvIndex = new LUVClass[160];

        public CIEConvert2()
        {
            finalLUV = new LUVClass();
            for (int i = 0; i < 160; i++)
            {   
                LuvIndex[i] = new LUVClass();
            }
            initLuvIndex();
        }

        public void RGBtoXYZ()
        {
            X = 0.607 * r + 0.174 * g + 0.200 * b;
            Y = 0.299 * r + 0.587 * g + 0.114 * b;
            Z = 0.066 * g + 1.116 * b;

            un = 0.2022217;
            vn = 0.4608889;
        }

        public double round(double value, int decimalPlace)
        {
            double powerOfTen = 1;
            while (decimalPlace-- > 0)
            {
                powerOfTen *= 10.0;
            }
            return Math.Round(value * powerOfTen) / powerOfTen;
        }

        public void XYZtoLUV()
        {
            double temp = X + 15.0 * Y + 3.0 * Z;

            if (Y > 0.008856)
            {
                finalLUV.L = 116.0 * Math.Pow(Y, 0.3333333) - 16.0;
            }
            else
            {
                finalLUV.L = 903.3 * Y;
            }

            if (temp > 0.000001)
            {
                uPrime = 4.0 * X / temp;
                vPrime = 9.0 * Y / temp;
                finalLUV.U = 13.0 * finalLUV.L * (uPrime - un);
                finalLUV.V = 13.0 * finalLUV.L * (vPrime - vn);
            }
            else
            {
                finalLUV.U = 0.0;
                finalLUV.V = 0.0;
            }

            finalLUV.L = round(finalLUV.L, 6);
            finalLUV.U = round(finalLUV.U, 6);
            finalLUV.V = round(finalLUV.V, 6);
        }


        public void RGBtoLUV()
        {
            RGBtoXYZ();
            XYZtoLUV();
        }

        public void LUVtoXYZ()
        {
            if (finalLUV.L == 0.0)
            {
                X = Y = Z = 0.0;
                return;
            }

            un = 0.2022217;
            vn = 0.4608889;

            uPrime = finalLUV.U / (13.0 * finalLUV.L) + un;
            vPrime = finalLUV.V / (13.0 * finalLUV.L) + vn;

            if (finalLUV.L < 7.9996248)
            {
                Y = finalLUV.L / 903.3;
            }
            else
            {
                Y = Math.Pow((finalLUV.L + 16.0) / 116.0, 3.0);
            }

            X = -(9.0 * Y * uPrime) / ((uPrime - 4.0) * vPrime - uPrime * vPrime);
            Z = (9.0 * Y - 15.0 * vPrime * Y - vPrime * X) / (3.0 * vPrime);
        }

        public void setValues(double nr, double ng, double nb)
        {
            r = nr;
            g = ng;
            b = nb;

            RGBtoLUV();
        }

        public void XYZtoRGB()
        {
            r = 1.910 * X - 0.532 * Y - 0.288 * Z;
            g = -0.985 * X + 1.999 * Y - 0.028 * Z;
            b = 0.058 * X - 0.118 * Y + 0.898 * Z;
        }

        public void LUVtoRGB()
        {
            LUVtoXYZ();
            XYZtoRGB();
        }

        public void initLuvIndex()
        {
            LuvIndex[0].L = 0.0; LuvIndex[0].U = 0.0; LuvIndex[0].V = 0.0;
            LuvIndex[1].L = 11.111111; LuvIndex[1].U = -14.363326; LuvIndex[1].V = 5.685223;
            LuvIndex[2].L = 11.111111; LuvIndex[2].U = 24.831306; LuvIndex[2].V = 5.685223;
            LuvIndex[3].L = 22.222222; LuvIndex[3].U = -14.363326; LuvIndex[3].V = -52.209036;
            LuvIndex[4].L = 22.222222; LuvIndex[4].U = -14.363326; LuvIndex[4].V = -23.261906;
            LuvIndex[5].L = 22.222222; LuvIndex[5].U = -14.363326; LuvIndex[5].V = 5.685223;
            LuvIndex[6].L = 22.222222; LuvIndex[6].U = 24.831306; LuvIndex[6].V = -23.261906;
            LuvIndex[7].L = 22.222222; LuvIndex[7].U = 24.831306; LuvIndex[7].V = 5.685223;
            LuvIndex[8].L = 22.222222; LuvIndex[8].U = 64.025939; LuvIndex[8].V = 5.685223;
            LuvIndex[9].L = 33.333333; LuvIndex[9].U = -14.363326; LuvIndex[9].V = -81.156165;
            LuvIndex[10].L = 33.333333; LuvIndex[10].U = -14.363326; LuvIndex[10].V = -52.209036;
            LuvIndex[11].L = 33.333333; LuvIndex[11].U = -14.363326; LuvIndex[11].V = -23.261906;
            LuvIndex[12].L = 33.333333; LuvIndex[12].U = -14.363326; LuvIndex[12].V = 5.685223;
            LuvIndex[13].L = 33.333333; LuvIndex[13].U = -14.363326; LuvIndex[13].V = 34.632352;
            LuvIndex[14].L = 33.333333; LuvIndex[14].U = 24.831306; LuvIndex[14].V = -52.209036;
            LuvIndex[15].L = 33.333333; LuvIndex[15].U = 24.831306; LuvIndex[15].V = -23.261906;
            LuvIndex[16].L = 33.333333; LuvIndex[16].U = 24.831306; LuvIndex[16].V = 5.685223;
            LuvIndex[17].L = 33.333333; LuvIndex[17].U = 24.831306; LuvIndex[17].V = 34.632352;
            LuvIndex[18].L = 33.333333; LuvIndex[18].U = 64.025939; LuvIndex[18].V = -23.261906;
            LuvIndex[19].L = 33.333333; LuvIndex[19].U = 64.025939; LuvIndex[19].V = 5.685223;
            LuvIndex[20].L = 33.333333; LuvIndex[20].U = 64.025939; LuvIndex[20].V = 34.632352;
            LuvIndex[21].L = 44.444444; LuvIndex[21].U = -53.557959; LuvIndex[21].V = -23.261906;
            LuvIndex[22].L = 44.444444; LuvIndex[22].U = -53.557959; LuvIndex[22].V = 5.685223;
            LuvIndex[23].L = 44.444444; LuvIndex[23].U = -53.557959; LuvIndex[23].V = 34.632352;
            LuvIndex[24].L = 44.444444; LuvIndex[24].U = -53.557959; LuvIndex[24].V = 63.579482;
            LuvIndex[25].L = 44.444444; LuvIndex[25].U = -14.363326; LuvIndex[25].V = -110.103295;
            LuvIndex[26].L = 44.444444; LuvIndex[26].U = -14.363326; LuvIndex[26].V = -81.156165;
            LuvIndex[27].L = 44.444444; LuvIndex[27].U = -14.363326; LuvIndex[27].V = -52.209036;
            LuvIndex[28].L = 44.444444; LuvIndex[28].U = -14.363326; LuvIndex[28].V = -23.261906;
            LuvIndex[29].L = 44.444444; LuvIndex[29].U = -14.363326; LuvIndex[29].V = 5.685223;
            LuvIndex[30].L = 44.444444; LuvIndex[30].U = -14.363326; LuvIndex[30].V = 34.632352;
            LuvIndex[31].L = 44.444444; LuvIndex[31].U = 24.831306; LuvIndex[31].V = -81.156165;
            LuvIndex[32].L = 44.444444; LuvIndex[32].U = 24.831306; LuvIndex[32].V = -52.209036;
            LuvIndex[33].L = 44.444444; LuvIndex[33].U = 24.831306; LuvIndex[33].V = -23.261906;
            LuvIndex[34].L = 44.444444; LuvIndex[34].U = 24.831306; LuvIndex[34].V = 5.685223;
            LuvIndex[35].L = 44.444444; LuvIndex[35].U = 24.831306; LuvIndex[35].V = 34.632352;
            LuvIndex[36].L = 44.444444; LuvIndex[36].U = 64.025939; LuvIndex[36].V = -52.209036;
            LuvIndex[37].L = 44.444444; LuvIndex[37].U = 64.025939; LuvIndex[37].V = -23.261906;
            LuvIndex[38].L = 44.444444; LuvIndex[38].U = 64.025939; LuvIndex[38].V = 5.685223;
            LuvIndex[39].L = 44.444444; LuvIndex[39].U = 64.025939; LuvIndex[39].V = 34.632352;
            LuvIndex[40].L = 44.444444; LuvIndex[40].U = 103.220572; LuvIndex[40].V = 5.685223;
            LuvIndex[41].L = 44.444444; LuvIndex[41].U = 103.220572; LuvIndex[41].V = 34.632352;
            LuvIndex[42].L = 44.444444; LuvIndex[42].U = 142.415204; LuvIndex[42].V = 34.632352;
            LuvIndex[43].L = 55.555556; LuvIndex[43].U = -53.557959; LuvIndex[43].V = -81.156165;
            LuvIndex[44].L = 55.555556; LuvIndex[44].U = -53.557959; LuvIndex[44].V = -52.209036;
            LuvIndex[45].L = 55.555556; LuvIndex[45].U = -53.557959; LuvIndex[45].V = -23.261906;
            LuvIndex[46].L = 55.555556; LuvIndex[46].U = -53.557959; LuvIndex[46].V = 5.685223;
            LuvIndex[47].L = 55.555556; LuvIndex[47].U = -53.557959; LuvIndex[47].V = 34.632352;
            LuvIndex[48].L = 55.555556; LuvIndex[48].U = -53.557959; LuvIndex[48].V = 63.579482;
            LuvIndex[49].L = 55.555556; LuvIndex[49].U = -14.363326; LuvIndex[49].V = -110.103295;
            LuvIndex[50].L = 55.555556; LuvIndex[50].U = -14.363326; LuvIndex[50].V = -81.156165;
            LuvIndex[51].L = 55.555556; LuvIndex[51].U = -14.363326; LuvIndex[51].V = -52.209036;
            LuvIndex[52].L = 55.555556; LuvIndex[52].U = -14.363326; LuvIndex[52].V = -23.261906;
            LuvIndex[53].L = 55.555556; LuvIndex[53].U = -14.363326; LuvIndex[53].V = 5.685223;
            LuvIndex[54].L = 55.555556; LuvIndex[54].U = -14.363326; LuvIndex[54].V = 34.632352;
            LuvIndex[55].L = 55.555556; LuvIndex[55].U = -14.363326; LuvIndex[55].V = 63.579482;
            LuvIndex[56].L = 55.555556; LuvIndex[56].U = 24.831306; LuvIndex[56].V = -110.103295;
            LuvIndex[57].L = 55.555556; LuvIndex[57].U = 24.831306; LuvIndex[57].V = -81.156165;
            LuvIndex[58].L = 55.555556; LuvIndex[58].U = 24.831306; LuvIndex[58].V = -52.209036;
            LuvIndex[59].L = 55.555556; LuvIndex[59].U = 24.831306; LuvIndex[59].V = -23.261906;
            LuvIndex[60].L = 55.555556; LuvIndex[60].U = 24.831306; LuvIndex[60].V = 5.685223;
            LuvIndex[61].L = 55.555556; LuvIndex[61].U = 24.831306; LuvIndex[61].V = 34.632352;
            LuvIndex[62].L = 55.555556; LuvIndex[62].U = 24.831306; LuvIndex[62].V = 63.579482;
            LuvIndex[63].L = 55.555556; LuvIndex[63].U = 64.025939; LuvIndex[63].V = -81.156165;
            LuvIndex[64].L = 55.555556; LuvIndex[64].U = 64.025939; LuvIndex[64].V = -52.209036;
            LuvIndex[65].L = 55.555556; LuvIndex[65].U = 64.025939; LuvIndex[65].V = -23.261906;
            LuvIndex[66].L = 55.555556; LuvIndex[66].U = 64.025939; LuvIndex[66].V = 5.685223;
            LuvIndex[67].L = 55.555556; LuvIndex[67].U = 64.025939; LuvIndex[67].V = 34.632352;
            LuvIndex[68].L = 55.555556; LuvIndex[68].U = 64.025939; LuvIndex[68].V = 63.579482;
            LuvIndex[69].L = 55.555556; LuvIndex[69].U = 103.220572; LuvIndex[69].V = -23.261906;
            LuvIndex[70].L = 55.555556; LuvIndex[70].U = 103.220572; LuvIndex[70].V = 5.685223;
            LuvIndex[71].L = 55.555556; LuvIndex[71].U = 103.220572; LuvIndex[71].V = 34.632352;
            LuvIndex[72].L = 55.555556; LuvIndex[72].U = 142.415204; LuvIndex[72].V = 5.685223;
            LuvIndex[73].L = 55.555556; LuvIndex[73].U = 142.415204; LuvIndex[73].V = 34.632352;
            LuvIndex[74].L = 55.555556; LuvIndex[74].U = 181.609837; LuvIndex[74].V = 34.632352;
            LuvIndex[75].L = 66.666667; LuvIndex[75].U = -92.752592; LuvIndex[75].V = 34.632352;
            LuvIndex[76].L = 66.666667; LuvIndex[76].U = -92.752592; LuvIndex[76].V = 63.579482;
            LuvIndex[77].L = 66.666667; LuvIndex[77].U = -92.752592; LuvIndex[77].V = 92.526611;
            LuvIndex[78].L = 66.666667; LuvIndex[78].U = -53.557959; LuvIndex[78].V = -81.156165;
            LuvIndex[79].L = 66.666667; LuvIndex[79].U = -53.557959; LuvIndex[79].V = -52.209036;
            LuvIndex[80].L = 66.666667; LuvIndex[80].U = -53.557959; LuvIndex[80].V = -23.261906;
            LuvIndex[81].L = 66.666667; LuvIndex[81].U = -53.557959; LuvIndex[81].V = 5.685223;
            LuvIndex[82].L = 66.666667; LuvIndex[82].U = -53.557959; LuvIndex[82].V = 34.632352;
            LuvIndex[83].L = 66.666667; LuvIndex[83].U = -53.557959; LuvIndex[83].V = 63.579482;
            LuvIndex[84].L = 66.666667; LuvIndex[84].U = -53.557959; LuvIndex[84].V = 92.526611;
            LuvIndex[85].L = 66.666667; LuvIndex[85].U = -14.363326; LuvIndex[85].V = -81.156165;
            LuvIndex[86].L = 66.666667; LuvIndex[86].U = -14.363326; LuvIndex[86].V = -52.209036;
            LuvIndex[87].L = 66.666667; LuvIndex[87].U = -14.363326; LuvIndex[87].V = -23.261906;
            LuvIndex[88].L = 66.666667; LuvIndex[88].U = -14.363326; LuvIndex[88].V = 5.685223;
            LuvIndex[89].L = 66.666667; LuvIndex[89].U = -14.363326; LuvIndex[89].V = 34.632352;
            LuvIndex[90].L = 66.666667; LuvIndex[90].U = -14.363326; LuvIndex[90].V = 63.579482;
            LuvIndex[91].L = 66.666667; LuvIndex[91].U = 24.831306; LuvIndex[91].V = -81.156165;
            LuvIndex[92].L = 66.666667; LuvIndex[92].U = 24.831306; LuvIndex[92].V = -52.209036;
            LuvIndex[93].L = 66.666667; LuvIndex[93].U = 24.831306; LuvIndex[93].V = -23.261906;
            LuvIndex[94].L = 66.666667; LuvIndex[94].U = 24.831306; LuvIndex[94].V = 5.685223;
            LuvIndex[95].L = 66.666667; LuvIndex[95].U = 24.831306; LuvIndex[95].V = 34.632352;
            LuvIndex[96].L = 66.666667; LuvIndex[96].U = 24.831306; LuvIndex[96].V = 63.579482;
            LuvIndex[97].L = 66.666667; LuvIndex[97].U = 64.025939; LuvIndex[97].V = -81.156165;
            LuvIndex[98].L = 66.666667; LuvIndex[98].U = 64.025939; LuvIndex[98].V = -52.209036;
            LuvIndex[99].L = 66.666667; LuvIndex[99].U = 64.025939; LuvIndex[99].V = -23.261906;
            LuvIndex[100].L = 66.666667; LuvIndex[100].U = 64.025939; LuvIndex[100].V = 5.685223;
            LuvIndex[101].L = 66.666667; LuvIndex[101].U = 64.025939; LuvIndex[101].V = 34.632352;
            LuvIndex[102].L = 66.666667; LuvIndex[102].U = 64.025939; LuvIndex[102].V = 63.579482;
            LuvIndex[103].L = 66.666667; LuvIndex[103].U = 103.220572; LuvIndex[103].V = -52.209036;
            LuvIndex[104].L = 66.666667; LuvIndex[104].U = 103.220572; LuvIndex[104].V = -23.261906;
            LuvIndex[105].L = 66.666667; LuvIndex[105].U = 103.220572; LuvIndex[105].V = 5.685223;
            LuvIndex[106].L = 66.666667; LuvIndex[106].U = 103.220572; LuvIndex[106].V = 34.632352;
            LuvIndex[107].L = 66.666667; LuvIndex[107].U = 103.220572; LuvIndex[107].V = 63.579482;
            LuvIndex[108].L = 66.666667; LuvIndex[108].U = 142.415204; LuvIndex[108].V = -23.261906;
            LuvIndex[109].L = 66.666667; LuvIndex[109].U = 142.415204; LuvIndex[109].V = 5.685223;
            LuvIndex[110].L = 66.666667; LuvIndex[110].U = 142.415204; LuvIndex[110].V = 34.632352;
            LuvIndex[111].L = 66.666667; LuvIndex[111].U = 142.415204; LuvIndex[111].V = 63.579482;
            LuvIndex[112].L = 66.666667; LuvIndex[112].U = 181.609837; LuvIndex[112].V = 63.579482;
            LuvIndex[113].L = 77.777778; LuvIndex[113].U = -92.752592; LuvIndex[113].V = -52.209036;
            LuvIndex[114].L = 77.777778; LuvIndex[114].U = -92.752592; LuvIndex[114].V = -23.261906;
            LuvIndex[115].L = 77.777778; LuvIndex[115].U = -92.752592; LuvIndex[115].V = 5.685223;
            LuvIndex[116].L = 77.777778; LuvIndex[116].U = -92.752592; LuvIndex[116].V = 34.632352;
            LuvIndex[117].L = 77.777778; LuvIndex[117].U = -92.752592; LuvIndex[117].V = 63.579482;
            LuvIndex[118].L = 77.777778; LuvIndex[118].U = -92.752592; LuvIndex[118].V = 92.526611;
            LuvIndex[119].L = 77.777778; LuvIndex[119].U = -53.557959; LuvIndex[119].V = -52.209036;
            LuvIndex[120].L = 77.777778; LuvIndex[120].U = -53.557959; LuvIndex[120].V = -23.261906;
            LuvIndex[121].L = 77.777778; LuvIndex[121].U = -53.557959; LuvIndex[121].V = 5.685223;
            LuvIndex[122].L = 77.777778; LuvIndex[122].U = -53.557959; LuvIndex[122].V = 34.632352;
            LuvIndex[123].L = 77.777778; LuvIndex[123].U = -53.557959; LuvIndex[123].V = 63.579482;
            LuvIndex[124].L = 77.777778; LuvIndex[124].U = -53.557959; LuvIndex[124].V = 92.526611;
            LuvIndex[125].L = 77.777778; LuvIndex[125].U = -14.363326; LuvIndex[125].V = -52.209036;
            LuvIndex[126].L = 77.777778; LuvIndex[126].U = -14.363326; LuvIndex[126].V = -23.261906;
            LuvIndex[127].L = 77.777778; LuvIndex[127].U = -14.363326; LuvIndex[127].V = 5.685223;
            LuvIndex[128].L = 77.777778; LuvIndex[128].U = -14.363326; LuvIndex[128].V = 34.632352;
            LuvIndex[129].L = 77.777778; LuvIndex[129].U = -14.363326; LuvIndex[129].V = 63.579482;
            LuvIndex[130].L = 77.777778; LuvIndex[130].U = -14.363326; LuvIndex[130].V = 92.526611;
            LuvIndex[131].L = 77.777778; LuvIndex[131].U = 24.831306; LuvIndex[131].V = -52.209036;
            LuvIndex[132].L = 77.777778; LuvIndex[132].U = 24.831306; LuvIndex[132].V = -23.261906;
            LuvIndex[133].L = 77.777778; LuvIndex[133].U = 24.831306; LuvIndex[133].V = 5.685223;
            LuvIndex[134].L = 77.777778; LuvIndex[134].U = 24.831306; LuvIndex[134].V = 34.632352;
            LuvIndex[135].L = 77.777778; LuvIndex[135].U = 24.831306; LuvIndex[135].V = 63.579482;
            LuvIndex[136].L = 77.777778; LuvIndex[136].U = 24.831306; LuvIndex[136].V = 92.526611;
            LuvIndex[137].L = 77.777778; LuvIndex[137].U = 64.025939; LuvIndex[137].V = -52.209036;
            LuvIndex[138].L = 77.777778; LuvIndex[138].U = 64.025939; LuvIndex[138].V = -23.261906;
            LuvIndex[139].L = 77.777778; LuvIndex[139].U = 64.025939; LuvIndex[139].V = 5.685223;
            LuvIndex[140].L = 77.777778; LuvIndex[140].U = 64.025939; LuvIndex[140].V = 34.632352;
            LuvIndex[141].L = 77.777778; LuvIndex[141].U = 64.025939; LuvIndex[141].V = 63.579482;
            LuvIndex[142].L = 77.777778; LuvIndex[142].U = 64.025939; LuvIndex[142].V = 92.526611;
            LuvIndex[143].L = 77.777778; LuvIndex[143].U = 103.220572; LuvIndex[143].V = 63.579482;
            LuvIndex[144].L = 88.888889; LuvIndex[144].U = -53.557959; LuvIndex[144].V = -23.261906;
            LuvIndex[145].L = 88.888889; LuvIndex[145].U = -53.557959; LuvIndex[145].V = 5.685223;
            LuvIndex[146].L = 88.888889; LuvIndex[146].U = -53.557959; LuvIndex[146].V = 34.632352;
            LuvIndex[147].L = 88.888889; LuvIndex[147].U = -53.557959; LuvIndex[147].V = 63.579482;
            LuvIndex[148].L = 88.888889; LuvIndex[148].U = -14.363326; LuvIndex[148].V = -23.261906;
            LuvIndex[149].L = 88.888889; LuvIndex[149].U = -14.363326; LuvIndex[149].V = 5.685223;
            LuvIndex[150].L = 88.888889; LuvIndex[150].U = -14.363326; LuvIndex[150].V = 34.632352;
            LuvIndex[151].L = 88.888889; LuvIndex[151].U = -14.363326; LuvIndex[151].V = 63.579482;
            LuvIndex[152].L = 88.888889; LuvIndex[152].U = -14.363326; LuvIndex[152].V = 92.526611;
            LuvIndex[153].L = 88.888889; LuvIndex[153].U = 24.831306; LuvIndex[153].V = -23.261906;
            LuvIndex[154].L = 88.888889; LuvIndex[154].U = 24.831306; LuvIndex[154].V = 5.685223;
            LuvIndex[155].L = 88.888889; LuvIndex[155].U = 24.831306; LuvIndex[155].V = 34.632352;
            LuvIndex[156].L = 88.888889; LuvIndex[156].U = 24.831306; LuvIndex[156].V = 63.579482;
            LuvIndex[157].L = 88.888889; LuvIndex[157].U = 24.831306; LuvIndex[157].V = 92.526611;
            LuvIndex[158].L = 100; LuvIndex[158].U = 0; LuvIndex[158].V = 0;
        }
        public int LuvIndexOf()
        {
            return LuvIndexOf(finalLUV);
        }

        public int LuvIndexOf(LUVClass luv)
        {
            int nL = 0;
            int nHi = 0;

            while (nHi < 159 && luv.L > LuvIndex[nHi].L)
            {
                nHi++;
            }

            if (luv.L != LuvIndex[nHi].L)
            {
                nHi--;
            }

            while (nL < 159 && LuvIndex[nL].L != LuvIndex[nHi].L)
            {
                nL++;
            }

            for (int i = nL; i < nHi; i++)
            {
                if (luv.U <= LuvIndex[i].U)
                {
                    if (luv.V <= LuvIndex[i].V)
                        return i;
                }
            }

            return nHi;
        }
    }

    public class LUVClass
    {
        public double L, U, V;
        public LUVClass()
        {
            L = 0.0;
            U = 0.0;
            V = 0.0;
        }
    }

    public class RGBClass
    {
        public double R, G, B;
        public RGBClass()
        {
            R = 0.0;
            G = 0.0;
            B = 0.0;
        }
        public RGBClass(double R, double G, double B)
        {
            this.R = R;
            this.G = G;
            this.B = B;
        }
    }
    */
}
