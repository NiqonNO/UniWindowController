using System;
using System.Runtime.InteropServices;
using System.Text;
using Kirurobo.Files;

namespace Kirurobo
{
	internal class LibUniWinC
	{
		[UnmanagedFunctionPointer(CallingConvention.Winapi)]
		public delegate void StringCallback([MarshalAs(UnmanagedType.LPWStr)] string returnString);

		[UnmanagedFunctionPointer((CallingConvention.Winapi))]
		public delegate void IntCallback([MarshalAs(UnmanagedType.I4)] int value);


		[DllImport("LibUniWinC", CallingConvention = CallingConvention.Winapi)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool IsActive();

		[DllImport("LibUniWinC", CallingConvention = CallingConvention.Winapi)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool IsTransparent();

		[DllImport("LibUniWinC", CallingConvention = CallingConvention.Winapi)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool IsBorderless();

		[DllImport("LibUniWinC", CallingConvention = CallingConvention.Winapi)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool IsTopmost();

		[DllImport("LibUniWinC", CallingConvention = CallingConvention.Winapi)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool IsBottommost();

		[DllImport("LibUniWinC", CallingConvention = CallingConvention.Winapi)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool IsMaximized();

		[DllImport("LibUniWinC", CallingConvention = CallingConvention.Winapi)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool IsFreePositioningEnabled();

		[DllImport("LibUniWinC", CallingConvention = CallingConvention.Winapi)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool AttachMyWindow();

		[DllImport("LibUniWinC", CallingConvention = CallingConvention.Winapi)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool AttachMyOwnerWindow();

		[DllImport("LibUniWinC", CallingConvention = CallingConvention.Winapi)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool AttachMyActiveWindow();

		[DllImport("LibUniWinC", CallingConvention = CallingConvention.Winapi)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool DetachWindow();

		[DllImport("LibUniWinC", CallingConvention = CallingConvention.Winapi)]
		public static extern void Update();

		[DllImport("LibUniWinC", CallingConvention = CallingConvention.Winapi)]
		public static extern void SetTransparent([MarshalAs(UnmanagedType.U1)] bool bEnabled);

		[DllImport("LibUniWinC", CallingConvention = CallingConvention.Winapi)]
		public static extern void SetBorderless([MarshalAs(UnmanagedType.U1)] bool bEnabled);

		[DllImport("LibUniWinC", CallingConvention = CallingConvention.Winapi)]
		public static extern void SetAlphaValue(float alpha);

		[DllImport("LibUniWinC", CallingConvention = CallingConvention.Winapi)]
		public static extern void SetClickThrough([MarshalAs(UnmanagedType.U1)] bool bEnabled);

		[DllImport("LibUniWinC", CallingConvention = CallingConvention.Winapi)]
		public static extern void SetTopmost([MarshalAs(UnmanagedType.U1)] bool bEnabled);

		[DllImport("LibUniWinC", CallingConvention = CallingConvention.Winapi)]
		public static extern void SetBottommost([MarshalAs(UnmanagedType.U1)] bool bEnabled);

		[DllImport("LibUniWinC", CallingConvention = CallingConvention.Winapi)]
		public static extern void SetMaximized([MarshalAs(UnmanagedType.U1)] bool bZoomed);

		[DllImport("LibUniWinC", CallingConvention = CallingConvention.Winapi)]
		public static extern void EnableFreePositioning([MarshalAs(UnmanagedType.U1)] bool bEnabled);

		[DllImport("LibUniWinC", CallingConvention = CallingConvention.Winapi)]
		public static extern void SetPosition(float x, float y);

		[DllImport("LibUniWinC", CallingConvention = CallingConvention.Winapi)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetPosition(out float x, out float y);

		[DllImport("LibUniWinC", CallingConvention = CallingConvention.Winapi)]
		public static extern void SetSize(float x, float y);

		[DllImport("LibUniWinC", CallingConvention = CallingConvention.Winapi)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetSize(out float x, out float y);

		[DllImport("LibUniWinC", CallingConvention = CallingConvention.Winapi)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetClientSize(out float width, out float height);

		[DllImport("LibUniWinC", CallingConvention = CallingConvention.Winapi)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetClientRectangle(out float x, out float y, out float width, out float height);

		[DllImport("LibUniWinC", CallingConvention = CallingConvention.Winapi)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool RegisterDropFilesCallback(
			[MarshalAs(UnmanagedType.FunctionPtr)] StringCallback callback);

		[DllImport("LibUniWinC", CallingConvention = CallingConvention.Winapi)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool UnregisterDropFilesCallback();

		[DllImport("LibUniWinC", CallingConvention = CallingConvention.Winapi)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool RegisterMonitorChangedCallback(
			[MarshalAs(UnmanagedType.FunctionPtr)] IntCallback callback);

		[DllImport("LibUniWinC", CallingConvention = CallingConvention.Winapi)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool UnregisterMonitorChangedCallback();

		[DllImport("LibUniWinC", CallingConvention = CallingConvention.Winapi)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool RegisterWindowStyleChangedCallback(
			[MarshalAs(UnmanagedType.FunctionPtr)] IntCallback callback);

		[DllImport("LibUniWinC", CallingConvention = CallingConvention.Winapi)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool UnregisterWindowStyleChangedCallback();

		[DllImport("LibUniWinC", CallingConvention = CallingConvention.Winapi)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool SetAllowDrop([MarshalAs(UnmanagedType.U1)] bool enabled);

		[DllImport("LibUniWinC", CallingConvention = CallingConvention.Winapi)]
		public static extern int GetCurrentMonitor();

		[DllImport("LibUniWinC", CallingConvention = CallingConvention.Winapi)]
		public static extern int GetMonitorCount();

		[DllImport("LibUniWinC", CallingConvention = CallingConvention.Winapi)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetMonitorRectangle(int index, out float x, out float y, out float width,
			out float height);
		
		[DllImport("LibUniWinC", CallingConvention = CallingConvention.Winapi)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetWorkSpaceRectangle(int index, out float x, out float y, out float width,
			out float height);

		[DllImport("LibUniWinC", CallingConvention = CallingConvention.Winapi)]
		public static extern void SetCursorPosition(float x, float y);

		[DllImport("LibUniWinC", CallingConvention = CallingConvention.Winapi)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool GetCursorPosition(out float x, out float y);

		[DllImport("LibUniWinC", CallingConvention = CallingConvention.Winapi)]
		public static extern int GetMouseButtons();

		[DllImport("LibUniWinC", CallingConvention = CallingConvention.Winapi)]
		public static extern int GetModifierKeys();


		#region Working on Windows only

		[DllImport("LibUniWinC", CallingConvention = CallingConvention.Winapi)]
		public static extern void SetTransparentType(int type);

		[DllImport("LibUniWinC", CallingConvention = CallingConvention.Winapi)]
		public static extern void SetKeyColor(uint colorref);

		[DllImport("LibUniWinC", CallingConvention = CallingConvention.Winapi)]
		public static extern int GetDebugInfo();

		[DllImport("LibUniWinC", CallingConvention = CallingConvention.Winapi)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool AttachWindowHandle(IntPtr hWnd);

		#endregion


		[DllImport("LibUniWinC", CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool OpenFilePanel(in PanelSettings settings,
			[MarshalAs(UnmanagedType.LPWStr), Out] StringBuilder buffer, UInt32 bufferSize);

		[DllImport("LibUniWinC", CharSet = CharSet.Unicode)]
		[return: MarshalAs(UnmanagedType.Bool)]
		public static extern bool OpenSavePanel(in PanelSettings settings,
			[MarshalAs(UnmanagedType.LPWStr), Out] StringBuilder buffer, UInt32 bufferSize);


		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct PanelSettings : IDisposable
		{
			public Int32 structSize;
			public Int32 flags;
			public IntPtr lpszTitle;
			public IntPtr lpszFilter;
			public IntPtr lpszInitialFile;
			public IntPtr lpszInitialDir;
			public IntPtr lpszDefaultExt;

			public PanelSettings(FilesSettings settings)
			{
				this.structSize = 0;
				//this.structSize = 4 * 2 + Marshal.SizeOf<IntPtr>() * 3;
				this.flags = (Int32)settings.flags;

				//this.lpTitleText = IntPtr.Zero;
				//this.lpFilterText = IntPtr.Zero;
				//this.lpDefaultPath = IntPtr.Zero;
				this.lpszTitle = Marshal.StringToHGlobalUni(settings.title);
				this.lpszFilter = Marshal.StringToHGlobalUni(FilesFilter.Join(settings.filters));
				this.lpszInitialFile = Marshal.StringToHGlobalUni(settings.initialFile);
				this.lpszInitialDir = Marshal.StringToHGlobalUni(settings.initialDirectory);
				//this.lpszDefaultExt = Marshal.StringToHGlobalUni(settings.defaultExtension);
				this.lpszDefaultExt = IntPtr.Zero;

				//this.structSize = Marshal.SizeOf(this);
				this.structSize = Marshal.SizeOf(this);
			}

			public void Dispose()
			{
				if (this.lpszTitle != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(lpszTitle);
					this.lpszTitle = IntPtr.Zero;
				}

				if (this.lpszFilter != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(lpszFilter);
					this.lpszFilter = IntPtr.Zero;
				}

				if (this.lpszInitialFile != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(lpszInitialFile);
					this.lpszInitialFile = IntPtr.Zero;
				}

				if (this.lpszInitialDir != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(lpszInitialDir);
					this.lpszInitialDir = IntPtr.Zero;
				}

				if (this.lpszDefaultExt != IntPtr.Zero)
				{
					Marshal.FreeHGlobal(lpszDefaultExt);
					this.lpszDefaultExt = IntPtr.Zero;
				}
			}
		}
	}
}