using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Security;
namespace GhostWinterCaptureImage
{
    
    [Guid("73262EF5-1F9F-4217-B4A0-77F9597171EE")]
    public partial class Capture : UserControl,IObjectSafety
    {
        private const int HOTKEY_ID = 1000;
        private const uint WM_HOTKEY = 0x312;
        private const uint MOD_ALT = 0x1;
        private const uint MOD_CONTROL = 0x2;
        private const uint MOD_SHIFT = 0x4;
        private FrmCapture frmCapture;
        public Capture()
        {
            Win32.RegisterHotKey(this.Handle, HOTKEY_ID, MOD_ALT | MOD_CONTROL, (int)Keys.P);
            InitializeComponent();
        }
        #region IObjectSafety 成员

        private const string _IID_IDispatch = "{00020400-0000-0000-C000-000000000046}";
        private const string _IID_IDispatchEx = "{a6ef9860-c720-11d0-9337-00a0c90dcaa9}";
        private const string _IID_IPersistStorage = "{0000010A-0000-0000-C000-000000000046}";
        private const string _IID_IPersistStream = "{00000109-0000-0000-C000-000000000046}";
        private const string _IID_IPersistPropertyBag = "{37D84F60-42CB-11CE-8135-00AA004BB851}";

        private const int INTERFACESAFE_FOR_UNTRUSTED_CALLER = 0x00000001;
        private const int INTERFACESAFE_FOR_UNTRUSTED_DATA = 0x00000002;
        private const int S_OK = 0;
        private const int E_FAIL = unchecked((int)0x80004005);
        private const int E_NOINTERFACE = unchecked((int)0x80004002);

        private bool _fSafeForScripting = true;
        private bool _fSafeForInitializing = true;

        [SecurityCritical]
        public int GetInterfaceSafetyOptions(ref Guid riid, ref int pdwSupportedOptions, ref int pdwEnabledOptions)
        {
            int Rslt = E_FAIL;

            string strGUID = riid.ToString("B");
            pdwSupportedOptions = INTERFACESAFE_FOR_UNTRUSTED_CALLER | INTERFACESAFE_FOR_UNTRUSTED_DATA;
            switch (strGUID)
            {
                case _IID_IDispatch:
                case _IID_IDispatchEx:
                    Rslt = S_OK;
                    pdwEnabledOptions = 0;
                    if (_fSafeForScripting == true)
                        pdwEnabledOptions = INTERFACESAFE_FOR_UNTRUSTED_CALLER;
                    break;
                case _IID_IPersistStorage:
                case _IID_IPersistStream:
                case _IID_IPersistPropertyBag:
                    Rslt = S_OK;
                    pdwEnabledOptions = 0;
                    if (_fSafeForInitializing == true)
                        pdwEnabledOptions = INTERFACESAFE_FOR_UNTRUSTED_DATA;
                    break;
                default:
                    Rslt = E_NOINTERFACE;
                    break;
            }

            return Rslt;
        }
        [SecurityCritical]
        public int SetInterfaceSafetyOptions(ref Guid riid, int dwOptionSetMask, int dwEnabledOptions)
        {
            int Rslt = E_FAIL;

            string strGUID = riid.ToString("B");
            switch (strGUID)
            {
                case _IID_IDispatch:
                case _IID_IDispatchEx:
                    if (((dwEnabledOptions & dwOptionSetMask) == INTERFACESAFE_FOR_UNTRUSTED_CALLER) &&
                         (_fSafeForScripting == true))
                        Rslt = S_OK;
                    break;
                case _IID_IPersistStorage:
                case _IID_IPersistStream:
                case _IID_IPersistPropertyBag:
                    if (((dwEnabledOptions & dwOptionSetMask) == INTERFACESAFE_FOR_UNTRUSTED_DATA) &&
                         (_fSafeForInitializing == true))
                        Rslt = S_OK;
                    break;
                default:
                    Rslt = E_NOINTERFACE;
                    break;
            }

            return Rslt;
        }

        #endregion
        private void button1_Click(object sender, EventArgs e)
        {
          
            CapturePic();
        }
        /// <summary>
        /// 截图
        /// </summary>
        public void CapturePic()
        {
            
            if (frmCapture == null || frmCapture.IsDisposed)
                frmCapture = new FrmCapture();
            frmCapture.IsCaptureCursor = true;
            frmCapture.Show();
            
            
        }
        
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == WM_HOTKEY)
            {
                this.CapturePic();
                
            }
            
