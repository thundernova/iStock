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
using System.IO;
using System.Net;
using System.Threading;
using System.Xml;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;

namespace FaceCat {
    /// <summary>
    /// 处理行情数据
    /// </summary>
    public class DataCenter {
        private static ExportService m_exportService;

        /// <summary>
        /// 获取导出服务
        /// </summary>
        public static ExportService ExportService {
            get { return m_exportService; }
        }

        private static bool m_isAppAlive = true;

        /// <summary>
        /// 获取或设置程序是否存活
        /// </summary>
        public static bool IsAppAlive {
            get { return DataCenter.m_isAppAlive; }
            set { DataCenter.m_isAppAlive = value; }
        }

        /// <summary>
        /// 画线工具
        /// </summary>
        private static Dictionary<String, String> m_plots = new Dictionary<String, String>();

        /// <summary>
        /// 获取画线工具
        /// </summary>
        public static Dictionary<String, String> Plots {
            get { return m_plots; }
        }

        private static UserCookieService m_userCookieService;

        /// <summary>
        /// 用户Cookie服务
        /// </summary>
        public static UserCookieService UserCookieService {
            get { return DataCenter.m_userCookieService; }
        }

        private static UserSecurityService m_userSecurityService;

        /// <summary>
        /// 获取自选股服务
        /// </summary>
        public static UserSecurityService UserSecurityService {
            get { return DataCenter.m_userSecurityService; }
        }

        static DataCenter() {
        }

        private static MainFrame m_mainUI;

        /// <summary>
        /// 获取或设置主界面
        /// </summary>
        public static MainFrame MainUI {
            get { return DataCenter.m_mainUI; }
            set { DataCenter.m_mainUI = value; }
        }

        /// <summary>
        /// 获取程序路径
        /// </summary>
        /// <returns>程序路径</returns>
        public static String getAppPath() {
            return Application.StartupPath;
        }

        /// <summary>
        /// 获取用户目录
        /// </summary>
        /// <returns>用户目录</returns>
        public static String getUserPath() {
            return Application.StartupPath;
        }

        /// <summary>
        /// 读取所有的画线工具
        /// </summary>
        private static void readPlots() {
            String xmlPath = Path.Combine(getAppPath(), "config\\Plots.xml");
            m_plots.Clear();
            if (File.Exists(xmlPath)) {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(xmlPath);
                XmlNode rootNode = xmlDoc.DocumentElement;
                foreach (XmlNode node in rootNode.ChildNodes) {
                    if (node.Name.ToUpper() == "PLOT") {
                        String name = String.Empty;
                        String text = String.Empty;
                        foreach (XmlNode childeNode in node.ChildNodes) {
                            if (childeNode.Name.ToUpper() == "NAME") {
                                name = childeNode.InnerText;
                            }
                            else if (childeNode.Name.ToUpper() == "TEXT") {
                                text = childeNode.InnerText;
                            }
                        }
                        m_plots[name] = text;
                    }
                }
            }
        }

        /// <summary>
        /// 启动服务
        /// </summary>
        /// <param name="fileName">文件名</param>
        public static void startService() {
            readPlots();
            m_userCookieService = new UserCookieService();
            m_exportService = new ExportService();
            m_userSecurityService = new UserSecurityService();
            SecurityService.start();
        }
    }
}
