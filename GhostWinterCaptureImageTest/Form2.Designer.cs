namespace GhostWinterCaptureImageTest
{
    partial class Form2
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.webSocketServer2 = new GhostWinterCaptureImage.WebSocketServer();
            this.webSocketClient2 = new GhostWinterCaptureImage.WebSocketClient();
            this.downLoadFile1 = new GhostWinterCaptureImage.DownLoadFile();
            this.webSocketServer1 = new GhostWinterCaptureImage.WebSocketServer();
            this.webSocketClient1 = new GhostWinterCaptureImage.WebSocketClient();
            this.videoCaptureControl1 = new GhostWinter.VideoCapture.VideoCaptureControl();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(783, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 6;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_3);
            // 
            // webSocketServer2
            // 
            this.webSocketServer2.Location = new System.Drawing.Point(377, 212);
            this.webSocketServer2.Name = "webSocketServer2";
            this.webSocketServer2.Size = new System.Drawing.Size(75, 23);
            this.webSocketServer2.TabIndex = 5;
            this.webSocketServer2.Text = "webSocketServer2";
            // 
            // webSocketClient2
            // 
            this.webSocketClient2.Location = new System.Drawing.Point(583, 204);
            this.webSocketClient2.Name = "webSocketClient2";
            this.webSocketClient2.Size = new System.Drawing.Size(75, 23);
            this.webSocketClient2.TabIndex = 4;
            this.webSocketClient2.Text = "webSocketClient2";
            // 
            // downLoadFile1
            // 
            this.downLoadFile1.Location = new System.Drawing.Point(141, 213);
            this.downLoadFile1.Name = "downLoadFile1";
            this.downLoadFile1.Size = new System.Drawing.Size(75, 23);
            this.downLoadFile1.TabIndex = 3;
            this.downLoadFile1.Text = "downLoadFile1";
            // 
            // webSocketServer1
            // 
            this.webSocketServer1.Location = new System.Drawing.Point(744, 56);
            this.webSocketServer1.Name = "webSocketServer1";
            this.webSocketServer1.Size = new System.Drawing.Size(75, 23);
            this.webSocketServer1.TabIndex = 0;
            this.webSocketServer1.Text = "webSocketServer1";
            // 
            // webSocketClient1
            // 
            this.webSocketClient1.Location = new System.Drawing.Point(490, 84);
            this.webSocketClient1.Name = "webSocketClient1";
            this.webSocketClient1.Size = new System.Drawing.Size(75, 23);
            this.webSocketClient1.TabIndex = 1;
            this.webSocketClient1.Text = "webSocketClient1";
            // 
            // videoCaptureControl1
            // 
            this.videoCaptureControl1.Location = new System.Drawing.Point(9, 6);
            this.videoCaptureControl1.Name = "videoCaptureControl1";
            this.videoCaptureControl1.Size = new System.Drawing.Size(768, 576);
            this.videoCaptureControl1.TabIndex = 7;
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(941, 626);
            this.Controls.Add(this.videoCaptureControl1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.webSocketServer2);
            this.Controls.Add(this.webSocketClient2);
            this.Controls.Add(this.downLoadFile1);
            this.Name = "Form2";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form2_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private GhostWinterCaptureImage.WebSocketServer webSocketServer1;
        private GhostWinterCaptureImage.WebSocketClient webSocketClient1;
        private GhostWinterCaptureImage.DownLoadFile downLoadFile1;
        private GhostWinterCaptureImage.WebSocketClient webSocketClient2;
        private GhostWinterCaptureImage.WebSocketServer webSocketServer2;
        private System.Windows.Forms.Button button1;
        private GhostWinter.VideoCapture.VideoCaptureControl videoCaptureControl1;




    }
}

