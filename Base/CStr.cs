/*基于FaceCat(捂脸猫)框架 v1.0 https://github.com/FaceCat007/facecat.git
 *  1.捂脸猫创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 *  2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 *  3.恒泰期货投资咨询总监/高级研究员-戴俊生(微信号:18345063201);
 *  4.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace FaceCat {
    public class FCStrEx {
        /// <summary>
        /// 获取证券代码的文件名称
        /// </summary>
        /// <param name="code">代码</param>
        /// <returns>文件名称</returns>
        public static String convertDBCodeToFileName(String code) {
            String fileName = code;
            if (fileName.IndexOf(".") != -1) {
                fileName = fileName.Substring(fileName.IndexOf('.') + 1) + fileName.Substring(0, fileName.IndexOf('.'));
            }
            fileName += ".txt";
            return fileName;
        }

        /// <summary>
        /// 将股票代码转化为成交代码
        /// </summary>
        /// <param name="code">股票代码</param>
        /// <returns>新浪代码</returns>
        public static String convertDBCodeToDealCode(String code) {
            String securityCode = code;
            int index = securityCode.IndexOf(".");
            if (index > 0) {
                securityCode = securityCode.Substring(0, index);
            }
            return securityCode;
        }

        /// <summary>
        /// 东财代码转化为代码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static String convertEMCodeToDBCode(String code) {
            return code.Substring(code.IndexOf(".") + 1) + code.Substring(0, code.IndexOf("."));
        }

        /// <summary>
        /// 将股票代码转化为新浪代码
        /// </summary>
        /// <param name="code">股票代码</param>
        /// <returns>新浪代码</returns>
        public static String convertDBCodeToSinaCode(String code) {
            String securityCode = code;
            int index = securityCode.IndexOf(".SH");
            if (index > 0) {
                securityCode = "sh" + securityCode.Substring(0, securityCode.IndexOf("."));
            }
            else {
                securityCode = "sz" + securityCode.Substring(0, securityCode.IndexOf("."));
            }
            return securityCode;
        }

        /// <summary>
        /// 将文本文件中的股票代码转换成内存中的股票代码
        /// </summary>
        /// <param name="code">文件中的股票代码</param>
        /// <returns>内存中的股票代码</returns>
        public static String convertFileCodeToMemoryCode(String code) {
            int a = (code.IndexOf("."));
            return code.Substring(code.IndexOf(".") + 1, 2) + code.Substring(0, code.IndexOf(".")).ToLower();
        }

        /// <summary>
        /// 将新浪代码转化为股票代码
        /// </summary>
        /// <param name="code">新浪代码</param>
        /// <returns>股票代码</returns>
        public static String convertSinaCodeToDBCode(String code) {
            int equalIndex = code.IndexOf('=');
            int startIndex = code.IndexOf("var hq_str_") + 11;
            String securityCode = equalIndex > 0 ? code.Substring(startIndex, equalIndex - startIndex) : code;
            securityCode = securityCode.Substring(2) + "." + securityCode.Substring(0, 2).ToUpper();
            return securityCode;
        }

        /// <summary>
        /// 获取数据库转义字符串
        /// </summary>
        /// <param name="str">字符串</param>
        /// <returns>转义字符串</returns>
        public static String getDBString(String str) {
            return str.Replace("'", "''");
        }
    }
}
