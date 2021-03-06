/*基于FaceCat(捂脸猫)框架 v1.0 https://github.com/FaceCat007/facecat.git
 *  1.捂脸猫创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 *  2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 *  3.恒泰期货投资咨询总监/高级研究员-戴俊生(微信号:18345063201);
 *  4.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 */

using System;
using System.Collections.Generic;
using System.Text;
using FaceCat;
using System.Drawing;

namespace FaceCat {
    /// <summary>
    /// 窗体状态
    /// </summary>
    public enum WindowStateA {
        /// <summary>
        /// 普通
        /// </summary>
        Normal,
        /// <summary>
        /// 最大化
        /// </summary>
        Max,
        /// <summary>
        /// 最小化
        /// </summary>
        Min
    }

    /// <summary>
    /// 重绘窗体
    /// </summary>
    public class WindowEx : FCWindow {
        /// <summary>
        /// 创建窗体
        /// </summary>
        public WindowEx() {
            BackColor = FCColor.None;
            BorderColor = FCDraw.FCCOLORS_LINECOLOR3;
            CaptionHeight = 25;
            Font = new FCFont("微软雅黑", 14, false, false, false);
            TextColor = FCColor.None;
            ShadowColor = FCDraw.FCCOLORS_BACKCOLOR5;
            ShadowSize = 0;
        }

        /// <summary>
        /// 动画的方向
        /// </summary>
        private int m_animateDirection;

        /// <summary>
        /// 普通尺寸
        /// </summary>
        private FCPoint m_normalLocation;

        /// <summary>
        /// 普通尺寸
        /// </summary>
        private FCSize m_normalSize;

        /// <summary>
        /// 随机种子
        /// </summary>
        private Random m_rd = new Random();

        /// <summary>
        /// 秒表计数
        /// </summary>
        private int m_tick;

        /// <summary>
        /// 秒表ID
        /// </summary>
        private int m_timerID = FCView.getNewTimerID();

        private bool m_animateMoving;

        /// <summary>
        /// 获取是否正在动画移动
        /// </summary>
        public bool AnimateMoving {
            get { return m_animateMoving; }
        }

        private WindowButton m_closeButton;

        /// <summary>
        /// 获取或设置关闭按钮
        /// </summary>
        public WindowButton CloseButton {
            get { return m_closeButton; }
            set { m_closeButton = value; }
        }

        private bool m_isChildWindow;

        /// <summary>
        /// 获取或设置是否子窗体
        /// </summary>
        public bool IsChildWindow {
            get { return m_isChildWindow; }
            set { m_isChildWindow = value; }
        }

        private WindowButton m_maxOrRestoreButton;

        /// <summary>
        /// 获取或设置最大化按钮
        /// </summary>
        public WindowButton MaxOrRestoreButton {
            get { return m_maxOrRestoreButton; }
            set { m_maxOrRestoreButton = value; }
        }

        private WindowButton m_minButton;

        /// <summary>
        /// 获取或设置最小化按钮
        /// </summary>
        public WindowButton MinButton {
            get { return m_minButton; }
            set { m_minButton = value; }
        }

        private bool m_showMaxBox = true;

        /// <summary>
        /// 获取或设置是否显示最大化按钮
        /// </summary>
        public bool ShowMaxBox {
            get { return m_showMaxBox; }
            set {
                m_showMaxBox = value;
                if (m_maxOrRestoreButton != null) {
                    m_maxOrRestoreButton.Visible = value;
                }
            }
        }

        private bool m_showMinBox = true;

        /// <summary>
        /// 获取或设置是否显示最小化按钮
        /// </summary>
        public bool ShowMinBox {
            get {
                return m_showMinBox;
            }
            set {
                m_showMinBox = value;
                if (m_minButton != null) {
                    m_minButton.Visible = value;
                }
            }
        }

        private bool m_useAnimation = true;

        /// <summary>
        /// 获取或设置是否使用动画
        /// </summary>
        public bool UseAnimation {
            get { return m_useAnimation; }
            set { m_useAnimation = value; }
        }

        private WindowStateA m_windowState = WindowStateA.Normal;

