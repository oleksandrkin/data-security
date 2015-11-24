using System;
using System.Collections.Generic;
using System.Text;

namespace DES_lab8
{
    public enum DESMode
    {
        Encription,
        Decription
    }

    public class DES
    {
        private BinaryNumber key;
        private BinaryNumber cKey;
        private BinaryNumber tempCKey;
        private BinaryNumber dKey;
        private BinaryNumber tempDKey;
        private const int KeyLength = 64;
        private const int DesSteps = 16;
        private DESMode mode;
        private List<BinaryNumber> encrypted;
        private Random r = new Random();

        public DES()
        {
            key = new BinaryNumber(KeyLength, r);
            cKey = Block(LeftPC1, key);
            dKey = Block(RightPC1, key);
        }

        public string Encript(string message)
        {
            mode = DESMode.Encription;
            byte[] byteMessage = PreprocessMessage(message);
            List<BinaryNumber> ipInput = new List<BinaryNumber>();
            BinaryNumber temp = new BinaryNumber();
            for (int i = 0; i < byteMessage.Length; i++)
            {
                temp.Add(ToBinaryNumber(byteMessage[i], 8));
                if ((i + 1)%8 == 0)
                {
                    ipInput.Add(new BinaryNumber(temp));
                    temp.Clear();
                }
            }
            encrypted = new List<BinaryNumber>();
            for (int i = 0; i < ipInput.Count; i++)
            {
                encrypted.Add(Process(ipInput[i]));
            }

            StringBuilder sb = new StringBuilder();
            foreach (var binaryNumber in encrypted)
            {
                sb.Append(binaryNumber);
            }
            return sb.ToString();
        }

        public string Decript()
        {
            mode = DESMode.Decription;
            List<BinaryNumber> decrypted = new List<BinaryNumber>();
            for (int i = 0; i < encrypted.Count; i++)
            {
                decrypted.Add(Process(encrypted[i]));
            }
            List<byte> decryptedBytes = new List<byte>();
            foreach (var binaryNumber in decrypted)
            {
                string b = "";
                for (int i = 0; i < binaryNumber.Length; i++)
                {
                    b += binaryNumber[i];
                    if ((i+1)%8 == 0)
                    {
                        decryptedBytes.Add(Convert.ToByte(b, 2));
                        b = "";
                    }
                }
            }
            return Encoding.ASCII.GetString(decryptedBytes.ToArray());
       }

        private BinaryNumber Process(BinaryNumber input)
        {
            List<BinaryNumber> ipOut = Block(IP, input).Divide(2);
            BinaryNumber left = ipOut[0];
            BinaryNumber right = ipOut[1];
            for (int i = 0; i < DesSteps; i++)
            {
                BinaryNumber fOut = Function(new BinaryNumber(right), i);
                BinaryNumber rTemp = new BinaryNumber(right);
                right = new BinaryNumber(fOut^left);
                left = new BinaryNumber(rTemp);
            }
            left.Add(right);
            return Block(IP_1, left);  
        }

        private BinaryNumber Function(BinaryNumber input, int stepId)
        {
            BinaryNumber eOut = Block(E, input);
            List<BinaryNumber> sInputs = (eOut ^ GetKey(stepId)).Divide(8);
            BinaryNumber sOut = new BinaryNumber();
            for(int i = 0; i < sInputs.Count; i++)
            {
                sOut.Add(SBlock(sInputs[i], i));
            }
            return Block(P, sOut);
        }


        private BinaryNumber GetKey(int stepId)
        {
            if (stepId == 0)
            {
                tempCKey = cKey;
                tempDKey = dKey;
            }
            if (mode == DESMode.Encription)
            {
                tempCKey.ShiftLeft(Rotation[stepId]);
                tempDKey.ShiftLeft(Rotation[stepId]);
            }
            else
            {
                tempCKey.ShiftRight(Rotation[stepId]);
                tempDKey.ShiftRight(Rotation[stepId]);
            }
            return Block(PC2, tempCKey + tempDKey);
        }

        private BinaryNumber SBlock(BinaryNumber input, int id)
        {
            BinaryNumber row = new BinaryNumber();
            row.Add(input[0]);
            row.Add(input.Last);
            BinaryNumber col = new BinaryNumber();
            for(int i = 1; i < input.Length - 1; i++)
                col.Add(input[i]);
            return ToBinaryNumber(S[id][row.ToInt()][col.ToInt()], 4);
        }

