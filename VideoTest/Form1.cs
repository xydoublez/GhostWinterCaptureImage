using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace VideoTest
{
    public partial class Form1 : Form
    {
        private static Socket mySock;
        string FileRN, modality, Hospital, ServerTime, patientname, processnum, patientsex, patientage, REGISTRATION_ID, imgcount, ip, aetitle;
        int port = 0;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            int deviceNum = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["DeviceNum"]);
            videoCaptureControl1.SelectDevice(deviceNum);
            videoCaptureControl1.Start();
            Listen(65432);
        }
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
            catch (Exception ex)
            {
                server.Stop();
                MessageBox.Show("服务启动失败" + ex.Message);
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
        public void RunCommand(string command)
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
                    else if (cmd.Length > 1)
                    {
                        System.Diagnostics.Process.Start(cmd[0], cmd[1]);
                    }
                    break;
            }
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
                    stream.Read(data, 0, data.Length);
                    string msg = System.Text.Encoding.UTF8.GetString(data);
                    string[] info = msg.Split('*');
                    this.modality = info[0];
                    this.Hospital = info[1];
                    this.ServerTime = info[2];
                    this.patientname = info[3];
                    this.processnum = info[4];
                    this.patientsex = info[5];
                    this.patientage = info[6];
                    this.REGISTRATION_ID = info[7];
                    this.imgcount = info[8];
                    this.ip = info[9];
                    this.port = Convert.ToInt32(info[10]);
                    this.aetitle = info[11].TrimEnd('\0');
                    videoCaptureControl1.ScanImage(modality, Hospital, ServerTime, patientname, processnum, patientsex, patientage, REGISTRATION_ID, imgcount, ip, port, aetitle);
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

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0);
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }
    }
}
