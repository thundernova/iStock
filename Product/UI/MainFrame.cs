/*基于FaceCat(捂脸猫)框架 v1.0 https://github.com/FaceCat007/facecat.git
 *  1.捂脸猫创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 *  2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 *  3.恒泰期货投资咨询总监/高级研究员-戴俊生(微信号:18345063201)
 */

using System;
using System.Collections.Generic;
using System.Text;
using FaceCat;
using System.Windows.Forms;
using System.Threading;
using System.Diagnostics;
using Newtonsoft.Json;

namespace FaceCat {
    /// <summary>
    /// 管理系统
    /// </summary>
    public class MainFrame : UIXmlEx {
        /// <summary>
        /// 创建行情系统
        /// </summary>
        public MainFrame() {
        }

        /// <summary>
        /// 要添加的代码
        /// </summary>
        private List<String> m_addCodes = new List<String>();

        /// <summary>
        /// 图形
        /// </summary>
        private FCChart m_chartScope;

        /// <summary>
        /// 自选股表格
        /// </summary>
        private FCGrid m_gridUserSecurities;

        /// <summary>
        /// 指标信息
        /// </summary>
        private FCScript m_indicator;

        /// <summary>
        /// 指标数据
        /// </summary>
        public List<IndicatorData> m_indicators = new List<IndicatorData>();

        /// <summary>
        /// 秒表ID
        /// </summary>
        private int m_timerID = FCView.getNewTimerID();

        private BarrageDiv m_barrageDiv;

        /// <summary>
        /// 获取或设置弹幕
        /// </summary>
        public BarrageDiv BarrageDiv {
            get { return m_barrageDiv; }
            set { m_barrageDiv = value; }
        }

        private SearchDiv m_searchDiv;

        /// <summary>
        /// 获取或设置搜索框
        /// </summary>
        public SearchDiv SearchDiv {
            get { return m_searchDiv; }
            set { m_searchDiv = value; }
        }

        /// <summary>
        /// 添加指标
        /// </summary>
        /// <param name="indicatorData"></param>
        public void addIndicator(IndicatorData indicatorData) {
            IndicatorButton indicatorButton = new IndicatorButton();
            indicatorButton.Text = indicatorData.m_name;
            indicatorButton.Tag = indicatorData;
            indicatorButton.Width = 200;
            indicatorButton.IsUser = true;
            indicatorButton.MainFrame = this;
            getDiv("divFunc").addControl(indicatorButton);
        }

        /// <summary>
        /// 设置主图指标
        /// </summary>
        /// <param name="name">名称</param>
        /// <param name="title">标题</param>
        /// <param name="script">脚本</param>
        /// <param name="parameters">参数</param>
        /// <param name="div">图层</param>
        /// <param name="update">是否更新</param>
        /// <returns>指标对象</returns>
        public FCScript addMainIndicator(String name, String title, String script, String parameters, ChartDiv div, bool update) {
            //计算数据
            FCScript indicator = SecurityDataHelper.createIndicator(m_chartScope, m_chartScope.DataSource, script, parameters);
            indicator.Name = name;
            //indicator.FullName = title;
            indicator.AttachVScale = AttachVScale.Left;
            m_indicator = indicator;
            indicator.Div = div;
            indicator.onCalculate(0);
            div.TitleBar.Text = title;
            //刷新图像
            if (update) {
                m_chartScope.update();
                Native.invalidate();
            }
            return indicator;
        }

        /// <summary>
        /// 添加自选股
        /// </summary>
        /// <param name="code">代码</param>
        public void addUserSecurity(UserSecurity userSecurity) {
            UserSecurity us = DataCenter.UserSecurityService.get(userSecurity.m_code);
            if (us != null) {
                userSecurity = us;
            }
            List<FCGridRow> rows = m_gridUserSecurities.m_rows;
            int rowsSize = rows.Count;
            for (int i = 0; i < rowsSize; i++) {
                FCGridRow findRow = rows[i];
                if (findRow.getCell("colP1").getString() == userSecurity.m_code) {
                    findRow.Tag = userSecurity;
                    findRow.getCell("colP11").setDouble(userSecurity.m_sell);
                    findRow.getCell("colP12").setDouble(userSecurity.m_buy);
                    findRow.getCell("colP13").setDouble(userSecurity.m_stop);
                    if (findRow.EditButton != null) {
                        FCView div = findRow.EditButton as FCView;
                        if (div.Parent != null) {
                            div.Parent.removeControl(div);
                        }
                        div.delete();
                        findRow.EditButton = getEditButton(userSecurity);
                    }
                    return;
                }
            }
            FCGridRow row = new FCGridRow();
            row.AllowEdit = true;
            row.Height = 30;
            m_gridUserSecurities.addRow(row);
            row.EditButton = getEditButton(userSecurity);
            row.Tag = userSecurity;
            row.addCell("colP1", new FCGridStringCell(userSecurity.m_code));
            row.addCell("colP2", new FCGridStringCell());
            GridDoubleCellEx cellP3 = new GridDoubleCellEx();
            cellP3.Digit = 2;
            row.addCell("colP3", cellP3);
            GridDoubleCellEx cellP4 = new GridDoubleCellEx();
            cellP4.Digit = 2;
            row.addCell("colP4", cellP4);
            GridDoubleCellEx cellP5 = new GridDoubleCellEx();
            cellP5.Digit = 2;
            cellP5.IsPercent = true;
            row.addCell("colP5", cellP5);
            GridDoubleCellEx cellP6 = new GridDoubleCellEx();
            cellP6.Digit = 2;
            row.addCell("colP6", cellP6);
            GridDoubleCellEx cellP7 = new GridDoubleCellEx();
            cellP7.Digit = 2;
            row.addCell("colP7", cellP7);
            GridDoubleCellEx cellP8 = new GridDoubleCellEx();
            cellP8.Digit = 2;
            row.addCell("colP8", cellP8);
            row.addCell("colP9", new GridDoubleCellEx());
            row.addCell("colP10", new GridDoubleCellEx());
            GridDoubleCellEx cellP11 = new GridDoubleCellEx();
            cellP11.Digit = 2;
            cellP11.setDouble(userSecurity.m_sell);
            row.addCell("colP11", cellP11);
            GridDoubleCellEx cellP12 = new GridDoubleCellEx();
            cellP12.Digit = 2;
            cellP12.setDouble(userSecurity.m_buy);
            row.addCell("colP12", cellP12);

            GridDoubleCellEx cellP13 = new GridDoubleCellEx();
            cellP13.Digit = 2;
            cellP13.setDouble(userSecurity.m_stop);
            row.addCell("colP13", cellP13);

            row.addCell("colP14", new FCGridStringCell(""));
            List<FCGridCell> cells = row.getCells();
            int cellsSize = cells.Count;
            for (int i = 0; i < cellsSize; i++) {
                cells[i].Style = new FCGridCellStyle();
                cells[i].Style.Font = new FCFont("微软雅黑", 14, true, false, false);
                if (i >= 10 && i != 13) {
                    cells[i].AllowEdit = true;
                }
            }
        }

