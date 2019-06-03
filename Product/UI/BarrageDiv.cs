/*基于FaceCat(捂脸猫)框架 v1.0 https://github.com/FaceCat007/facecat.git
 *  1.捂脸猫创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 *  2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 *  3.恒泰期货投资咨询总监/高级研究员-戴俊生(微信号:18345063201)
 */

using System;
using System.Collections.Generic;
using System.Text;
using FaceCat;
using System.Drawing;

namespace FaceCat
{
    /// <summary>
    /// 弹幕对象
    /// </summary>
    public class Barrage
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public Barrage()
        {

        }

        private long m_color;

        /// <summary>
        /// 获取或设置颜色
        /// </summary>
        public long Color
        {
            get { return m_color; }
            set { m_color = value; }
        }

        private FCFont m_font = new FCFont("SimSun", 40, true, false, false);

        /// <summary>
        /// 获取或设置字体
        /// </summary>
        public FCFont Font
        {
            get { return m_font; }
            set { m_font = value; }
        }

        private int m_mode;

        /// <summary>
        /// 获取或设置模式
        /// </summary>
        public int Mode
        {
            get { return m_mode; }
            set { m_mode = value; }
        }

        private FCRect m_rect;

        /// <summary>
        /// 获取或设置范围
        /// </summary>
        public FCRect Rect
        {
            get { return m_rect; }
            set { m_rect = value; }
        }

        private int m_speed = 10;

        /// <summary>
        /// 获取或设置速度
        /// </summary>
        public int Speed 
        {
            get { return m_speed; }
            set { m_speed = value; }
        }

        private int m_tick = 200;

        /// <summary>
        /// 获取或设置倒计时读数
        /// </summary>
        public int Tick
        {
            get { return m_tick; }
            set { m_tick = value; }
        }

        private String m_text;

        /// <summary>
        /// 获取或设置文字
        /// </summary>
        public String Text
        {
            get { return m_text; }
            set { m_text = value; }
        }

