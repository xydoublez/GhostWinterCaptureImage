using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace videoclient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            webSocketClient1.SendData("127.0.0.1", Convert.ToInt32(this.textBox1.Text), this.richTextBox1.Text);
            //webSocketClient1.SendData("127.0.0.1", 65432, "US*诸城人民医院**tew*5537*男*男**0");
        }

        private void webSocketClient1_Click(object sender, EventArgs e)
        {

        }
    }
}
