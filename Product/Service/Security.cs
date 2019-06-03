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
    /// 股票信息
    /// </summary>
    public class Security {
        /// <summary>
        /// 创建键盘精灵
        /// </summary>
        public Security() {
        }

        /// <summary>
        /// 股票代码
        /// </summary>
        public String m_code = "";

        /// <summary>
        /// 股票名称
        /// </summary>
        public String m_name = "";

        /// <summary>
        /// 拼音
        /// </summary>
        public String m_pingyin = "";

        /// <summary>
        /// 状态
        /// </summary>
        public int m_status;

        /// <summary>
        /// 市场类型
        /// </summary>
        public int m_type;
    }

    /// <summary>
    /// 证券历史数据
    /// </summary>
    public class SecurityData {
        /// <summary>
        /// 平均价格
        /// </summary>
        public double m_avgPrice;

        /// <summary>
        /// 收盘价
        /// </summary>
        public double m_close;

        /// <summary>
        /// 上次的成交量
        /// </summary>
        public double m_dVolume;

        /// <summary>
        /// 日期
        /// </summary>
        public double m_date;

        /// <summary>
        /// 最高价
        /// </summary>
        public double m_high;

        /// <summary>
        /// 最低价
        /// </summary>
        public double m_low;

        /// <summary>
        /// 开盘价
        /// </summary>
        public double m_open;

        /// <summary>
        /// 成交量
        /// </summary>
        public double m_volume;

        /// <summary>
        /// 成交额
        /// </summary>
        public double m_amount;

        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="data">数据</param>
        public void copy(SecurityData data) {
            m_close = data.m_close;
            m_date = data.m_date;
            m_high = data.m_high;
            m_low = data.m_low;
            m_open = data.m_open;
            m_volume = data.m_volume;
            m_amount = data.m_amount;
        }
    }

    /// <summary>
    /// 股票实时数据
    /// </summary>
    public class SecurityLatestData {
        /// <summary>
        /// 成交额
        /// </summary>
        public double m_amount;

        /// <summary>
        /// 委买总量
        /// </summary>
        public double m_allBuyVol;

        /// <summary>
        /// 委卖总量
        /// </summary>
        public double m_allSellVol;

        /// <summary>
        /// 加权平均委卖价格
        /// </summary>
        public double m_avgBuyPrice;

        /// <summary>
        /// 加权平均委卖价格
        /// </summary>
        public double m_avgSellPrice;

        /// <summary>
        /// 买一量
        /// </summary>
        public int m_buyVolume1;

        /// <summary>
        /// 买二量
        /// </summary>
        public int m_buyVolume2;

        /// <summary>
        /// 买三量
        /// </summary>
        public int m_buyVolume3;

        /// <summary>
        /// 买四量
        /// </summary>
        public int m_buyVolume4;

        /// <summary>
        /// 买五量
        /// </summary>
        public int m_buyVolume5;

        /// <summary>
        /// 买五量
        /// </summary>
        public int m_buyVolume6;

        /// <summary>
        /// 买五量
        /// </summary>
        public int m_buyVolume7;

        /// <summary>
        /// 买五量
        /// </summary>
        public int m_buyVolume8;

        /// <summary>
        /// 买五量
        /// </summary>
        public int m_buyVolume9;

        /// <summary>
        /// 买五量
        /// </summary>
        public int m_buyVolume10;

        /// <summary>
        /// 买一价
        /// </summary>
        public double m_buyPrice1;

        /// <summary>
        /// 买二价
        /// </summary>
        public double m_buyPrice2;

        /// <summary>
        /// 买三价
        /// </summary>
        public double m_buyPrice3;

        /// <summary>
        /// 买四价
        /// </summary>
        public double m_buyPrice4;

        /// <summary>
        /// 买五价
        /// </summary>
        public double m_buyPrice5;

        /// <summary>
        /// 买一价
        /// </summary>
        public double m_buyPrice6;

        /// <summary>
        /// 买二价
        /// </summary>
        public double m_buyPrice7;

        /// <summary>
        /// 买三价
        /// </summary>
        public double m_buyPrice8;

        /// <summary>
        /// 买四价
        /// </summary>
        public double m_buyPrice9;

        /// <summary>
        /// 买五价
        /// </summary>
        public double m_buyPrice10;

        /// <summary>
        /// 当前价格
        /// </summary>
        public double m_close;

        /// <summary>
        /// 股票代码
        /// </summary>
        public String m_code = "";

        /// <summary>
        /// 上次成交量
        /// </summary>
        public double m_dVolume;

        /// <summary>
        /// 日期及时间
        /// </summary>
        public double m_date;

        /// <summary>
        /// 最高价
        /// </summary>
        public double m_high;

        /// <summary>
        /// 内盘成交量
        /// </summary>
        public int m_innerVol;

        /// <summary>
        /// 昨日收盘价
        /// </summary>
        public double m_lastClose;

        /// <summary>
        /// 最低价
        /// </summary>
        public double m_low;

        /// <summary>
        /// 开盘价
        /// </summary>
        public double m_open;

        /// <summary>
        /// 期货持仓量
        /// </summary>
        public double m_openInterest;

        /// <summary>
        /// 外盘成交量
        /// </summary>
        public int m_outerVol;

        /// <summary>
        /// 卖一量
        /// </summary>
        public int m_sellVolume1;

        /// <summary>
        /// 卖二量
        /// </summary>
        public int m_sellVolume2;

        /// <summary>
        /// 卖三量
        /// </summary>
        public int m_sellVolume3;

        /// <summary>
        /// 卖四量
        /// </summary>
        public int m_sellVolume4;

        /// <summary>
        /// 卖五量
        /// </summary>
        public int m_sellVolume5;

        /// <summary>
        /// 卖一量
        /// </summary>
        public int m_sellVolume6;

        /// <summary>
        /// 卖二量
        /// </summary>
        public int m_sellVolume7;

        /// <summary>
        /// 卖三量
        /// </summary>
        public int m_sellVolume8;

        /// <summary>
        /// 卖四量
        /// </summary>
        public int m_sellVolume9;

        /// <summary>
        /// 卖五量
        /// </summary>
        public int m_sellVolume10;

        /// <summary>
        /// 卖一价
        /// </summary>
        public double m_sellPrice1;

        /// <summary>
        /// 卖二价
        /// </summary>
        public double m_sellPrice2;

        /// <summary>
        /// 卖三价
        /// </summary>
        public double m_sellPrice3;

        /// <summary>
        /// 卖四价
        /// </summary>
        public double m_sellPrice4;

        /// <summary>
        /// 卖五价
        /// </summary>
        public double m_sellPrice5;

        /// <summary>
        /// 卖一价
        /// </summary>
        public double m_sellPrice6;

        /// <summary>
        /// 卖二价
        /// </summary>
        public double m_sellPrice7;

        /// <summary>
        /// 卖三价
        /// </summary>
        public double m_sellPrice8;

        /// <summary>
        /// 卖四价
        /// </summary>
        public double m_sellPrice9;

        /// <summary>
        /// 卖五价
        /// </summary>
        public double m_sellPrice10;

        /// <summary>
        /// 期货结算价
        /// </summary>
        public double m_settlePrice;

        /// <summary>
        /// 换手率
        /// </summary>
        public double m_turnoverRate;

        /// <summary>
        /// 成交量
        /// </summary>
        public double m_volume;

        /// <summary>
        /// 复制数据
        /// </summary>
        /// <param name="data">数据</param>
        public void copy(SecurityLatestData data) {
            if (data == null) return;
            m_amount = data.m_amount;
            m_allBuyVol = data.m_allBuyVol;
            m_allSellVol = data.m_allSellVol;
            m_avgBuyPrice = data.m_avgBuyPrice;
            m_avgSellPrice = data.m_avgSellPrice;
            m_buyVolume1 = data.m_buyVolume1;
            m_buyVolume2 = data.m_buyVolume2;
            m_buyVolume3 = data.m_buyVolume3;
            m_buyVolume4 = data.m_buyVolume4;
            m_buyVolume5 = data.m_buyVolume5;
            m_buyPrice1 = data.m_buyPrice1;
            m_buyPrice2 = data.m_buyPrice2;
            m_buyPrice3 = data.m_buyPrice3;
            m_buyPrice4 = data.m_buyPrice4;
            m_buyPrice5 = data.m_buyPrice5;
            m_buyVolume6 = data.m_buyVolume6;
            m_buyVolume7 = data.m_buyVolume7;
            m_buyVolume8 = data.m_buyVolume8;
            m_buyVolume9 = data.m_buyVolume9;
            m_buyVolume10 = data.m_buyVolume10;
            m_buyPrice6 = data.m_buyPrice6;
            m_buyPrice7 = data.m_buyPrice7;
            m_buyPrice8 = data.m_buyPrice8;
            m_buyPrice9 = data.m_buyPrice9;
            m_buyPrice10 = data.m_buyPrice10;
            m_close = data.m_close;
            m_date = data.m_date;
            m_high = data.m_high;
            m_innerVol = data.m_innerVol;
            m_lastClose = data.m_lastClose;
            m_low = data.m_low;
            m_open = data.m_open;
            m_openInterest = data.m_openInterest;
            m_outerVol = data.m_outerVol;
            m_code = data.m_code;
            m_sellVolume1 = data.m_sellVolume1;
            m_sellVolume2 = data.m_sellVolume2;
            m_sellVolume3 = data.m_sellVolume3;
            m_sellVolume4 = data.m_sellVolume4;
            m_sellVolume5 = data.m_sellVolume5;
            m_sellPrice1 = data.m_sellPrice1;
            m_sellPrice2 = data.m_sellPrice2;
            m_sellPrice3 = data.m_sellPrice3;
            m_sellPrice4 = data.m_sellPrice4;
            m_sellPrice5 = data.m_sellPrice5;
            m_settlePrice = data.m_settlePrice;
            m_sellVolume6 = data.m_sellVolume6;
            m_sellVolume7 = data.m_sellVolume7;
            m_sellVolume8 = data.m_sellVolume8;
            m_sellVolume9 = data.m_sellVolume9;
            m_sellVolume10 = data.m_sellVolume10;
            m_sellPrice6 = data.m_sellPrice6;
            m_sellPrice7 = data.m_sellPrice7;
            m_sellPrice8 = data.m_sellPrice8;
            m_sellPrice9 = data.m_sellPrice9;
            m_sellPrice10 = data.m_sellPrice10;
            m_settlePrice = data.m_settlePrice;
            m_turnoverRate = data.m_turnoverRate;
            m_volume = data.m_volume;
        }

        /// <summary>
        /// 比较是否相同
        /// </summary>
        /// <param name="data">数据</param>
        /// <returns>是否相同</returns>
        public bool equal(SecurityLatestData data) {
            if (data == null) return false;
            if (m_amount == data.m_amount
            && m_buyVolume1 == data.m_buyVolume1
            && m_buyVolume2 == data.m_buyVolume2
            && m_buyVolume3 == data.m_buyVolume3
            && m_buyVolume4 == data.m_buyVolume4
            && m_buyVolume5 == data.m_buyVolume5
            && m_buyPrice1 == data.m_buyPrice1
            && m_buyPrice2 == data.m_buyPrice2
            && m_buyPrice3 == data.m_buyPrice3
            && m_buyPrice4 == data.m_buyPrice4
            && m_buyPrice5 == data.m_buyPrice5
            && m_close == data.m_close
            && m_date == data.m_date
            && m_high == data.m_high
            && m_innerVol == data.m_innerVol
            && m_lastClose == data.m_lastClose
            && m_low == data.m_low
            && m_open == data.m_open
            && m_openInterest == data.m_openInterest
            && m_outerVol == data.m_outerVol
            && m_code == data.m_code
            && m_sellVolume1 == data.m_sellVolume1
            && m_sellVolume2 == data.m_sellVolume2
            && m_sellVolume3 == data.m_sellVolume3
            && m_sellVolume4 == data.m_sellVolume4
            && m_sellVolume5 == data.m_sellVolume5
            && m_sellPrice1 == data.m_sellPrice1
            && m_sellPrice2 == data.m_sellPrice2
            && m_sellPrice3 == data.m_sellPrice3
            && m_sellPrice4 == data.m_sellPrice4
            && m_sellPrice5 == data.m_sellPrice5
            && m_settlePrice == data.m_settlePrice
            && m_turnoverRate == data.m_turnoverRate
            && m_volume == data.m_volume) {
                return true;
            }
            return false;
        }
    }

    /// <summary>
    /// 历史数据信息
    /// </summary>
    public class HistoryDataInfo {
        /// <summary>
        /// 周期
        /// </summary>
        public int m_cycle;

        /// <summary>
        /// 结束日期
        /// </summary>
        public double m_endDate;

        /// <summary>
        /// 是否需要推送数据
        /// </summary>
        public bool m_pushData;

        /// <summary>
        /// 股票代码
        /// </summary>
        public String m_code;

        /// <summary>
        /// 数据条数
        /// </summary>
        public int m_size;

        /// <summary>
        /// 开始日期
        /// </summary>
        public double m_startDate;

        /// <summary>
        /// 复权模式
        /// </summary>
        public int m_subscription;

        /// <summary>
        /// 类型
        /// </summary>
        public int m_type;
    }

    /// <summary>
    /// 最新数据信息
    /// </summary>
    public class LatestDataInfo {
        /// <summary>
        /// 代码
        /// </summary>
        public String m_codes = "";

        /// <summary>
        /// 格式
        /// </summary>
        public int m_formatType;

        /// <summary>
        /// 是否包含LV2
        /// </summary>
        public int m_lv2;

        /// <summary>
        /// 数据条数
        /// </summary>
        public int m_size;
    }

    /// <summary>
    /// 公共字段
    /// </summary>
    public class KeyFields {
        /// <summary>
        /// 收盘价
        /// </summary>
        public const String CLOSE = "CLOSE";
        /// <summary>
        /// 最高价
        /// </summary>
        public const String HIGH = "HIGH";
        /// <summary>
        /// 最低价
        /// </summary>
        public const String LOW = "LOW";
        /// <summary>
        /// 开盘价
        /// </summary>
        public const String OPEN = "OPEN";
        /// <summary>
        /// 成交量
        /// </summary>
        public const String VOL = "VOL";
        /// <summary>
        /// 成交额
        /// </summary>
        public const String AMOUNT = "AMOUNT";

        /// <summary>
        /// 平均价格
        /// </summary>
        public const String AVGPRICE = "AVGPRICE";

        /// <summary>
        /// 收盘价字段
        /// </summary>
        public const int CLOSE_INDEX = 0;
        /// <summary>
        /// 最高价字段
        /// </summary>
        public const int HIGH_INDEX = 1;
        /// <summary>
        /// 最低价字段
        /// </summary>
        public const int LOW_INDEX = 2;
        /// <summary>
        /// 开盘价字段
        /// </summary>
        public const int OPEN_INDEX = 3;
        /// <summary>
        /// 成交量字段
        /// </summary>
        public const int VOL_INDEX = 4;
        /// <summary>
        /// 成交额字段
        /// </summary>
        public const int AMOUNT_INDEX = 5;

        /// <summary>
        /// 平均价格字段
        /// </summary>
        public const int AVGPRICE_INDEX = 6;

        public const int VOLHISTORY_INDEX = 0;
        public const int AMOUNTHISTORY_INDEX = 1;
    }

    /// <summary>
    /// 指标数据
    /// </summary>
    public class IndicatorData {
        /// <summary>
        /// 名称
        /// </summary>
        public String m_name;

        /// <summary>
        /// 参数
        /// </summary>
        public String m_parameters;

        /// <summary>
        /// 脚本
        /// </summary>
        public String m_script;

    }

    public class MinuteKLineDate {
        public int m_year;
        public int m_month;
        public int m_day;
        public int m_hour;
        public int m_minute;
        public int m_hour_cycle;
        public int m_minute_cycle;
        public int m_cycle;
    }

    /// <summary>
    /// 指标对象
    /// </summary>
    public class Indicator {
        /// <summary>
        /// 类别
        /// </summary>
        public String m_category = "";

        /// <summary>
        /// 预定显示坐标
        /// </summary>
        public String m_coordinate = "";

        /// <summary>
        /// 描述
        /// </summary>
        public String m_description = "";

        /// <summary>
        /// 显示小数的位数
        /// </summary>
        public int m_digit;

        /// <summary>
        /// 指标ID
        /// </summary>
        public String m_indicatorID = "";

        /// <summary>
        /// 名称
        /// </summary>
        public String m_name = "";

        /// <summary>
        /// 列表顺序
        /// </summary>
        public int m_orderNum;

        /// <summary>
        /// 画线方法
        /// </summary>
        public int m_paintType;

        /// <summary>
        /// 参数
        /// </summary>
        public String m_parameters = "";

        /// <summary>
        /// 密码
        /// </summary>
        public String m_password = "";

        /// <summary>
        /// 特殊Y轴坐标
        /// </summary>
        public String m_specialCoordinate = "";

        /// <summary>
        /// 文本
        /// </summary>
        public String m_text = "";

        /// <summary>
        /// 类型
        /// </summary>
        public int m_type;

        /// <summary>
        /// 是否使用密码
        /// </summary>
        public int m_usePassword;

        /// <summary>
        /// 用户ID
        /// </summary>
        public int m_userID;

        /// <summary>
        /// 版本
        /// </summary>
        public int m_version;
    }

    /// <summary>
    /// 交易信息
    /// </summary>
    public class OrderInfo {
        public String m_code;
        public float m_price;
        public int m_qty;
    }
}