        /// <summary>
        /// 计算位置
        /// </summary>
        public void calculate()
        {
            m_rect.left -= m_speed;
            m_rect.right -= m_speed;         
        }
    }

    /// <summary>
    /// 弹幕系统
    /// </summary>
    public class BarrageDiv : FCView
    {
        /// <summary>
        /// 创建弹幕系统
        /// </summary>
        public BarrageDiv()
        {
            BackColor = FCColor.None;
        }

        /// <summary>
        /// 弹幕列表
        /// </summary>
        private List<Barrage> m_barrages = new List<Barrage>();

        /// <summary>
        /// 系统颜色
        /// </summary>
        private long[] m_sysColors = new long[] { FCColor.argb(255, 255, 255), FCColor.argb(255,255,0), FCColor.argb(255, 0, 255),
            FCColor.argb(0, 255, 0), FCColor.argb(82, 255, 255), FCColor.argb(255, 82, 82) };

        /// <summary>
        /// 随机种子
        /// </summary>
        private Random m_rd = new Random();

        /// <summary>
        /// 计数
        /// </summary>
        private int m_tick;

        /// <summary>
        /// 秒表ID
        /// </summary>
        private int m_timerID = FCView.getNewTimerID();

        private MainFrame m_mainFrame;

        /// <summary>
        /// 获取或设置主框架
        /// </summary>
        public MainFrame MainFrame {
            get { return m_mainFrame; }
            set { m_mainFrame = value; }
        }

        private int m_progress = -1;

        /// <summary>
        /// 获取进度
        /// </summary>
        public int Progress {
            get { return m_progress; }
            set { m_progress = value; }
        }

        private String m_progressText = "";

        /// <summary>
        /// 获取或设置进度文字
        /// </summary>
        public String ProgressText {
            get { return m_progressText; }
            set { m_progressText = value; }
        }

        /// <summary>
        /// 启动弹幕
        /// </summary>
        /// <param name="barrage">弹幕参数</param>
        public void addBarrage(Barrage barrage)
        {
            barrage.Color = m_sysColors[m_tick % 6];
            int width = Width, height = Height;
            if (width < 100)
            {
                width = 100;
            }
            if (height < 100)
            {
                height = 100;
            }
            int mode = barrage.Mode;
            if (mode == 0)
            {
                barrage.Rect = new FCRect(width, m_rd.Next(0, height), width, 0);
            }
            else
            {
                int left = 0, top = 0;
                if (width > 200)
                {
                    left = m_rd.Next(0, width - 200);
                }
                if (height > 200)
                {
                    top = m_rd.Next(0, height - 200);
                }
                barrage.Rect = new FCRect(left, top, left, 0);
            }
            lock (m_barrages)
            {
                m_barrages.Add(barrage);
            }
            m_tick++;
        }

        /// <summary>
        /// 是否包含坐标
        /// </summary>
        /// <param name="point">坐标</param>
        /// <returns>是否包含</returns>
        public override bool containsPoint(FCPoint point)
        {
            return false;
        }

        /// <summary>
        /// 销毁资源方法
        /// </summary>
        public override void delete()
        {
            if (!IsDeleted)
            {
                stopTimer(m_timerID);
                lock (m_barrages)
                {
                    m_barrages.Clear();
                }
            }
            base.delete();
        }

        /// <summary>
        /// 控件加载方法
        /// </summary>
        public override void onLoad()
        {
            base.onLoad();
            startTimer(m_timerID, 10);
        }

        /// <summary>
        /// 重绘前景方法
        /// </summary>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        public override void onPaintBackground(FCPaint paint, FCRect clipRect) {
            base.onPaintBackground(paint, clipRect);
            int width = Width, height = Height;
            lock (m_barrages) {
                int barragesSize = m_barrages.Count;
                for (int i = 0; i < barragesSize; i++) {
                    Barrage brg = m_barrages[i];
                    FCFont font = brg.Font;
                    FCRect rect = brg.Rect;
                    String str = brg.Text;
                    FCSize size = paint.textSize(str, font);
                    rect.right = rect.left + size.cx;
                    rect.bottom = rect.top + size.cy;
                    brg.Rect = rect;
                    long color = brg.Color;
                    int mode = brg.Mode;
                    if (mode == 1) {
                        int a = 0, r = 0, g = 0, b = 0;
                        FCColor.toArgb(null, color, ref a, ref r, ref g, ref b);
                        a = a * brg.Tick / 400;
                        color = FCColor.argb(a, r, g, b);
                    }
                    paint.drawText(str, color, font, rect);
                }
            }
            if (m_progress > 0) {
                int tWidth = 500;
                int tHeight = 60;
                int showWidth = m_progress * 500 / 100;
                FCRect pRect = new FCRect((width - tWidth) / 2, (height - tHeight) / 2, (width + tWidth) / 2, (height + tHeight) / 2);
                paint.fillRect(FCColor.argb(255, 0, 0), pRect);
                FCRect pRect2 = new FCRect((width - tWidth) / 2, (height - tHeight) / 2, (width - tWidth) / 2 + showWidth, (height + tHeight) / 2);
                paint.fillRect(FCColor.argb(255, 255, 0), pRect2);
                paint.drawRect(FCColor.argb(255, 255, 255), 1, 0, pRect);
                FCFont pFont = new FCFont("微软雅黑", 20, true, false, false);
                FCSize pSize = paint.textSize(m_progressText, pFont);
                FCDraw.drawText(paint, m_progressText, FCColor.argb(0, 0, 0), pFont, (width - pSize.cx) / 2, (height - pSize.cy) / 2);
            }
        }

        /// <summary>
        /// 秒表方法
        /// </summary>
        /// <param name="timerID">秒表ID</param>
        public override void onTimer(int timerID)
        {
            base.onTimer(timerID);
            if (m_timerID == timerID)
            {
                bool paint = false;
                lock (m_barrages)
                {
                    int barragesSize = m_barrages.Count;
                    if (barragesSize > 0)
                    {
                        int width = Width, height = Height;
                        for (int i = 0; i < barragesSize; i++)
                        {
                            Barrage brg = m_barrages[i];
                            int mode = brg.Mode;
                            if (mode == 0)
                            {
                                if (brg.Rect.right < 0)
                                {
                                    m_barrages.Remove(brg);
                                    i--;
                                    barragesSize--;
                                }
                                else
                                {
                                    brg.calculate();
                                }
                                paint = true;
                            }
                            else if (mode == 1)
                            {
                                int tick = brg.Tick;
                                tick--;
                                if (tick <= 0)
                                {
                                    m_barrages.Remove(brg);
                                    i--;
                                    barragesSize--;
                                }
                                else
                                {
                                    brg.Tick = tick;
                                }
                                if (tick % 20 == 0)
                                {
                                    paint = true;
                                }
                            }
                        }
                    }
                    if (m_mainFrame != null) {
                        m_mainFrame.onTimer();
                    }
                }
                if (paint)
                {
                    invalidate();
                }
            }
        }
    }
}
