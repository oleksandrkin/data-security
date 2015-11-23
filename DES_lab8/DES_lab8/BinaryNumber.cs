using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DES_lab8
{
    public class BinaryNumber
    {
        private List<int> binary;
        private const int N = 8;

        public BinaryNumber(byte[] data)
        {
            binary = new List<int>();
            foreach (var b in data)
            {
                string binString = Convert.ToString(b, 2);
                for (int i = 0; i < N; i++)
                {
                    int id = N - binString.Length;
                    if (i < id)
                        binary.Add(0);
                    else
                        binary.Add(binString[i - id] - 48);
                }
            }
        }

        public BinaryNumber(List<int> binArray)
        {
            binary = binArray;
        }

        public List<int> Value
        {
            get { return binary; }
        }

        public static byte[] ToBytes(BinaryNumber value)
        {
            List<byte> byteList = new List<byte>();
            string b = "";
            for (int i = 0; i < value.Value.Count; i++)
            {
                b += value.Value[i];
                if ((i+1)%8 == 0)
                {
                    string t = b.TrimStart('0');
                    byteList.Add(Convert.ToByte(t, 2));
                    b = "";
                }
            }
            return byteList.ToArray();
        }

        public static BinaryNumber operator^(BinaryNumber lhs, BinaryNumber rhs)
        {
            List<int> newBinary = new List<int>();
            for (int i = 0; i < lhs.Value.Count; i++)
            {
                newBinary.Add(lhs.Value[i] ^ rhs.Value[i]);
            }
            return new BinaryNumber(newBinary);
        }
    }
}
