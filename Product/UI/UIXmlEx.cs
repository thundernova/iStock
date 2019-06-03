/*基于FaceCat(捂脸猫)框架 v1.0 https://github.com/FaceCat007/facecat.git
 *  1.捂脸猫创始人-矿洞程序员-上海宁米科技创始人-脉脉KOL-陶德 (微信号:suade1984);
 *  2.联合创始人-上海宁米科技创始人-袁立涛(微信号:wx627378127);
 *  3.恒泰期货投资咨询总监/高级研究员-戴俊生(微信号:18345063201)
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Drawing;
using System.Data;

namespace FaceCat {
    /// <summary>
    /// 股票图形控件Xml解析
    /// </summary>
    public class UIXmlEx : FCUIXml {
        private double m_scaleFactor = 1;

        /// <summary>
        /// 获取或设置缩放因子
        /// </summary>
        public double ScaleFactor {
            get { return m_scaleFactor; }
            set { m_scaleFactor = value; }
        }

        /// <summary>
        /// 创建控件
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="type">类型</param>
        /// <returns>控件</returns>
        public override FCView createControl(XmlNode node, String type) {
            FCNative native = Native;
            if (type == "barragediv") {
                return new BarrageDiv();
            }
            else if (type == "column" || type == "th") {
                return new GridColumnEx();
            }
            else if (type == "ribbonbutton") {
                return new RibbonButton();
            }
            else if (type == "indexdiv") {
                return new IndexDiv();
            }
            else if (type == "windowex") {
                return new WindowEx();
            } else if (type == "indicatorbutton") {
                return new IndicatorButton();
            }
            FCView control = base.createControl(node, type);
            if (control != null) {
                control.Font.m_fontFamily = "微软雅黑";
                if (control is FCCheckBox) {
                    (control as FCCheckBox).ButtonBackColor = FCDraw.FCCOLORS_BACKCOLOR8;
                }
            }
            return control;
        }

        /// <summary>
        /// 创建菜单项
        /// </summary>
        /// <param name="node">节点</param>
        /// <param name="menu">菜单</param>
        /// <param name="parentItem">父节点</param>
        protected override void createMenuItem(XmlNode node, FCMenu menu, FCMenuItem parentItem) {
            FCMenuItem item = new FCMenuItem();
            item.Native = Native;
            item.Font = new FCFont("微软雅黑", 12, false, false, false);
            setAttributesBefore(node, item);
            if (parentItem != null) {
                parentItem.addItem(item);
            }
            else {
                menu.addItem(item);
            }
            if (node.ChildNodes != null && node.ChildNodes.Count > 0) {
                foreach (XmlNode subNode in node.ChildNodes) {
                    createMenuItem(subNode, menu, item);
                }
            }
            setAttributesAfter(node, item);
            onAddControl(item, node);
        }

        /// <summary>
        /// 退出方法
        /// </summary>
        public virtual void exit() {
        }

        /// <summary>
        /// 导出到Excel
        /// </summary>
        /// <param name="path">路径</param>
        public static void exportToExcel(String fileName, FCGrid grid) {
            DataTable dataTable = new DataTable();
            List<FCGridColumn> columns = grid.m_columns;
            int columnsSize = columns.Count;
            for (int i = 0; i < columnsSize; i++) {
                dataTable.Columns.Add(new DataColumn(columns[i].Text));
            }
            List<FCGridRow> rows = grid.m_rows;
            int rowsSize = rows.Count;
            for (int i = 0; i < rowsSize; i++) {
                if (rows[i].Visible) {
                    DataRow dr = dataTable.NewRow();
                    for (int j = 0; j < columnsSize; j++) {
                        FCGridCell cell = grid.m_rows[i].getCell(j);
                        if (cell is FCGridStringCell) {
                            dr[j] = cell.getString();
                        }
                        else {
                            dr[j] = cell.getDouble();
                        }
                    }
                    dataTable.Rows.Add(dr);
                }
            }
            DataCenter.ExportService.ExportDataTableToExcel(dataTable, fileName);
            dataTable.Dispose();
        }

        /// <summary>
        /// 导出到Word
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="html">内容</param>
        public static void exportToWord(String fileName, String html) {
            DataCenter.ExportService.ExportHtmlToWord(fileName, html);
        }

        /// <summary>
        /// 导出到Txt
        /// </summary>
        /// <param name="fileName">文件名</param>
        /// <param name="grid">表格控件</param>
        public static void exportToTxt(String fileName, FCGrid grid) {
            StringBuilder sb = new StringBuilder();
            List<FCGridColumn> columns = grid.m_columns;
            int columnsSize = columns.Count;
            for (int i = 0; i < columnsSize; i++) {
                sb.Append(columns[i].Text);
                if (i != columnsSize - 1) {
                    sb.Append(",");
                }
            }
            List<FCGridRow> rows = grid.m_rows;
            int rowsSize = rows.Count;
            List<FCGridRow> visibleRows = new List<FCGridRow>();
            for (int i = 0; i < rowsSize; i++) {
                if (rows[i].Visible) {
                    visibleRows.Add(rows[i]);
                }
            }
            int visibleRowsSize = visibleRows.Count;
            if (visibleRowsSize > 0) {
                sb.Append("\r\n");
                for (int i = 0; i < visibleRowsSize; i++) {
                    for (int j = 0; j < columnsSize; j++) {
                        FCGridCell cell = visibleRows[i].getCell(j);
                        String cellValue = cell.getString();
                        sb.Append(cellValue);
                        if (j != columnsSize - 1) {
                            sb.Append(",");
                        }
                    }
                    if (i != visibleRowsSize - 1) {
                        sb.Append("\r\n");
                    }
                }
            }
            DataCenter.ExportService.ExportHtmlToTxt(fileName, sb.ToString());
        }

        /// <summary>
        /// 加载XML
        /// </summary>
        /// <param name="xmlPath">XML地址</param>
        public virtual void load(String xmlPath) {
        }

        /// <summary>
        /// 加载数据
        /// </summary>
        public virtual void loadData() {
        }

        /// <summary>
        /// 重置缩放尺寸
        /// </summary>
        /// <param name="clientSize">客户端大小</param>
        public void resetScaleSize(FCSize clientSize) {
            FCNative native = Native;
            if (native != null) {
                FCHost host = native.Host;
                FCSize nativeSize = native.DisplaySize;
                List<FCView> controls = native.getControls();
                int controlsSize = controls.Count;
                for (int i = 0; i < controlsSize; i++) {
                    FCWindowFrame frame = controls[i] as FCWindowFrame;
                    if (frame != null) {
                        WindowEx window = frame.getControls()[0] as WindowEx;
                        if (window != null && !window.AnimateMoving) {
                            FCPoint location = window.Location;
                            if (location.x < 10 || location.x > nativeSize.cx - 10) {
                                location.x = 0;
                            }
                            if (location.y < 30 || location.y > nativeSize.cy - 30) {
                                location.y = 0;
                            }
                            window.Location = location;
                        }
                    }
                }
                native.ScaleSize = new FCSize((int)(clientSize.cx * m_scaleFactor), (int)(clientSize.cy * m_scaleFactor));
                native.update();
            }
        }

        /// <summary>
        /// 是否确认关闭
        /// </summary>
        /// <returns>不处理</returns>
        public virtual int submit() {
            return 0;
        }
    }

    /// <summary>
    /// 窗体XML扩展
    /// </summary>
    public class WindowXmlEx : UIXmlEx {
        /// <summary>
        /// 调用控件方法事件
        /// </summary>
        private FCInvokeEvent m_invokeEvent;

        protected UIXmlEx m_parent;

        /// <summary>
        /// 获取或设置父容器
        /// </summary>
        public UIXmlEx Parent {
            get { return m_parent; }
            set { m_parent = value; }
        }

        protected WindowEx m_window;

        /// <summary>
        /// 获取或设置窗体
        /// </summary>
        public WindowEx Window {
            get { return m_window; }
        }

        /// <summary>
        /// 按钮点击事件
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="mp">坐标</param>
        /// <param name="button">按钮</param>
        /// <param name="click">点击次数</param>
        /// <param name="delta">滚轮滚动值</param>
        private void clickButton(object sender, FCTouchInfo touchInfo) {
            if (touchInfo.m_firstTouch && touchInfo.m_clicks == 1) {
                FCView control = sender as FCView;
                if (m_window != null && control == m_window.CloseButton) {
                    close();
                }
            }
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public virtual void close() {
            m_window.invoke("close");
        }

        /// <summary>
        /// 销毁方法
        /// </summary>
        public override void delete() {
            if (!IsDeleted) {
                if (m_window != null) {
                    m_invokeEvent = null;
                    m_window.close();
                    m_window.delete();
                    m_window = null;
                }
                base.delete();
            }
        }

        /// <summary>
        /// 加载界面
        /// </summary>
        public virtual void load(FCNative native, String xmlName, String windowName) {
            Native = native;
            String xmlPath = DataCenter.getAppPath() + "\\config\\" + xmlName + ".html";
            Script = new FaceCatScript(this);
            loadFile(xmlPath, null);
            m_window = findControl(windowName) as WindowEx;
            m_invokeEvent = new FCInvokeEvent(invoke);
            m_window.addEvent(m_invokeEvent, FCEventID.INVOKE);
            //注册点击事件
            registerEvents(m_window);
        }

        /// <summary>
        /// 调用控件线程方法
        /// </summary>
        /// <param name="sender">调用者</param>
        /// <param name="args">参数</param>
        private void invoke(object sender, object args) {
            onInvoke(args);
        }

        /// <summary>
        /// 调用控件线程方法
        /// </summary>
        /// <param name="args">参数</param>
        public void onInvoke(object args) {
            if (args != null && args.ToString() == "close") {
                delete();
                Native.invalidate();
            }
        }

        /// <summary>
        /// 注册事件
        /// </summary>
        /// <param name="control">控件</param>
        private void registerEvents(FCView control) {
            FCTouchEvent clickButtonEvent = new FCTouchEvent(clickButton);
            List<FCView> controls = control.getControls();
            int controlsSize = controls.Count;
            for (int i = 0; i < controlsSize; i++) {
                FCButton button = controls[i] as FCButton;
                if (button != null) {
                    button.addEvent(clickButtonEvent, FCEventID.CLICK);
                }
                registerEvents(controls[i]);
            }
        }

        /// <summary>
        /// 显示
        /// </summary>
        public virtual void show() {
            m_window.animateShow(false);
            m_window.invalidate();
        }

        /// <summary>
        /// 显示
        /// </summary>
        public virtual void showDialog() {
            m_window.animateShow(true);
            m_window.invalidate();
        }
    }

    /// <summary>
    /// 表格列扩展
    /// </summary>
    public class GridColumnEx : FCGridColumn {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GridColumnEx() {
            Font = new FCFont("微软雅黑", 12, false, false, false);
        }

        /// <summary>
        /// 重绘背景方法
        /// </summary>
        /// <param name="paint">绘图区域</param>
        /// <param name="clipRect">裁剪对象</param>
        public override void onPaintBackground(FCPaint paint, FCRect clipRect) {
            int width = Width, height = Height;
            FCRect drawRect = new FCRect(0, 0, width, height);
            paint.fillGradientRect(FCDraw.FCCOLORS_BACKCOLOR, FCDraw.FCCOLORS_BACKCOLOR2, drawRect, 0, 90);
        }
    }
}