        /// <summary>
        /// 获取或设置当前的状态
        /// </summary>
        public WindowStateA WindowState {
            get { return m_windowState; }
            set { m_windowState = value; }
        }

        /// <summary>
        /// 以动画形式显示
        /// </summary>
        /// <param name="showDialog">是否对话框打开</param>
        public void animateShow(bool showDialog) {
            FCNative native = Native;
            FCSize nativeSize = native.DisplaySize;
            int width = Width, height = Height, mx = (nativeSize.cx - width) / 2, my = (nativeSize.cy - height) / 2, x = mx, y = my;
            FCPoint location = new FCPoint(x, y);
            Location = location;
            if (showDialog) {
                this.showDialog();
            } else {
                show();
            }
            update();
        }

        /// <summary>
        /// 点击按钮方法
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="mp">坐标</param>
        /// <param name="button">按钮</param>
        /// <param name="click">点击次数</param>
        /// <param name="delta">滚轮滚动值</param>
        private void clickButton(object sender, FCTouchInfo touchInto) {
            if (touchInto.m_firstTouch && touchInto.m_clicks == 1) {
                FCView control = sender as FCView;
                String name = control.Name;
                if (name == "btnMaxOrRestore") {
                    maxOrRestore();
                } else if (name == "btnMin") {
                    min();
                }
            }
        }

        /// <summary>
        /// 销毁控件方法
        /// </summary>
        public override void delete() {
            if (!IsDeleted) {
                m_animateMoving = false;
                stopTimer(m_timerID);
            }
            base.delete();
        }

        /// <summary>
        /// 获取属性
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="value">值</param>
        /// <param name="type">类型</param>
        public override void getProperty(String name, ref String value, ref String type) {
            if (name == "showmaxbox") {
                type = "bool";
                value = FCStr.convertBoolToStr(ShowMaxBox);
            } else if (name == "showminbox") {
                type = "bool";
                value = FCStr.convertBoolToStr(ShowMinBox);
            } else if (name == "windowstate") {
                type = "enum:WindowStateA";
                WindowStateA windowState = WindowState;
                if (windowState == WindowStateA.Max) {
                    value = "Max";
                } else if (windowState == WindowStateA.Min) {
                    value = "Min";
                } else if (windowState == WindowStateA.Normal) {
                    value = "Normal";
                }
            } else {
                base.getProperty(name, ref value, ref type);
            }
        }

        /// <summary>
        /// 获取属性名称列表
        /// </summary>
        /// <returns>名称列表</returns>
        public override ArrayList<String> getPropertyNames() {
            ArrayList<String> propertyNames = base.getPropertyNames();
            propertyNames.AddRange(new String[] { "ShowMaxBox", "ShowMinBox", "WindowState" });
            return propertyNames;
        }

        /// <summary>
        /// 控件添加方法
        /// </summary>
        public override void onAdd() {
            base.onAdd();
            if (m_closeButton == null) {
                m_closeButton = new WindowButton();
                m_closeButton.Name = "btnClose";
                FCSize buttonSize = new FCSize(20, 20);
                m_closeButton.Size = buttonSize;
                addControl(m_closeButton);
            }
            if (m_maxOrRestoreButton == null) {
                m_maxOrRestoreButton = new WindowButton();
                m_maxOrRestoreButton.Name = "btnMaxOrRestore";
                m_maxOrRestoreButton.Style = WindowButtonStyle.Max;
                FCSize buttonSize = new FCSize(20, 20);
                m_maxOrRestoreButton.Size = buttonSize;
                addControl(m_maxOrRestoreButton);
                m_maxOrRestoreButton.Visible = m_showMaxBox;
                m_maxOrRestoreButton.addEvent(new FCTouchEvent(clickButton), FCEventID.CLICK);
            }
            if (m_minButton == null) {
                m_minButton = new WindowButton();
                m_minButton.Name = "btnMin";
                m_minButton.Style = WindowButtonStyle.Min;
                FCSize buttonSize = new FCSize(20, 20);
                m_minButton.Size = buttonSize;
                addControl(m_minButton);
                m_minButton.Visible = m_showMinBox;
                m_minButton.addEvent(new FCTouchEvent(clickButton), FCEventID.CLICK);
            }
            startTimer(m_timerID, 10);
        }

