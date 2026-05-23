using System;

namespace Kirurobo
{
	/// <summary>
	/// Identifies the type of <see cref="OnStateChanged">OnStateChanged</see> event when it occurs
	/// </summary>
	[Flags]
	public enum WindowStateEventType : int
	{
		None = 0,
		StyleChanged = 1,
		Resized = 2,
		SizeMoveExit = 3,

		// 以降は仕様変更もありえる
		TopMostEnabled = 16 + 1 + 8,
		TopMostDisabled = 16 + 1,
		BottomMostEnabled = 32 + 1 + 8,
		BottomMostDisabled = 32 + 1,
		WallpaperModeEnabled = 64 + 1 + 8,
		WallpaperModeDisabled = 64 + 1,
	};
}