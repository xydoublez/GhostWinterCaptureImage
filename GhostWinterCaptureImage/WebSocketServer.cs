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
using System.Reflection;
using System.Threading;
namespace GhostWinterCaptureImage
{
    /// <summary>
    /// This interface shows events to javascript
    /// </summary>
    [Guid("F4547EEB-4F41-42DD-8F1E-242D6E106142")]
    [InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
    public interface ControlEvents
    {
        //Add a DispIdAttribute to any members in the source interface 
        //to specify the COM DispId.
        [DispId(0x60020001)]
        void  OnReceiveMsg(string msg);
    } 
    [Guid("E5466526-8A78-4A02-9654-22C46FB7DAA3")]
    [ClassInterface(ClassInterfaceType.AutoDual), ComSourceInterfaces(typeof(ControlEvents))] //Implementing interface that will be visible from JS
    public partial class WebSocketServer : Control,IObjectSafety
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
        private static Socket mySock;
        public WebSocketServer()
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
        ///// <param name="param">参数</param>
        //private void ExecuteJsFunc(string data)
        //{
        //    if (html != null)
        //    {
        //        html.execScript("SocketCallBack('" + data + "');", "javascript");
              
                
        //    }

        //}
        //#region ActiveX调用Javascript的函数 方法二
        ///// <summary>
        ///// 调用Javascript
        ///// </summary>
        ///// <param name="Filenames">The filenames.</param>
        //private void CallJavaScript(string param)
        //{
        //    //反射获取当前的控件的ClientSite
        //    Type typeIOleObject = this.GetType().GetInterface("IOleObject", true);
        //    object oleClientSite = typeIOleObject.InvokeMember("GetClientSite",
        //    BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.Public,
        //    null,
        //    this,
        //    null);

        //    //获取Container
        //    IOleClientSite oleClientSite2 = oleClientSite as IOleClientSite;
        //    IOleContainer pObj;
        //    oleClientSite2.GetContainer(out pObj);

        //    //参数数组   
        //    object[] args = new object[1];
        //    args[0] = param;

        //    //获取页面的Script集合   
        //    IHTMLDocument pDoc2 = (IHTMLDocument)pObj;
        //    object script = pDoc2.Script;

        //    //try
        //    //{
        //        //调用JavaScript方法OnScaned并传递参数，因为此方法可能并没有在页面中实现，所以要进行异常处理   
        //        script.GetType().InvokeMember("SocketCallBack", BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.Public,
        //       null,
        //        script,
        //        args);
        //    //}
        //    //catch { }
        //}
        //#endregion
        ///// <summary>
        ///// 将window对象传递进来
        ///// </summary>
        ///// <param name="obj">The obj.</param>
        //public void GetHtmlWindow(object obj)
        //{
        //    //html = obj as mshtml.HTMLWindow2Class;
            
        //}
        //#endregion
        public delegate void MessageHander(string msg);
        public event MessageHander OnReceiveMsg;   
           #region 服务端
        TcpListener server;
        /// <summary>
        /// 开启服务器开启监听
        /// </summary>
        /// <returns></returns>
        public bool Listen(int port)
        {
            try
            {
                  
                    server = new TcpListener(IPAddress.Any, port);
                    server.Start();
                    //MessageBox.Show("服务启动成功");
                    //Receive();
                    Thread t = new Thread(new ThreadStart(Receive));
                    t.Start();
             
            }
            catch(Exception ex)
            {
                server.Stop();
                MessageBox.Show("服务启动失败"+ex.Message);
                return false;
            }
            return true;

        }
        public bool StopListen()
        {
            try
            {
                server.Stop();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void  RunCommand(string command)
        {
            switch (command)
            {
                case "shutdown":
                    break;
                case "logoff":
                    break;
                case "resetcomputer":
                    break;
                default:
                    string[] cmd = command.Split('&');
                    if (cmd.Length == 1)
                    {
                        System.Diagnostics.Process.Start(cmd[0]);
                    }
                    else if(cmd.Length>1)
                    {
                        System.Diagnostics.Process.Start(cmd[0], cmd[1]);
                    }
                    break;
            }
        }
        private void BeginListen()
        {
            
        }
        
        private void Receive()
        {
            //MessageBox.Show("接收启动");
            while (true)
            {
                mySock = server.AcceptSocket();
                if (mySock.Connected)
                {
                      
                    //MessageBox.Show("接收socket成功");
                    //Thread t = new Thread(new ThreadStart(Round));
                    //t.Start();
                    NetworkStream stream = new NetworkStream(mySock);
                    byte[] data = new byte[2048];
                    int bs = stream.Read(data, 0, data.Length);
                    string msg = System.Text.Encoding.UTF8.GetString(data,0,bs);
                    //RunCommand(msg);
                    OnReceivedMsg(msg); 
                }
        

            
            }
        }
        private void Round()
        {
           
            NetworkStream stream = new NetworkStream(mySock);
            byte[] data = new byte[2048];
            stream.Read(data, 0, data.Length);
            string msg = System.Text.Encoding.UTF8.GetString(data);

            OnReceivedMsg(msg);
           
        }
        private void OnReceivedMsg(string msg)
        {
            if (OnReceiveMsg != null && msg != "")
            {
                OnReceiveMsg(msg);
                
            }
        }

        #endregion 
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

    }
}
