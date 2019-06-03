/*基于FaceCat(捂脸猫)框架 v1.0 https://github.com/FaceCat007/facecat.git
 *  1.捂脸猫创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 *  2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 *  3.恒泰期货投资咨询总监/高级研究员-戴俊生(微信号:18345063201)
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Net;
using System.IO;

namespace FaceCat {
    /// <summary>
    /// 提示方法
    /// </summary>
    public class NFunctionEx : CFunction {
        /// <summary>
        /// 创建方法
        /// </summary>
        /// <param name="indicator">指标</param>
        /// <param name="id">ID</param>
        /// <param name="name">名称</param>
        /// <param name="withParameters">是否有参数</param>
        public NFunctionEx(FCScript indicator, int id, String name, FCUIXml xml) {
            m_indicator = indicator;
            m_ID = id;
            m_name = name;
            m_xml = xml;
        }

        /// <summary>
        /// 指标
        /// </summary>
        public FCScript m_indicator;

        /// <summary>
        /// XML对象
        /// </summary>
        public FCUIXml m_xml;

        /// <summary>
        /// 方法字段
        /// </summary>
        private const String FUNCTIONS = "";

        /// <summary>
        /// 开始索引
        /// </summary>
        private const int STARTINDEX = 1000000;

        /// <summary>
        /// 计算
        /// </summary>
        /// <param name="var">变量</param>
        /// <returns>结果</returns>
        public override double onCalculate(CVariable var) {
            switch (var.m_functionID) {
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 创建指标
        /// </summary>
        /// <param name="id">编号</param>
        /// <param name="script">脚本</param>
        /// <param name="xml">XML</param>
        /// <returns>指标</returns>
        public static FCScript createIndicator(String id, String script, FCUIXml xml) {
            FCScript indicator = new FCScript();
            indicator.Name = id;
            FCDataTable table = new FCDataTable();
            indicator.DataSource = table;
            NFunctionBase.addFunctions(indicator);
            NFunctionUI.addFunctions(indicator, xml);
            NFunctionWin.addFunctions(indicator);
            int index = STARTINDEX;
            String[] functions = FUNCTIONS.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            int functionsSize = functions.Length;
            for (int i = 0; i < functionsSize; i++) {
                indicator.addFunction(new NFunctionEx(indicator, index + i, functions[i], xml));
            }
            indicator.Script = script;
            table.addColumn(0);
            table.set(0, 0, 0);
            indicator.onCalculate(0);
            return indicator;
        }
    }
}