        /// <summary>
        /// 绑定历史数据
        /// </summary>
        /// <param name="historyDatas">历史数据</param>
        private void bindHistoryDatas(List<SecurityData> historyDatas) {
            m_chartScope.clear();
			m_chartScope.update();
			int []fields = new int[7];
			fields[0] = KeyFields.CLOSE_INDEX;
			fields[1] = KeyFields.HIGH_INDEX;
			fields[2] = KeyFields.LOW_INDEX;
			fields[3] = KeyFields.OPEN_INDEX;
			fields[4] = KeyFields.VOL_INDEX;
			fields[5] = KeyFields.AMOUNT_INDEX;
            fields[6] = KeyFields.AVGPRICE_INDEX;
			List<FCScript> indicators = new List<FCScript>();
			SecurityDataHelper.bindHistoryDatas(m_chartScope, m_chartScope.DataSource, indicators, fields, historyDatas);
            FCDataTable dataSource = m_chartScope.DataSource;
            BarShape scopeBar = m_chartScope.getDivs().get(1).getShapes(SortType.ASC).get(0) as BarShape;
            int rowsSize = dataSource.RowsCount;
			for (int i = 0; i < rowsSize; i++){
				double volume = dataSource.get2(i, KeyFields.VOL_INDEX);
                double close = dataSource.get2(i, KeyFields.CLOSE_INDEX);
                double open = dataSource.get2(i, KeyFields.OPEN_INDEX);
				if (close >= open){
					dataSource.set2(i, scopeBar.StyleField, 1);
					dataSource.set2(i, scopeBar.ColorField, FCDraw.FCCOLORS_UPCOLOR);
				}
				else{
					dataSource.set2(i, scopeBar.StyleField, 0);
					dataSource.set2(i, scopeBar.ColorField, FCColor.argb(80, 255, 255));
				}
			}
            if (m_indicator != null) {
                m_indicator.onCalculate(0);
            }
			m_chartScope.update();
			m_chartScope.invalidate();
        }

