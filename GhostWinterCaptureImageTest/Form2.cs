using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace GhostWinterCaptureImageTest
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            
           InitializeComponent();
         
        }

        private void colorBox1_ColorChanged(object sender, GhostWinterCaptureImage.ColorChangedEventArgs args)
        {
            MessageBox.Show(args.Color.Name);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }

        private void capture1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            ////capture1.CapturePic();
            //MessageBox.Show(this.Handle.ToInt32().ToString());
            //MessageBox.Show(capture1.Parent.Handle.ToInt32().ToString());
            //capture1.Exec("cmd.exe");
           // capture1.LoadReport();
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            this.downLoadFile1.DownloadFile(@"D:\mycode\DICOM\DICOMFILES\DXStudy\1.2.840.113619.2.67.2158294438.16324010109084338.243.dcm", @"f:\test\1.dcm"); 
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            //this.webSocketServer1.Listen(12345);
        }

        private void button1_Click_3(object sender, EventArgs e)
        {
            //this.videoCaptureControl1.StartCameras("");
            this.videoCaptureControl1.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.videoCaptureControl1.CaptureImage();
        }

       
    }
}
