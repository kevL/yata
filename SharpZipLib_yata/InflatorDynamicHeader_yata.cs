using System;
using System.Collections.Generic;


namespace yata
{
	sealed class InflatorDynamicHeader
	{
		#region Fields (static)
		/// <summary>
		/// Maximum number of literal/length codes.
		/// </summary>
		const int LITLEN_MAX = 286;

		/// <summary>
		/// Maximum number of distance codes.
		/// </summary>
		const int DIST_MAX = 30;

		/// <summary>
		/// Maximum meta code length codes to read.
		/// </summary>
		const int META_MAX = 19;

		/// <summary>
		/// Maximum data code lengths to read.
		/// </summary>
		const int CODELEN_MAX = LITLEN_MAX + DIST_MAX;

		static readonly int[] MetaCodeLengthIndex =
		{
			16, 17, 18, 0, 8, 7, 9, 6, 10, 5, 11, 4, 12, 3, 13, 2, 14, 1, 15
		};
		#endregion Fields (static)


		#region Fields
		readonly StreamManipulator _input;
		readonly IEnumerator<bool> _state;
		readonly IEnumerable<bool> _stateMachine;

		byte[] _codeLengths = new byte[CODELEN_MAX];

		InflatorHuffmanTree _litLenTree;
		InflatorHuffmanTree _distTree;

		int _litlenCodeCount, _distCodeCount, _metaCodeCount;
		#endregion Fields


		#region cTor
		/// <summary>
		/// 
		/// </summary>
		/// <param name="input"></param>
		internal InflatorDynamicHeader(StreamManipulator input)
		{
			_input = input;
			_stateMachine = CreateStateMachine();
			_state = _stateMachine.GetEnumerator();
		}
		#endregion cTor


		#region Methods
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		IEnumerable<bool> CreateStateMachine()
		{
			// read initial code length counts from header
			while (!_input.TryGetBits(5, ref _litlenCodeCount, 257)) yield return false;
			while (!_input.TryGetBits(5, ref _distCodeCount,     1)) yield return false;
			while (!_input.TryGetBits(4, ref _metaCodeCount,     4)) yield return false;

			int dataCodeCount = _litlenCodeCount + _distCodeCount;

			// load code lengths for the meta tree from the header bits
			for (int i = 0; i < _metaCodeCount; ++i)
			{
				while (!_input.TryGetBits(3, ref _codeLengths, MetaCodeLengthIndex[i])) yield return false;
			}

			var metaCodeTree = new InflatorHuffmanTree(_codeLengths);

			// decompress the meta tree symbols into the data table code lengths
			int index = 0;
			while (index < dataCodeCount)
			{
				int symbol;

				while ((symbol = metaCodeTree.GetSymbol(_input)) < 0) yield return false;

				if (symbol < 16)
				{
					_codeLengths[index++] = (byte)symbol; // append literal code length
				}
				else
				{
					byte codeLength;

					int repeatCount = 0;

					if (symbol == 16) // Repeat last code length 3..6 times
					{
						codeLength = _codeLengths[index - 1];

						// 2 bits + 3, [3..6]
						while (!_input.TryGetBits(2, ref repeatCount, 3)) yield return false;
					}
					else if (symbol == 17) // Repeat zero 3..10 times
					{
						codeLength = 0;

						// 3 bits + 3, [3..10]
						while (!_input.TryGetBits(3, ref repeatCount, 3)) yield return false;
					}
					else // (symbol == 18), Repeat zero 11..138 times
					{
						codeLength = 0;

						// 7 bits + 11, [11..138]
						while (!_input.TryGetBits(7, ref repeatCount, 11)) yield return false;
					}

					while (repeatCount-- > 0)
						_codeLengths[index++] = codeLength;
				}
			}

			_litLenTree = new InflatorHuffmanTree(ConvertArrayToList(_codeLengths, 0, _litlenCodeCount));
			_distTree   = new InflatorHuffmanTree(ConvertArrayToList(_codeLengths, _litlenCodeCount, _distCodeCount));

			yield return true;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="array"></param>
		/// <param name="start"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		static IList<byte> ConvertArrayToList(IList<byte> array, int start, int length)
		{
			var list = new List<byte>();
			for (int i = start; i != start + length; ++i)
				list.Add(array[i]);

			return list;
		}

		/// <summary>
		/// Continue decoding header from <c><see cref="_input"/></c> until more
		/// bits are needed or decoding has been completed.
		/// </summary>
		/// <returns>whether decoding could be completed</returns>
		internal bool AttemptRead()
		{
			return !_state.MoveNext() || _state.Current;
		}

		/// <summary>
		/// Gets literal length huffman tree.
		/// </summary>
		/// <remarks>Must not be used before
		/// <c><see cref="AttemptRead()">AttemptRead()</see></c> has returned
		/// <c>true</c>.</remarks>
		internal InflatorHuffmanTree LiteralLengthTree
		{
			get { return _litLenTree; }
		}

		/// <summary>
		/// Gets distance huffman tree.
		/// </summary>
		/// <remarks>Must not be used before
		/// <c><see cref="AttemptRead()">AttemptRead()</see></c> has returned
		/// <c>true</c>.</remarks>
		internal InflatorHuffmanTree DistanceTree
		{
			get { return _distTree; }
		}
		#endregion Methods
	}
}
