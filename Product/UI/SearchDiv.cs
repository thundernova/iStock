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
    /// 键盘精灵
    /// </summary>
    public class SearchDiv : FCMenu {
        /// <summary>
        /// 创建键盘精灵
        /// </summary>
        public SearchDiv() {
            m_gridCellClickEvent = new FCGridCellTouchEvent(gridCellClick);
            m_gridKeyDownEvent = new FCKeyEvent(GridKeyDown);
            m_textBoxInputChangedEvent = new FCEvent(textBoxInputChanged);
            m_textBoxKeyDownEvent = new FCKeyEvent(textBoxKeyDown);
            BackColor = FCColor.None;
            IsWindow = true;
        }

        /// <summary>
        /// 表格控件
        /// </summary>
        private FCGrid m_grid;

        /// <summary>
        /// 表格单元格点击事件
        /// </summary>
        private FCGridCellTouchEvent m_gridCellClickEvent;

        /// <summary>
        /// 表格键盘事件
        /// </summary>
        private FCKeyEvent m_gridKeyDownEvent;

        /// <summary>
        /// 文本框输入改变事件
        /// </summary>
        private FCEvent m_textBoxInputChangedEvent;

        /// <summary>
        /// 文本框键盘事件
        /// </summary>
        private FCKeyEvent m_textBoxKeyDownEvent;

        private MainFrame m_mainFrame;

        /// <summary>
        /// 获取或设置股票控件
        /// </summary>
        public MainFrame MainFrame {
            get { return m_mainFrame; }
            set { m_mainFrame = value; }
        }

        private FCTextBox m_searchTextBox;

        /// <summary>
        /// 获取或设置查找文本框
        /// </summary>
        public FCTextBox SearchTextBox {
            get { return m_searchTextBox; }
            set { m_searchTextBox = value; }
        }

        /// <summary>
        /// 销毁方法
        /// </summary>
        public override void delete() {
            if (!IsDeleted) {
                if (m_grid != null) {
                    m_grid.removeEvent(m_gridCellClickEvent, FCEventID.GRIDCELLCLICK);
                    m_grid.removeEvent(m_gridKeyDownEvent, FCEventID.KEYDOWN);
                }
                if (m_searchTextBox != null) {
                    if (m_textBoxInputChangedEvent != null) {
                        m_searchTextBox.removeEvent(m_textBoxInputChangedEvent, FCEventID.TEXTCHANGED);
                        m_textBoxInputChangedEvent = null;
                    }
                    if (m_textBoxKeyDownEvent != null) {
                        m_searchTextBox.removeEvent(m_textBoxKeyDownEvent, FCEventID.KEYDOWN);
                        m_textBoxKeyDownEvent = null;
                    }
                }
            }
            base.delete();
        }

        /// <summary>
        /// 过滤查找
        /// </summary>
        public void filterSearch() {
            String sText = m_searchTextBox.Text.ToUpper();
            m_grid.beginUpdate();
            m_grid.clearRows();
            int row = 0;
            CList<Security> securities = SecurityService.FilterCode(sText);
            if (securities != null) {
                int rowCount = securities.size();
                for (int i = 0; i < rowCount; i++) {
                    Security security = securities.get(i);
                    FCGridRow gridRow = new FCGridRow();
                    m_grid.addRow(gridRow);
                    gridRow.addCell(0, new FCGridStringCell(security.m_code));
                    gridRow.addCell(1, new FCGridStringCell(security.m_name));
                    row++;
                }
            }
            securities.delete();
            m_grid.endUpdate();
        }

        /// <summary>
        /// 表格单元格点击事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="cell">单元格</param>
        /// <param name="mp">坐标</param>
        /// <param name="button">按钮</param>
        /// <param name="clicks">点击次数</param>
        /// <param name="delta">滚轮值</param>
        private void gridCellClick(object sender, FCGridCell cell, FCTouchInfo touchInfo) {
            if (touchInfo.m_firstTouch && touchInfo.m_clicks == 2) {
                onSelectRow();
            }
        }

        /// <summary>
        /// 表格键盘事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="key">按键</param>
        private void GridKeyDown(object sender, char key) {
            if (key == 13) {
                onSelectRow();
            }
        }

        /// <summary>
        /// 添加控件方法
        /// </summary>
        public override void onLoad() {
            base.onLoad();
            if (m_grid == null) {
                m_grid = new FCGrid();
                m_grid.AutoEllipsis = true;
                m_grid.GridLineColor = FCColor.None;
                m_grid.Size = new FCSize(240, 200);
                m_grid.addEvent(m_gridCellClickEvent, FCEventID.GRIDCELLCLICK);
                m_grid.addEvent(m_gridKeyDownEvent, FCEventID.KEYDOWN);
                addControl(m_grid);
                m_grid.beginUpdate();
                //添加列
                FCGridColumn securityCodeColumn = new FCGridColumn("股票代码");
                securityCodeColumn.BackColor = FCDraw.FCCOLORS_BACKCOLOR;
                securityCodeColumn.BorderColor = FCColor.None;
                securityCodeColumn.Font = new FCFont("Simsun", 14, true, false, false);
                securityCodeColumn.TextColor = FCDraw.FCCOLORS_FORECOLOR;
                securityCodeColumn.TextAlign = FCContentAlignment.MiddleLeft;
                securityCodeColumn.Width = 120;
                m_grid.addColumn(securityCodeColumn);
                FCGridColumn securityNameColumn = new FCGridColumn("股票名称");
                securityNameColumn.BackColor = FCDraw.FCCOLORS_BACKCOLOR;
                securityNameColumn.BorderColor = FCColor.None;
                securityNameColumn.Font = new FCFont("Simsun", 14, true, false, false);
                securityNameColumn.TextColor = FCDraw.FCCOLORS_FORECOLOR;
                securityNameColumn.TextAlign = FCContentAlignment.MiddleLeft;
                securityNameColumn.Width = 110;
                m_grid.addColumn(securityNameColumn);
                m_grid.endUpdate();
            }
            if (m_searchTextBox == null) {
                m_searchTextBox = new FCTextBox();
                m_searchTextBox.Location = new FCPoint(0, 200);
                m_searchTextBox.Size = new FCSize(240, 20);
                m_searchTextBox.Font = new FCFont("SimSun", 16, true, false, false);
                m_searchTextBox.addEvent(m_textBoxInputChangedEvent, FCEventID.TEXTCHANGED);
                m_searchTextBox.addEvent(m_textBoxKeyDownEvent, FCEventID.KEYDOWN);
                addControl(m_searchTextBox);
            }
        }

        /// <summary>
        /// 选中行方法
        /// </summary>
        private void onSelectRow() {
            List<FCGridRow> rows = m_grid.SelectedRows;
            if (rows != null && rows.Count > 0) {
                FCGridRow selectedRow = rows[0];
                Security security = new Security();
                SecurityService.getSecurityByCode(selectedRow.getCell(0).Text, ref security);
                m_mainFrame.findControl("txtCode").Text = security.m_code;
                Visible = false;
                invalidate();
            }
        }

        /// <summary>
        /// 键盘按下方法
        /// </summary>
        /// <param name="sender">控件</param>
        /// <param name="key">按键</param>
        /// <returns>是否处理</returns>
        private void textBoxKeyDown(object sender, char key) {
            if (key == 13) {
                onSelectRow();
            }
            else if (key == 38 || key == 40) {
                onKeyDown(key);
            }
        }

        /// <summary>
        /// 文本框输入
        /// </summary>
        /// <param name="sender">控件</param>
        private void textBoxInputChanged(object sender) {
            FCTextBox control = sender as FCTextBox;
            SearchTextBox = control;
            filterSearch();
            String text = control.Text;
            if (text != null && text.Length == 0) {
                Visible = false;
            }
            invalidate();
        }

        /// <summary>
        /// 键盘方法
        /// </summary>
        /// <param name="key">按键</param>
        public override void onKeyDown(char key) {
            base.onKeyDown(key);
            if (key == 13) {
                onSelectRow();
            }
            else if (key == 38 || key == 40) {
                m_grid.onKeyDown(key);
            }
        }
    }
}
