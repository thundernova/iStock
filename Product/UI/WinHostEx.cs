/*基于FaceCat(捂脸猫)框架 v1.0 https://github.com/FaceCat007/facecat.git
 *  1.捂脸猫创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 *  2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 *  3.恒泰期货投资咨询总监/高级研究员-戴俊生(微信号:18345063201)
 */

using System;
using System.Collections.Generic;
using System.Text;
using FaceCat;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;

namespace FaceCat {
    /// <summary>
    /// 设计器管理
    /// </summary>
    public class WinHostEx : WinHost {
        [Flags]
        enum MouseEventFlag : uint {
            Move = 0x0001,
            LeftDown = 0x0002,
            LeftUp = 0x0004,
            RightDown = 0x0008,
            RightUp = 0x0010,
            MiddleDown = 0x0020,
            MiddleUp = 0x0040,
            XDown = 0x0080,
            XUp = 0x0100,
            Wheel = 0x0800,
            VirtualDesk = 0x4000,
            Absolute = 0x8000
        }

        public const int WM_KEYDOWN = 0x0100;
        public const int WM_KEYUP = 0x0101;
        public const int WM_CHAR = 0x0102;
        public const int WM_SYSKEYDOWN = 0x0104;
        public const int WM_SYSKEYUP = 0x0105;
        public const int WM_SYSCHAR = 0x0106;

        [DllImport("user32.dll")]
        private static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        private static extern void mouse_event(MouseEventFlag flags, int dx, int dy, int data, int extraInfo);

        [DllImport("user32.dll", EntryPoint = "SendMessageA")]
        private static extern int SendMessage(int hwnd, int wMsg, int wParam, int lParam);

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(int hWnd, int Msg, int wParam, String lParam);

        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int SendMessage(int hWnd, int Msg, int wParam, StringBuilder lParam);

        [DllImport("user32.dll", EntryPoint = "WindowFromPoint")]
        private static extern IntPtr WindowFromPoint(int xPoint, int yPoint);

        [DllImport("user32.dll", EntryPoint = "GetCursorPos")]
        public static extern bool GetCursorPos(ref FCPoint lpPoint);

        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern int FindWindow(String lpClassName, String lpWindowName);

