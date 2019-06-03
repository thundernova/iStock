/*基于FaceCat(捂脸猫)框架 v1.0 https://github.com/FaceCat007/facecat.git
 *  1.捂脸猫创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 *  2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 *  3.恒泰期货投资咨询总监/高级研究员-戴俊生(微信号:18345063201)
 */

using System;
using System.Collections.Generic;
using System.Text;

namespace FaceCat {
    /// <summary>
    /// 透明按钮
    /// </summary>
    public class RibbonButton : FCButton {
        /// <summary>
        /// 创建透明按钮
        /// </summary>
        public RibbonButton() {
            BackColor = FCColor.None;
            BorderColor = FCColor.None;
            Font = new FCFont("微软雅黑", 12, false, false, false);
        }

        private int m_angle = 90;

        /// <summary>
        /// 获取或设置渐变角度
        /// </summary>
        public int Angle {
            get { return m_angle; }
            set { m_angle = value; }
        }

        private int m_arrowType;

        /// <summary>
        /// 获取或设置箭头类型
        /// </summary>
        public int ArrowType {
            get { return m_arrowType; }
            set { m_arrowType = value; }
        }

        private bool m_isClose;

        /// <summary>
        /// 获取或设置是否是关闭按钮
        /// </summary>
        public bool IsClose {
            get { return m_isClose; }
            set { m_isClose = value; }
        }

        /// <summary>
        /// 获取或设置是否选中
        /// </summary>
        public bool Selected {
            get {
                FCView parent = Parent;
                if (parent != null) {
                    FCTabControl tabControl = parent as FCTabControl;
                    if (tabControl != null) {
                        FCTabPage selectedTabPage = tabControl.SelectedTabPage;
                        if (selectedTabPage != null) {
                            if (this == selectedTabPage.HeaderButton) {
                                return true;
                            }
                        }
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// 获取要绘制的前景色
        /// </summary>
        /// <returns>前景色</returns>
        protected override long getPaintingTextColor() {
            if (Enabled) {
                if (Selected) {
                    return FCDraw.FCCOLORS_FORECOLOR4;
                }
                else {
                    return FCDraw.FCCOLORS_FORECOLOR;
                }
            }
            else {
                return FCDraw.FCCOLORS_FORECOLOR2;
            }
        }

        /// <summary>
        /// 重绘背景
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public override void onPaintBackground(FCPaint paint, FCRect clipRect) {
            FCNative native = Native;
            int width = Width, height = Height;
            int mw = width / 2, mh = height / 2;
            FCRect drawRect = new FCRect(0, 0, width, height);
            int cornerRadius = 0;
            if (m_isClose) {
                long lineColor = FCDraw.FCCOLORS_LINECOLOR;
                FCRect ellipseRect = new FCRect(1, 1, width - 2, height - 2);
                //paint->FillEllipse(FCCOLORS_BACKCOLOR7, ellipseRect);
                paint.drawLine(lineColor, 2, 0, 4, 4, width - 7, height - 7);
                paint.drawLine(lineColor, 2, 0, 4, height - 7, width - 7, 3);
            }
            else {
                cornerRadius = 0;
                if (m_arrowType > 0) {
                    cornerRadius = 0;
                }
                FCView parent = Parent;
                if (parent != null) {
                    FCTabControl tabControl = parent as FCTabControl;
                    if (tabControl != null) {
                        cornerRadius = 0;
                    }
                }
                paint.fillGradientRect(FCDraw.FCCOLORS_BACKCOLOR, FCDraw.FCCOLORS_BACKCOLOR2, drawRect, cornerRadius, 90);
                paint.drawRect(FCDraw.FCCOLORS_EXCOLOR1, 1, 0, drawRect);
                cornerRadius = 0;
                if (m_arrowType > 0) {
                    FCPoint point1 = new FCPoint();
                    FCPoint point2 = new FCPoint();
                    FCPoint point3 = new FCPoint();
                    int dSize = Math.Min(mw, mh) / 2;
                    switch (m_arrowType) {
                        case 1:
                            point1.x = mw - dSize;
                            point1.y = mh;
                            point2.x = mw + dSize;
                            point2.y = mh - dSize;
                            point3.x = mw + dSize;
                            point3.y = mh + dSize;
                            break;
                        case 2:
                            point1.x = mw + dSize;
                            point1.y = mh;
                            point2.x = mw - dSize;
                            point2.y = mh - dSize;
                            point3.x = mw - dSize;
                            point3.y = mh + dSize;
                            break;
                        case 3:
                            point1.x = mw;
                            point1.y = mh - dSize;
                            point2.x = mw - dSize;
                            point2.y = mh + dSize;
                            point3.x = mw + dSize;
                            point3.y = mh + dSize;
                            break;
                        case 4:
                            point1.x = mw;
                            point1.y = mh + dSize;
                            point2.x = mw - dSize;
                            point2.y = mh - dSize;
                            point3.x = mw + dSize;
                            point3.y = mh - dSize;
                            break;
                    }
                    FCPoint[] points = new FCPoint[3];
                    points[0] = point1;
                    points[1] = point2;
                    points[2] = point3;
                    paint.fillPolygon(FCDraw.FCCOLORS_FORECOLOR, points);
                }
            }
            bool state = false;
            if (Selected) {
                state = true;
                paint.fillRoundRect(FCDraw.FCCOLORS_BACKCOLOR8, drawRect, cornerRadius);
            }
            else if (this == native.PushedControl) {
                state = true;
                paint.fillRoundRect(FCDraw.FCCOLORS_BACKCOLOR6, drawRect, cornerRadius);
            }
            else if (this == native.HoveredControl) {
                state = true;
                paint.fillRoundRect(FCDraw.FCCOLORS_BACKCOLOR5, drawRect, cornerRadius);
            }
            if (state) {
                if (cornerRadius > 0) {
                    paint.drawRoundRect(FCColor.Border, 2, 0, drawRect, cornerRadius);
                }
                else {
                    paint.drawRect(FCColor.Border, 1, 0, drawRect);
                }
            }
        }
    }
}
