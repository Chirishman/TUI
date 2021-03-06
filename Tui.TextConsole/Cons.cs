﻿using System;
using System.Text;
using System.Runtime.InteropServices;

// HISTORY
///
// Version 0.1: wherin PInvoke.net snippets are copied in, and missing functions coded and submitted to PInvoke.net

namespace Tui.TextConsole
{
    /// <summary>
    ///
    /// --- begin MSDN ---
    /// http://msdn.microsoft.com/en-us/library/ms682073(VS.85).aspx
    /// Console Functions
    /// The following functions are used to access a console.
    /// --- end MSDN ---
    ///
    /// </summary>
    public static class Cons
    {

        static Cons()
        {
            int codePage = (int)GetConsoleOutputCP();
            _encoding = Encoding.GetEncoding(codePage);
        }

        private readonly static Encoding _encoding;

        public static bool WriteConsoleOutputCharacterEncoded(string lpCharacter, uint nLength, Coord dwWriteCoord, out uint lpNumberOfCharsWritten)
        {


            byte[] bs = Encoding.Unicode.GetBytes(lpCharacter);
            bs = Encoding.Convert(Encoding.Unicode, _encoding, bs);
            char[] chars = new char[bs.Length];

            for (int i = 0; i < bs.Length; i++)
                chars[i] = (char)bs[i];
            return WriteConsoleOutputCharacter(ConsoleHandle, new string(chars), nLength, dwWriteCoord, out lpNumberOfCharsWritten);
        }





        private const int STD_OUTPUT_HANDLE = -11;

        public static IntPtr ConsoleHandle = GetStdHandle(STD_OUTPUT_HANDLE);

        // http://pinvoke.net/default.aspx/kernel32/AddConsoleAlias.html
        [DllImport("kernel32", SetLastError = true)]
        public static extern bool AddConsoleAlias(
            string Source,
            string Target,
            string ExeName
            );

        // http://pinvoke.net/default.aspx/kernel32/AllocConsole.html
        [DllImport("kernel32", SetLastError = true)]
        public static extern bool AllocConsole();

        // http://pinvoke.net/default.aspx/kernel32/AttachConsole.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool AttachConsole(
             uint dwProcessId
             );