        /// <summary>
        /// 拖动开始方法
        /// </summary>
        /// <param name="startOffset">偏移启动量</param>
        public override void onDragReady(ref FCPoint startOffset) {
            startOffset.x = 0;
            startOffset.y = 0;
        }

        /// <summary>
        /// 重绘前景方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public override void onPaintBackground(FCPaint paint, FCRect clipRect) {
            int width = Width;
            int height = Height;
            FCRect rect = new FCRect(0, 0, width, height);
            long backColor = FCDraw.FCCOLORS_WINDOWBACKCOLOR;
            long foreColor = FCColor.argb(255, 255, 255);
            if (paint.supportTransparent()) {
                backColor = FCDraw.FCCOLORS_WINDOWBACKCOLOR2;
            }
            long borderColor = FCColor.argb(50, 105, 217);
            if (m_tick >= 200) {
                borderColor = FCColor.argb(255 - (m_tick - 200) * 2, 50, 105, 217);
                foreColor = FCColor.argb(255 - (m_tick - 200) * 2, 255, 255, 255);
            }
            int dw1 = 30;
            int dw2 = 50;
            int dSize = 10;
            int dSize2 = 20;
            if (m_tick >= 200) {
                dw1 += (m_tick - 200) / 6;
                dw2 += (m_tick - 200) / 6;
                dSize += (m_tick - 200) / 6;
                dSize2 += (m_tick - 200) / 6;
            }
            FCPoint[] points1 = new FCPoint[6];
            points1[0] = new FCPoint(dSize, dw1 + dSize);
            points1[1] = new FCPoint(dSize + dw1, dSize);
            points1[2] = new FCPoint(width - dSize, dSize);
            points1[3] = new FCPoint(width - dSize, height - dw2 - dSize2);
            points1[4] = new FCPoint(width - dSize - dw2, height - dSize2);
            points1[5] = new FCPoint(dSize, height - dSize2);
            paint.fillGradientPolygon(FCColor.argb(255, 9, 30, 42), FCColor.argb(200, 9, 30, 42), points1, m_tick);
            //paint.fillPolygon(backColor, points1);
            paint.drawPolygon(borderColor, 3, 0, points1);
            paint.drawLine(borderColor, 2, 0, width - dSize - 30, 1, width - 1, 1);
            paint.drawLine(borderColor, 2, 0, width - 1, 1, width - 1, dSize * 2);
            paint.drawLine(borderColor, 2, 0, width - dSize - 20, dSize + 5, width - dSize - 5, dSize + 5);
            paint.drawLine(borderColor, 2, 0, width - dSize - 5, dSize + 5, width - dSize - 5, dSize + 20);
            FCPoint[] points2 = new FCPoint[4];
            points2[0] = new FCPoint(dSize + 10, dSize + dw1 + 30);
            points2[1] = new FCPoint(dSize + 10, dSize + dw1 + 2);
            points2[2] = new FCPoint(dSize + dw1 + 2, dSize + 10);
            points2[3] = new FCPoint(dSize + dw1 + 30, dSize + 10);
            paint.drawPolyline(borderColor, 2, 0, points2);
            paint.drawLine(borderColor, 2, 0, dSize + dw1 + 31, dSize + 10, dSize + dw1 + 50, dSize + 10);
            paint.drawLine(borderColor, 2, 0, dSize + dw1 + 10, dSize + 18, dSize + dw1 + 40, dSize + 18);
            paint.drawLine(borderColor, 2, 0, dSize + 10, height - dSize * 2 - 8, dSize + 60, height - dSize * 2 - 8);
            FCPoint[] points3 = new FCPoint[4];
            points3[0] = new FCPoint(dSize + 14, height - dSize2 - 8);
            points3[1] = new FCPoint(dSize + 18, height - dSize2 - 12);
            points3[2] = new FCPoint(dSize + 30, height - dSize2 - 12);
            points3[3] = new FCPoint(dSize + 34, height - dSize2 - 8);
            paint.fillPolygon(borderColor, points3);
            FCPoint[] points4 = new FCPoint[6];
            points4[0] = new FCPoint(dSize, height - dSize2 - 20);
            points4[1] = new FCPoint(dSize - 8, height - dSize2 - 10);
            points4[2] = new FCPoint(dSize - 8, height - 4 - dSize);
            points4[3] = new FCPoint(dSize + 16, height - 4 - dSize);
            points4[4] = new FCPoint(dSize + 20, height - dSize2);
            points4[5] = new FCPoint(dSize, height - dSize2);
            paint.fillPolygon(borderColor, points4);
            FCPoint[] points5 = new FCPoint[4];
            points5[0] = new FCPoint(width - dSize, height - dSize2 - dw2 - 20);
            points5[1] = new FCPoint(width - dSize - 4, height - dSize2 - dw2 - 16);
            points5[2] = new FCPoint(width - dSize - 4, height - dSize2 - dw2);
            points5[3] = new FCPoint(width - dSize, height - dSize2 - dw2);
            paint.fillPolygon(borderColor, points5);
            FCPoint[] points6 = new FCPoint[4];
            points6[0] = new FCPoint(width - dSize, height - dSize2 - dw2);
            points6[1] = new FCPoint(width - dSize - 4, height - dSize2 - dw2 - 2);
            points6[2] = new FCPoint(width - dSize - 22, height - dSize2 - dw2 + 16);
            points6[3] = new FCPoint(width - dSize - 20, height - dSize2 - dw2 + 22);
            paint.fillPolygon(borderColor, points6);
            FCPoint[] points7 = new FCPoint[3];
            points7[0] = new FCPoint(width - 1, height - dSize2 - dw2 - 30);
            points7[1] = new FCPoint(width - 1, height - dSize2 - dw2 + 3);
            points7[2] = new FCPoint(width - 1 - dw2 + 20, height - dSize2 - 16);
            paint.drawPolyline(borderColor, 2, 0, points7);
            FCPoint[] points8 = new FCPoint[3];
            points8[0] = new FCPoint(width - 1 - dw2 + 19, height - dSize2 - 15);
            points8[1] = new FCPoint(width - dSize - dw2 + 1, height - dSize + 3);
            points8[2] = new FCPoint(width - dSize - dw2 - 25, height - dSize + 3);
            paint.drawPolyline(borderColor, 2, 0, points8);
            FCPoint[] points9 = new FCPoint[6];
            points9[0] = new FCPoint(width - dSize - dw2 + 1, height - dSize + 3);
            points9[1] = new FCPoint(width - dSize - dw2 - 14, height - dSize + 3);
            points9[2] = new FCPoint(width - dSize - dw2 - 10, height - dSize + 8);
            points9[3] = new FCPoint(width - dSize - dw2 + 3, height - dSize + 8);
            points9[4] = new FCPoint(width - dSize - dw2 + 16, height - dSize - 6);
            points9[5] = new FCPoint(width - dSize - dw2 + 16, height - dSize - 14);
            paint.fillPolygon(borderColor, points9);

