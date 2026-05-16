using System;
using System.Runtime.InteropServices;
using AOT;
using Kirurobo.Files;

namespace Kirurobo
{
	internal static class ObserverEventHub
	{
		public static event Action<string[]> OnDroppedFiles;
		public static event Action OnMonitorChanged;
		public static event Action<WindowStateEventType> OnWindowStyleChanged;
		
		/// <summary>
		/// モニタまたは解像度が変化したときのコールバック
		/// この中での処理は最低限にするため、フラグを立てるのみ
		/// </summary>
		/// <param name="monitorCount"></param>
		[MonoPInvokeCallback(typeof(LibUniWinC.IntCallback))]
		internal static void _monitorChangedCallback([MarshalAs(UnmanagedType.I4)] int monitorCount)
		{
			OnMonitorChanged?.Invoke();
		}

		/// <summary>
		/// ウィンドウスタイルや最大化、最小化等で呼ばれるコールバック
		/// この中での処理は最低限にするため、フラグを立てるのみ
		/// </summary>
		/// <param name="e"></param>
		[MonoPInvokeCallback(typeof(LibUniWinC.IntCallback))]
		internal static void _windowStyleChangedCallback([MarshalAs(UnmanagedType.I4)] int e)
		{
			OnWindowStyleChanged?.Invoke((WindowStateEventType)e);
		}

		/// <summary>
		/// ファイル、フォルダがドロップされた時に呼ばれるコールバック
		/// 文字列を配列に直すことと、フラグを立てるまで行う
		/// </summary>
		/// <param name="paths"></param>
		[MonoPInvokeCallback(typeof(LibUniWinC.StringCallback))]
		internal static void _dropFilesCallback([MarshalAs(UnmanagedType.LPWStr)] string paths)
		{
			// LF 区切りで届いた文字列を分割してパスの配列に直す
			//char[] delimiters = { '\n', '\0' };
			//string[] files = paths.Split(delimiters).Where(s => s != "").ToArray();
			string[] files = FilePanel.ParsePaths(paths);

			if (files.Length > 0)
			{
				OnDroppedFiles?.Invoke(files);
			}
		}
	}
}