        /// <summary>
        /// 点击事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="mp">坐标</param>
        /// <param name="button">按钮</param>
        /// <param name="clicks">点击次数</param>
        /// <param name="delta">滚轮值</param>
        private void clickEvent(object sender, FCTouchInfo touchInfo) {
            if (touchInfo.m_firstTouch && touchInfo.m_clicks == 1) {
                FCView control = sender as FCView;
                String name = control.Name;
                if (name == "btnAdd") {
                    String code = findControl("txtCode").Text;
                    Security security = new Security();
                    if (SecurityService.getSecurityByCode(code, ref security) > 0) {
                        UserSecurity userSecurity = new UserSecurity();
                        userSecurity.m_code = code;
                        userSecurity.m_state = 1;
                        DataCenter.UserSecurityService.add(userSecurity);
                        addUserSecurity(userSecurity);
                        m_gridUserSecurities.update();
                        m_gridUserSecurities.invalidate();
                    }
                } else if (name == "btnAddUserSecurity") {
                    List<FCGridRow> selectedRows = m_gridUserSecurities.SelectedRows;
                    int selectedRowsSize = selectedRows.Count;
                    if (selectedRowsSize > 0) {
                        FCGridRow selectedRow = selectedRows[0];
                        UserSecurity userSecurity = selectedRow.Tag as UserSecurity;
                        userSecurity.m_state = 1;
                        DataCenter.UserSecurityService.add(userSecurity);
                        addUserSecurity(userSecurity);
                        m_gridUserSecurities.onRowEditEnd();
                        m_gridUserSecurities.update();
                        m_gridUserSecurities.invalidate();
                    }
                } else if (name == "btnDeleteSecurity") {
                    List<FCGridRow> selectedRows = m_gridUserSecurities.SelectedRows;
                    int selectedRowsSize = selectedRows.Count;
                    if (selectedRowsSize > 0) {
                        FCGridRow selectedRow = selectedRows[0];
                        m_gridUserSecurities.animateRemoveRow(selectedRow);
                        m_gridUserSecurities.onRowEditEnd();
                        String dir = DataCenter.getAppPath() + "\\TradingCodes";
                        FCFile.removeFile(dir + "\\" + selectedRow.getCell("colP1").getString() + "-1");
                        FCFile.removeFile(dir + "\\" + selectedRow.getCell("colP1").getString() + "-2");
                        FCFile.removeFile(dir + "\\" + selectedRow.getCell("colP1").getString() + "-3");
                    }
                } else if (name == "btnDeleteUserSecurity") {
                    List<FCGridRow> selectedRows = m_gridUserSecurities.SelectedRows;
                    int selectedRowsSize = selectedRows.Count;
                    if (selectedRowsSize > 0) {
                        FCGridRow selectedRow = selectedRows[0];
                        UserSecurity userSecurity = selectedRow.Tag as UserSecurity;
                        userSecurity.m_state = 0;
                        DataCenter.UserSecurityService.delete(userSecurity);
                        addUserSecurity(userSecurity);
                        m_gridUserSecurities.onRowEditEnd();
                        m_gridUserSecurities.update();
                        m_gridUserSecurities.invalidate();
                        String dir = DataCenter.getAppPath() + "\\TradingCodes";
                        FCFile.removeFile(dir + "\\" + userSecurity.m_code + "-1");
                        FCFile.removeFile(dir + "\\" + userSecurity.m_code + "-2");
                        FCFile.removeFile(dir + "\\" + userSecurity.m_code + "-3");
                    }
                } else if (name == "btnMergeHistoryDatas") {
                    SecurityService.startWork3();
                    MessageBox.Show("日线落地完成!", "提示");
                } else if (name == "btnActiveStock" || name == "btnSecondNewStock"
                      || name == "btnUpperLimitStock" || name == "btnDownLimitStock"
                      || name == "btnUnTradeStock" || name == "btnSwingStock"
                      || name == "btnLowPriceStock" || name == "btnAmountsStock"
                      || name == "btnVolumesStock" || name == "btnMACDGold"
                      || name == "btnMACDDead" || name == "btnMACDDeviation") {
                    m_gridUserSecurities.clearRows();
                    List<String> codes = new List<String>();
                    if (name == "btnActiveStock") {
                        SecurityService.getActiveCodes(codes, 50);
                    } else if (name == "btnSecondNewStock") {
                        SecurityService.getSecondNewCodes(codes, 50);
                    } else if (name == "btnUpperLimitStock") {
                        SecurityService.getLimitUp(codes);
                    } else if (name == "btnDownLimitStock") {
                        SecurityService.getLimitDown(codes);
                    } else if (name == "btnUnTradeStock") {
                        SecurityService.getNotTradedCodes(codes);
                    } else if (name == "btnSwingStock") {
                        SecurityService.getCodesBySwing(codes, 50);
                    } else if (name == "btnLowPriceStock") {
                        SecurityService.getCodesByPrice(codes, 50);
                    } else if (name == "btnAmountsStock") {
                        SecurityService.getCodesByAmount(codes, 50);
                    } else if (name == "btnVolumesStock") {
                        SecurityService.getCodesByVolume(codes, 50);
                    }
                    int codesSize = codes.Count;
                    m_gridUserSecurities.beginUpdate();
                    for (int i = 0; i < codesSize; i++) {
                        UserSecurity userSecurity = new UserSecurity();
                        userSecurity.m_code = codes[i];
                        addUserSecurity(userSecurity);
                    }
                    m_gridUserSecurities.endUpdate();
                    m_gridUserSecurities.invalidate();
                } else if (name == "btnExport") {
                    exportToTxt("Stocks.txt", m_gridUserSecurities);
                } else if (name == "btnUserSecurity") {
                    m_gridUserSecurities.beginUpdate();
                    m_gridUserSecurities.clearRows();
                    List<UserSecurity> codes = DataCenter.UserSecurityService.m_codes;
                    int codesSize = codes.Count;
                    if (codesSize > 0) {
                        for (int i = 0; i < codesSize; i++) {
                            addUserSecurity(codes[i]);
                        }
                    }
                    m_gridUserSecurities.endUpdate();
                    m_gridUserSecurities.invalidate();
                } else if (name == "btnCreateSelect") {
                    CreateWindow createWindow = new CreateWindow(Native);
                    createWindow.MainFrame = this;
                    createWindow.showDialog();
                }
                if (control is IndicatorButton) {
                    IndicatorButton indicatorButton = control as IndicatorButton;
                    if (indicatorButton.IsUser) {
                        Native.Host.AllowOperate = false;
                        m_gridUserSecurities.clearRows();
                        Thread tThread = new Thread(new ParameterizedThreadStart(startRun));
                        IndicatorData indicatorData = indicatorButton.Tag as IndicatorData;
                        tThread.Start(indicatorData);
                        if (m_indicator != null) {
                            m_indicator.delete();
                        }
                        addMainIndicator(indicatorData.m_name, indicatorData.m_name, indicatorData.m_script, "", m_chartScope.getDivs().get(2), true);
                    }
                }
            }
        }

        /// <summary>
        /// 退出程序
        /// </summary>
        public override void exit() {
        }

