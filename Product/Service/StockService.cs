/*基于FaceCat(捂脸猫)框架 v1.0 https://github.com/FaceCat007/facecat.git
 *  1.捂脸猫创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 *  2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 *  3.恒泰期货投资咨询总监/高级研究员-戴俊生(微信号:18345063201)
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Diagnostics;
using FaceCat;

namespace FaceCat {
    /// <summary>
    /// 股票服务
    /// </summary>
    public class StockService {
        /// <summary>
        /// 获取指定年月日的星期
        /// </summary>
        /// <param name="year">年</param>
        /// <param name="month">月</param>
        /// <param name="day">日</param>
        /// <returns>星期</returns>
        public static int dayOfWeek(int y, int m, int d) {
            if (m == 1 || m == 2) {
                m += 12;
                y--;
            }
            return (d + 2 * m + 3 * (m + 1) / 5 + y + y / 4 - y / 100 + y / 400) % 7;
        }

        /// <summary>
        /// 获取分时线的历史数据
        /// </summary>
        /// <param name="str">数据字符串</param>
        /// <param name="datas">历史数据</param>
        /// <returns>状态</returns>
        public static int getHistoryDatasByMinuteStr(String str, List<SecurityData> datas) {
            String[] strs = str.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            int strLen = strs.Length;
            double lClose = 0, lHigh = 0, lLow = 0, lOpen = 0;
            double lVolume = 0, lAmount = 0, fVolume = 0, fAmount = 0, sum = 0;
            int lYear = 0, lMonth = 0, lDay = 0, lHour = 0, lMinute = 0;
            int startIndex = 0;
            for (int i = startIndex; i < strLen; i++) {
                String[] strs2 = strs[i].Split(',');
                if (strs2.Length == 4) {
                    double date = FCStr.convertStrToDouble(strs2[0]);
                    double close = FCStr.convertStrToDouble(strs2[1]);
                    double volume = FCStr.convertStrToDouble(strs2[2]);
                    double amount = FCStr.convertStrToDouble(strs2[3]);
                    int year = 0, month = 0, day = 0, hour = 0, minute = 0, second = 0, ms = 0;
                    FCStr.getDateByNum(date, ref year, ref month, ref day, ref hour, ref minute, ref second, ref ms);
                    if (hour * 60 + minute >= 899) {
                        hour = 14;
                        minute = 59;
                    }
                    if (i == startIndex) {
                        lClose = close;
                        lHigh = close;
                        lLow = close;
                        lOpen = close;
                        lVolume = volume;
                        lAmount = amount;
                        lYear = year;
                        lMonth = month;
                        lDay = day;
                        lHour = hour;
                        lMinute = minute;
                    }
                    bool inSameTime = false;
                    if (hour == lHour && minute == lMinute) {
                        inSameTime = true;
                        if (close > lHigh) {
                            lHigh = close;
                        }
                        if (close < lLow) {
                            lLow = close;
                        }
                    }
                    if (!inSameTime || i == strLen - 1) {
                        SecurityData data = new SecurityData();
                        data.m_date = FCStr.getDateNum(lYear, lMonth, lDay, lHour, lMinute, 0, 0);
                        data.m_close = lClose;
                        if (lHigh != 0) {
                            data.m_high = lHigh;
                        }
                        else {
                            data.m_high = lClose;
                        }
                        if (lLow != 0) {
                            data.m_low = lLow;
                        }
                        else {
                            data.m_low = lClose;
                        }
                        if (lOpen != 0) {
                            data.m_open = lOpen;
                        }
                        else {
                            data.m_open = lClose;
                        }
                        data.m_volume = lVolume - fVolume;
                        data.m_amount = lAmount - fAmount;
                        if (data.m_close != 0 && data.m_volume != 0) {
                            sum += data.m_close;
                            data.m_avgPrice = sum / (datas.Count + 1);
                            datas.Add(data);
                        }
                        fVolume = lVolume;
                        fAmount = lAmount;
                    }
                    if (!inSameTime) {
                        lHigh = close;
                        lLow = close;
                        lOpen = close;
                        lYear = year;
                        lMonth = month;
                        lDay = day;
                        lHour = hour;
                        lMinute = minute;
                    }
                    lClose = close;
                    lVolume = volume;
                    lAmount = amount;
                }
            }
            return 1;
        }

        /// <summary>
        /// 获取通达信的历史数据
        /// </summary>
        /// <param name="str">数据字符串</param>
        /// <param name="datas">历史数据</param>
        /// <returns>状态</returns>
        public static int getHistoryDatasByTdxStr(String str, List<SecurityData> datas) {
            String[] strs = str.Split(new String[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);
            int strLen = strs.Length;
            for (int i = 2; i < strLen - 1; i++) {
                String[] strs2 = strs[i].Split(',');
                int strs2Size = strs2.Length;
                if (strs2Size >= 7) {
                    String[] dateStrs = strs2[0].Split('-');
                    int year = 0, month = 0, day = 0, hour = 0, minute = 0;
                    if (dateStrs.Length == 3) {
                        year = FCStr.convertStrToInt(dateStrs[0]);
                        month = FCStr.convertStrToInt(dateStrs[1]);
                        day = FCStr.convertStrToInt(dateStrs[2]);
                    }
                    int si = 1;
                    if (strs2Size == 8) {
                        si = 2;
                        hour = FCStr.convertStrToInt(strs2[1].Substring(0, 2));
                        minute = FCStr.convertStrToInt(strs2[1].Substring(2, 2));
                    }
                    double open = FCStr.convertStrToDouble(strs2[si]);
                    double high = FCStr.convertStrToDouble(strs2[si + 1]);
                    double low = FCStr.convertStrToDouble(strs2[si + 2]);
                    double close = FCStr.convertStrToDouble(strs2[si + 3]);
                    double volume = FCStr.convertStrToDouble(strs2[si + 4]);
                    double amount = FCStr.convertStrToDouble(strs2[si + 5]);
                    if (volume == 0) {
                        continue;
                    }
                    if (year != 0 && month != 0 && day != 0) {
                        SecurityData securityData = new SecurityData();
                        if (hour != 0 || minute != 0) {
                            securityData.m_date = FCStr.getDateNum(year, month, day, hour, minute, 0, 0) - 300;
                        }
                        else {
                            securityData.m_date = FCStr.getDateNum(year, month, day, 0, 0, 0, 0);
                        }
                        securityData.m_open = open;
                        securityData.m_high = high;
                        securityData.m_low = low;
                        securityData.m_close = close;
                        securityData.m_volume = volume;
                        securityData.m_amount = amount;
                        datas.Add(securityData);
                    }
                }
            }
            return 1;
        }

        /// <summary>
        /// 获取周线的历史数据
        /// </summary>
        /// <param name="weekDatas">周线历史数据</param>
        /// <param name="dayDatas">日线历史数据</param>
        /// <returns>状态</returns>
        public static int getHistoryWeekDatas(List<SecurityData> weekDatas, List<SecurityData> dayDatas) {
            int weekDatasSize = dayDatas.Count;
            if (weekDatasSize > 0) {
                SecurityData weekData = new SecurityData();
                weekData.copy(dayDatas[0]);
                int lDayOfWeek = 0, lDays = 0;
                for (int i = 0; i < weekDatasSize; i++) {
                    SecurityData dayData = new SecurityData();
                    dayData.copy(dayDatas[i]);
                    int year = 0, month = 0, day = 0, hour = 0, minute = 0, second = 0, ms = 0, days = (int)dayData.m_date / (3600 * 24);
                    FCStr.getDateByNum(dayData.m_date, ref year, ref month, ref day, ref hour, ref minute, ref second, ref ms);
                    int dow = dayOfWeek(year, month, day);
                    bool isNextWeek = true;
                    bool add = false;
                    if (days - lDays <= 5) {
                        if (days != lDays) {
                            isNextWeek = dow <= lDayOfWeek;
                        }
                    }
                    if (isNextWeek || i == weekDatasSize - 1) {
                        add = true;
                    }
                    if (!isNextWeek) {
                        weekData.m_close = dayData.m_close;
                        weekData.m_amount += dayData.m_amount;
                        weekData.m_volume += dayData.m_volume;
                        if (weekData.m_high < dayData.m_high) {
                            weekData.m_high = dayData.m_high;
                        }
                        if (weekData.m_low > dayData.m_low) {
                            weekData.m_low = dayData.m_low;
                        }

                    }
                    if (add) {
                        weekDatas.Add(weekData);
                        weekData = dayData;
                    }
                    if (isNextWeek && i == weekDatasSize - 1) {
                        weekData = dayData;
                        weekDatas.Add(weekData);
                    }
                    lDayOfWeek = dow;
                    lDays = days;
                }
            }
            return 1;
        }

        /// <summary>
        /// 获取月线的历史数据
        /// </summary>
        /// <param name="weekDatas">月线历史数据</param>
        /// <param name="dayDatas">日线历史数据</param>
        /// <returns>状态</returns>
        public static int getHistoryMonthDatas(List<SecurityData> monthDatas, List<SecurityData> dayDatas) {
            int monthDatasSize = dayDatas.Count;
            if (monthDatasSize > 0) {
                SecurityData monthData = new SecurityData();
                monthData.copy(dayDatas[0]);
                int lYear = 0, lMonth = 0, lDay = 0;
                for (int i = 0; i < monthDatasSize; i++) {
                    SecurityData dayData = new SecurityData();
                    dayData.copy(dayDatas[i]);
                    int year = 0, month = 0, day = 0, hour = 0, minute = 0, second = 0, ms = 0;
                    FCStr.getDateByNum(dayData.m_date, ref year, ref month, ref day, ref hour, ref minute, ref second, ref ms);
                    bool isNextMonth = year * 12 + month > lYear * 12 + lMonth;
                    bool add = false;
                    if (i == monthDatasSize - 1 || (i > 0 && isNextMonth)) {
                        add = true;
                    }
                    if (!isNextMonth) {
                        monthData.m_close = dayData.m_close;
                        monthData.m_amount += dayData.m_amount;
                        monthData.m_volume += dayData.m_volume;
                        if (monthData.m_high < dayData.m_high) {
                            monthData.m_high = dayData.m_high;
                        }
                        if (monthData.m_low > dayData.m_low) {
                            monthData.m_low = dayData.m_low;
                        }
                    }
                    if (add) {
                        monthDatas.Add(monthData);
                        monthData = dayData;
                    }
                    if (isNextMonth && i == monthDatasSize - 1) {
                        monthData = dayData;
                        monthDatas.Add(monthData);
                    }
                    lYear = year;
                    lMonth = month;
                    lDay = day;
                }
            }
            return 1;
        }

        /// <summary>
        /// 根据字符串获取新浪的最新数据
        /// </summary>
        /// <param name="str">数据字符串</param>
        /// <param name="formatType">格式</param>
        /// <param name="data">最新数据</param>
        /// <returns>状态</returns>
        public static int getLatestDataBySinaStr(String str, int formatType, ref SecurityLatestData data) {
            //分析数据
            String date = "";
            String[] strs2 = str.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            int strLen2 = strs2.Length;
            bool szIndex = false;
            for (int j = 0; j < strLen2; j++) {
                String str2 = strs2[j];
                switch (j) {
                    case 0:
                        data.m_code = FCStrEx.convertSinaCodeToDBCode(str2);
                        if (data.m_code.StartsWith("399")) {
                            szIndex = true;
                        }
                        break;
                    case 1: {
                            data.m_open = FCStr.convertStrToDouble(str2);
                            break;
                        }
                    case 2: {
                            data.m_lastClose = FCStr.convertStrToDouble(str2);
                            break;
                        }
                    case 3: {
                            data.m_close = FCStr.convertStrToDouble(str2);
                            break;
                        }
                    case 4: {
                            data.m_high = FCStr.convertStrToDouble(str2);
                            break;
                        }
                    case 5: {
                            data.m_low = FCStr.convertStrToDouble(str2);
                            break;
                        }
                    case 8: {
                            data.m_volume = FCStr.convertStrToDouble(str2);
                            if (szIndex) {
                                data.m_volume /= 100;
                            }
                            break;
                        }
                    case 9: {
                            data.m_amount = FCStr.convertStrToDouble(str2);
                            break;
                        }
                    case 10: {
                            if (formatType == 0) {
                                data.m_buyVolume1 = (int)FCStr.convertStrToDouble(str2);
                            }
                            break;
                        }
                    case 11: {
                            if (formatType == 0) {
                                data.m_buyPrice1 = FCStr.convertStrToDouble(str2);
                            }
                            break;
                        }
                    case 12: {
                            if (formatType == 0) {
                                data.m_buyVolume2 = (int)FCStr.convertStrToDouble(str2);
                            }
                            break;
                        }
                    case 13: {
                            if (formatType == 0) {
                                data.m_buyPrice2 = FCStr.convertStrToDouble(str2);
                            }
                            break;
                        }
                    case 14: {
                            if (formatType == 0) {
                                data.m_buyVolume3 = (int)FCStr.convertStrToDouble(str2);
                            }
                            break;
                        }
                    case 15: {
                            if (formatType == 0) {
                                data.m_buyPrice3 = FCStr.convertStrToDouble(str2);
                            }
                            break;
                        }
                    case 16: {
                            if (formatType == 0) {
                                data.m_buyVolume4 = (int)FCStr.convertStrToDouble(str2);
                            }
                            break;
                        }
                    case 17: {
                            if (formatType == 0) {
                                data.m_buyPrice4 = FCStr.convertStrToDouble(str2);
                            }
                            break;
                        }
                    case 18: {
                            if (formatType == 0) {
                                data.m_buyVolume5 = (int)FCStr.convertStrToDouble(str2);
                            }
                            break;
                        }
                    case 19: {
                            if (formatType == 0) {
                                data.m_buyPrice5 = FCStr.convertStrToDouble(str2);
                            }
                            break;
                        }
                    case 20: {
                            if (formatType == 0) {
                                data.m_sellVolume1 = (int)FCStr.convertStrToDouble(str2);
                            }
                            break;
                        }
                    case 21: {
                            if (formatType == 0) {
                                data.m_sellPrice1 = FCStr.convertStrToDouble(str2);
                            }
                            break;
                        }
                    case 22: {
                            if (formatType == 0) {
                                data.m_sellVolume2 = (int)FCStr.convertStrToDouble(str2);
                            }
                            break;
                        }
                    case 23: {
                            if (formatType == 0) {
                                data.m_sellPrice2 = FCStr.convertStrToDouble(str2);
                            }
                            break;
                        }
                    case 24: {
                            if (formatType == 0) {
                                data.m_sellVolume3 = (int)FCStr.convertStrToDouble(str2);
                            }
                            break;
                        }
                    case 25: {
                            if (formatType == 0) {
                                data.m_sellPrice3 = FCStr.convertStrToDouble(str2);
                            }
                            break;
                        }
                    case 26: {
                            if (formatType == 0) {
                                data.m_sellVolume4 = (int)FCStr.convertStrToDouble(str2);
                            }
                            break;
                        }
                    case 27: {
                            if (formatType == 0) {
                                data.m_sellPrice4 = FCStr.convertStrToDouble(str2);
                            }
                            break;
                        }
                    case 28: {
                            if (formatType == 0) {
                                data.m_sellVolume5 = (int)FCStr.convertStrToDouble(str2);
                            }
                            break;
                        }
                    case 29: {
                            if (formatType == 0) {
                                data.m_sellPrice5 = FCStr.convertStrToDouble(str2);
                            }
                            break;
                        }
                    case 30:
                        date = str2;
                        break;
                    case 31:
                        date += " " + str2;
                        break;
                }
            }
            //获取时间
            if (date != null && date.Length > 0) {
                DateTime dateTime = Convert.ToDateTime(date);
                data.m_date = FCStr.getDateNum(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour, dateTime.Minute, dateTime.Second, 0);
            }
            //价格修正
            if (data.m_close != 0) {
                if (data.m_open == 0) {
                    data.m_open = data.m_close;
                }
                if (data.m_high == 0) {
                    data.m_high = data.m_close;
                }
                if (data.m_low == 0) {
                    data.m_low = data.m_close;
                }
            }
            return 0;
        }

        /// <summary>
        /// 根据字符串获取新浪最新数据
        /// </summary>
        /// <param name="str">数据字符串</param>
        /// <param name="formatType">格式</param>
        /// <param name="datas">最新数据</param>
        /// <returns>状态</returns>
        public static int getLatestDatasBySinaStr(String str, int formatType, List<SecurityLatestData> datas) {
            String[] strs = str.Split(new String[] { ";\n" }, StringSplitOptions.RemoveEmptyEntries);
            int strLen = strs.Length;
            for (int i = 0; i < strLen; i++) {
                SecurityLatestData latestData = new SecurityLatestData();
                String dataStr = strs[i];
                getLatestDataBySinaStr(strs[i], formatType, ref latestData);
                if (latestData.m_date > 0) {
                    datas.Add(latestData);
                }
            }
            return 1;
        }

        /// <summary>
        /// 获取切分后的日期
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="divide">切分值</param>
        /// <returns>切分后的日期</returns>
        public static double getDivideDate(double date, long divide) {
            return (double)((long)date / divide);
        }

        /// <summary>
        /// 根据股票代码获取新浪最新数据
        /// </summary>
        /// <param name="codes">股票代码列表</param>
        /// <returns>字符串</returns>
        public static String getSinaLatestDatasStrByCodes(String codes) {
            String[] strs = codes.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries);
            int strLen = strs.Length;
            List<String> sinaCodes = new List<String>();
            List<String> dcCodes = new List<String>();
            for (int i = 0; i < strLen; i++) {
                String postCode = strs[i];
                sinaCodes.Add(FCStrEx.convertDBCodeToSinaCode(postCode));
            }
            String requestCode = "";
            int sinaCodesSize = sinaCodes.Count;
            for (int i = 0; i < sinaCodesSize; i++) {
                String postCode = sinaCodes[i];
                requestCode += postCode;
                if (i != strLen - 1) {
                    requestCode += ",";
                }
            }
            String result = "";
            if (sinaCodesSize > 0) {
                String url = "http://hq.sinajs.cn/list=" + requestCode.ToLower();
                result = FCHttpGetService.get(url);
            }
            return result;
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
            }
            else {
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

        /// <summary>
        /// 获取通达信的历史数据
        /// </summary>
        /// <param name="code">股票代码</param>
        /// <param name="path">本地文件路径</param>
        /// <param name="datas">返回数据</param>
        /// <returns>状态</returns>
        public static int getTdxHistoryDatasByCode(String code, String path, List<SecurityData> datas) {
            String result = getTdxHistoryDatasStrByCode(code, path);
            if (result != null && result.Length > 0) {
                getHistoryDatasByTdxStr(result, datas);
            }
            return 1;
        }

        /// <summary>
        /// 获取通达信的历史数据的字符串
        /// </summary>
        /// <param name="code">股票代码</param>
        /// <param name="path">本地文件路径</param>
        /// <returns>数据字符串</returns>
        public static String getTdxHistoryDatasStrByCode(String code, String path) {
            String fileName = FCStrEx.convertDBCodeToFileName(code);
            String result = "";
            String filePath = path + fileName;
            if (FCFile.isFileExist(filePath)) {
                FCFile.read(filePath, ref result);
            }
            return result;
        }
    }
}
