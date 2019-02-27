using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Security;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Diagnostics;
using System.IO;
using System.Data.OleDb;
using Dicom.Network;
using Dicom.Imaging;
using Dicom;
namespace GhostWinter.VideoCapture
{
    [Guid("EAA1B6D4-43E3-4558-BB0D-965F58A4A4A6")]
    public partial class VideoCaptureControl : UserControl,IObjectSafety
    {
        private VideoCaptureDevice videoSource;
        FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
        private VideoCapabilities[] videoCapabilities;
        private VideoCapabilities[] snapshotCapabilities;
        string FileRN, modality, Hospital, ServerTime, patientname, processnum, patientsex, patientage, REGISTRATION_ID, imgcount, ip, aetitle;
        int port = 0;
        int deviceNum = 0;
        public VideoCaptureControl()
        {
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
        private void VideoCaptureControl_Load(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// 根据序号选择设备
        /// </summary>
        /// <param name="num"></param>
        public void SelectDevice(int num)
        {
            deviceNum = num;
        }
        // Start cameras
        public  void Start()
        {
            try
            {
                videoSource = new VideoCaptureDevice(videoDevices[deviceNum].MonikerString);
                videoCapabilities = videoSource.VideoCapabilities;
                snapshotCapabilities = videoSource.SnapshotCapabilities;
                if ((snapshotCapabilities != null) && (snapshotCapabilities.Length != 0))
                {
                    videoSource.ProvideSnapshots = true;
                    videoSource.DesiredSnapshotSize = new Size(768, 576);
                    videoSource.SnapshotFrame += new NewFrameEventHandler(videoDevice_SnapshotFrame);
                    videoSource.NewFrame += new NewFrameEventHandler(videoDevice_Video);
                }
                videoSource.DesiredFrameRate = 25;
                videoSourcePlayer.VideoSource = videoSource;
                videoSourcePlayer.Start();
                
                timer.Start();
            }
            catch(Exception e)
            {
                MessageBox.Show("设备号不正确！"+e.Message+e.StackTrace);
                Application.Exit();
            }
        }

        // Stop cameras
        public  void Stop()
        {
            timer.Stop();
            videoSourcePlayer.SignalToStop();
            videoSourcePlayer.WaitForStop();
        }
        //采集
        public void CaptureImage()
        {
            if ((videoSource != null) && (videoSource.ProvideSnapshots))
            {
                videoSource.SimulateTrigger();
            }
        }
        // New snapshot frame is available
        //截图
        private void videoDevice_SnapshotFrame(object sender, NewFrameEventArgs eventArgs)
        {
            MessageBox.Show("开始截图！");
            Bitmap bmp = new Bitmap(eventArgs.Frame.Size.Width, eventArgs.Frame.Size.Height);
            bmp=((Bitmap)eventArgs.Frame.Clone());
            if (File.Exists("c:\\lzqtemp.jpg")) File.Delete("c:\\lzqtemp.jpg");
            if (File.Exists("c:\\lzqtemp.dcm")) File.Delete("c:\\lzqtemp.dcm");
            if(File.Exists("c:\\lzqtemp0.dcm")) File.Delete("c:\\lzqtemp0.dcm");
            bmp.RotateFlip(RotateFlipType.Rotate90FlipNone);
            bmp.Save("c:\\lzqtemp.jpg",System.Drawing.Imaging.ImageFormat.Jpeg);
            MessageBox.Show("保存成功！");
            //BmpToDcm("c:\\lzqtemp.jpg", modality, Hospital, ServerTime, patientname, processnum, patientsex, patientage, REGISTRATION_ID, imgcount);
            //SendDcm("c:\\lzqtemp.dcm", ip, port, aetitle);
            
        }
        private void videoDevice_Video(object sender, NewFrameEventArgs arg)
        {
            AForge.Video.FFMPEG.VideoFileWriter writer = new AForge.Video.FFMPEG.VideoFileWriter();
            
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            //IVideoSource videoSource1 = videoSourcePlayer.VideoSource;

            //int framesReceived1 = 0;

            //// get number of frames for the last second
            //if (videoSource1 != null)
            //{
            //    framesReceived1 = videoSource1.FramesReceived;
            //}

       

            //if (stopWatch == null)
            //{
            //    stopWatch = new Stopwatch();
            //    stopWatch.Start();
            //}
            //else
            //{
            //    stopWatch.Stop();
            //    float fps1 = 1000.0f * framesReceived1 / stopWatch.ElapsedMilliseconds;
            //    camera1FpsLabel.Text = fps1.ToString("F2") + " fps";
            //    stopWatch.Reset();
            //    stopWatch.Start();
            //}
        }
        public void Setup()
        {
            if ((videoSource != null) && (videoSource is VideoCaptureDevice))
            {
                try
                {
                    ((VideoCaptureDevice)videoSource).DisplayPropertyPage(this.Handle);
                }
                catch (NotSupportedException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        public void ChooseDevice()
        {
            VideoCaptureDeviceForm form = new VideoCaptureDeviceForm();

            if (form.ShowDialog(this) == DialogResult.OK)
            {
                // create video source
                VideoCaptureDevice videoSource = form.VideoDevice;

                // open it
                OpenVideoSource(videoSource);
            }
        }
        // Close video source if it is running
        private void CloseCurrentVideoSource()
        {
            if (videoSourcePlayer.VideoSource != null)
            {
                videoSourcePlayer.SignalToStop();

                // wait ~ 3 seconds
                for (int i = 0; i < 30; i++)
                {
                    if (!videoSourcePlayer.IsRunning)
                        break;
                    System.Threading.Thread.Sleep(100);
                }

                if (videoSourcePlayer.IsRunning)
                {
                    videoSourcePlayer.Stop();
                }

                videoSourcePlayer.VideoSource = null;
            }
        }
        // Open video source
        private void OpenVideoSource(IVideoSource source)
        {
            // set busy cursor
            this.Cursor = Cursors.WaitCursor;

            // stop current video source
            CloseCurrentVideoSource();

            // start new video source
            videoSourcePlayer.VideoSource = source;
            videoSourcePlayer.Start();

            // reset stop watch
            

            // start timer
            timer.Start();

            this.Cursor = Cursors.Default;
        }
        /// <summary>
        /// 扫描图像
        /// </summary>
        /// <returns></returns>
        public void ScanImage(string modality, string Hospital, string ServerTime, string patientname, string processnum, string patientsex, string patientage, string REGISTRATION_ID, string imgcount, string ip, int port, string aetitle)
        {
            try
            {
                this.modality = modality;
                this.Hospital = Hospital;
                this.ServerTime = ServerTime;
                this.patientname = patientname;
                this.processnum = processnum;
                this.patientsex = patientsex;
                this.patientage = patientage;
                this.REGISTRATION_ID = REGISTRATION_ID;
                this.imgcount = imgcount;
                this.ip = ip;
                this.port = port;
                this.aetitle = aetitle;
                CaptureImage();

            }
            catch { }





        }
        /// <summary>
        /// 发送DCM
        /// </summary>
        /// <param name="DcmFile"></param>
        /// <param name="ip"></param>
        /// <param name="port"></param>
        /// <param name="aetitle"></param>
        /// <returns></returns>
        public int SendDcm(string DcmFile, string ip, int port, string aetitle)
        {
            var client = new DicomClient();

            // queue C-Store request to send DICOM file
            client.AddRequest(new DicomCStoreRequest(DcmFile));

            //// queue C-Store request with additional proposed transfer syntaxes
            //client.AddRequest(new DicomCStoreRequest(@"test2.dcm")
            //{
            //    AdditionalTransferSyntaxes = new DicomTransferSyntax[] {
            //            DicomTransferSyntax.JPEGLSLossless,
            //            DicomTransferSyntax.JPEG2000Lossless
            //        }
            //});

            // connect and send queued requests
            client.Send(ip, port, false, aetitle, aetitle);
            return 0;
        }
        /// <summary>
        /// BMP 转换成 Dicom
        /// </summary>
        /// <param name="imageFile"></param>
        /// <param name="DcmFile"></param>
        /// <param name="patientName"></param>
        /// <returns></returns>
        public int BmpToDcm(string FileRN, string modality, string Hospital, string ServerTime, string patientname, string processnum, string patientsex, string patientage, string REGISTRATION_ID, string imgcount)
        {
            
            //DICOMX1.DICOMSOPClassUID = "1.2.840.10008.5.1.4.1.1.7";
            //DICOMX1.DICOMSOPInstanceUID = "1.2.7.3.8." + System.DateTime.Now.ToString("yyyy.MM.dd") + "." + System.DateTime.Now.ToString("hh.mm.ss") + ".1";
            //DICOMX1.DICOMImplementationClassUID = "1.2.7.3.8.2007.10.17.23.00.00.3";
            //DICOMX1.DICOMImplementationVersionName = "MASSUN";
            //DICOMX1.DICOMSourceApplicationEntityTitle = "ZYPACS";
            //DICOMX1.DICOMManufacturer = "ZYPACS";
            //DICOMX1.DICOMModality = modality;
            //DICOMX1.DICOMPatientName = patientname;
            //DICOMX1.DICOMPatientSex = patientsex;
            //DICOMX1.DICOMSeriesNumber = 0;
            //DICOMX1.DICOMImageNumber = 0;
            //DICOMX1.DICOMStudyInstanceUID = "1.2.7.3.8." + System.DateTime.Now.ToString("yyyy.MM.dd") + "." + System.DateTime.Now.ToString("hh.mm.ss") + ".101";
            //DICOMX1.DICOMSeriesInstanceUID = "1.2.7.3.8." + System.DateTime.Now.ToString("yyyy.MM.dd") + "." + System.DateTime.Now.ToString("hh.mm.ss");
            //DICOMX1.DICOMPatientID = processnum;
            //DICOMX1.DICOMStudyID = processnum;
            //DICOMX1.DICOMInstitutionName = Hospital;
            //DICOMX1.DICOMStudyDescrp = "ZYPACS";
            //DICOMX1.DICOMCityName = "City";
            //DICOMX1.DICOMRefPhyName = "ExDr";
            //DICOMX1.DICOMPixelSpaceHeight = 1;
            //DICOMX1.DICOMPixelSpaceWidth = 1;
            //DICOMX1.ConvertFileToDICOM(FileRN, "c:\\lzqtemp0.dcm");
            //DicomImage img = new DicomImage("C:\\lzqtemp0.DCM");
            //DicomDataset ds = img.Dataset;
            //ds.Add(DicomTag.PlanarConfiguration, (ushort)0);
            //DicomFile dcm = new DicomFile(ds);
            
            //dcm.Save("c:\\lzqtemp.dcm");
            

            return 0;

        }

        /// <summary>
        /// 开始录像
        /// </summary>
        /// <param name="outFileName"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public int Video(string outFileName, string format)
        {
            
            return 0;
        }
  
    

    }
}