        /// <summary>
        /// 获取编辑按钮
        /// </summary>
        /// <param name="userSecurity"></param>
        /// <returns></returns>
        public FCView getEditButton(UserSecurity userSecurity) {
            FCView editButton = new FCView();
            editButton.Height = 30;
            editButton.Width = 200;
            editButton.Native = Native;
            int left = 0;
            FCButton removeButton = new FCButton();
            removeButton.addEvent(new FCTouchEvent(clickEvent), FCEventID.CLICK);
            removeButton.addEvent(new FCTouchEvent(clickEvent), FCEventID.CLICK);
            removeButton.Font = new FCFont("微软雅黑", 16, true, false, false);
            removeButton.BackColor = FCColor.argb(80, 80, 255);
            removeButton.TextColor = FCColor.argb(255, 255, 255);
            if (userSecurity.m_state == 1) {
                removeButton.Text = "移出自选";
                removeButton.Name = "btnDeleteUserSecurity";
            }
            else {
                removeButton.Text = "加入自选";
                removeButton.Name = "btnAddUserSecurity";
            }
            removeButton.Size = new FCSize(100, 30);
            editButton.addControl(removeButton);
            left = 100;

            FCButton deleteButton = new FCButton();
            deleteButton.addEvent(new FCTouchEvent(clickEvent), FCEventID.CLICK);
            deleteButton.Font = new FCFont("微软雅黑", 16, true, false, false);
            deleteButton.BackColor = FCColor.argb(255, 80, 80);
            deleteButton.TextColor = FCColor.argb(255, 255, 255);
            deleteButton.Text = "删除";
            deleteButton.Left = left;
            deleteButton.Name = "btnDeleteSecurity";
            deleteButton.Size = new FCSize(100, 30);
            editButton.addControl(deleteButton);
            return editButton;
        }

        /// <summary>
        /// 单元格双击事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="cell">单元格</param>
        /// <param name="mp">坐标</param>
        /// <param name="button">按钮</param>
        /// <param name="clicks">点击次数</param>
        /// <param name="delta">滚轮值</param>
        private void gridCellClick(object sender, FCGridCell cell, FCTouchInfo touchInfo) {
            if (touchInfo.m_clicks == 2) {
                if (cell.Column.Name != "colP11" && cell.Column.Name != "colP12" && cell.Column.Name != "colP13") {
                    searchSecurity(cell.Row.getCell("colP1").getString());
                }
            }
            else {
                FCTextBox txtCode = getTextBox("txtCode");
                txtCode.Text = cell.Row.getCell("colP1").getString();
                txtCode.invalidate();
            }
        }

        /// <summary>
        /// 单元格编辑结束事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="cell">单元格</param>
        private void gridCellEditEnd(object sender, FCGridCell cell) {
            if (cell != null) {
                String colName = cell.Column.Name;
                String cellValue = cell.getString();
                UserSecurity userSecurity = DataCenter.UserSecurityService.get(cell.Row.getCell("colP1").getString());
                if (userSecurity == null) {
                    userSecurity = new UserSecurity();
                    userSecurity.m_code = cell.Row.getCell("colP1").getString();
                }
                userSecurity.m_state = 1;
                String dir = DataCenter.getAppPath() + "\\TradingCodes";
                if (colName == "colP11") {
                    userSecurity.m_sell = cell.getDouble();
                    cell.Row.Tag = userSecurity;
                    DataCenter.UserSecurityService.add(userSecurity);
                    FCFile.removeFile(dir + "\\" + userSecurity.m_code + "-1");
                }
                else if (colName == "colP12") {
                    userSecurity.m_buy = cell.getDouble();
                    cell.Row.Tag = userSecurity;
                    DataCenter.UserSecurityService.add(userSecurity);
                    FCFile.removeFile(dir + "\\" + userSecurity.m_code + "-2");
                }
                else if (colName == "colP13") {
                    userSecurity.m_stop = cell.getDouble();
                    cell.Row.Tag = userSecurity;
                    DataCenter.UserSecurityService.add(userSecurity);
                    FCFile.removeFile(dir + "\\" + userSecurity.m_code + "-3");
                }
            }
        }

