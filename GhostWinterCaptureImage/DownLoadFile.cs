//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Text;
//using System.Windows.Forms;
//using System.Runtime.InteropServices;
//using System.ServiceModel;
//using System.IO;
//using MSS.ConsoleClient;
//namespace GhostWinterCaptureImage
//{
//    [Guid("E4AEC1BC-5AF3-4D51-B8A5-7D870A992007")]
//    public partial class DownLoadFile : Control,IObjectSafety
//    {
//        #region IObjectSafety 成员

//        private const string _IID_IDispatch = "{00020400-0000-0000-C000-000000000046}";
//        private const string _IID_IDispatchEx = "{a6ef9860-c720-11d0-9337-00a0c90dcaa9}";
//        private const string _IID_IPersistStorage = "{0000010A-0000-0000-C000-000000000046}";
//        private const string _IID_IPersistStream = "{00000109-0000-0000-C000-000000000046}";
//        private const string _IID_IPersistPropertyBag = "{37D84F60-42CB-11CE-8135-00AA004BB851}";

//        private const int INTERFACESAFE_FOR_UNTRUSTED_CALLER = 0x00000001;
//        private const int INTERFACESAFE_FOR_UNTRUSTED_DATA = 0x00000002;
//        private const int S_OK = 0;
//        private const int E_FAIL = unchecked((int)0x80004005);
//        private const int E_NOINTERFACE = unchecked((int)0x80004002);

//        private bool _fSafeForScripting = true;
//        private bool _fSafeForInitializing = true;


//        public int GetInterfaceSafetyOptions(ref Guid riid, ref int pdwSupportedOptions, ref int pdwEnabledOptions)
//        {
//            int Rslt = E_FAIL;

//            string strGUID = riid.ToString("B");
//            pdwSupportedOptions = INTERFACESAFE_FOR_UNTRUSTED_CALLER | INTERFACESAFE_FOR_UNTRUSTED_DATA;
//            switch (strGUID)
//            {
//                case _IID_IDispatch:
//                case _IID_IDispatchEx:
//                    Rslt = S_OK;
//                    pdwEnabledOptions = 0;
//                    if (_fSafeForScripting == true)
//                        pdwEnabledOptions = INTERFACESAFE_FOR_UNTRUSTED_CALLER;
//                    break;
//                case _IID_IPersistStorage:
//                case _IID_IPersistStream:
//                case _IID_IPersistPropertyBag:
//                    Rslt = S_OK;
//                    pdwEnabledOptions = 0;
//                    if (_fSafeForInitializing == true)
//                        pdwEnabledOptions = INTERFACESAFE_FOR_UNTRUSTED_DATA;
//                    break;
//                default:
//                    Rslt = E_NOINTERFACE;
//                    break;
//            }

//            return Rslt;
//        }

//        public int SetInterfaceSafetyOptions(ref Guid riid, int dwOptionSetMask, int dwEnabledOptions)
//        {
//            int Rslt = E_FAIL;

//            string strGUID = riid.ToString("B");
//            switch (strGUID)
//            {
//                case _IID_IDispatch:
//                case _IID_IDispatchEx:
//                    if (((dwEnabledOptions & dwOptionSetMask) == INTERFACESAFE_FOR_UNTRUSTED_CALLER) &&
//                         (_fSafeForScripting == true))
//                        Rslt = S_OK;
//                    break;
//                case _IID_IPersistStorage:
//                case _IID_IPersistStream:
//                case _IID_IPersistPropertyBag:
//                    if (((dwEnabledOptions & dwOptionSetMask) == INTERFACESAFE_FOR_UNTRUSTED_DATA) &&
//                         (_fSafeForInitializing == true))
//                        Rslt = S_OK;
//                    break;
//                default:
//                    Rslt = E_NOINTERFACE;
//                    break;
//            }

//            return Rslt;
//        }

//        #endregion

//        public DownLoadFile()
//        {
//            InitializeComponent();
//        }

//        protected override void OnPaint(PaintEventArgs pe)
//        {
//            base.OnPaint(pe);
//        }
//        public  void DownloadFile(string sourceFile, string destFile)
//        {

//            ChannelFactory<IFileTransferServiceChannel> factory = new ChannelFactory<IFileTransferServiceChannel>("BasicHttpBinding_IFileTransferService");
//            IFileTransferServiceChannel channel = factory.CreateChannel();
//            if (!Directory.Exists(Path.GetDirectoryName(destFile)))
//            {
//                Directory.CreateDirectory(Path.GetDirectoryName(destFile));
//            }
//            FileStream localFileStream = new FileStream(destFile, FileMode.Create, FileAccess.Write, FileShare.Write, 4096, FileOptions.Asynchronous);
//            AsynchronousFileTransfer asynchronousFileTransfer = new AsynchronousFileTransfer()
//            {
//                FileFullName = sourceFile
//            };
//            Action<IFileTransferServiceChannel, Stream, Exception> completed = (fileTransferServiceChannel, stream, exception) =>
//            {
//                fileTransferServiceChannel.Close();
//                stream.Close();
//                if (exception != null)
//                {
//                    throw new Exception(exception.Message);

//                }

//            };
//            asynchronousFileTransfer.AsyncDownloadFile(channel, localFileStream, completed);




//        }   
//    }
//}
