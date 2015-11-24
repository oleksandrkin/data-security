using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace DES_lab8
{
    public class BinaryNumber
    {
        private List<int> binary;

        public BinaryNumber()
        {
            binary = new List<int>();
        }

        public BinaryNumber(BinaryNumber bn)
        {
            binary = new List<int>();
            binary.AddRange(bn.Value);
        }

        public BinaryNumber(int length, Random r)
        {
            binary = new List<int>();
            for (int i = 0; i < length; i++)
            {
                binary.Add(r.Next()%2);
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

        public void Add(int i)
        {
            binary.Add(i);
        }

        public void Add(BinaryNumber bn)
        {
            binary.AddRange(bn.Value);
        }

        public int Length
        {
            get { return binary.Count; }
        }

        public void Clear()
        {
            binary.Clear();
        }

        public List<BinaryNumber> Divide(int n)
        {
            List<BinaryNumber> parts = new List<BinaryNumber>();
            BinaryNumber part = new BinaryNumber();
            for (int i = 0; i < Length; i++)
            {
                part.Add(binary[i]);
                if ((i + 1)%(Length/n) == 0)
                {
                    parts.Add(new BinaryNumber(part));
                    part.Clear();
                }
            }
            return parts;
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var i in binary)
            {
                sb.Append(i);
            }
            return sb.ToString();
        }

        public int this[int i]
        {
            get { return binary[i]; }
        }

        public int Last
        {
            get { return binary.Last(); }
        }

        public int ToInt()
        {
            return Convert.ToInt32(ToString(), 2);
        }

        public void ShiftLeft(int n)
        {
            for (int i = 0; i < n; i++)
            {
                binary.Add(binary.First());
                binary.RemoveAt(0);
            }
        }

        public void ShiftRight(int n)
        {
            List<int> newBinary = new List<int>();
            for (int i = 0; i < n; i++)
            {
                newBinary.Add(binary.Last());
                binary.RemoveAt(Length - 1);
            }
            newBinary.AddRange(binary);
            binary = newBinary;
        }

        public static BinaryNumber operator ^(BinaryNumber lhs, BinaryNumber rhs)
        {
            List<int> newBinary = new List<int>();
            for (int i = 0; i < lhs.Length; i++)
            {
                newBinary.Add(lhs[i] ^ rhs[i]);
            }
            return new BinaryNumber(newBinary);
        }

        public static BinaryNumber operator +(BinaryNumber lhs, BinaryNumber rhs)
        {
            BinaryNumber res = new BinaryNumber();
            res.Add(lhs);
            res.Add(rhs);
            return res;
        }
    }
}
