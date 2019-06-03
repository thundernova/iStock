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
using FaceCat;
using Newtonsoft.Json;

namespace FaceCat {
    /// <summary>
    /// 创建窗体
    /// </summary>
    public class CreateWindow : WindowXmlEx {
        /// <summary>
        /// 创建登录窗体
        /// </summary>
        /// <param name="native">方法库</param>
        public CreateWindow(FCNative native) {
            load(native, "CreateWindow", "windowCreate");
            //注册点击事件
            registerEvents(m_window);
        }

        private IndicatorData m_indicatorData;

        /// <summary>
        /// 获取或设置指标数据
        /// </summary>
        public IndicatorData IndicatorData {
            get { return m_indicatorData; }
            set { m_indicatorData = value; }
        }

        private MainFrame m_mainFrame;

        /// <summary>
        /// 获取或设置行情控件
        /// </summary>
        public MainFrame MainFrame {
            get { return m_mainFrame; }
            set { m_mainFrame = value; }
        }

        private bool m_isEdit;

        /// <summary>
        /// 获取或设置是否是编辑状态
        /// </summary>
        public bool IsEdit {
            get { return m_isEdit; }
            set { m_isEdit = value; }
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
                String name = control.Name;
                if (name == "btnCancel") {
                    close();
                } else if (name == "btnOK") {
                    FCTextBox txtName = getTextBox("txtName");
                    FCTextBox txtScript = getTextBox("txtScript");
                    List<IndicatorData> indicatorDatas = m_mainFrame.m_indicators;
                    IndicatorData newIndicatorData = null;
                    //判断是否重复
                    int indicatorDatasSize = indicatorDatas.Count;
                    for (int i = 0; i < indicatorDatasSize; i++) {
                        IndicatorData indicatorData = indicatorDatas[i];
                        if (indicatorData.m_name == txtName.Text) {
                            if (m_isEdit) {
                                newIndicatorData = indicatorData;
                            } else {
                                MessageBox.Show("该指标名称已经存在!", "提示");
                                return;
                            }
                        }
                    }
                    //添加指标
                    if (!m_isEdit) {
                        newIndicatorData = new IndicatorData();
                        indicatorDatas.Add(newIndicatorData);
                    }
                    newIndicatorData.m_name = txtName.Text;
                    newIndicatorData.m_script = txtScript.Text;
                    String path = Application.StartupPath + "\\indicators.txt";
                    String content = JsonConvert.SerializeObject(indicatorDatas);
                    FCFile.write(path, content);
                    if (!m_isEdit) {
                        m_mainFrame.addIndicator(newIndicatorData);
                    }
                    close();
                    Native.update();
                    Native.invalidate();
                } else if (name == "lblDelete") {
                    m_mainFrame.removeIndicator(m_indicatorData);
                    close();
                }
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
                FCLabel linkLabel = controls[i] as FCLabel;
                if (button != null) {
                    button.addEvent(clickButtonEvent, FCEventID.CLICK);
                }
                else if (linkLabel != null) {
                    linkLabel.addEvent(clickButtonEvent, FCEventID.CLICK);
                }
                registerEvents(controls[i]);
            }
        }

        /// <summary>
        /// 显示方法
        /// </summary>
        public override void showDialog() {
            base.showDialog();
            FCButton btnOK = getButton("btnOK");
            FCButton btnCancel = getButton("btnCancel");
            FCLabel lblDelete = getLabel("lblDelete");
            if (m_isEdit) {
                FCTextBox txtName = getTextBox("txtName");
                FCTextBox txtScript = getTextBox("txtScript");
                txtName.Text = m_indicatorData.m_name;
                txtScript.Text = m_indicatorData.m_script;
                txtName.Enabled = false;
                lblDelete.Visible = true;
                btnOK.Text = "修改";
                m_window.Text = "修改指标";
            } else {
                lblDelete.Visible = false;
                btnOK.Text = "新增";
                m_window.Height -= 50;
            }
            Native.invalidate();
        }
    }
}
