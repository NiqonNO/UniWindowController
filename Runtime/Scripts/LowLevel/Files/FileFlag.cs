using System;

namespace Kirurobo.Files
{
	/// <summary>
	/// ダイアログの設定フラグ
	/// </summary>
	[Flags]
	public enum Flag
	{
		None = 0,
		FileMustExist = 1,            // Windows only
		FolderMustExist = 2,          // Windows only
		AllowMultipleSelection = 4,
		CanCreateDirectories = 16,
		OverwritePrompt = 256,        // Always enabled on macOS
		CreatePrompt = 512,           // Always enabled on macOS
		ShowHiddenFiles = 4096,
		RetrieveLink = 8192,
	}
}