        private void initChart(FCChart chart) {
            FCDataTable dataSource = chart.DataSource;
            chart.BorderColor = FCDraw.FCCOLORS_LINECOLOR4;
            chart.CanMoveShape = true;
            chart.ScrollAddSpeed = true;
            chart.LeftVScaleWidth = 85;
            chart.RightVScaleWidth = 85;
            chart.HScalePixel = 10;
            chart.HScaleFieldText = "日期";
            ChartDiv candleDiv = chart.getDivs().get(0);
            candleDiv.TitleBar.Text = "日线";
            candleDiv.VGrid.Visible = true;
            candleDiv.LeftVScale.NumberStyle = NumberStyle.UnderLine;
            candleDiv.LeftVScale.PaddingTop = 2;
            candleDiv.LeftVScale.PaddingBottom = 2;
            FCFont vFont = new FCFont("Arial", 14, false, false, false);
            candleDiv.LeftVScale.Font = vFont;
            candleDiv.RightVScale.NumberStyle = NumberStyle.UnderLine;
            candleDiv.RightVScale.Font = vFont;
            candleDiv.RightVScale.PaddingTop = 2;
            candleDiv.RightVScale.PaddingBottom = 2;
            ChartTitle priceTitle = new ChartTitle(KeyFields.CLOSE_INDEX, "CLOSE", FCDraw.FCCOLORS_FORECOLOR, 2, true);
            priceTitle.FieldTextMode = TextMode.Value;
            candleDiv.TitleBar.Titles.add(priceTitle);
            CandleShape candle = new CandleShape();
            candleDiv.addShape(candle);
            candle.CloseField = KeyFields.CLOSE_INDEX;
            candle.OpenField = KeyFields.OPEN_INDEX;
            candle.HighField = KeyFields.HIGH_INDEX;
            candle.LowField = KeyFields.LOW_INDEX;
            candle.CloseFieldText = "收盘";
            candle.HighFieldText = "最高";
            candle.LowFieldText = "最低";
            candle.OpenFieldText = "开盘";
            ChartDiv volumeDiv = chart.getDivs().get(1);
            volumeDiv.VGrid.Distance = 30;
            volumeDiv.LeftVScale.Digit = 0;
            volumeDiv.LeftVScale.Font = vFont;
            volumeDiv.RightVScale.Digit = 0;
            volumeDiv.RightVScale.Font = vFont;
            BarShape bar = new BarShape();
            int styleField = FCDataTable.AutoField;
            int colorField = FCDataTable.AutoField;
            bar.ColorField = colorField;
            bar.StyleField = styleField;
            bar.FieldName = KeyFields.VOL_INDEX;
            bar.FieldText = KeyFields.VOL;
            volumeDiv.addShape(bar);
            volumeDiv.TitleBar.Text = "成交量";
            ChartTitle barTitle = new ChartTitle(KeyFields.VOL_INDEX, "成交量", bar.DownColor, 0, true);
            barTitle.FieldTextMode = TextMode.Value;
            volumeDiv.TitleBar.Titles.add(barTitle);
            volumeDiv.LeftVScale.TextColor = FCDraw.FCCOLORS_FORECOLOR2;
            volumeDiv.RightVScale.TextColor = FCDraw.FCCOLORS_FORECOLOR2;
            dataSource.addColumn(KeyFields.CLOSE_INDEX);
            dataSource.addColumn(KeyFields.OPEN_INDEX);
            dataSource.addColumn(KeyFields.HIGH_INDEX);
            dataSource.addColumn(KeyFields.LOW_INDEX);
            dataSource.addColumn(KeyFields.VOL_INDEX);
            dataSource.addColumn(KeyFields.AMOUNT_INDEX);
            dataSource.addColumn(KeyFields.AVGPRICE_INDEX);
            dataSource.addColumn(colorField);
            dataSource.addColumn(styleField);
            dataSource.setColsCapacity(16);
            dataSource.setColsGrowStep(4);
            ChartDiv indDiv = chart.getDivs().get(2);
            indDiv.VGrid.Distance = 40;
            indDiv.LeftVScale.PaddingTop = 2;
            indDiv.LeftVScale.PaddingBottom = 2;
            indDiv.LeftVScale.Font = new FCFont("Arial", 14, false, false, false);
            indDiv.RightVScale.PaddingTop = 2;
            indDiv.RightVScale.PaddingBottom = 2;
            indDiv.RightVScale.Font = new FCFont("Arial", 14, false, false, false);
            //设置X轴不可见
            candleDiv.HScale.Visible = false;
            candleDiv.HScale.Height = 0;
            volumeDiv.HScale.Visible = false;
            volumeDiv.HScale.Height = 0;
            indDiv.HScale.Visible = true;
            indDiv.HScale.Height = 22;
            indDiv.LeftVScale.TextColor = FCColor.argb(255, 255, 255);
            indDiv.RightVScale.TextColor = FCColor.argb(255, 255, 255);
            addMainIndicator("KDJ", "KDJ", "RSV:=(CLOSE-LLV(LOW,9))/(HHV(HIGH,N)-LLV(LOW,9))*100; K:SMA(RSV,3,1); D:SMA(K,3,1);J:3*K-2*D;", "", indDiv, true);
        }