        /// <summary>
        /// 创建内部控件
        /// </summary>
        /// <param name="parent">父控件</param>
        /// <param name="clsid">控件标识</param>
        /// <returns>内部控件</returns>
        public override FCView createInternalControl(FCView parent, String clsid) {
            //日历控件
            FCCalendar calendar = parent as FCCalendar;
            if (calendar != null) {
                if (clsid == "datetitle") {
                    return new DateTitle(calendar);
                }
                else if (clsid == "headdiv") {
                    HeadDiv headDiv = new HeadDiv(calendar);
                    headDiv.Width = parent.Width;
                    headDiv.Dock = FCDockStyle.Top;
                    return headDiv;
                }
                else if (clsid == "lastbutton") {
                    return new ArrowButton(calendar);
                }
                else if (clsid == "nextbutton") {
                    ArrowButton nextBtn = new ArrowButton(calendar);
                    nextBtn.ToLast = false;
                    return nextBtn;
                }
            }
            //分割层
            FCSplitLayoutDiv splitLayoutDiv = parent as FCSplitLayoutDiv;
            if (splitLayoutDiv != null) {
                if (clsid == "splitter") {
                    FCButton splitter = new FCButton();
                    splitter.BackColor = FCColor.Border;
                    splitter.BorderColor = FCColor.Border;
                    splitter.Size = new FCSize(5, 5);
                    return splitter;
                }
            }
            //滚动条
            FCScrollBar scrollBar = parent as FCScrollBar;
            if (scrollBar != null) {
                scrollBar.BorderColor = FCColor.None;
                scrollBar.BackColor = FCColor.None;
                if (clsid == "addbutton") {
                    RibbonButton addButton = new RibbonButton();
                    addButton.Size = new FCSize(15, 15);
                    if (scrollBar is FCHScrollBar) {
                        addButton.ArrowType = 2;
                    }
                    else if (scrollBar is FCVScrollBar) {
                        addButton.ArrowType = 4;
                    }
                    return addButton;
                }
                else if (clsid == "backbutton") {
                    FCButton backButton = new FCButton();
                    backButton.BorderColor = FCColor.None;
                    backButton.BackColor = FCColor.None;
                    return backButton;
                }
                else if (clsid == "scrollbutton") {
                    RibbonButton scrollButton = new RibbonButton();
                    scrollButton.AllowDrag = true;
                    if (scrollBar is FCVScrollBar) {
                        scrollButton.Angle = 0;
                    }
                    return scrollButton;
                }
                else if (clsid == "reducebutton") {
                    RibbonButton reduceButton = new RibbonButton();
                    reduceButton.Size = new FCSize(15, 15);
                    if (scrollBar is FCHScrollBar) {
                        reduceButton.ArrowType = 1;
                    }
                    else if (scrollBar is FCVScrollBar) {
                        reduceButton.ArrowType = 3;
                    }
                    return reduceButton;
                }
            }
            //页夹
            FCTabPage tabPage = parent as FCTabPage;
            if (tabPage != null) {
                if (clsid == "headerbutton") {
                    RibbonButton button = new RibbonButton();
                    button.AllowDrag = true;
                    FCSize size = new FCSize(100, 20);
                    button.Size = size;
                    return button;
                }
            }
            //下拉列表
            FCComboBox comboBox = parent as FCComboBox;
            if (comboBox != null) {
                if (clsid == "dropdownbutton") {
                    RibbonButton dropDownButton = new RibbonButton();
                    dropDownButton.ArrowType = 4;
                    dropDownButton.DisplayOffset = false;
                    int width = comboBox.Width;
                    int height = comboBox.Height;
                    FCPoint location = new FCPoint(width - 20, 0);
                    dropDownButton.Location = location;
                    FCSize size = new FCSize(20, height);
                    dropDownButton.Size = size;
                    return dropDownButton;
                }
                else if (clsid == "dropdownmenu") {
                    FCComboBoxMenu comboBoxMenu = new FCComboBoxMenu();
                    comboBoxMenu.ComboBox = comboBox;
                    comboBoxMenu.Popup = true;
                    FCSize size = new FCSize(100, 200);
                    comboBoxMenu.Size = size;
                    return comboBoxMenu;
                }
            }
            //日期选择
            FCDateTimePicker datePicker = parent as FCDateTimePicker;
            if (datePicker != null) {
                if (clsid == "dropdownbutton") {
                    RibbonButton dropDownButton = new RibbonButton();
                    dropDownButton.ArrowType = 4;
                    dropDownButton.DisplayOffset = false;
                    int width = datePicker.Width;
                    int height = datePicker.Height;
                    FCPoint location = new FCPoint(width - 16, 0);
                    dropDownButton.Location = location;
                    FCSize size = new FCSize(16, height);
                    dropDownButton.Size = size;
                    return dropDownButton;
                }
                else if (clsid == "dropdownmenu") {
                    FCMenu dropDownMenu = new FCMenu();
                    dropDownMenu.Padding = new FCPadding(1);
                    dropDownMenu.Popup = true;
                    FCSize size = new FCSize(200, 200);
                    dropDownMenu.Size = size;
                    return dropDownMenu;
                }
            }
            //数字选择
            FCSpin spin = parent as FCSpin;
            if (spin != null) {
                if (clsid == "downbutton") {
                    RibbonButton downButton = new RibbonButton();
                    downButton.DisplayOffset = false;
                    downButton.ArrowType = 4;
                    FCSize size = new FCSize(16, 16);
                    downButton.Size = size;
                    return downButton;
                }
                else if (clsid == "upbutton") {
                    RibbonButton upButton = new RibbonButton();
                    upButton.DisplayOffset = false;
                    upButton.ArrowType = 3;
                    FCSize size = new FCSize(16, 16);
                    upButton.Size = size;
                    return upButton;
                }
            }
            //容器层
            FCDiv div = parent as FCDiv;
            if (div != null) {
                if (clsid == "hscrollbar") {
                    FCHScrollBar hScrollBar = new FCHScrollBar();
                    hScrollBar.Visible = false;
                    hScrollBar.Size = new FCSize(10, 10);
                    return hScrollBar;
                }
                else if (clsid == "vscrollbar") {
                    FCVScrollBar vScrollBar = new FCVScrollBar();
                    vScrollBar.Visible = false;
                    vScrollBar.Size = new FCSize(10, 10);
                    return vScrollBar;
                }
            }
            //表格
            FCGrid grid = parent as FCGrid;
            if (grid != null) {
                if (clsid == "edittextbox") {
                    return new FCTextBox();
                }
            }
            return base.createInternalControl(parent, clsid);
        }