        // http://pinvoke.net/default.aspx/kernel32/CreateConsoleScreenBuffer.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr CreateConsoleScreenBuffer(
             uint dwDesiredAccess,
             uint dwShareMode,
             IntPtr lpSecurityAttributes,
             uint dwFlags,
             IntPtr lpScreenBufferData
             );

        // http://pinvoke.net/default.aspx/kernel32/FillConsoleOutputAttribute.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool FillConsoleOutputAttribute(
               IntPtr hConsoleOutput,
               ushort wAttribute,
               uint nLength,
               Coord dwWriteCoord,
               out uint lpNumberOfAttrsWritten
               );

        // http://pinvoke.net/default.aspx/kernel32/FillConsoleOutputCharacter.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool FillConsoleOutputCharacter(
              IntPtr hConsoleOutput,
              char cCharacter,
              uint nLength,
              Coord dwWriteCoord,
              out uint lpNumberOfCharsWritten
              );

        // http://pinvoke.net/default.aspx/kernel32/FlushConsoleInputBuffer.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool FlushConsoleInputBuffer(
              IntPtr hConsoleInput
              );

        // http://pinvoke.net/default.aspx/kernel32/FreeConsole.html
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        public static extern bool FreeConsole();

        // http://pinvoke.net/default.aspx/kernel32/GenerateConsoleCtrlEvent.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GenerateConsoleCtrlEvent(
              uint dwCtrlEvent,
              uint dwProcessGroupId
              );

        // http://pinvoke.net/default.aspx/kernel32/GetConsoleAlias.html
        [DllImport("kernel32", SetLastError = true)]
        public static extern bool GetConsoleAlias(
              string Source,
              out StringBuilder TargetBuffer,
              uint TargetBufferLength,
              string ExeName
              );

        // http://pinvoke.net/default.aspx/kernel32/GetConsoleAliases.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint GetConsoleAliases(
            StringBuilder[] lpTargetBuffer,
            uint targetBufferLength,
            string lpExeName
            );

        // http://pinvoke.net/default.aspx/kernel32/GetConsoleAliasesLength.html
        [DllImport("kernel32", SetLastError = true)]
        public static extern uint GetConsoleAliasesLength(
              string ExeName
              );

        // http://pinvoke.net/default.aspx/kernel32/GetConsoleAliasExes.html
        [DllImport("kernel32", SetLastError = true)]
        public static extern uint GetConsoleAliasExes(
             out StringBuilder ExeNameBuffer,
             uint ExeNameBufferLength
             );

        // http://pinvoke.net/default.aspx/kernel32/GetConsoleAliasExesLength.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint GetConsoleAliasExesLength();

        // http://pinvoke.net/default.aspx/kernel32/GetConsoleCP.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint GetConsoleCP();

        // http://pinvoke.net/default.aspx/kernel32/GetConsoleCursorInfo.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetConsoleCursorInfo(
             IntPtr hConsoleOutput,
             out CONSOLE_CURSOR_INFO lpConsoleCursorInfo
             );

        // http://pinvoke.net/default.aspx/kernel32/GetConsoleDisplayMode.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetConsoleDisplayMode(
              out uint ModeFlags
              );

        // http://pinvoke.net/default.aspx/kernel32/GetConsoleFontSize.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern Coord GetConsoleFontSize(
             IntPtr hConsoleOutput,
             Int32 nFont
             );

        // http://pinvoke.net/default.aspx/kernel32/GetConsoleHistoryInfo.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetConsoleHistoryInfo(
             out CONSOLE_HISTORY_INFO ConsoleHistoryInfo
             );

        // http://pinvoke.net/default.aspx/kernel32/GetConsoleMode.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetConsoleMode(
              IntPtr hConsoleHandle,
              out uint lpMode
              );

        // http://pinvoke.net/default.aspx/kernel32/GetConsoleOriginalTitle.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint GetConsoleOriginalTitle(
             out StringBuilder ConsoleTitle,
             uint Size
             );

        // http://pinvoke.net/default.aspx/kernel32/GetConsoleOutputCP.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint GetConsoleOutputCP();

        // http://pinvoke.net/default.aspx/kernel32/GetConsoleProcessList.html
        // TODO: Test - what's an out uint[] during interop? This probably isn't quite right, but provides a starting point:
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint GetConsoleProcessList(
              out uint[] ProcessList,
              uint ProcessCount
              );

        // http://pinvoke.net/default.aspx/kernel32/GetConsoleScreenBufferInfo.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetConsoleScreenBufferInfo(
             IntPtr hConsoleOutput,
             out CONSOLE_SCREEN_BUFFER_INFO lpConsoleScreenBufferInfo
             );

        // http://pinvoke.net/default.aspx/kernel32/GetConsoleScreenBufferInfoEx.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetConsoleScreenBufferInfoEx(
              IntPtr hConsoleOutput,
              out CONSOLE_SCREEN_BUFFER_INFO_EX ConsoleScreenBufferInfo
              );

        // http://pinvoke.net/default.aspx/kernel32/GetConsoleSelectionInfo.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetConsoleSelectionInfo(
             CONSOLE_SELECTION_INFO ConsoleSelectionInfo
             );

        // http://pinvoke.net/default.aspx/kernel32/GetConsoleTitle.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern uint GetConsoleTitle(
            [Out] StringBuilder lpConsoleTitle,
            uint nSize
            );

        // http://pinvoke.net/default.aspx/kernel32/GetConsoleWindow.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetConsoleWindow();

        // http://pinvoke.net/default.aspx/kernel32/GetCurrentConsoleFont.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetCurrentConsoleFont(
              IntPtr hConsoleOutput,
              bool bMaximumWindow,
              out CONSOLE_FONT_INFO lpConsoleCurrentFont
              );

        //// http://pinvoke.net/default.aspx/kernel32/GetCurrentConsoleFontEx.html
        //[DllImport("kernel32.dll", SetLastError = true)]
        //static extern bool GetCurrentConsoleFontEx(
        //    IntPtr ConsoleOutput,
        //    bool MaximumWindow,
        //    out CONSOLE_FONT_INFO_EX ConsoleCurrentFont
        //    );

        // http://pinvoke.net/default.aspx/kernel32/GetLargestConsoleWindowSize.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern Coord GetLargestConsoleWindowSize(
             IntPtr hConsoleOutput
             );

        // http://pinvoke.net/default.aspx/kernel32/GetNumberOfConsoleInputEvents.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetNumberOfConsoleInputEvents(
              IntPtr hConsoleInput,
              out uint lpcNumberOfEvents
              );

        // http://pinvoke.net/default.aspx/kernel32/GetNumberOfConsoleMouseButtons.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool GetNumberOfConsoleMouseButtons(
            ref uint lpNumberOfMouseButtons
            );

        // http://pinvoke.net/default.aspx/kernel32/GetStdHandle.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern IntPtr GetStdHandle(
              int nStdHandle
              );

        // http://pinvoke.net/default.aspx/kernel32/HandlerRoutine.html
        // Delegate type to be used as the Handler Routine for SCCH
        public delegate bool ConsoleCtrlDelegate(CtrlTypes CtrlType);

        // http://pinvoke.net/default.aspx/kernel32/PeekConsoleInput.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool PeekConsoleInput(
              IntPtr hConsoleInput,
              [Out] INPUT_RECORD[] lpBuffer,
              uint nLength,
              out uint lpNumberOfEventsRead
              );

        // http://pinvoke.net/default.aspx/kernel32/ReadConsole.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadConsole(
              IntPtr hConsoleInput,
              [Out] StringBuilder lpBuffer,
              uint nNumberOfCharsToRead,
              out uint lpNumberOfCharsRead,
              IntPtr lpReserved
              );

        // http://pinvoke.net/default.aspx/kernel32/ReadConsoleInput.html
        [DllImport("kernel32.dll", EntryPoint = "ReadConsoleInputW", CharSet = CharSet.Unicode)]
        public static extern bool ReadConsoleInput(
              IntPtr hConsoleInput,
              [Out] INPUT_RECORD[] lpBuffer,
              uint nLength,
              out uint lpNumberOfEventsRead
              );

        // http://pinvoke.net/default.aspx/kernel32/ReadConsoleOutput.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadConsoleOutput(
             IntPtr hConsoleOutput,
             [Out] CHAR_INFO[] lpBuffer,
             Coord dwBufferSize,
             Coord dwBufferCoord,
             ref SMALL_RECT lpReadRegion
             );

        // http://pinvoke.net/default.aspx/kernel32/ReadConsoleOutputAttribute.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadConsoleOutputAttribute(
              IntPtr hConsoleOutput,
              [Out] ushort[] lpAttribute,
              uint nLength,
              Coord dwReadCoord,
              out uint lpNumberOfAttrsRead
              );

        // http://pinvoke.net/default.aspx/kernel32/ReadConsoleOutputCharacter.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ReadConsoleOutputCharacter(
               IntPtr hConsoleOutput,
               [Out] StringBuilder lpCharacter,
               uint nLength,
               Coord dwReadCoord,
               out uint lpNumberOfCharsRead
               );

        // http://pinvoke.net/default.aspx/kernel32/ScrollConsoleScreenBuffer.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool ScrollConsoleScreenBuffer(
             IntPtr hConsoleOutput,
            [In] ref SMALL_RECT lpScrollRectangle,
             IntPtr lpClipRectangle,
            Coord dwDestinationOrigin,
             [In] ref CHAR_INFO lpFill
             );

        // http://pinvoke.net/default.aspx/kernel32/SetConsoleActiveScreenBuffer.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleActiveScreenBuffer(
               IntPtr hConsoleOutput
               );

        // http://pinvoke.net/default.aspx/kernel32/SetConsoleCP.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleCP(
             uint wCodePageID
             );

        // http://pinvoke.net/default.aspx/kernel32/SetConsoleCtrlHandler.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleCtrlHandler(
             ConsoleCtrlDelegate HandlerRoutine,
             bool Add
             );

        // http://pinvoke.net/default.aspx/kernel32/SetConsoleCursorInfo.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleCursorInfo(
             IntPtr hConsoleOutput,
             [In] ref CONSOLE_CURSOR_INFO lpConsoleCursorInfo
             );

        // http://pinvoke.net/default.aspx/kernel32/SetConsoleCursorPosition.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleCursorPosition(
              IntPtr hConsoleOutput,
             Coord dwCursorPosition
              );

        // http://pinvoke.net/default.aspx/kernel32/SetConsoleDisplayMode.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleDisplayMode(
              IntPtr ConsoleOutput,
              uint Flags,
              out Coord NewScreenBufferDimensions
              );

        // http://pinvoke.net/default.aspx/kernel32/SetConsoleHistoryInfo.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleHistoryInfo(
               CONSOLE_HISTORY_INFO ConsoleHistoryInfo
               );

        // http://pinvoke.net/default.aspx/kernel32/SetConsoleMode.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleMode(
              IntPtr hConsoleHandle,
              uint dwMode
              );

        // http://pinvoke.net/default.aspx/kernel32/SetConsoleOutputCP.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleOutputCP(
              uint wCodePageID
              );

        // http://pinvoke.net/default.aspx/kernel32/SetConsoleScreenBufferInfoEx.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleScreenBufferInfoEx(
              IntPtr ConsoleOutput,
              CONSOLE_SCREEN_BUFFER_INFO_EX ConsoleScreenBufferInfoEx
              );

        // http://pinvoke.net/default.aspx/kernel32/SetConsoleScreenBufferSize.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleScreenBufferSize(
              IntPtr hConsoleOutput,
              Coord dwSize
              );

        // http://pinvoke.net/default.aspx/kernel32/SetConsoleTextAttribute.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleTextAttribute(
             IntPtr hConsoleOutput,
            ushort wAttributes
             );

        // http://pinvoke.net/default.aspx/kernel32/SetConsoleTitle.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleTitle(
             string lpConsoleTitle
             );

        // http://pinvoke.net/default.aspx/kernel32/SetConsoleWindowInfo.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetConsoleWindowInfo(
              IntPtr hConsoleOutput,
              bool bAbsolute,
              [In] ref SMALL_RECT lpConsoleWindow
              );

        //// http://pinvoke.net/default.aspx/kernel32/SetCurrentConsoleFontEx.html
        //[DllImport("kernel32.dll", SetLastError = true)]
        //static extern bool SetCurrentConsoleFontEx(
        //    IntPtr ConsoleOutput,
        //    bool MaximumWindow,
        //    CONSOLE_FONT_INFO_EX ConsoleCurrentFontEx
        //    );

        // http://pinvoke.net/default.aspx/kernel32/SetStdHandle.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool SetStdHandle(
               uint nStdHandle,
               IntPtr hHandle
               );

        // http://pinvoke.net/default.aspx/kernel32/WriteConsole.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteConsole(
               IntPtr hConsoleOutput,
               string lpBuffer,
               uint nNumberOfCharsToWrite,
               out uint lpNumberOfCharsWritten,
               IntPtr lpReserved
               );

        // http://pinvoke.net/default.aspx/kernel32/WriteConsoleInput.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteConsoleInput(
              IntPtr hConsoleInput,
              INPUT_RECORD[] lpBuffer,
              uint nLength,
              out uint lpNumberOfEventsWritten
              );

        // http://pinvoke.net/default.aspx/kernel32/WriteConsoleOutput.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteConsoleOutput(
             IntPtr hConsoleOutput,
             CHAR_INFO[] lpBuffer,
             Coord dwBufferSize,
             Coord dwBufferCoord,
             ref SMALL_RECT lpWriteRegion
             );

        // http://pinvoke.net/default.aspx/kernel32/WriteConsoleOutputAttribute.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteConsoleOutputAttribute(
             IntPtr hConsoleOutput,
             ushort[] lpAttribute,
             uint nLength,
             Coord dwWriteCoord,
             out uint lpNumberOfAttrsWritten
             );






        // http://pinvoke.net/default.aspx/kernel32/WriteConsoleOutputCharacter.html
        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern bool WriteConsoleOutputCharacter(
             IntPtr hConsoleOutput,
             string lpCharacter,
             uint nLength,
             Coord dwWriteCoord,
             out uint lpNumberOfCharsWritten
             );



        public struct SMALL_RECT
        {

            public short Left;
            public short Top;
            public short Right;
            public short Bottom;

        }

        public struct CONSOLE_SCREEN_BUFFER_INFO
        {

            public Coord dwSize;
            public Coord dwCursorPosition;
            public short wAttributes;
            public SMALL_RECT srWindow;
            public Coord dwMaximumWindowSize;

        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CONSOLE_SCREEN_BUFFER_INFO_EX
        {
            public uint cbSize;
            public Coord dwSize;
            public Coord dwCursorPosition;
            public short wAttributes;
            public SMALL_RECT srWindow;
            public Coord dwMaximumWindowSize;

            public ushort wPopupAttributes;
            public bool bFullscreenSupported;

            // Hack Hack Hack
            // Too lazy to figure out the array at the moment...
            //public COLORREF[16] ColorTable;
            public COLORREF color0;
            public COLORREF color1;
            public COLORREF color2;
            public COLORREF color3;

            public COLORREF color4;
            public COLORREF color5;
            public COLORREF color6;
            public COLORREF color7;

            public COLORREF color8;
            public COLORREF color9;
            public COLORREF colorA;
            public COLORREF colorB;

            public COLORREF colorC;
            public COLORREF colorD;
            public COLORREF colorE;
            public COLORREF colorF;
        }

        //[StructLayout(LayoutKind.Sequential)]
        //struct COLORREF
        //{
        //    public byte R;
        //    public byte G;
        //    public byte B;
        //}

        public struct Color
        {
            public uint R;
            public uint G;
            public uint B;
            public Color(uint r, uint g, uint b)
            {
                R = r;
                G = g;
                B = b;
            }
        }


        [StructLayout(LayoutKind.Sequential)]
        public struct COLORREF
        {
            public uint ColorDWORD;

            public COLORREF(Color color)
            {
                ColorDWORD = color.R + (color.G << 8) + (color.B << 16);
            }

            public Color GetColor()
            {
                return new Color((0x000000FFU & ColorDWORD),
                   (0x0000FF00U & ColorDWORD) >> 8, (0x00FF0000U & ColorDWORD) >> 16);
            }

            public void SetColor(Color color)
            {
                ColorDWORD = color.R + ((color.G) << 8) + ((color.B) << 16);
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CONSOLE_FONT_INFO
        {
            public int nFont;
            public Coord dwFontSize;
        }


        //[StructLayout(LayoutKind.Sequential)]
        //public unsafe struct CONSOLE_FONT_INFO_EX
        //{
        //    public uint cbSize;
        //    public uint nFont;
        //    public Coord dwFontSize;
        //    public ushort FontFamily;
        //    public ushort FontWeight;
        //    fixed char FaceName[LF_FACESIZE];

        //    const uint LF_FACESIZE = 32;
        //}

        [StructLayout(LayoutKind.Explicit)]
        public struct INPUT_RECORD
        {
            [FieldOffset(0)]
            public ushort EventType;
            [FieldOffset(4)]
            public KEY_EVENT_RECORD KeyEvent;
            [FieldOffset(4)]
            public MOUSE_EVENT_RECORD MouseEvent;
            [FieldOffset(4)]
            public WINDOW_BUFFER_SIZE_RECORD WindowBufferSizeEvent;
            [FieldOffset(4)]
            public MENU_EVENT_RECORD MenuEvent;
            [FieldOffset(4)]
            public FOCUS_EVENT_RECORD FocusEvent;
        };

        [StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
        public struct KEY_EVENT_RECORD
        {
            [FieldOffset(0), MarshalAs(UnmanagedType.Bool)]
            public bool bKeyDown;
            [FieldOffset(4), MarshalAs(UnmanagedType.U2)]
            public ushort wRepeatCount;
            [FieldOffset(6), MarshalAs(UnmanagedType.U2)]
            //public VirtualKeys wVirtualKeyCode;
            public ushort wVirtualKeyCode;
            [FieldOffset(8), MarshalAs(UnmanagedType.U2)]
            public ushort wVirtualScanCode;
            [FieldOffset(10)]
            public char UnicodeChar;
            [FieldOffset(12), MarshalAs(UnmanagedType.U4)]
            //public ControlKeyState dwControlKeyState;
            public uint dwControlKeyState;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSE_EVENT_RECORD
        {
            public Coord dwMousePosition;
            public uint dwButtonState;
            public uint dwControlKeyState;
            public uint dwEventFlags;
        }

        public struct WINDOW_BUFFER_SIZE_RECORD
        {
            public Coord dwSize;

            public WINDOW_BUFFER_SIZE_RECORD(short x, short y)
            {
                dwSize = new Coord();
                dwSize.X = x;
                dwSize.Y = y;
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MENU_EVENT_RECORD
        {
            public uint dwCommandId;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct FOCUS_EVENT_RECORD
        {
            public bool bSetFocus;
        }

        //CHAR_INFO struct, which was a union in the old days
        // so we want to use LayoutKind.Explicit to mimic it as closely
        // as we can
        [StructLayout(LayoutKind.Explicit)]
        public struct CHAR_INFO
        {
            [FieldOffset(0)]
            public char UnicodeChar;
            [FieldOffset(0)]
            public char AsciiChar;
            [FieldOffset(2)] //2 bytes seems to work properly
            public UInt16 Attributes;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CONSOLE_CURSOR_INFO
        {
            uint Size;
            bool Visible;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CONSOLE_HISTORY_INFO
        {
            ushort cbSize;
            ushort HistoryBufferSize;
            ushort NumberOfHistoryBuffers;
            uint dwFlags;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct CONSOLE_SELECTION_INFO
        {
            uint Flags;
            Coord SelectionAnchor;
            SMALL_RECT Selection;

            // Flags values:
            const uint CONSOLE_MOUSE_DOWN = 0x0008; // Mouse is down
            const uint CONSOLE_MOUSE_SELECTION = 0x0004; //Selecting with the mouse
            const uint CONSOLE_NO_SELECTION = 0x0000; //No selection
            const uint CONSOLE_SELECTION_IN_PROGRESS = 0x0001; //Selection has begun
            const uint CONSOLE_SELECTION_NOT_EMPTY = 0x0002; //Selection rectangle is not empty
        }

        // Enumerated type for the control messages sent to the handler routine
        public enum CtrlTypes : uint
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT,
            CTRL_CLOSE_EVENT,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT
        }
    }
}
