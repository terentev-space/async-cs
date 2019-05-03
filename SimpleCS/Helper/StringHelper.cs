using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCS.Helper
{
    public static class StringHelper
    {
        public static string BytesToString(byte[] Data) => System.Text.Encoding.UTF8.GetString(Data);
        public static string BytesToString(byte[] Data, int Index, int Count) => System.Text.Encoding.UTF8.GetString(Data, Index, Count);
        public static byte[] StringToBytes(string Data) => System.Text.Encoding.UTF8.GetBytes(Data);
        public static byte[] StringToBytes(char[] Data, int Index, int Count) => System.Text.Encoding.UTF8.GetBytes(Data, Index, Count);
    }
}
