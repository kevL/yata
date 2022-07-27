using System;
using System.Collections.Generic;


namespace yata
{
	/// <summary>
	/// Huffman tree used for inflation.
	/// </summary>
	public class InflatorHuffmanTree
	{
		#region Fields (static)
		const int MAX_BITLEN = 15;
		#endregion Fields (static)


		#region Fields
		readonly short[] _tree;
		#endregion Fields


		#region cTor
		/// <summary>
		/// Constructs a Huffman tree from the array of codelengths.
		/// </summary>
		/// <param name="codeLengths">the array of codelengths</param>
		internal InflatorHuffmanTree(IList<byte> codeLengths)
		{
			var blCount  = new int[MAX_BITLEN + 1];
			var nextCode = new int[MAX_BITLEN + 1];

			int bits;

			for (int i = 0; i != codeLengths.Count; ++i)
			{
				if ((bits = codeLengths[i]) > 0)
					++blCount[bits];
			}

			int code = 0;
			int treeSize = 512;

			for (bits = 1; bits <= MAX_BITLEN; ++bits)
			{
				nextCode[bits] = code;
				code += blCount[bits] << (16 - bits);
				if (bits >= 10)
				{
					// need an extra table for bit lengths >= 10
					int start = nextCode[bits] & 0x1ff80;
					int end = code & 0x1ff80;
					treeSize += (end - start) >> (16 - bits);
				}
			}

			// now create and fill the extra tables from longest to shortest bit
			// length - this way the subtrees will be aligned
			_tree = new short[treeSize];

			int treePtr = 512;

			for (bits = MAX_BITLEN; bits >= 10; --bits)
			{
				int end = code & 0x1ff80;
				code -= blCount[bits] << (16 - bits);
				int start = code & 0x1ff80;
				for (int i = start; i < end; i += 1 << 7)
				{
					_tree[ByteOrderUtil.BitReverse(i)] = (short)((-treePtr << 4) | bits);
					treePtr += 1 << (bits - 9);
				}
			}

			for (int i = 0; i != codeLengths.Count; ++i)
			{
				if ((bits = codeLengths[i]) != 0)
				{
					code = nextCode[bits];
					int revcode = ByteOrderUtil.BitReverse(code);
					if (bits <= 9)
					{
						do
						{
							_tree[revcode] = (short)((i << 4) | bits);
							revcode += 1 << bits;
						}
						while (revcode < 512);
					}
					else
					{
						int subTree = _tree[revcode & 511];
						int treeLen = 1 << (subTree & 15);
						subTree = -(subTree >> 4);
						do
						{
							_tree[subTree | (revcode >> 9)] = (short)((i << 4) | bits);
							revcode += 1 << bits;
						}
						while (revcode < treeLen);
					}
					nextCode[bits] = code + (1 << (16 - bits));
				}
			}
		}
		#endregion cTor


		#region Methods
		/// <summary>
		/// Reads the next symbol from input. The symbol is encoded using the
		/// huffman tree.
		/// </summary>
		/// <param name="input">input the input source</param>
		/// <returns>the next symbol or -1 if not enough input is available</returns>
		internal int GetSymbol(StreamManipulator input)
		{
			int lookahead, symbol;

			if ((lookahead = input.PeekBits(9)) >= 0)
			{
				symbol = _tree[lookahead];
				int bitlen = symbol & 15;

				if (symbol >= 0)
				{
					input.DropBits(bitlen);
					return symbol >> 4;
				}

				int subtree = -(symbol >> 4);

				if ((lookahead = input.PeekBits(bitlen)) >= 0)
				{
					symbol = _tree[subtree | (lookahead >> 9)];
					input.DropBits(symbol & 15);
					return symbol >> 4;
				}
				else // kL_note: why is that needed w/ return above
				{
					int bits = input.AvailableBits;
					lookahead = input.PeekBits(bits);
					symbol = _tree[subtree | (lookahead >> 9)];
					if ((symbol & 15) <= bits)
					{
						input.DropBits(symbol & 15);
						return symbol >> 4;
					}
					return -1;
				}
			}
			else // less than 9 bits
			{
				int bits = input.AvailableBits;
				lookahead = input.PeekBits(bits);
				symbol = _tree[lookahead];
				if (symbol >= 0 && (symbol & 15) <= bits)
				{
					input.DropBits(symbol & 15);
					return symbol >> 4;
				}
				return -1;
			}
		}
		#endregion Methods
	}
}
