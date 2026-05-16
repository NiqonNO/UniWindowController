using System;
using System.Text;

namespace Kirurobo.Files
{
	/// <summary>
	/// Provides static methods to open native file dialog
	/// </summary>
	public class FilePanel
	{
		/// <summary>
		/// ファイルやフォルダ―のパス受け渡しUTF-16バッファの文字数
		///     複数パスが改行区切りで入るため 260 では少ない。
		/// </summary>
		private const int pathBufferSize = 2560;

		/// <summary>
		/// Open file selection dialog
		/// </summary>
		/// <param name="settings"></param>
		/// <param name="action"></param>
		public static void OpenFilePanel(FilesSettings settings, Action<string[]> action)
		{
			LibUniWinC.PanelSettings ps = new LibUniWinC.PanelSettings(settings);
			StringBuilder sb = new StringBuilder(pathBufferSize);

			if (LibUniWinC.OpenFilePanel(in ps, sb, (uint)sb.Capacity))
			{
				string[] files = ParsePaths(sb.ToString());
				action.Invoke(files);
			}

			ps.Dispose(); // Settings を渡したコンストラクタでメモリが確保されるため、解放が必要
		}

		/// <summary>
		/// Open save-file selection dialog
		/// </summary>
		/// <param name="settings"></param>
		/// <param name="action"></param>
		public static void SaveFilePanel(FilesSettings settings, Action<string[]> action)
		{
			LibUniWinC.PanelSettings ps = new LibUniWinC.PanelSettings(settings);
			StringBuilder sb = new StringBuilder(pathBufferSize);

			if (LibUniWinC.OpenSavePanel(in ps, sb, (uint)sb.Capacity))
			{
				string[] files = ParsePaths(sb.ToString());
				action.Invoke(files);
			}

			ps.Dispose(); // Settings を渡したコンストラクタでメモリが確保されるため、解放が必要
		}

		/// <summary>
		/// ダブルクオーテーション囲み、LF（またはnull）区切りの文字列を配列に直して返す
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		public static string[] ParsePaths(string text)
		{
			System.Collections.Generic.List<string> list = new System.Collections.Generic.List<string>();
			bool inEscaped = false;
			int len = text.Length;
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < len; i++)
			{
				char c = text[i];
				if (c == '"')
				{
					if (inEscaped)
					{
						if (((i + 1) < len) && text[i + 1] == '"')
						{
							i++;
							sb.Append(c); // 連続ダブルクォーテーションは１つのダブルクオーテーションとする
							continue;
						}
					}

					inEscaped = !inEscaped; // 連続でなければ囲み内か否かの切り替え
				}
				else if (c == '\n')
				{
					if (inEscaped)
					{
						// 囲み内ならパスの一部とする
						sb.Append(c);
					}
					else
					{
						// 囲み内でなければ、区切りとして、次のパスに移る
						if (sb.Length > 0)
						{
							list.Add(sb.ToString());
							//sb.Clear();   // for .NET 4 or later
							sb.Length = 0; // for .NET 2
						}
					}
				}
				else if (c == '\0')
				{
					// ヌル文字は、常に区切りとして、次のパスに移る
					if (sb.Length > 0)
					{
						list.Add(sb.ToString());
						//sb.Clear();   // for .NET 4 or later
						sb.Length = 0; // for .NET 2
					}
				}
				else
				{
					sb.Append(c);
				}
			}

			if (sb.Length > 0)
			{
				list.Add(sb.ToString());
			}

			// 空文字列の要素は除去
			list.RemoveAll(v => v.Length == 0);
			return list.ToArray();
		}
	}
}
