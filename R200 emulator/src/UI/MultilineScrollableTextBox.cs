using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;

// Derived from "How to Get the Position of Scroll Bar for MultiLine TextBox in Windows Form" project
// https://code.msdn.microsoft.com/windowsapps/How-to-Get-the-Position-of-89652d40/sourcecode?fileId=148046&pathId=1539366228
// Distributed under Apache License 2.0

namespace ScrollbarPosition
{
    /// <summary>
    /// This class is created to explicitly capture windows messages sent to a
    /// multiline textbox control present in the windows form and track the
    /// immediate position of its associated vertical scrollbar.
    /// </summary>
    class MultilineScrollableTextBox : TextBox
    {
        // This windows message is sent when a scroll event occurs in the window's
        // standard vertical scroll bar.
        private const int WM_VSCROLL = 0x115;
        // This message is sent to the focus window when the mouse wheel is rotated.
        private const int WM_MOUSEWHEEL = 0x20A;
        private Form parentForm;

        public event ScrollEventHandler ScrollPositionChanged;

        public MultilineScrollableTextBox() : base() { }

        // The GetScrollInfo function retrieves the parameters of a scroll bar, including
        // the minimum and maximum scrolling positions, the page size, and the position
        // of the scroll box (thumb).
        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetScrollInfo(IntPtr hwnd, int fnBar,
            ref SCROLLINFO lps);

        /// <summary>
        /// The structure contains scroll bar parameters to be retrieved by the
        /// 'GetScrollInfo' function.
        /// </summary>
        [Serializable, StructLayout(LayoutKind.Sequential)]
        struct SCROLLINFO
        {
            public uint cbSize; // Specifies the size, in bytes, of this structure.
            public uint fMask; // Specifies the scroll bar parameters to set or retrieve.
            public int nMin; // Specifies the minimum scrolling position.
            public int nMax; // Specifies the maximum scrolling position.
            public uint nPage; // Specifies the page size, in device units.
            public int nPos; // Specifies the position of the scroll box.
            public int nTrackPos; // Specifies the immediate position of a scroll box.
        }

        // This enum specifies the type of scroll bar for which to retrieve parameters.
        private enum ScrollbarOrientation : int
        {
            SB_HORZ = 0x0, // Represents scroll bar control.
            SB_VERT = 0x1, // Represents standard vertical scroll bar.
            SB_CTL = 0x2, // Represents standard horizontal scroll bar.
            SB_BOTH = 0x3 // Represents both vertical and horizontal scroll bars.
        }

        // This enum specifies the scroll bar parameters to set or retrieve.
        private enum ScrollInfoMask : uint
        {
            // The nMin and nMax members contain the minimum and maximum values for
            // the scrolling range.
            SIF_RANGE = 0x1,
            // The nPage member contains the page size for a proportional scroll bar.
            SIF_PAGE = 0x2,
            // The nPos member contains the scroll box position, which is not updated
            // while the user drags the scroll box.
            SIF_POS = 0x4,
            // This value is used only when setting a scroll bar's parameters. If the
            // scroll bar's new parameters make the scroll bar unnecessary, disable
            // the scroll bar instead of removing it.
            SIF_DISABLENOSCROLL = 0x8,
            // The nTrackPos member contains the current position of the scroll box
            // while the user is dragging it.
            SIF_TRACKPOS = 0x10,
            // Combination of SIF_PAGE, SIF_POS, SIF_RANGE, and SIF_TRACKPOS.
            SIF_ALL = (SIF_RANGE | SIF_PAGE | SIF_POS | SIF_TRACKPOS)
        }