        private BinaryNumber Block(int[][] block, BinaryNumber input)
        {
            BinaryNumber output = new BinaryNumber();
            for (int i = 0; i < block.Length; i++)
            {
                for (int j = 0; j < block[0].Length; j++)
                { 
                    output.Add(input[block[i][j] - 1]);
                }
            }
            return output;
        }

        private byte[] PreprocessMessage(string message)
        {
            byte[] byteMessage = Encoding.ASCII.GetBytes(message);
            while (byteMessage.Length%8 != 0)
            {
                byteMessage = Encoding.ASCII.GetBytes(message += " ");
            }
            return byteMessage;
        }

        private BinaryNumber ToBinaryNumber(int b, int n)
        {
            List<int> binary = new List<int>();
            string binString = Convert.ToString(b, 2);
            for (int i = 0; i < n; i++)
            {
                int id = n - binString.Length;
                if (i < id)
                    binary.Add(0);
                else
                    binary.Add(binString[i - id] - 48);
            }
            return new BinaryNumber(binary);
        }

        private int[][] IP =
        {
            new[] {58, 50, 42, 34, 26, 18, 10, 2},
            new[] {60, 52, 44, 36, 28, 20, 12, 4},
            new[] {62, 54, 46, 38, 30, 22, 14, 6},
            new[] {64, 56, 48, 40, 32, 24, 16, 8},
            new[] {57, 49, 41, 33, 25, 17, 9, 1},
            new[] {59, 51, 43, 35, 27, 19, 11, 3},
            new[] {61, 53, 45, 37, 29, 21, 13, 5},
            new[] {63, 55, 47, 39, 31, 23, 15, 7}
        };

        private int[][] IP_1 =
        {
            new[] {40, 8, 48, 16, 56, 24, 64, 32},
            new[] {39, 7, 47, 15, 55, 23, 63, 31},
            new[] {38, 6, 46, 14, 54, 22, 62, 30},
            new[] {37, 5, 45, 13, 53, 21, 61, 29},
            new[] {36, 4, 44, 12, 52, 20, 60, 28},
            new[] {35, 3, 43, 11, 51, 19, 59, 27},
            new[] {34, 2, 42, 10, 50, 18, 58, 26},
            new[] {33, 1, 41, 9, 49, 17, 57, 25}
        };

        private int[][] E =
        {
            new[] {32, 1, 2, 3, 4, 5},
            new[] {4, 5, 6, 7, 8, 9},
            new[] {8, 9, 10, 11, 12, 13},
            new[] {12, 13, 14, 15, 16, 17},
            new[] {16, 17, 18, 19, 20, 21},
            new[] {20, 21, 22, 23, 24, 25},
            new[] {24, 25, 26, 27, 28, 29},
            new[] {28, 29, 30, 31, 32, 1}
        };

        private int[][] P =
        {
            new[] {16, 7, 20, 21, 29, 12, 28, 17},
            new[] {1, 15, 23, 26, 5, 18, 31, 10},
            new[] {2, 8, 24, 14, 32, 27, 3, 9},
            new[] {19, 13, 30, 6, 22, 11, 4, 25}
        };