            int rwidth = width * 3 / 5;
            if (rwidth > width - dSize * 2 - dw2) {
                rwidth = width - dSize2 * 2 - dw2;
            }
            FCPoint[] points10 = new FCPoint[4];
            points10[0] = new FCPoint(rwidth - 8, height - dSize2 - 6);
            points10[1] = new FCPoint(rwidth - 12, height - dSize2);
            points10[2] = new FCPoint(rwidth + 12, height - dSize2);
            points10[3] = new FCPoint(rwidth + 8, height - dSize2 - 6);
            paint.fillPolygon(borderColor, points10);
            rwidth = width * 7 / 10;
            if (rwidth > width - 30) {
                rwidth = width - 30;
            }
            FCPoint[] points11 = new FCPoint[4];
            points11[0] = new FCPoint(rwidth - 16, dSize - 6);
            points11[1] = new FCPoint(rwidth - 20, dSize);
            points11[2] = new FCPoint(rwidth + 20, dSize);
            points11[3] = new FCPoint(rwidth + 16, dSize - 6);
            paint.fillPolygon(borderColor, points11);
            FCDraw.drawText(paint, Text, foreColor, Font, 100, 15);

            if (m_tick > 200) {
                int count = (m_tick - 200) / 8 + 1;
                int left = 80;
                for (int i = 0; i < count; i++) {
                    FCRect pRect = new FCRect(left, height - 10, left + 8, height - 6);
                    paint.fillRect(borderColor, pRect);
                    left += 10;
                }
                left = 15;
                int top = 80;
                count = (m_tick - 200) / 10 + 1;
                for (int i = 0; i < count; i++) {
                    FCRect pRect = new FCRect(left, top, left + 8, top + 6);
                    paint.fillRect(borderColor, pRect);
                    top += 10;
                }
            }
        }

