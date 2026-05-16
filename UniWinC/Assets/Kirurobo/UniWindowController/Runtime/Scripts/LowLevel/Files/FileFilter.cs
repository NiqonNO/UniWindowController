using System;

namespace Kirurobo.Files
{
	/// <summary>
	/// File filter
	/// </summary>
	public class FilesFilter
	{
		protected string title;
		protected string[] extensions;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="title">Filter title. (Not available on macOS yet)</param>
		/// <param name="extensions">Extensions like ["png", "jpg", "txt"]</param>
		public FilesFilter(string title, params string[] extensions)
		{
			this.title = title;
			this.extensions = extensions;
		}

		public override string ToString()
		{
			return title + "\t" + String.Join("\t", extensions);
		}

		/// <summary>
		/// Returns converted string from Filter array
		/// </summary>
		/// <param name="filters"></param>
		/// <returns></returns>
		public static string Join(FilesFilter[] filters)
		{
			if (filters == null) return "";

			string result = "";
			bool isFirstItem = true;
			foreach (var filter in filters) {
				if (!isFirstItem) result += "\n";
				result += filter.ToString();
				isFirstItem = false;
			}
			return result;
		}
	}
}