            base.WndProc(ref m);
        }
        public int Exec(string filename, string args="")
        {
            try
            {
                System.Diagnostics.Process process = new System.Diagnostics.Process();
                System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
                startInfo.FileName = filename;
                startInfo.Arguments = args;
                process.StartInfo = startInfo;
                process.Start();
                return 1;
            }
            catch
            {
                return 0;
            }
        }
        public void LoadReport()
        {
            //editor e = new editor("<emrtextdoc version=\"1.0\" checkcount=\"0\" senior=\"\"><underwritemarks /><savelogs /><headstring></headstring><headheight>0</headheight><footerstring>&lt;xml&gt;    &lt;textimg fontsize=\"16\"&gt;  &lt;text&gt;             报告医师：&lt;/text&gt;    &lt;img id=\"sign1\"&gt;    &lt;/img&gt;    &lt;text&gt;审核医师：&lt;/text&gt;    &lt;img id=\"sign2\"&gt;&lt;/img&gt;  &lt;/textimg&gt;      &lt;line thick=\"2\" fontsize=\"16\"&gt;&lt;/line&gt;&lt;text fontname=\"宋体_GB2312\" fontsize=\"10\" center=\"center\" &gt;备注：该诊断结果仅供临床医生在诊断、治疗疾病时参考，不做任何法律依据&lt;/text&gt;&lt;/xml&gt;</footerstring><footerheight>318</footerheight><docsetting title=\"模板库.数字胃肠报告.数字胃肠报告单\" id=\"M01040000001\" modifytime=\"20121127123818\" version=\"1.0\" modifier=\"-1\" filename=\"C:\\Users\\Administrator\\Desktop\\rf.xml\" /><script runpwd=\"\" editpwd=\"\"><![CDATA[]]></script><text>#hospitalname#  数 字 胃 肠 报 告 单  检查号：#studynum#       卡号：#zh#    #rylb#  姓名:#xm#      性别:#xb#    年龄:#nl#      科室：#kksmc#  病室：#bs# 床号：#ch# 检查日期：#studydate# 审核日期：#checkdate#  病情摘要：#illsummary#  临床诊?希?ClinicalDiagnosis#  透视所见：检查描述     透视诊断：诊断提示     </text><body id=\"C349FED73\"><text>#hospitalname#  数 字 胃 肠 报 告 单  检查号：#studynum#       卡号：#zh#    #rylb#  姓名:#xm#      性别:#xb#    年龄:#nl#      科室：#kksmc#  病室：#bs# 床号：#ch# 检查日期：#studydate# 审核日期：#checkdate#  病情摘要：#illsummary#  临床诊断：#ClinicalDiagnosis#  透视所见：    透视诊断：    </text><img src=\"C:\\Documents and Settings\\Administrator\\桌面\\医疗标志.jpg\" type=\"\" width=\"114\" height=\"110\" saveinfile=\"1\">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</img><textflag text=\"#hospitalname#\" fontsize=\"15\" fontname=\"楷体_GB2312\" fontbold=\"1\" /><p align=\"2\" /><textflag text=\"数 字 胃 肠 报 告 单\" fontsize=\"20\" fontbold=\"1\" /><p align=\"2\" /><textflag text=\"检查号：\" fontbold=\"1\" fontname=\"楷体_GB2312\" /><span>#studynum#       </span><textflag text=\"卡号：\" fontbold=\"1\" fontname=\"楷体_GB2312\" /><span>#zh#    #rylb#</span><p align=\"2\" /><hr /><textflag text=\"姓名\" fontbold=\"1\" fontname=\"楷体_GB2312\" /><span>:</span><textflag text=\"#xm#\" /><span>      </span><textflag text=\"性别\" fontbold=\"1\" fontname=\"楷体_GB2312\" /><span>:</span><textflag text=\"#xb#\" /><span>    </span><textflag text=\"年龄\" fontbold=\"1\" fontname=\"楷体_GB2312\" /><span>:</span><textflag text=\"#nl#\" /><span>      </span><textflag text=\"科室：\" fontbold=\"1\" fontname=\"楷体_GB2312\" /><textflag text=\"#kksmc#\" /><p /><textflag text=\"病室：\" fontbold=\"1\" fontname=\"楷体_GB2312\" /><textflag text=\"#bs#\" /><span> </span><textflag text=\"床号：\" fontbold=\"1\" fontname=\"楷体_GB2312\" /><textflag text=\"#ch#\" /><span> </span><textflag text=\"检查日期：\" fontbold=\"1\" fontname=\"楷体_GB2312\" /><textflag text=\"#studydate#\" /><span> </span><textflag text=\"审核日期：\" fontbold=\"1\" fontname=\"楷体_GB2312\" /><textflag text=\"#checkdate#\" /><p /><hr /><textflag text=\"病情摘要：\" fontbold=\"1\" fontname=\"楷体_GB2312\" /><span>#illsummary#</span><p /><textflag text=\"临床诊断：\" fontbold=\"1\" fontname=\"楷体_GB2312\" /><span>#ClinicalDiagnosis#</span><p /><textflag text=\"透视所见：\" fontbold=\"1\" fontname=\"楷体_GB2312\" /><div id=\"impression\" name=\"插入文本块\" title=\"检查描述\" hidetitle=\"1\"><text>  </text><p /></div><p /><textflag text=\"透视诊断：\" fontbold=\"1\" fontname=\"楷体_GB2312\" /><div id=\"conclusion\" name=\"插入文本块\" title=\"诊断提示\" hidetitle=\"1\"><text>  </text><p /></div><p /></body></emrtextdoc>");
            //e.Show();
        }
    }
}