        // This enum specifies different messages that are sent to a window when a scroll
        // event occurs in the window's standard vertical or horizontal scroll bar.
        private enum ScrollBarCommands
        {
            SB_LINEUP = 0, // Scrolls one line up.
            SB_LINELEFT = 0, // Scrolls left by one unit.
            SB_LINEDOWN = 1, // Scrolls one line down.
            SB_LINERIGHT = 1, // Scrolls right by one unit.
            SB_PAGEUP = 2, // Scrolls one page up.
            SB_PAGELEFT = 2, // Scrolls left by the width of the window.
            SB_PAGEDOWN = 3, // Scrolls one page down.
            SB_PAGERIGHT = 3, // Scrolls right by the width of the window.
            SB_THUMBPOSITION = 4, // Scroll box is dragged and mouse button is released.
            SB_THUMBTRACK = 5, // Scroll box is being dragged.
            SB_TOP = 6, // Scrolls to the upper left.
            SB_LEFT = 6, // Scrolls to the upper left.
            SB_BOTTOM = 7, // Scrolls to the lower right.
            SB_RIGHT = 7, // Scrolls to the lower right.
            SB_ENDSCROLL = 8 // Ends scroll.
        }

        public MultilineScrollableTextBox(Form parentForm)
        {
            this.parentForm = parentForm;
        }

        /// <summary>
        /// This function retrieves the immediate position of a scroll box that the
        /// user is dragging in a multiline textbox control.
        /// </summary>
        /// <returns>
        /// Current position of the scroll box.
        /// </returns>
        private int GetScrollbarPosition()
        {
            SCROLLINFO scrollInfo = new SCROLLINFO();
            
            scrollInfo.cbSize = (uint)Marshal.SizeOf(scrollInfo);
            scrollInfo.fMask = (int)ScrollInfoMask.SIF_TRACKPOS;
            // This function 'GetScrollInfo' retrieves the parameters of a scroll bar,
            // including the minimum and maximum scrolling positions, the page size,
            // and the position of the scroll box (thumb).
            GetScrollInfo(this.Handle, (int)ScrollbarOrientation.SB_VERT, ref scrollInfo);            
            
            return scrollInfo.nTrackPos;            
        }
        
        /// <summary>
        /// Retrieves the high-order word from the specified 32-bit value.
        /// </summary>
        /// <param name="number">
        /// Input parameter whose high-order word needs to be retrieved.
        /// </param>
        /// <returns>
        /// The return value is the high-order word of the specified value.
        /// </returns>    
        private static Int32 HiWord(IntPtr number)
        {
            Int32 val32 = number.ToInt32();
            return ((val32 >> 16) & 0xFFFF);
        }

        /// <summary>
        /// Retrieves the low-order word from the specified value.
        /// </summary>
        /// <param name="number">
        /// The value to be converted.
        /// </param>
        /// <returns>
        /// The return value is the low-order word of the specified value.
        /// </returns>
        private static Int32 LoWord(IntPtr number)
        {
            Int32 val32 = number.ToInt32();
            return (val32 & 0xFFFF);
        }

        /// <summary>
        /// An application-defined function that processes messages sent to a window.
        /// </summary>
        /// <param name="message">
        /// System defined windows message.
        /// </param>
        protected override void WndProc(ref Message message)
        {            
            if (message.Msg == WM_VSCROLL || message.Msg == WM_MOUSEWHEEL ||
                LoWord(message.WParam) == (int)ScrollBarCommands.SB_LINEUP ||
                LoWord(message.WParam) == (int)ScrollBarCommands.SB_LINEDOWN ||
                LoWord(message.WParam) == (int)ScrollBarCommands.SB_PAGEUP ||
                LoWord(message.WParam) == (int)ScrollBarCommands.SB_PAGEDOWN ||
                LoWord(message.WParam) == (int)ScrollBarCommands.SB_THUMBTRACK ||
                LoWord(message.WParam) == (int)ScrollBarCommands.SB_THUMBPOSITION)
            {
                // Update immediate position of the scrollbar returned by the function
                // 'GetScrollbarPosition' in the label 'lbScrollbarPosition'.
                ScrollPositionChanged?.Invoke(this, new ScrollEventArgs(ScrollEventType.EndScroll, GetScrollbarPosition()));
            }

            base.WndProc(ref message);
        }
    }
}
