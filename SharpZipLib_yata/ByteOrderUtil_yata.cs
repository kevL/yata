using System;
using System.Collections.Generic;
using System.IO;


namespace yata
{
	static class ByteOrderUtil
	{
		#region ZipFile
		/// <summary>
		/// Reads an <c>int</c> in little-endian byte order.
		/// </summary>
		/// <param name="stream"></param>
		/// <returns></returns>
		internal static int ReadLEInt(this Stream stream)
		{
			return SwappedS32(ReadBytes(stream, 4));
		}

		/// <summary>
		/// Fills a <c>byte</c> array with <paramref name="count"/> of bytes
		/// from the current position in <paramref name="stream"/>.
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="count"></param>
		/// <returns>the <c>byte</c> array</returns>
		static byte[] ReadBytes(this Stream stream, int count)
		{
			var bytes = new byte[count];
			stream.Read(bytes, 0, count);

			return bytes;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="bytes"></param>
		/// <returns></returns>
		static int SwappedS32(IList<byte> bytes)
		{
			return (int)bytes[0]
				| ((int)bytes[1] <<  8)
				| ((int)bytes[2] << 16)
				| ((int)bytes[3] << 24);
		}


		/// <summary>
		/// Reads a <c>uint</c> in little-endian byte order.
		/// </summary>
		/// <returns>the value read</returns>
		internal static uint ReadLEUint(this Stream stream)
		{
			return (uint)ReadLEUshort(stream) | ((uint)ReadLEUshort(stream) << 16);
		}

		/// <summary>
		/// Reads an <c>ushort</c> in little-endian byte order.
		/// </summary>
		/// <returns>the value read</returns>
		internal static ushort ReadLEUshort(this Stream stream)
		{
			int data1 = stream.ReadByte();
			int data2 = stream.ReadByte();

			return (ushort)(data1 | (data2 << 8));
		}
		#endregion ZipFile


		#region InflatorHuffmanTree
		/// <summary>
		/// Reverses the bits of a 16-bit value.
		/// </summary>
		/// <param name="val"><c>int</c> to reverse bits of</param>
		/// <returns><c>short</c> with bits reversed</returns>
		internal static short BitReverse(int val)
		{
			return (short)(bit4Reverse[ val        & 0xf] << 12
						 | bit4Reverse[(val >>  4) & 0xf] <<  8
						 | bit4Reverse[(val >>  8) & 0xf] <<  4
						 | bit4Reverse[ val >> 12]);
		}

		/// <summary>
		/// 
		/// </summary>
		static readonly byte[] bit4Reverse =
		{
			0, 8, 4, 12, 2, 10, 6, 14, 1, 9, 5, 13, 3, 11, 7, 15
		};
		#endregion InflatorHuffmanTree
	}
}