        /// <summary>
        /// 是否有窗体显示
        /// </summary>
        /// <returns>是否显示</returns>
        public bool isWindowShowing() {
            List<FCView> controls = Native.getControls();
            int controlsSize = controls.Count;
            for (int i = 0; i < controlsSize; i++) {
                FCWindowFrame frame = controls[i] as FCWindowFrame;
                if (frame != null) {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 加载XML
        /// </summary>
        /// <param name="xmlPath">XML路径</param>
        public override void load(String xmlPath) {
            loadFile(xmlPath, null);
            FCView control = Native.getControls()[0];
            control.BackColor = FCColor.Back;
            String dir = DataCenter.getAppPath() + "\\TradingCodes";
            FCFile.createDirectory(dir);
            m_gridUserSecurities = getGrid("gridUserSecurities");
            //m_gridUserSecurities.UseAnimation = true;
            m_gridUserSecurities.GridLineColor = FCColor.None;
            m_gridUserSecurities.BackColor = FCColor.argb(0, 0, 0);
            m_gridUserSecurities.RowStyle = new FCGridRowStyle();
            m_gridUserSecurities.RowStyle.BackColor = FCColor.argb(0, 0, 0);
            m_gridUserSecurities.addEvent(new FCTimerEvent(timerEvent), FCEventID.TIMER);
            m_gridUserSecurities.startTimer(m_timerID, 1000);
            m_gridUserSecurities.addEvent(new FCGridCellTouchEvent(gridCellClick), FCEventID.GRIDCELLCLICK);
            m_gridUserSecurities.addEvent(new FCGridCellEvent(gridCellEditEnd), FCEventID.GRIDCELLEDITEND);
            DataCenter.MainUI = this;
            (findControl("divIndex") as IndexDiv).MainFrame = this;
            List<UserSecurity> codes = DataCenter.UserSecurityService.m_codes;
            m_gridUserSecurities.beginUpdate();
            int codesSize = codes.Count;
            if (codesSize > 0) {
                for (int i = 0; i < codesSize; i++) {
                    addUserSecurity(codes[i]);
                }
                Security security = new Security();
                SecurityService.getSecurityByCode(codes[0].m_code, ref security);
            }
            m_gridUserSecurities.endUpdate();
            m_gridUserSecurities.invalidate();
            m_barrageDiv = new BarrageDiv();
            m_barrageDiv.TopMost = true;
            m_barrageDiv.Dock = FCDockStyle.Fill;
            m_barrageDiv.MainFrame = this;
            m_chartScope = findControl("chartScope") as FCChart;
            initChart(m_chartScope);
            String indicatorStr = "";
            String path = Application.StartupPath + "\\indicators.txt";
            if (FCFile.isFileExist(path)) {
                FCFile.read(path, ref indicatorStr);
                m_indicators = JsonConvert.DeserializeObject<List<IndicatorData>>(indicatorStr);
                foreach (IndicatorData data in m_indicators) {
                    addIndicator(data);
                }
            }
            Native.addControl(m_barrageDiv);
            registerEvents(control);
        }

        /// <summary>
        /// 秒表方法
        /// </summary>
        public void onTimer() {
            List<String> newCodes = new List<string>();
            lock (m_addCodes) {
                foreach (String code in m_addCodes) {
                    newCodes.Add(code);
                }
                m_addCodes.Clear();
            }
            if (newCodes.Count > 0) {
                m_gridUserSecurities.beginUpdate();
                foreach (String addCode in newCodes) {
                    UserSecurity userSecurity = new UserSecurity();
                    userSecurity.m_code = addCode;
                    addUserSecurity(userSecurity);
                }
                m_gridUserSecurities.endUpdate();
                m_gridUserSecurities.invalidate();
            }
        }

        /// <summary>
        /// 重绘菜单布局
        /// </summary>
        /// <param name="sender">调用对象</param>
        /// <param name="paint">绘图对象</param>
        /// <param name="clipRect">裁剪区域</param>
        private void paintLayoutDiv(object sender, FCPaint paint, FaceCat.FCRect clipRect) {
            FCView control = sender as FCView;
            int width = control.Width, height = control.Height;
            FaceCat.FCRect drawRect = new FaceCat.FCRect(0, 0, width, height);
            paint.fillGradientRect(FCDraw.FCCOLORS_BACKCOLOR, FCDraw.FCCOLORS_BACKCOLOR2, drawRect, 0, 90);
        }

        /// 注册事件
        /// </summary>
        /// <param name="control">控件</param>
        private void registerEvents(FCView control) {
            FCTouchEvent clickButtonEvent = new FCTouchEvent(clickEvent);
            List<FCView> controls = control.getControls();
            int controlsSize = controls.Count;
            for (int i = 0; i < controlsSize; i++) {
                FCView subControl = controls[i];
                FCButton button = subControl as FCButton;
                if (button != null) {
                    button.addEvent(clickButtonEvent, FCEventID.CLICK);
                }
                registerEvents(controls[i]);
            }
        }

        /// <summary>
        /// 移除指标
        /// </summary>
        /// <param name="indicatorData">指标数据</param>
        public void removeIndicator(IndicatorData indicatorData) {
            FCDiv div = getDiv("divFunc");
            ArrayList<FCView> controls = div.m_controls;
            for (int i = 0; i < controls.size(); i++) {
                FCView control = controls.get(i);
                if (control is IndicatorButton) {
                    if (control.Tag == indicatorData) {
                        div.removeControl(control);
                        control.delete();
                        break;
                    }
                }
            }
            int indicatorDatasSize = m_indicators.Count;
            for (int i = 0; i < indicatorDatasSize; i++) {
                if (m_indicators[i] == indicatorData) {
                    m_indicators.RemoveAt(i);
                    break;
                }
            }
            String path = Application.StartupPath + "\\indicators.txt";
            String content = JsonConvert.SerializeObject(m_indicators);
            FCFile.write(path, content);
            div.update();
            Native.update();
            Native.invalidate();
        }

        /// <summary>
        /// 搜索股票
        /// </summary>
        /// <param name="code">代码</param>
        public void searchSecurity(String code) {
            Security security = new Security();
            SecurityService.getSecurityByCode(code, ref security);
            //if(security.m_type == 1 || security.m_type==2)
            {
                getTabControl("tabMain").SelectedIndex = 1;
                List<SecurityData> datas = new List<SecurityData>();
                if(SecurityService.m_historyDatas.ContainsKey(code)){
                    datas = SecurityService.m_historyDatas[code];
                }
                SecurityLatestData latestData = null;
                if (SecurityService.m_latestDatas.ContainsKey(code)) {
                    latestData = SecurityService.m_latestDatas[code];
                }
                if (latestData != null) {
                    SecurityData newData = new SecurityData();
                    SecurityFilterExternFunc.getSecurityData(latestData, latestData.m_lastClose, 1440, 0, ref newData);
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
                bindHistoryDatas(datas);
            }
        }

        /// <summary>
        /// 显示提示窗口
        /// </summary>
        /// <param name="text">文本</param>
        /// <param name="caption">标题</param>
        /// <param name="uType">格式</param>
        /// <returns>结果</returns>
        public int showMessageBox(String text, String caption, int uType) {
            MessageBox.Show(text, caption);
            return 1;
        }

        /// <summary>
        /// 显示键盘精灵层
        /// </summary>
        /// <param name="key">按键</param>
        public void showSearchDiv(char key) {
            FCView focusedControl = Native.FocusedControl;
            if (focusedControl != null) {
                String name = focusedControl.Name;
                if (isWindowShowing() && name != "txtCode") {
                    return;
                }
                if (!(focusedControl is FCTextBox) || (m_searchDiv != null && focusedControl == m_searchDiv.SearchTextBox)
                    || name == "txtCode") {
                    Keys keyData = (Keys)key;
                    //创建键盘精灵
                    if (m_searchDiv == null) {
                        m_searchDiv = new SearchDiv();
                        m_searchDiv.Popup = true;
                        m_searchDiv.Size = new FCSize(240, 200);
                        m_searchDiv.Visible = false;
                        Native.addControl(m_searchDiv);
                        m_searchDiv.bringToFront();
                        m_searchDiv.MainFrame = this;
                    }
                    //退出
                    if (keyData == Keys.Escape) {
                        m_searchDiv.Visible = false;
                        m_searchDiv.invalidate();
                    }
                    //输入
                    else {
                        if (!m_searchDiv.Visible) {
                            char ch = '\0';
                            if ((keyData >= Keys.D0) && (keyData <= Keys.D9)) {
                                ch = (char)((0x30 + keyData) - 0x30);
                            }
                            else if ((keyData >= Keys.A) && (keyData <= Keys.Z)) {
                                ch = (char)((0x41 + keyData) - 0x41);
                            }
                            else if ((keyData >= Keys.NumPad0) && (keyData <= Keys.NumPad9)) {
                                ch = (char)((0x30 + keyData) - 0x60);
                            }
                            if (ch != '\0') {
                                FCSize size = Native.Host.getSize();
                                FCPoint location = new FCPoint(size.cx - m_searchDiv.Width, size.cy - m_searchDiv.Height);
                                if (name == "txtCode") {
                                    FCPoint fPoint = new FCPoint(0, 0);
                                    fPoint = focusedControl.pointToNative(fPoint);
                                    location = new FCPoint(fPoint.x, fPoint.y + focusedControl.Height);
                                    m_searchDiv.Location = location;
                                    m_searchDiv.SearchTextBox.Text = "";
                                    m_searchDiv.filterSearch();
                                    m_searchDiv.Visible = true;
                                    m_searchDiv.SearchTextBox.Focused = true;
                                    m_searchDiv.update();
                                    m_searchDiv.invalidate();
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 开始执行
        /// </summary>
        /// <param name="args"></param>
        private void startRun(object args) {
            IndicatorData indicatorData = args as IndicatorData;
            String script = indicatorData.m_script;
            StringBuilder sb = new StringBuilder();
            int id = SecurityFilterExternFunc.createIndicatorExtern(script, "", sb);
            int pos = 0;
            m_barrageDiv.Progress = 0;
            int lastProgress = 0;
            int totalSize = SecurityService.m_codedMap.Count;
            foreach (String code in SecurityService.m_codedMap.Keys) {
                double result = 0;
                SecurityFilterExternFunc.calculateIndicatorExtern(id, code, ref result);
                if (result != 0) {
                    lock (m_addCodes) {
                        m_addCodes.Add(code);
                    }
                }
                int newProgress = 100 * (pos + 1) / totalSize;
                if (newProgress != lastProgress) {
                    lastProgress = newProgress;
                    m_barrageDiv.Progress = newProgress;
                    m_barrageDiv.ProgressText = "正在计算" + indicatorData.m_name + " " + (pos + 1).ToString() + "/" + totalSize.ToString();
                }
                pos++;
            }
            m_barrageDiv.Progress = -1;
            SecurityFilterExternFunc.deleteIndicatorExtern(id);
            Barrage barrage = new Barrage();
            barrage.Text = "执行完成";
            m_barrageDiv.addBarrage(barrage);
            Native.Host.AllowOperate = true;
        }

        /// <summary>
        /// 秒表事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="sender">秒表ID</param>
        private void timerEvent(object sender, int timerID) {
            if (timerID == m_timerID) {
                List<FCGridRow> rows = m_gridUserSecurities.m_rows;
                int rowsSize = rows.Count;
                int columnsSize = m_gridUserSecurities.m_columns.Count;
                for (int i = 0; i < rowsSize; i++) {
                    FCGridRow row = rows[i];
                    Dictionary<String, FCGridCell> cellsMap = new Dictionary<string, FCGridCell>();
                    foreach (FCGridColumn column in m_gridUserSecurities.m_columns) {
                        cellsMap[column.Name] = row.getCell(column.Name);
                    }
                    String code = cellsMap["colP1"].getString();
                    Security security = new Security();
                    cellsMap["colP1"].Style.TextColor = FCColor.argb(255, 255, 255);
                    SecurityLatestData latestData = new SecurityLatestData();
                    SecurityService.getSecurityByCode(code, ref security);
                    SecurityService.getLatestData(code, ref latestData);
                    cellsMap["colP2"].setString(security.m_name);
                    UserSecurity userSecurity = row.Tag as UserSecurity;
                    if (userSecurity.m_state == 1) {
                        cellsMap["colP2"].Style.TextColor = FCColor.argb(255, 80, 255);
                    }
                    else {
                        cellsMap["colP2"].Style.TextColor = FCColor.argb(255, 255, 80);
                    }
                    double diff = 0, diffRange = 0;
                    if (latestData.m_lastClose != 0) {
                        diff = latestData.m_close - latestData.m_lastClose;
                        diffRange = 100 * (latestData.m_close - latestData.m_lastClose) / latestData.m_lastClose;
                    }
                    cellsMap["colP3"].setDouble(latestData.m_close);
                    cellsMap["colP3"].Style.TextColor = FCDraw.getPriceColor(latestData.m_close, latestData.m_lastClose);
                    cellsMap["colP4"].setDouble(diff);
                    cellsMap["colP4"].Style.TextColor = FCDraw.getPriceColor(latestData.m_close, latestData.m_lastClose);
                    cellsMap["colP5"].setDouble(diffRange);
                    cellsMap["colP5"].Style.TextColor = FCDraw.getPriceColor(latestData.m_close, latestData.m_lastClose);
                    cellsMap["colP6"].setDouble(latestData.m_high);
                    cellsMap["colP6"].Style.TextColor = FCDraw.getPriceColor(latestData.m_high, latestData.m_lastClose);
                    cellsMap["colP7"].setDouble(latestData.m_low);
                    cellsMap["colP7"].Style.TextColor = FCDraw.getPriceColor(latestData.m_low, latestData.m_lastClose);
                    cellsMap["colP8"].setDouble(latestData.m_open);
                    cellsMap["colP8"].Style.TextColor = FCDraw.getPriceColor(latestData.m_open, latestData.m_lastClose);
                    cellsMap["colP9"].setDouble(latestData.m_volume);
                    cellsMap["colP9"].Style.TextColor = FCColor.argb(80, 255, 255);
                    cellsMap["colP10"].setDouble(latestData.m_amount);
                    cellsMap["colP10"].Style.TextColor = FCColor.argb(80, 255, 255);
                    cellsMap["colP11"].Style.TextColor = FCColor.argb(255, 80, 80);
                    cellsMap["colP12"].Style.TextColor = FCColor.argb(80, 255, 80);
                    cellsMap["colP13"].Style.TextColor = FCColor.argb(255, 80, 255);
                    cellsMap["colP14"].Style.TextColor = FCColor.argb(255, 255, 80);
                    bool alarm1 = false, alarm2 = false, alarm3 = false;
                    if (latestData.m_close > 0) {
                        double up = userSecurity.m_sell, down = userSecurity.m_buy, stop = userSecurity.m_stop;
                        if (up != 0) {
                            if (latestData.m_close > up) {
                                Sound.play("alarm.wav");
                                Barrage barrage = new Barrage();
                                barrage.Text = latestData.m_code + "超过自动卖出价了!";
                                m_barrageDiv.addBarrage(barrage);
                                alarm1 = true;
                            }
                        }
                        if (down != 0) {
                            if (latestData.m_close < down) {
                                Sound.play("alarm.wav");
                                Barrage barrage = new Barrage();
                                barrage.Text = latestData.m_code + "低于自动买入价了!";
                                m_barrageDiv.addBarrage(barrage);
                                alarm2 = true;
                            }
                        }
                        if (stop != 0) {
                            if (latestData.m_close < stop) {
                                Sound.play("alarm.wav");
                                Barrage barrage = new Barrage();
                                barrage.Text = latestData.m_code + "低于止损位了!";
                                m_barrageDiv.addBarrage(barrage);
                                alarm3 = true;
                            }
                        }
                    }
                    List<FCGridCell> cells = row.getCells();
                    int cellsSize = cells.Count;
                    String dir = DataCenter.getAppPath() + "\\TradingCodes";
                    for (int j = 0; j < cellsSize; j++) {
                        FCGridCell cCell = cells[j];
                        if (alarm1) {
                            cCell.Style.BackColor = FCColor.argb(100, 255, 80, 80);
                            String filePath = dir + "\\" + latestData.m_code + "-1";
                            if (!FCFile.isFileExist(filePath)) {
                                OrderInfo info = new OrderInfo();
                                info.m_code = FCStrEx.convertDBCodeToDealCode(latestData.m_code);
                                info.m_price = (float)Math.Round(latestData.m_close, 2);
                                info.m_qty = 100;
                                Thread.Sleep(3000);
                                FCFile.write(filePath, " ");
                            }
                        }
                        if (alarm2) {
                            cCell.Style.BackColor = FCColor.argb(100, 80, 255, 80);
                            String filePath = dir + "\\" + latestData.m_code + "-2";
                            if (!FCFile.isFileExist(filePath)) {
                                OrderInfo info = new OrderInfo();
                                info.m_code = FCStrEx.convertDBCodeToDealCode(latestData.m_code);
                                info.m_price = (float)Math.Round(latestData.m_close, 2);
                                info.m_qty = 100;
                                Thread.Sleep(3000);
                                FCFile.write(filePath, " ");
                            }
                        }
                        if (alarm3) {
                            cCell.Style.BackColor = FCColor.argb(100, 255, 80, 255);
                            String filePath = dir + "\\" + latestData.m_code + "-3";
                            if (!FCFile.isFileExist(filePath)) {
                                OrderInfo info = new OrderInfo();
                                info.m_code = FCStrEx.convertDBCodeToDealCode(latestData.m_code);
                                info.m_price = (float)Math.Round(latestData.m_close, 2);
                                info.m_qty = 100;
                                Thread.Sleep(3000);
                                FCFile.write(filePath, " ");
                            }
                        }
                        if (!alarm1 && !alarm2 && !alarm3) {
                            cCell.Style.BackColor = FCColor.None;
                        }
                    }
                }
                m_gridUserSecurities.invalidate();
            }
        }
    }

    public class GridDoubleCellEx : FCGridDoubleCell {
        private int m_digit;

        /// <summary>
        /// 获取或设置保留位数
        /// </summary>
        public int Digit {
            get { return m_digit; }
            set { m_digit = value; }
        }

        private bool m_isPercent;

        /// <summary>
        /// 获取或设置是否百分比
        /// </summary>
        public bool IsPercent {
            get { return m_isPercent; }
            set { m_isPercent = value; }
        }

        /// <summary>
        /// 获取绘制文字
        /// </summary>
        /// <returns>绘制文字</returns>
        public override String getPaintText() {
            String text = FCStr.getValueByDigit(m_value, m_digit);
            if (m_isPercent) {
                text += "%";
            }
            return text;
        }
    }
}
