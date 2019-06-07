/*基于FaceCat(捂脸猫)框架 v1.0 https://github.com/FaceCat007/facecat.git
 *  1.捂脸猫创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 *  2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 *  3.恒泰期货投资咨询总监/高级研究员-戴俊生(微信号:18345063201);
 *  4.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace FaceCat {
    /// <summary>
    /// 选股的外部方法
    /// </summary>
    public class SecurityFilterExternFunc {
        /// <summary>
        /// 股票控件
        /// </summary>
        private static FCChart m_chart;

        /// <summary>
        /// 方法库
        /// </summary>
        private static FCNative m_native;

        /// <summary>
        /// 流水号
        /// </summary>
        private static int m_serialNumber;

        /// <summary>
        /// 指标集合
        /// </summary>
        private static Dictionary<int, FCScript> m_indicators = new Dictionary<int, FCScript>();

        /// <summary>
        /// 创建指标
        /// </summary>
        /// <param name="text">脚本</param>
        /// <param name="parameters">参数</param>
        /// <returns>指标ID</returns>
        public static int createIndicatorExtern(String text, String parameters, StringBuilder fields) {
            try {
                if (m_native == null) {
                    m_native = new FCNative();
                }
                if (m_chart == null) {
                    m_chart = new FCChart();
                    m_chart.Native = m_native;
                }
                m_serialNumber++;
                FCDataTable dataSource = new FCDataTable();
                dataSource.addColumn(KeyFields.CLOSE_INDEX);
                dataSource.addColumn(KeyFields.HIGH_INDEX);
                dataSource.addColumn(KeyFields.LOW_INDEX);
                dataSource.addColumn(KeyFields.OPEN_INDEX);
                dataSource.addColumn(KeyFields.VOL_INDEX);
                dataSource.addColumn(KeyFields.AMOUNT_INDEX);
                FCScript indicator = SecurityDataHelper.createIndicator(m_chart, dataSource, text, parameters);
                m_indicators[m_serialNumber] = indicator;
                indicator.onCalculate(0);
                int pos = 0;
                int variablesSize = indicator.MainVariables.Count;
                foreach (String field in indicator.MainVariables.Keys) {
                    fields.Append(field);
                    if (pos != variablesSize - 1) {
                        fields.Append(",");
                    }
                    pos++;
                }
            } catch (Exception ex) {
                Console.WriteLine(ex.Message + "\r\n" + ex.StackTrace);
            }
            return m_serialNumber;
        }

        /// <summary>
        /// 计算指标
        /// </summary>
        /// <param name="id">指标ID</param>
        /// <param name="code">代码</param>
        /// <param name="path">路径</param>
        /// <param name="type">类型</param>
        /// <param name="cycle">周期</param>
        /// <param name="subscription">复权方式</param>
        /// <param name="date">日期</param>
        /// <param name="open">开盘价</param>
        /// <param name="high">最高价</param>
        /// <param name="low">最低价</param>
        /// <param name="close">收盘价</param>
        /// <param name="volume">成交量</param>
        /// <param name="amount">成交额</param>
        /// <returns>返回数据</returns>
        public static double[] calculateIndicatorExtern(int id, String code, ref double result) {
            if (m_indicators.ContainsKey(id)) {
                FCScript indicator = m_indicators[id];
                List<FCScript> indicators = new List<FCScript>();
                indicators.Add(indicator);
                List<SecurityData> datas = new List<SecurityData>();
                if (SecurityService.m_historyDatas.ContainsKey(code)) {
                    datas = SecurityService.m_historyDatas[code];
                    SecurityLatestData latestData = null;
                    if (SecurityService.m_latestDatas.ContainsKey(code)) {
                        latestData = SecurityService.m_latestDatas[code];
                    }
                    if (latestData != null) {
                        SecurityData newData = new SecurityData();
                        getSecurityData(latestData, latestData.m_lastClose, 1440, 0, ref newData);
                        if (datas.Count == 0) {
                            datas.Add(newData);
                        } else {
                            if (newData.m_date > datas[datas.Count - 1].m_date) {
                                datas.Add(newData);
                            } else {
                                datas[datas.Count - 1] = newData;
                            }
                        }
                    }
                    FCDataTable dataSource = indicator.DataSource;
                    int[] fields = new int[] { KeyFields.CLOSE_INDEX, KeyFields.HIGH_INDEX, KeyFields.LOW_INDEX, KeyFields.OPEN_INDEX, KeyFields.VOL_INDEX, KeyFields.AMOUNT_INDEX };
                    SecurityDataHelper.bindHistoryDatas(m_chart, dataSource, indicators, fields, datas);;
                    int rowsCount = dataSource.RowsCount;
                    int variablesSize = indicator.MainVariables.Count;
                    double[] list = new double[variablesSize];
                    if (rowsCount > 0) {
                        int pos = 0;
                        foreach (String name in indicator.MainVariables.Keys) {
                            int field = indicator.MainVariables[name];
                            double value = dataSource.get2(rowsCount - 1, field);
                            list[pos] = value;
                            pos++;
                        }
                    }
                    result = indicator.m_result;
                    dataSource.clear();
                    return list;
                }
            } 
            return null;
        }

        /// <summary>
        /// 删除指标
        /// </summary>
        /// <param name="id">指标ID</param>
        public static void deleteIndicatorExtern(int id) {
            if (m_indicators.ContainsKey(id)) {
                FCScript indicator = m_indicators[id];
                m_indicators.Remove(id);
                indicator.clear();
                indicator.DataSource.delete();
                indicator.DataSource = null;
                indicator.delete();
            }
        }

        /// <summary>
        /// 获取切分后的日期
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="divide">切分值</param>
        /// <returns></returns>
        public static double getDivideDate(double date, long divide) {
            return (double)((long)date / divide);
        }

        /// <summary>
        /// 获取股票历史数据
        /// </summary>
        /// <param name="latestData">最新数据</param>
        /// <param name="lastClose">上一期收盘价</param>
        /// <param name="cycle">周期</param>
        /// <param name="subscription">复权模式</param>
        /// <param name="securityData">历史数据</param>
        /// <returns>历史数据</returns>
        public static void getSecurityData(SecurityLatestData latestData, double lastClose, int cycle, int subscription, ref SecurityData securityData) {
            if (cycle <= 60) {
                securityData.m_date = getDivideDate(latestData.m_date, 60 * 60);
            } else {
                securityData.m_date = (long)latestData.m_date / (3600 * 24) * (3600 * 24);
            }
            //前复权计算
            double factor = 1;
            if (lastClose > 0 && latestData.m_lastClose > 0 && subscription == 2) {
                factor = lastClose / latestData.m_lastClose;
            }
            securityData.m_close = latestData.m_close * factor;
            securityData.m_high = latestData.m_high * factor;
            securityData.m_low = latestData.m_low * factor;
            securityData.m_open = latestData.m_open * factor;
            securityData.m_volume = latestData.m_volume;
            securityData.m_amount = latestData.m_amount;
        }
    }
}
