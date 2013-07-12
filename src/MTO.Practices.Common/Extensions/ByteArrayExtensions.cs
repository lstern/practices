﻿namespace MTO.Practices.Common.Extensions
{
    using System;

    /// <summary>
    /// Extensões para o ByteArray
    /// </summary>
    public static class ByteHelper
    {
        /// <summary>
        /// The UTF-8 BOM
        /// </summary>
        private static readonly string Utf8Bom = "\u00EF\u00BB\u00BF";

        /// <summary>
        /// Tabela para codificação de caracteres
        /// </summary>
        private static readonly char[][] CharLookupTable;

        /// <summary>
        /// Tabela para decodificação do octeto menor
        /// </summary>
        private static readonly byte[] LookupTableLow = new byte[] 
            {
              0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 
              0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 
              0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 
              0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 
              0xff, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 
              0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 
              0xff, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 
              0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff
            };

        /// <summary>
        /// Tabela para decodificação do octeto maior
        /// </summary>
        private static readonly byte[] LookupTableHigh = new byte[] 
            {
              0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 
              0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 
              0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 
              0x00, 0x10, 0x20, 0x30, 0x40, 0x50, 0x60, 0x70, 0x80, 0x90, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 
              0xff, 0xa0, 0xb0, 0xc0, 0xd0, 0xe0, 0xf0, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 
              0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 
              0xff, 0xa0, 0xb0, 0xc0, 0xd0, 0xe0, 0xf0, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 
              0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff
           };

        /// <summary>
        /// Initializes static members of the <see cref="ByteHelper"/> class.
        /// </summary>
        static ByteHelper()
        {
            CharLookupTable = new char[256][];
            for (var i = 0; i < 256; i++)
            {
                CharLookupTable[i] = i.ToString("x2").ToUpper().ToCharArray();
            }
        }

        /// <summary>
        /// Converte uma string para um array de bytes
        /// </summary>
        /// <param name="s">
        /// string a ser convertida
        /// </param>
        /// <returns>
        /// Array de bytes
        /// </returns>
        public static byte[] FromHexString(string s)
        {
            var chars = s.ToCharArray();
            var u = chars.Length;
            var bytes = new byte[u / 2];
            int offset = 0;

            int b = 0;
            while (offset < u)
            {
                bytes[b++] = (byte)(LookupTableHigh[chars[offset++]] | LookupTableLow[chars[offset]]);
                ++offset;
            }

            return bytes;
        }

        /// <summary>
        /// Converte um bytearray para uma string de hexadecimais
        /// </summary>
        /// <param name="bytes">
        /// O array de bytes a ser convertido
        /// </param>
        /// <returns>
        /// string com a representação hexa do array.
        /// </returns>
        public static string ToHexString(this byte[] bytes)
        {
            var u = bytes.Length;
            var chars = new char[u * 2];
            int offset = 0;

            int b = 0;
            while (offset < u)
            {
                var ch = CharLookupTable[bytes[offset]];
                chars[b++] = ch[0];
                chars[b++] = ch[1];
                ++offset;
            }

            return new string(chars);
        }

        /// <summary>
        /// Removes the BOM (http://en.wikipedia.org/wiki/Byte_order_mark) from UTF-8 strings, if present
        /// </summary>
        /// <param name="text">The UTF-8 string</param>
        /// <returns>The UTF-8 string without the BOM</returns>
        public static byte[] RemoveByteOrderMark(this byte[] text)
        {
            if (Utf8Bom[0] == text[0] 
                && Utf8Bom[1] == text[1] 
                && Utf8Bom[2] == text[2])
            {
                return text.SubArray(Utf8Bom.Length, text.Length - Utf8Bom.Length);
            }

            return text;
        }
    }
}
