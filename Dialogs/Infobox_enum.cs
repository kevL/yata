using System;


namespace yata
{
	#region Enums (internal)
	/// <summary>
	/// Specifies the head-bgcolor of an <c><see cref="Infobox"/></c>.
	/// </summary>
	enum InfoboxType
	{
		/// <summary>
		/// head bg-color bluish
		/// </summary>
		Info,
		/// <summary>
		/// head bg-color yellowish
		/// </summary>
		Warn,
		/// <summary>
		/// head bg-color reddish
		/// </summary>
		Error,
		/// <summary>
		/// head bg-color greenish
		/// </summary>
		Success
	}

	/// <summary>
	/// Specifies the buttons that appear in an <c><see cref="Infobox"/></c>.
	/// </summary>
	/// <remarks>Buttons are shown in reverse order right-to-left in the dialog.</remarks>
	enum InfoboxButtons
	{
		/// <summary>
		/// returns
		/// <list type="bullet">
		/// <item><c>DialogResult.Cancel</c></item>
		/// </list>
		/// </summary>
		Okay,
		/// <summary>
		/// returns
		/// <list type="bullet">
		/// <item><c>DialogResult.Cancel</c></item>
		/// <item><c>DialogResult.OK</c></item>
		/// </list>
		/// </summary>
		CancelYes,
		/// <summary>
		/// returns
		/// <list type="bullet">
		/// <item><c>DialogResult.Cancel</c></item>
		/// <item><c>DialogResult.OK</c></item>
		/// <item><c>DialogResult.Retry</c></item>
		/// </list>
		/// </summary>
		AbortLoadNext,
		/// <summary>
		/// returns
		/// <list type="bullet">
		/// <item><c>DialogResult.Cancel</c></item>
		/// </list>
		/// </summary>
		Abort
	}
	#endregion Enums (internal)
}
