using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Sockets;
using mshtml;
using System.Reflection;
namespace GhostWinterCaptureImage
{
    [Guid("17377FAC-ED6F-47FA-8E99-75BB56DD0BB7")]
    public partial class WebSocketClient : Control, IObjectSafety
    {
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

        public WebSocketClient()
        {
            InitializeComponent();
        }
        //#region 调用js回调方法
        ///// <summary>
        ///// HTMLWindow2Class对象
        ///// </summary>
        //private mshtml.IHTMLWindow2 html = null;
        ///// <summary>
        ///// 执行JS方法
        ///// </summary>
        //public void ExecuteJsFunc(string  data)
        //{
        //    html.execScript("SocketSend('" + data + "');", "javascript");
        //}
        ///// <summary>
        ///// 将window对象传递进来
        ///// </summary>
        ///// <param name="obj">The obj.</param>
        //public void GetHtmlWindow(object obj)
        //{
        //    html = obj as mshtml.IHTMLWindow2;
        //}
        //#endregion



        //#endregion
        #region 客户端

        TcpClient client;
        private bool  Connect(string IP,int port)
        {
            try
            {
                client= new TcpClient();
                IPAddress ip = IPAddress.Parse(IP);
                client.Connect(new IPEndPoint(ip, port));
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                client.Close();
                return false;
            }
            return true;
            

        }
        public bool  SendData(string ip,int port,string data)
        {
            if (Connect(ip, port))
            {
                try
                {
                    using (Socket mySock = client.Client)
                    {
                        mySock.Send(System.Text.Encoding.GetEncoding("gb2312").GetBytes(data));

                    }
                }
                catch
                {
                    return false;
                }

            }
            else
            {
                return false;
            }
          
            return true;
        }
        
        public void BeginListen()
        {
        }
        public void Access()
        {
        }

        #endregion
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
