/*基于FaceCat(捂脸猫)框架 v1.0 https://github.com/FaceCat007/facecat.git
 *  1.捂脸猫创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 *  2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 *  3.恒泰期货投资咨询总监/高级研究员-戴俊生(微信号:18345063201)
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Media;
using FaceCat;
using System.Threading;
using System.Runtime.InteropServices;

namespace FaceCat {
    /// <summary>
    /// 声音类
    /// </summary>
    public class Sound {
        /// <summary>
        /// 向媒体控制接口发送控制命令
        /// </summary>
        /// <param name="lpszCommand">命令，参见
        /// http://msdn.microsoft.com/en-us/library/windows/desktop/dd743572(v=vs.85).aspx </param>
        /// <param name="lpszReturnString">命令返回的信息，如果没有需要返回的信息可以为null</param>
        /// <param name="cchReturn">指定返回信息的字符串大小</param>
        /// <param name="hwndCallback">回调句柄，如果命令参数中没有指定notify标识，可以为new IntPtr(0)</param>
        /// <returns>返回命令执行状态的错误代码</returns>
        [DllImport("winmm.dll")]
        static extern Int32 mciSendString(String lpszCommand, StringBuilder returnString, int bufferSize, IntPtr hwndCallback);
        /// <summary>
        /// 返回对执行状态错误代码的描述
        /// </summary>
        /// <param name="errorCode">mciSendCommand或者mciSendString返回的错误代码</param>
        /// <param name="errorText">对错误代码的描述字符串</param>
        /// <param name="errorTextSize">指定字符串的大小</param>
        /// <returns>如果ERROR Code未知，返回false</returns>
        [DllImport("winmm.dll")]
        static extern bool mciGetErrorString(Int32 errorCode, StringBuilder errorText, Int32 errorTextSize);

        private static Dictionary<String, String> m_plays = new Dictionary<String, String>();

        /// <summary>
        /// 开始播放声音
        /// </summary>
        /// <param name="args">参数</param>
        private static void startPlay(object args) {
            String fileName = Application.StartupPath + "\\config\\" + args.ToString();
            if (FCFile.isFileExist(fileName)) {
                try {
                    int error = mciSendString("open " + fileName, null, 0, new IntPtr(0));
                    if (error == 0) {
                        mciSendString("play " + fileName, null, 0, new IntPtr(0));
                        Thread.Sleep(3000);
                        mciSendString("stop " + fileName, null, 0, new IntPtr(0));
                        mciSendString("close " + fileName, null, 0, new IntPtr(0));
                    }
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.Message + "\r\n" + ex.StackTrace);
                }
            }
            lock (m_plays) {
                if (m_plays.ContainsKey(args.ToString())) {
                    m_plays.Remove(args.ToString());
                }
            }
        }

        /// <summary>
        /// 播放
        /// </summary>
        /// <param name="key">文件名</param>
        public static void play(String key) {
            lock (m_plays) {
                if (!m_plays.ContainsKey(key)) {
                    m_plays[key] = "";
                    Thread thread = new Thread(new ParameterizedThreadStart(startPlay));
                    thread.Start(key);
                }
            }
        }
    }
}
