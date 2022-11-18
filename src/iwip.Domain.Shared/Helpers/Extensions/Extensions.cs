using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using static System.Net.WebRequestMethods;

namespace iwip.Helpers.Extensions
{
    public static class Extensions
    {
        public static byte[] ToBase64String(this FileStream stream)
        {
            using (stream)
            {
                using (MemoryStream memStream = new MemoryStream())
                {
                    stream.CopyTo(memStream);
                    return memStream.ToArray();
                }
            }
        }

        public static void ToString(this string fileName, string serializedFile)
        {
            using (System.IO.FileStream reader = System.IO.File.Create(fileName))
            {
                byte[] buffer = Convert.FromBase64String(serializedFile);
                reader.Write(buffer, 0, buffer.Length);
            }
        }

        public static Guid Int2Guid(int value)
        {
            byte[] bytes = new byte[16];
            BitConverter.GetBytes(value).CopyTo(bytes, 0);
            return new Guid(bytes);
        }

        public static int Guid2Int(Guid value)
        {
            byte[] b = value.ToByteArray();
            int bint = BitConverter.ToInt32(b, 0);
            return bint;
        }

    }
}
