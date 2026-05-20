/*
 * UniWinCore.cs
 * 
 * Author: Kirurobo http://twitter.com/kirurobo
 * License: MIT
 */

using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Kirurobo
{
    /// <summary>
    /// Native plugin wrapper for LibUniWinC
    /// </summary>
    public class UniWinCore : IDisposable
    {
        public event Action<string[]> OnDroppedFiles;
        public event Action OnMonitorChanged;
        public event Action<WindowStateEventType> OnWindowStyleChanged;
        
#if UNITY_EDITOR
        /// <summary>
        /// Get the Unity editor window
        /// </summary>
        /// <returns></returns>
        /// <seealso href="http://baba-s.hatenablog.com/entry/2017/09/17/135018"/>
        public static EditorWindow GetGameView()
        {
            var assembly = typeof(EditorWindow).Assembly;
            var type = assembly.GetType("UnityEditor.GameView");
            var gameView = EditorWindow.GetWindow(type);
            return gameView;
        }
#endif

        /// <summary>
        /// Determines whether a window is attached and available
        /// </summary>
        /// <value><c>true</c> if this instance is active; otherwise, <c>false</c>.</value>
        public bool IsActive { get; private set; } = false;

        /// <summary>
        /// Determines whether the attached window is always on the front
        /// </summary>
        public bool IsTopmost { get { return (IsActive && _isTopmost); } }
        private bool _isTopmost = false;

        /// <summary>
        /// Determines whether the attached window is always on the bottom
        /// </summary>
        public bool IsBottommost { get { return (IsActive && _isBottommost); } }
        private bool _isBottommost = false;

        /// <summary>
        /// Determines whether the attached window is transparent
        /// </summary>
        public bool IsTransparent { get { return (IsActive && _isTransparent); } }
        private bool _isTransparent = false;

        /// <summary>
        /// Determines whether the attached window is click-through (i.e., does not receive any mouse action)
        /// </summary>
        public bool IsClickThrough { get { return (IsActive && _isClickThrough); } }
        private bool _isClickThrough = false;

        /// <summary>
        /// Determines whether the attached window is borderless (no title bar and borders)
        /// </summary>
        public bool IsBorderless { get { return (IsActive && _isBorderless); } }
        private bool _isBorderless = false;

        /// <summary>
        /// Determines whether the attached window can be freely positioned (macOS only)
        /// </summary>
        public bool IsFreePositioningEnabled { get { return (IsActive && _isFreePositioningEnabled); } }
        private bool _isFreePositioningEnabled = false;

        /// <summary>
        /// Type of transparent method for Windows
        /// </summary>
        private TransparentType transparentType = TransparentType.Alpha;

        /// <summary>
        /// The color to use for transparency when the transparentType is ColorKey
        /// </summary>
        private Color32 keyColor = new Color32(1, 0, 1, 0);


        #region Constructor or destructor
        /// <summary>
        /// ウィンドウ制御のコンストラクタ
        /// </summary>
        public UniWinCore()
        {
            IsActive = false;
            ObserverEventHub.OnMonitorChanged += InvokeMonitorChanged;
            ObserverEventHub.OnDroppedFiles += InvokeDroppedFiles;
            ObserverEventHub.OnWindowStyleChanged += InvokeWindowStyleChanged;
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~UniWinCore()
        {
            ObserverEventHub.OnMonitorChanged -= InvokeMonitorChanged;
            ObserverEventHub.OnDroppedFiles -= InvokeDroppedFiles;
            ObserverEventHub.OnWindowStyleChanged -= InvokeWindowStyleChanged;
            Dispose();
        }

        /// <summary>
        /// 終了時の処理
        /// </summary>
        public void Dispose()
        {
            // 最後にウィンドウ状態を戻すとそれが目についてしまうので、あえて戻さないことにしてみるためコメントアウト
            //DetachWindow();

            // Instead of DetachWindow()
            LibUniWinC.UnregisterDropFilesCallback();
            LibUniWinC.UnregisterMonitorChangedCallback();
            LibUniWinC.UnregisterWindowStyleChangedCallback();
        }
        #endregion

        #region Callbacks
        
        private void InvokeMonitorChanged()
        {
            OnMonitorChanged?.Invoke();
        }
        private void InvokeDroppedFiles(string[] paths)
        {
            OnDroppedFiles?.Invoke(paths);
        }
        private void InvokeWindowStyleChanged(WindowStateEventType type)
        {
            OnWindowStyleChanged?.Invoke(type);
        }
        
        #endregion
        
        #region Find, attach or detach 

        /// <summary>
        /// ウィンドウ状態を最初に戻して操作対象から解除
        /// </summary>
        public void DetachWindow()
        {
#if UNITY_EDITOR
            // エディタの場合、ウィンドウスタイルでは常に最前面と得られていない可能性があるため、
            //  最前面ではないのが本来と決め打ちで、デタッチ時無効化する
            EnableTopmost(false);
#endif
            LibUniWinC.DetachWindow();
        }

        /// <summary>
        /// 自分のウィンドウ（ゲームビューが独立ウィンドウならそれ）を探して操作対象とする
        /// </summary>
        /// <returns></returns>
        public bool AttachMyWindow()
        {
#if UNITY_EDITOR_WIN
            // 確実にゲームビューを得る方法がなさそうなので、フォーカスを与えて直後にアクティブなウィンドウを取得
            var gameView = GetGameView();
            if (gameView)
            {
                gameView.Focus();
                LibUniWinC.AttachMyActiveWindow();
            }
#else
            LibUniWinC.AttachMyWindow();
#endif
            // Add event handlers
            LibUniWinC.RegisterDropFilesCallback(ObserverEventHub._dropFilesCallback);
            LibUniWinC.RegisterMonitorChangedCallback(ObserverEventHub._monitorChangedCallback);
            LibUniWinC.RegisterWindowStyleChangedCallback(ObserverEventHub._windowStyleChangedCallback);

            IsActive = LibUniWinC.IsActive();
            return IsActive;
        }

        public bool AttachWindowHandle(IntPtr hWnd)
        {
            LibUniWinC.AttachWindowHandle(hWnd);
            IsActive = LibUniWinC.IsActive();
            return IsActive;
        }

        /// <summary>
        /// 自分のプロセスで現在アクティブなウィンドウを選択
        /// エディタの場合、ウィンドウが閉じたりドッキングしたりするため、フォーカス時に呼ぶ
        /// </summary>
        /// <returns></returns>
        public bool AttachMyActiveWindow()
        {
            LibUniWinC.AttachMyActiveWindow();
            IsActive = LibUniWinC.IsActive();
            return IsActive;
        }

        #endregion

        #region About window status
        /// <summary>
        /// Call this periodically to maintain window style
        /// TODO: This is a workaround for cocoa overriding window properties on mac. Currently no way of testing this, 
        /// </summary>
        public void Update()
        {
            LibUniWinC.Update();
        }

        string GetDebubgWindowSizeInfo()
        {
            float x, y, cx, cy;
            LibUniWinC.GetSize(out x, out y);
            LibUniWinC.GetClientSize(out cx, out cy);
            return $"W:{x},H:{y} CW:{cx},CH:{cy}";
        }

        /// <summary>
        /// 透過を設定／解除
        /// </summary>
        /// <param name="isTransparent"></param>
        public void EnableTransparent(bool isTransparent)
        {
            // エディタは透過できなかったり、枠が通常と異なるのでスキップ
#if !UNITY_EDITOR
            LibUniWinC.SetTransparent(isTransparent);
            LibUniWinC.SetBorderless(isTransparent);
#endif
            this._isTransparent = isTransparent;
        }

        /// <summary>
        /// Set the window alpha
        /// </summary>
        /// <param name="alpha">0.0 - 1.0</param>
        public void SetAlphaValue(float alpha)
        {
            // Windowsのエディタでは、一度半透明にしてしまうと表示が更新されなくなるため無効化。MacならOK
#if !UNITY_EDITOR_WIN
            LibUniWinC.SetAlphaValue(alpha);
#endif
        }

        /// <summary>
        /// Set the window z-order (Topmost or not).
        /// </summary>
        /// <param name="isTopmost">If set to <c>true</c> is top.</param>
        public void EnableTopmost(bool isTopmost)
        {
            LibUniWinC.SetTopmost(isTopmost);
            this._isTopmost = isTopmost;
            this._isBottommost = false;    // Exclusive
        }

        /// <summary>
        /// Set the window z-order (Bottommost or not).
        /// </summary>
        /// <param name="isBottommost">If set to <c>true</c> is bottom.</param>
        public void EnableBottommost(bool isBottommost)
        {
            LibUniWinC.SetBottommost(isBottommost);
            this._isBottommost = isBottommost;
            this._isTopmost = false;    // Exclusive
        }

        /// <summary>
        /// クリックスルーを設定／解除
        /// </summary>
        /// <param name="isThrough"></param>
        public void EnableClickThrough(bool isThrough)
        {
            // エディタでクリックスルーされると操作できなくなる可能性があるため、スキップ
#if !UNITY_EDITOR
            LibUniWinC.SetClickThrough(isThrough);
#endif
            this._isClickThrough = isThrough;
        }

        /// <summary>
        /// ウィンドウを最大化（Macではズーム）する
        /// 最大化された後にサイズ変更がされることもあり、現状、確実には動作しない可能性があります
        /// </summary>
        public void SetZoomed(bool isZoomed)
        {
            LibUniWinC.SetMaximized(isZoomed);
        }

        /// <summary>
        /// ウィンドウが最大化（Macではズーム）されているかを取得
        /// 最大化された後にサイズ変更がされることもあり、現状、確実には動作しない可能性があります
        /// </summary>
        public bool GetZoomed()
        {
            return LibUniWinC.IsMaximized();
        }

        /// <summary>
        /// Set the window position.
        /// </summary>
        /// <param name="position">Position.</param>
        public void SetWindowPosition(Vector2 position)
        {
            LibUniWinC.SetPosition(position.x, position.y);
        }

        /// <summary>
        /// Get the window position.
        /// </summary>
        /// <returns>The position.</returns>
        public Vector2 GetWindowPosition()
        {
            Vector2 pos = Vector2.zero;
            LibUniWinC.GetPosition(out pos.x, out pos.y);
            return pos;
        }

        /// <summary>
        /// Set the window size.
        /// </summary>
        /// <param name="size">x is width and y is height</param>
        public void SetWindowSize(Vector2 size)
        {
            LibUniWinC.SetSize(size.x, size.y);
        }

        /// <summary>
        /// Get the window Size.
        /// </summary>
        /// <returns>x is width and y is height</returns>
        public Vector2 GetWindowSize()
        {
            Vector2 size = Vector2.zero;
            LibUniWinC.GetSize(out size.x, out size.y);
            return size;
        }

        /// <summary>
        /// Get the client area ize.
        /// </summary>
        /// <returns>x is width and y is height</returns>
        public Vector2 GetClientSize()
        {
            Vector2 size = Vector2.zero;
            LibUniWinC.GetClientSize(out size.x, out size.y);
            return size;
        }

        /// <summary>
        /// Get the client area ize.
        /// </summary>
        /// <returns>x is width and y is height</returns>
        public Rect GetClientRectangle()
        {
            Vector2 pos = Vector2.zero;
            Vector2 size = Vector2.zero;
            LibUniWinC.GetClientRectangle(out pos.x, out pos.y, out size.x, out size.y);
            return new Rect(pos.x, pos.y, size.x, size.y);
        }

#endregion

        #region File opening
        public void SetAllowDrop(bool enabled)
        {
            LibUniWinC.SetAllowDrop(enabled);
        }

#endregion

#region About mouse cursor
        /// <summary>
        /// Set the mouse pointer position.
        /// </summary>
        /// <param name="position">Position.</param>
        public static void SetCursorPosition(Vector2 position)
        {
            LibUniWinC.SetCursorPosition(position.x, position.y);
        }

        /// <summary>
        /// Get the mouse pointer position.
        /// </summary>
        /// <returns>The position.</returns>
        public static Vector2 GetCursorPosition()
        {
            Vector2 pos = Vector2.zero;
            LibUniWinC.GetCursorPosition(out pos.x, out pos.y);
            return pos;
        }
        
        public void EnforceCursorPosition()
        {
            Vector2 mousePos = GetCursorPosition();
            Vector2 winPos = GetWindowPosition();
            Rect clientRect = GetClientRectangle();
            Vector2 unityPos = new Vector2(
                (mousePos.x - winPos.x - clientRect.x) * Screen.width / clientRect.width,
                (mousePos.y - winPos.y - clientRect.y) * Screen.height / clientRect.height
            );
            InputState.Change(Mouse.current.position, unityPos);
        }

        /// <summary>
        /// Get pressed mouse buttons.
        /// </summary>
        /// <returns>Bit flags of pressed buttons</returns>
        public static int GetMouseButtons()
        {
            return LibUniWinC.GetMouseButtons();
        }

        /// <summary>
        /// Get pressed modifier keys.
        /// </summary>
        /// <returns>Bit flags of pressed keys</returns>
        public static int GetModifierKeys()
        {
            return LibUniWinC.GetModifierKeys();
        }

        // Not implemented
        public static bool GetCursorVisible()
        {
            return true;
        }
#endregion

#region for Windows only
        /// <summary>
        /// 透過方法を指定（Windowsのみ対応）
        /// </summary>
        /// <param name="type"></param>
        public void SetTransparentType(TransparentType type)
        {
            LibUniWinC.SetTransparentType((Int32)type);
            transparentType = type;
        }

        /// <summary>
        /// 単色透過の場合の透明色を指定（Windowsのみ対応）
        /// </summary>
        /// <param name="color"></param>
        public void SetKeyColor(Color32 color)
        {
            LibUniWinC.SetKeyColor((UInt32)(color.b * 0x10000 + color.g * 0x100 + color.r));
            keyColor = color;
        }
        #endregion

        #region for macOS only
        /// <summary>
        /// ウィンドウの自由配置を設定／解除（macOSのみ対応）
        /// </summary>
        /// <param name="enabled"></param>
        public void EnableFreePositioning(bool enabled)
        {
            LibUniWinC.EnableFreePositioning(enabled);
            _isFreePositioningEnabled = LibUniWinC.IsFreePositioningEnabled();
        }
        #endregion

        #region About monitors
        /// <summary>
        /// Get the monitor index where the window is located
        /// </summary>
        /// <returns>Monitor index</returns>
        public int GetCurrentMonitor()
        {
            return LibUniWinC.GetCurrentMonitor();
        }

        /// <summary>
        /// Get the number of connected monitors
        /// </summary>
        /// <returns>Count</returns>
        public static int GetMonitorCount()
        {
            return LibUniWinC.GetMonitorCount();
        }

        /// <summary>
        /// Get monitor position and size
        /// </summary>
        /// <param name="index"></param>
        /// <param name="position"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static bool GetMonitorRectangle(int index, out Vector2 position, out Vector2 size)
        {
            return LibUniWinC.GetMonitorRectangle(index, out position.x, out position.y, out size.x, out size.y);
        }

        /// <summary>
        /// Fit the window to specified monitor
        /// </summary>
        /// <param name="monitorIndex"></param>
        /// <returns></returns>
        public bool FitToMonitor(int monitorIndex)
        {
            float dx, dy, dw, dh;
            if (LibUniWinC.GetMonitorRectangle(monitorIndex, out dx, out dy, out dw, out dh))
            {
                // 最大化状態なら一度戻す
                if (LibUniWinC.IsMaximized()) LibUniWinC.SetMaximized(false);

                // 指定モニタ中央座標
                float cx = dx + (dw / 2);
                float cy = dy + (dh / 2);

                // ウィンドウ中央を指定モニタ中央に移動
                float ww, wh;
                LibUniWinC.GetSize(out ww, out wh);
                float wx = cx - (ww / 2);
                float wy = cy - (wh / 2);
                LibUniWinC.SetPosition(wx, wy);

                // 最大化
                LibUniWinC.SetMaximized(true);

                //Debug.Log(String.Format("Monitor {4} : {0},{1} - {2},{3}", dx, dy, dw, dh, monitorIndex));
                return true;
            }
            return false;
        }

        /// <summary>
        /// Print monitor list
        /// </summary>
        [Obsolete]
        public static void DebugMonitorInfo()
        {
            int monitors = LibUniWinC.GetMonitorCount();

            int currentMonitorIndex = LibUniWinC.GetCurrentMonitor();

            string message = "Current monitor: " + currentMonitorIndex + "\r\n";

            for (int i = 0; i < monitors; i++)
            {
                float x, y, w, h;
                bool result = LibUniWinC.GetMonitorRectangle(i, out x, out y, out w, out h);
                message += String.Format(
                    "Monitor {0}: X:{1}, Y:{2} - W:{3}, H:{4}\r\n",
                    i, x, y, w, h
                );
            }
            Debug.Log(message);
        }


        /// <summary>
        /// Receive information for debugging
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public static int GetDebugInfo()
        {
            return LibUniWinC.GetDebugInfo();
        }
#endregion

    }
}