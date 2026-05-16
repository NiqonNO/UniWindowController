namespace Kirurobo.Files
{
	/// <summary>
	/// Parameters for file dialog
	/// </summary>
	public struct FilesSettings
	{
		public string title;
		public FilesFilter[] filters;
		public string initialDirectory;
		public string initialFile;
		public string defaultExtension;    // Not implemented
		public Flag flags;
	}
}