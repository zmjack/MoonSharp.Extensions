using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace MoonSharp.Extensions
{
    public static partial class SecuritySupport_RSAExtentions
    {
        private const byte INTEGER = 0x02;
        private const byte BIT_STRING = 0x03;
        private const byte OCTET_STRING = 0x04;
        private const byte SEQUENCE = 0x30;

        private static void WriteMemoryStream(MemoryStream @this, params byte[] bytes)
        {
            @this.Write(bytes, 0, bytes.Length);
        }

        private enum DataType
        {
            Integer = INTEGER,
            BitString = BIT_STRING,
            OctetString = OCTET_STRING,
            Sequence = SEQUENCE,
        }

        private static readonly byte[] RSA_OID =
            { 0x6, 0x9, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0xD, 0x1, 0x1, 0x1, 0x5, 0x0 };

        private static byte[] Wrap(DataType type, params byte[][] bytesArray)
        {
            using (var memory = new MemoryStream())
            {
                WriteMemoryStream(memory, new[] { (byte)type });
                WriteMemoryStream(memory, SignLength(bytesArray.Sum(a => a.Length)));
                foreach (var bytes in bytesArray)
                {
                    memory.Write(bytes, 0, bytes.Length);
                }
                return memory.ToArray();
            }
        }
        private static byte[] Integer(params byte[][] bytesArray)
        {
            return bytesArray[0][0] > 0x80 ?
                Wrap(DataType.Integer, new byte[][] { new byte[] { 0x00 } }.Concat(bytesArray).ToArray())
                : Wrap(DataType.Integer, bytesArray);
        }
        private static byte[] BitString(params byte[][] bytesArray) => Wrap(DataType.BitString, bytesArray);
        private static byte[] OctetString(params byte[][] bytesArray) => Wrap(DataType.OctetString, bytesArray);
        private static byte[] Sequence(params byte[][] bytesArray) => Wrap(DataType.Sequence, bytesArray);

        private static byte[] SignLength(int length)
        {
            switch (length)
            {
                case int len when len < 0x80:
                    return new byte[] { Convert.ToByte(length) };

                case int len when len <= 0xFF:
                    return new byte[] { 0x81, Convert.ToByte(length) };

                default:
                    return new byte[]
                    {
                        0x82,
                        Convert.ToByte(length / (byte.MaxValue + 1)),
                        Convert.ToByte(length % (byte.MaxValue + 1)),
                    };
            }
        }

        private static byte[] GetParameter(byte[] src, int srcOffset, int count)
        {
            var buffer = new byte[count];
            Buffer.BlockCopy(src, srcOffset, buffer, 0, count);
            return buffer;
        }

        private static RSAParameters GetParametersFromPem(string pemString, bool includePrivateParameters)
        {
            var @params = new RSAParameters();
            var pem = Convert.FromBase64String(pemString);

            if (!includePrivateParameters)
            {
                @params.Modulus = GetParameter(pem, 0x1D, 128);
                @params.Exponent = GetParameter(pem, 0x9F, 3);
            }
            else
            {
                @params.Modulus = GetParameter(pem, 0x25, 0x80);        //11
                @params.Exponent = GetParameter(pem, 0xA7, 3);          //141
                @params.D = GetParameter(pem, 0xAE, 0x80);              //147       
                @params.P = GetParameter(pem, 0x131, 0x40);             //278
                @params.Q = GetParameter(pem, 0x174, 0x40);             //345
                @params.DP = GetParameter(pem, 0x1B6, 0x40);            //412
                @params.DQ = GetParameter(pem, 0x1F8, 0x40);            //478
                @params.InverseQ = GetParameter(pem, 0x23B, 0x40);      //545
            }

            return @params;
        }

        private static T[][] GroupArray<T>(this T[] @this, int count)
        {
            if (@this.Length == 0) return new T[0][];

            var maxIndex = @this.Length - 1;
            var ret = new T[maxIndex / count + 1][];

            for (int page = 0; page < ret.Length; page++)
            {
                if (page != ret.Length - 1)
                    ret[page] = new T[count];
                else ret[page] = new T[maxIndex % count + 1];
            }

            for (int i = 0; i < @this.Length; i++)
            {
                ret[i / count][i % count] = @this[i];
            }

            return ret;
        }

    }

}
