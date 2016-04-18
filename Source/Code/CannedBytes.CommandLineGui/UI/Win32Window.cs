using System;
using System.Windows;
using System.Windows.Interop;

namespace CannedBytes.CommandLineGui.UI
{
    /// <summary>
    /// Helper class to pass a Wpf Window to a Forms control using its native window handle.
    /// </summary>
    sealed class Win32Window : System.Windows.Forms.IWin32Window, System.Windows.Interop.IWin32Window
    {
        /// <summary>
        /// Constructs a new instance based on a WPF window.
        /// </summary>
        /// <param name="wpfWindow">Must not be null.</param>
        public Win32Window(Window wpfWindow)
        {
            var wih = new WindowInteropHelper(wpfWindow);
            this.Handle = wih.Handle;
        }

        /// <summary>
        /// The native window handle.
        /// </summary>
        public IntPtr Handle { get; private set; }
    }
}
