using System;


namespace yata
{
	/// <summary>
	/// The NwN2 toolset DLLs obfuscate strings (and private method
	/// identifiers). This class can revert those strings to intelligible text.
	/// </summary>
	static class StringDecryptor
	{
		/// <summary>
		/// Deobfuscates obfuscated strings in the NwN2 toolset DLLs.
		/// </summary>
		/// <param name="st">their crap</param>
		/// <returns>readable text</returns>
		internal static string Decrypt(string st)
		{
			char[] array0;
			char[] array1 = (array0 = st.ToCharArray());

			int p1 = array1.Length;
			while (p1 != 0)
			{
				int p0 = p1 - 1;
				array1[p0] = (char)(array0[p0] - 5225);
				array1 = array0; // wtf.
				p1 = p0;
			}

			return String.Intern(new string(array1));
		}
	}
}
