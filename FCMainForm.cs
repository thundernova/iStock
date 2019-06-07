/*基于FaceCat(捂脸猫)框架 v1.0 https://github.com/FaceCat007/facecat.git
 *  1.捂脸猫创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 *  2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 *  3.恒泰期货投资咨询总监/高级研究员-戴俊生(微信号:18345063201);
 *  4.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using FaceCat;

namespace FaceCat {
    /// <summary>
    /// 窗体
    /// </summary>
    public partial class FCMainForm : Form {
        /// <summary>
        ///  创建图形控件
        /// </summary>
        public FCMainForm() {
            InitializeComponent();
        }

        /// <summary>
        /// 控件管理器
        /// </summary>
        private WinHost m_host;

        /// <summary>
        /// 控件库
        /// </summary>
        private FCNative m_native;

        /// <summary>
        /// 计时器
        /// </summary>
        private int m_tick = 60;

        /// <summary>
        /// XML
        /// </summary>
        private UIXmlEx m_xml;

        /// <summary>
        /// 获取客户端尺寸
        /// </summary>
        /// <returns>客户端尺寸</returns>
        public FCSize GetClientSize() {
            return new FCSize(ClientSize.Width, ClientSize.Height);
        }

        /// <summary>
        /// 加载
        /// </summary>
        /// <param name="name">名称</param>
        public void loadXml(String name) {
            if (name == "MainFrame") {
                m_xml = new MainFrame();
            }
            m_xml.createNative();
            m_native = m_xml.Native;
            m_native.Paint = new GdiPlusPaintEx();
            m_host = new WinHostEx();
            m_host.Native = m_native;
            m_native.Host = m_host;
            m_host.HWnd = Handle;
            m_native.AllowScaleSize = true;
            m_native.DisplaySize = new FCSize(ClientSize.Width, ClientSize.Height);
            m_xml.resetScaleSize(GetClientSize());
            m_xml.Script = new FaceCatScript(m_xml);
            m_xml.Native.ResourcePath = DataCenter.getAppPath() + "\\config";
            m_xml.load(DataCenter.getAppPath() + "\\config\\" + name + ".html");
            m_host.ToolTip = new FCToolTip();
            m_host.ToolTip.Font = new FCFont("SimSun", 20, true, false, false);
            (m_host.ToolTip as FCToolTip).InitialDelay = 250;
            m_native.update();
            Invalidate();
        }

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnFormClosing(FormClosingEventArgs e) {
            m_xml.exit();
            Environment.Exit(0);
            base.OnFormClosing(e);
        }

        /// <summary>
        /// 键盘事件
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnKeyDown(KeyEventArgs e) {
            base.OnKeyDown(e);
            m_tick = 60;
        }

        /// <summary>
        /// 鼠标事件
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnMouseDown(MouseEventArgs e) {
            base.OnMouseDown(e);
            m_tick = 60;
        }

        /// <summary>
        /// 尺寸改变方法
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnSizeChanged(EventArgs e) {
            base.OnSizeChanged(e);
            if (m_host != null) {
                m_xml.resetScaleSize(GetClientSize());
                Invalidate();
            }
        }

        /// <summary>
        /// 鼠标滚动方法
        /// </summary>
        /// <param name="e">参数</param>
        protected override void OnMouseWheel(MouseEventArgs e) {
            base.OnMouseWheel(e);
            if (m_host != null) {
                if (m_host.isKeyPress(0x11)) {
                    double scaleFactor = m_xml.ScaleFactor;
                    if (e.Delta > 0) {
                        if (scaleFactor > 0.2) {
                            scaleFactor -= 0.1;
                        }
                    }
                    else if (e.Delta < 0) {
                        if (scaleFactor < 10) {
                            scaleFactor += 0.1;
                        }
                    }
                    m_xml.ScaleFactor = scaleFactor;
                    m_xml.resetScaleSize(GetClientSize());
                    Invalidate();
                }
            }
        }

        /// <summary>
        /// 秒表事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="e">参数</param>
        private void timer_Tick(object sender, EventArgs e) {
            m_tick--;
            if (m_tick <= 0) {
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// 消息监听
        /// </summary>
        /// <param name="m"></param>
        protected override void WndProc(ref Message m) {
            if (m.Msg == 0x100 || m.Msg == 260) {
                if (m_native != null) {
                    char key = (char)m.WParam;
                    if (m_xml is MainFrame) {
                        (m_xml as MainFrame).showSearchDiv(key);
                    }
                }
            }
            if (m_host != null) {
                if (m_host.onMessage(ref m) > 0) {
                    return;
                }
            }
            base.WndProc(ref m);
        }
    }
}
