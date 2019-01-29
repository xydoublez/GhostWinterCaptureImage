using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
namespace GhostWinterCaptureImage
{
    /// <summary>
    /// 服务端
    /// </summary>
    public class MyTcpListener
    {

        private TcpListener server = new TcpListener(IPAddress.Any, 0);
        private Socket mySock;
        public MyTcpListener()
        {
            
        }
        public void Start()
        {
            server.Start();
            //Thread t = new Thread(new ThreadStart(Recceive));
            //t.Start();
        }
        public void Recceive()
        {
            while (true)
            {
                mySock = server.AcceptSocket();
                if (mySock.Connected)
                {
                    NetworkStream netStream = new NetworkStream(mySock);
                    byte[] data = new byte[2048];
                    netStream.Read(data, 0, data.Length);
                    //将data给谁处理
                }

            }

        }
        public void Send()
        {

        }
    }
}