        private List<int[][]> S = new List<int[][]>()
        {
            // s1
            new []
            {
                new[] {14, 4,  13, 1, 2	, 15, 11, 8	, 3	, 10, 6, 12, 5,  9,  0, 7},
            	new[] { 0, 15, 7,  4, 14, 2,  13, 1	, 10, 6, 12, 11, 9,  5,  3, 8},
            	new[] { 4, 1,  14, 8, 13, 6,  2,  11, 15, 12, 9, 7,  3,  10, 5, 0},
            	new[] {15, 12, 8,  2, 4	, 9,  1,  7,  5,  11, 3, 14, 10, 0,  6, 13}
            },

            //s2
            new []
            {
                new[] {15	,1	,8	,14	,6	,11	,3	,4	,9	,7	,2	,13	,12	,0	,5	,10},
                new[] {3	,13	,4	,7	,15	,2	,8	,14	,12	,0	,1	,10	,6	,9	,11	,5},
                new[] {0	,14	,7	,11	,10	,4	,13	,1	,5	,8	,12	,6	,9	,3	,2	,15},
                new[] {13	,8	,10	,1	,3	,15	,4	,2	,11	,6	,7	,12	,0	,5	,14	,9}
            },

            //s3
            new[]
            {
                new[] {10	,0	,9	,14	,6	,3	,15	,5	,1	,13	,12	,7	,11	,4	,2	,8},
            	new[] {13	,7	,0	,9	,3	,4	,6	,10	,2	,8	,5	,14	,12	,11	,15	,1},
            	new[] {13	,6	,4	,9	,8	,15	,3	,0	,11	,1	,2	,12	,5	,10	,14	,7},
            	new[] {1	,10	,13	,0	,6	,9	,8	,7	,4	,15	,14	,3	,11	,5	,2	,12}
            },

            //s4
            new []
            {
                new[] {7	,13	,14	,3	,0	,6	,9	,10	,1	,2	,8	,5	,11	,12	,4	,15},
                new[] {13	,8	,11	,5	,6	,15	,0	,3	,4	,7	,2	,12	,1	,10	,14	,9},
                new[] {10	,6	,9	,0	,12	,11	,7	,13	,15	,1	,3	,14	,5	,2	,8	,4},
                new[] {3	,15	,0	,6	,10	,1	,13	,8	,9	,4	,5	,11	,12	,7	,2	,14}
            },

            //s5
            new []
            {
                new[] {2	,12	,4	,1	,7	,10	,11	,6	,8	,5	,3	,15	,13	,0	,14	,9},
                new[] {14	,11	,2	,12	,4	,7	,13	,1	,5	,0	,15	,10	,3	,9	,8	,6},
                new[] {4	,2	,1	,11	,10	,13	,7	,8	,15	,9	,12	,5	,6	,3	,0	,14},
                new[] {11	,8	,12	,7	,1	,14	,2	,13	,6	,15	,0	,9	,10	,4	,5	,3}
            },

            //s6
            new []
            {
                new[] {12	,1	,10	,15	,9	,2	,6	,8	,0	,13	,3	,4	,14	,7	,5	,11},
                new[] {10	,15	,4	,2	,7	,12	,9	,5	,6	,1	,13	,14	,0	,11	,3	,8},
                new[] {9	,14	,15	,5	,2	,8	,12	,3	,7	,0	,4	,10	,1	,13	,11	,6},
                new[] {4	,3	,2	,12	,9	,5	,15	,10	,11	,14	,1	,7	,6	,0	,8	,13}
            },

            //s7
            new []
            {
                new[] {4	,11	,2	,14	,15	,0	,8	,13	,3	,12	,9	,7	,5	,10	,6	,1},
                new[] {13	,0	,11	,7	,4	,9	,1	,10	,14	,3	,5	,12	,2	,15	,8	,6},
                new[] {1	,4	,11	,13	,12	,3	,7	,14	,10	,15	,6	,8	,0	,5	,9	,2},
                new[] {6	,11	,13	,8	,1	,4	,10	,7	,9	,5	,0	,15	,14	,2	,3	,12}
            },

            //s8
            new []
            {
                new[] {13	,2	,8	,4	,6	,15	,11	,1	,10	,9	,3	,14	,5	,0	,12	,7},
                new[] {1	,15	,13	,8	,10	,3	,7	,4	,12	,5	,6	,11	,0	,14	,9	,2},
                new[] {7	,11	,4	,1	,9	,12	,14	,2	,0	,6	,10	,13	,15	,3	,5	,8},
                new[] {2	,1	,14	,7	,4	,10	,8	,13	,15	,12	,9	,0	,3	,5	,6	,11}
            }
        };

        private int[][] LeftPC1 =
        {
            new [] {57	,49	,41	,33	,25	,17	,9 },
            new [] {1	,58	,50	,42	,34	,26	,18},
            new [] {10	,2	,59	,51	,43	,35	,27},
            new [] {19	,11	,3	,60	,52	,44	,36}
        };

        private int[][] RightPC1 =
        {
            new [] {63	,55	,47	,39	,31	,23	,15},
            new [] {7	,62	,54	,46	,38	,30	,22},
            new [] {14	,6	,61	,53	,45	,37	,29},
            new [] {21	,13	,5	,28	,20	,12	,4}
        };

        private int[][] PC2 =
        {
            new [] {14	,17	,11	,24	,1	,5	,3	,28},
            new [] {15	,6	,21	,10	,23	,19	,12	,4}, 
            new [] {26	,8	,16	,7	,27	,20	,13	,2},
            new [] {41	,52	,31	,37	,47	,55	,30	,40},
            new [] {51	,45	,33	,48	,44	,49	,39	,56},
            new [] {34	,53	,46	,42	,50	,36	,29	,32}
        };

        private int[] Rotation =
        {
            1,
            1,
            2,
            2,
            2,
            2,
            2,
            2,
            1,
            2,
            2,
            2,
            2,
            2,
            2,
            1
        };
    }
}