        /// <summary>
        /// 执行程序
        /// </summary>
        /// <param name="cmd">命令</param>
        public static void execute(String cmd) {
            try {
                Process.Start(cmd);
            }
            catch { }
        }

        /// <summary>
        /// 获取文本
        /// </summary>
        /// <returns>文本</returns>
        public static String getText() {
            FCPoint mp = new FCPoint();
            GetCursorPos(ref mp);
            IntPtr handle = WindowFromPoint(mp.x, mp.y);
            if (handle != IntPtr.Zero) {
                StringBuilder sb = new StringBuilder(10240);
                SendMessage((int)handle, 0xD, 10240, sb);
                return sb.ToString();
            }
            return "";
        }

        /// <summary>
        /// 触发鼠标事件
        /// </summary>
        /// <param name="eventID">事件ID</param>
        /// <param name="dx">横坐标</param>
        /// <param name="dy">纵坐标</param>
        /// <param name="data">滚轮值</param>
        public static void mouseEvent(String eventID, int dx, int dy, int data) {
            MouseEventFlag flag = MouseEventFlag.Move;
            if (eventID == "SETCURSOR") {
                SetCursorPos(dx, dy);
                return;
            }
            else if (eventID == "MOVE") {
                flag = MouseEventFlag.Move;
            }
            else if (eventID == "LEFTDOWN") {
                flag = MouseEventFlag.LeftDown;
            }
            else if (eventID == "LEFTUP") {
                flag = MouseEventFlag.LeftUp;
            }
            else if (eventID == "RIGHTDOWN") {
                flag = MouseEventFlag.RightDown;
            }
            else if (eventID == "RIGHTUP") {
                flag = MouseEventFlag.RightUp;
            }
            else if (eventID == "MIDDLEDOWN") {
                flag = MouseEventFlag.MiddleDown;
            }
            else if (eventID == "MIDDLEUP") {
                flag = MouseEventFlag.MiddleUp;
            }
            else if (eventID == "XDOWN") {
                flag = MouseEventFlag.XDown;
            }
            else if (eventID == "XUP") {
                flag = MouseEventFlag.XUp;
            }
            else if (eventID == "WHEEL") {
                flag = MouseEventFlag.Wheel;
            }
            else if (eventID == "VIRTUALDESK") {
                flag = MouseEventFlag.VirtualDesk;
            }
            else if (eventID == "ABSOLUTE") {
                flag = MouseEventFlag.Absolute;
            }
            mouse_event(flag, dx, dy, data, 0);
        }

        /// <summary>
        /// 触发键盘事件
        /// </summary>
        /// <param name="key">命令</param>
        public static void sendKey(String key) {
            SendKeys.Send(key);
        }

        /// <summary>
        /// 设置文字
        /// </summary>
        /// <param name="text">文字</param>
        public static void setText(String text) {
            if (text != null && text.Length > 0) {
                Clipboard.Clear();
                Clipboard.SetText(text);
                SendKeys.SendWait("^v");
            }
        }

        /// <summary>
        /// 显示ToolTip
        /// </summary>
        /// <param name="text">文字</param>
        /// <param name="mp">位置</param>
        public override void showToolTip(String text, FCPoint mp) {
            //toolTip.Show(text, Control.FromHandle(HWnd), new Point(mp.x, mp.y));
            base.showToolTip(text, mp);
        }
    }
}
