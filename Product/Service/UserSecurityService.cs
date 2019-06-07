/*基于FaceCat(捂脸猫)框架 v1.0 https://github.com/FaceCat007/facecat.git
 *  1.捂脸猫创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 *  2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 *  3.恒泰期货投资咨询总监/高级研究员-戴俊生(微信号:18345063201);
 *  4.联合创始人-肖添龙(微信号:xiaotianlong_luu);
 */

using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace FaceCat {
    /// <summary>
    /// 自选股
    /// </summary>
    public class UserSecurity {
        /// <summary>
        /// 代码
        /// </summary>
        public String m_code;

        /// <summary>
        /// 自动买入价
        /// </summary>
        public double m_buy;

        /// <summary>
        /// 状态
        /// </summary>
        public int m_state;

        /// <summary>
        /// 自动卖出价
        /// </summary>
        public double m_sell;

        /// <summary>
        /// 止损价
        /// </summary>
        public double m_stop;
    }

    /// <summary>
    /// 自选股服务
    /// </summary>
    public class UserSecurityService {
        /// <summary>
        /// 创建服务
        /// </summary>
        public UserSecurityService() {
            UserCookie cookie = new UserCookie();
            UserCookieService cookieService = DataCenter.UserCookieService;
            if (cookieService.getCookie("USERSECURITY", ref cookie) > 0) {
                try {
                    m_codes = JsonConvert.DeserializeObject<List<UserSecurity>>(cookie.m_value);
                }
                catch (Exception ex) {
                }
                if (m_codes == null) {
                    try {
                        m_codes = JsonConvert.DeserializeObject<List<UserSecurity>>(cookie.m_value);
                    }
                    catch (Exception ex) {
                    }
                }
            }
        }

        /// <summary>
        /// 自选股信息
        /// </summary>
        public List<UserSecurity> m_codes = new List<UserSecurity>();

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="code">自选股</param>
        public void add(UserSecurity code) {
            bool modify = false;
            int awardsSize = m_codes.Count;
            for (int i = 0; i < awardsSize; i++) {
                if (m_codes[i] == code) {
                    modify = true;
                    m_codes[i] = code;
                    save();
                    break;
                }
            }
            if (!modify) {
                m_codes.Add(code);
                save();
            }
        }

        /// <summary>
        /// 删除代码
        /// </summary>
        /// <param name="code">代码</param>
        public void delete(UserSecurity userSecurity) {
            int codesSize = m_codes.Count;
            for (int i = 0; i < codesSize; i++) {
                if (m_codes[i].m_code == userSecurity.m_code) {
                    m_codes.RemoveAt(i);
                    save();
                    break;
                }
            }
        }

        /// <summary>
        /// 获取嘉奖信息
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>嘉奖信息</returns>
        public UserSecurity get(String code) {
            int codesSize = m_codes.Count;
            for (int i = 0; i < codesSize; i++) {
                if (m_codes[i].m_code == code) {
                    return m_codes[i];
                }
            }
            return null;
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        public void save() {
            UserCookie cookie = new UserCookie();
            cookie.m_key = "USERSECURITY";
            cookie.m_value = JsonConvert.SerializeObject(m_codes);
            UserCookieService cookieService = DataCenter.UserCookieService;
            cookieService.addCookie(cookie);
        }
    }
}