        /// <summary>
        /// 秒表方法
        /// </summary>
        /// <param name="timerID">秒表ID</param>
        public override void onTimer(int timerID) {
            base.onTimer(timerID);
            if (m_useAnimation) {
                if (m_animateDirection == 0) {
                    m_tick += 3;
                    if (m_tick > 300) {
                        m_animateDirection = 1;
                    }
                } else if (m_animateDirection == 1) {
                    m_tick -= 3;
                    if (m_tick < 0) {
                        m_animateDirection = 0;
                    }
                }
                if (m_tick >= 190 && !IsDragging) {
                    invalidate();
                }
            }
        }

        /// <summary>
        /// 最大化或恢复
        /// </summary>
        public void max() {
            m_normalLocation = Location;
            m_normalSize = Size;
            Dock = FCDockStyle.Fill;
            m_windowState = WindowStateA.Max;
            FCPoint maxLocation = new FCPoint(0, 0);
            Location = maxLocation;
            FCSize maxSize = Native.DisplaySize;
            Size = maxSize;
            m_maxOrRestoreButton.Style = WindowButtonStyle.Restore;
            Native.update();
            Native.invalidate();
        }

        /// <summary>
        /// 最大化或恢复
        /// </summary>
        public void maxOrRestore() {
            if (m_windowState == WindowStateA.Normal) {
                max();
            } else {
                restore();
            }
        }

        /// <summary>
        /// 最小化
        /// </summary>
        public void min() {
            m_normalLocation = Location;
            m_normalSize = Size;
            Dock = FCDockStyle.None;
            m_windowState = WindowStateA.Min;
            m_maxOrRestoreButton.Style = WindowButtonStyle.Restore;
            FCSize minSize = new FCSize(150, CaptionHeight);
            Size = minSize;
            update();
            Native.invalidate();
        }

        /// <summary>
        /// 恢复
        /// </summary>
        public void restore() {
            Dock = FCDockStyle.None;
            m_windowState = WindowStateA.Normal;
            Location = m_normalLocation;
            Size = m_normalSize;
            m_maxOrRestoreButton.Style = WindowButtonStyle.Max;
            Native.update();
            Native.invalidate();
        }

        /// <summary>
        /// 设置属性
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="value">值</param>
        public override void setProperty(String name, String value) {
            if (name == "showmaxbox") {
                ShowMaxBox = FCStr.convertStrToBool(value);
            } else if (name == "showminbox") {
                ShowMinBox = FCStr.convertStrToBool(value);
            } else if (name == "windowstate") {
                String str = value.ToLower();
                if (str == "max") {
                    WindowState = WindowStateA.Max;
                } else if (str == "min") {
                    WindowState = WindowStateA.Min;
                } else if (str == "normal") {
                    WindowState = WindowStateA.Normal;
                }
            } else {
                base.setProperty(name, value);
            }
        }

        /// <summary>
        /// 布局改变方法
        /// </summary>
        public override void update() {
            base.update();
            int width = Width;
            if (m_closeButton != null) {
                FCPoint location = new FCPoint(width - 26, 2);
                m_closeButton.Location = location;
            }
            if (m_maxOrRestoreButton != null && m_maxOrRestoreButton.Visible) {
                FCPoint location = new FCPoint(width - 48, 2);
                m_maxOrRestoreButton.Location = location;
            }
            if (m_minButton != null && m_minButton.Visible) {
                FCPoint location = new FCPoint(width - 70, 2);
                if (m_maxOrRestoreButton != null && !m_maxOrRestoreButton.Visible) {
                    location.x = width - 48;
                }
                m_minButton.Location = location;
            }
        }
    }
}
