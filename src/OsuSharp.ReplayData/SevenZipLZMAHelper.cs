﻿using System;
using System.IO;
using SevenZip;
using SevenZip.Compression.LZMA;

namespace OsuSharp.Domain
{
    //http://www.nullskull.com/a/768/7zip-lzma-inmemory-compression-with-c.aspx
    internal static class SevenZipLZMAHelper
    {
        private const int _dictionary = 1 << 23;
        private const bool _eos = false;

        private static readonly CoderPropID[] _propIds =
        {
            CoderPropID.DictionarySize,
            CoderPropID.PosStateBits,
            CoderPropID.LitContextBits,
            CoderPropID.LitPosBits,
            CoderPropID.Algorithm,
            CoderPropID.NumFastBytes,
            CoderPropID.MatchFinder,
            CoderPropID.EndMarker
        };

        private static readonly object[] _properties =
        {
            _dictionary,
            2,
            3,
            0,
            2,
            128,
            "bt4",
            _eos
        };

        public static byte[] Compress(byte[] inputBytes)
        {
            var inStream = new MemoryStream(inputBytes);
            var outStream = new MemoryStream();

            var encoder = new Encoder();
            encoder.SetCoderProperties(_propIds, _properties);
            encoder.WriteCoderProperties(outStream);

            var fileSize = inStream.Length;
            for (var i = 0; i < 8; i++)
            {
                outStream.WriteByte((byte)(fileSize >> (8 * i)));
            }

            encoder.Code(inStream, outStream, -1, -1, null);

            return outStream.ToArray();
        }

        public static byte[] Decompress(byte[] inputBytes)
        {
            var newInStream = new MemoryStream(inputBytes);
            newInStream.Seek(0, 0);

            var decoder = new Decoder();
            var newOutStream = new MemoryStream();

            var properties2 = new byte[5];
            if (newInStream.Read(properties2, 0, 5) != 5)
            {
                throw (new Exception("input .lzma is too short"));
            }

            long outSize = 0;
            for (var i = 0; i < 8; i++)
            {
                var v = newInStream.ReadByte();
                if (v < 0)
                {
                    throw (new Exception("Can't Read 1"));
                }

                outSize |= ((long)(byte)v) << (8 * i);
            }

            decoder.SetDecoderProperties(properties2);

            var compressedSize = newInStream.Length - newInStream.Position;
            decoder.Code(newInStream, newOutStream, compressedSize, outSize, null);

            return newOutStream.ToArray();
        }
    }
}
