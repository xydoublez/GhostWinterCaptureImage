using System;
using System.Collections.Generic;

using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net;
using System.Net.Sockets;
namespace GhostWinterCaptureImageWebTest
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            TcpClient client = new TcpClient();
            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000));
            NetworkStream ns = client.GetStream();
            ns.Write(System.Text.Encoding.UTF8.GetBytes("成功"), 0, "成功".Length);
            ns.Close();
            client.Close();
            
        }
    }
}