using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Net.Security;
using System.Threading;
using Newtonsoft.Json;
namespace _10bClient
{
    public partial class Form1 : Form
    {
        _10blib.Connection conn;
        bool auth = false;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            conn = new _10blib.Connection("10bit.danopia.net", 10817);
            conn.ReadString(new AsyncCallback(ReadCall));
        }

        void ReadCall(IAsyncResult ar)
        {
            foreach (_10blib.Message msg in conn.Messages)
            {
                this.BeginInvoke((Action)(() =>
                {
                    txtStatus.AppendText(">>> " + msg.Contents + "\r\n");
                }));
            }
            conn.Messages.Clear();
            if (!auth)
            {
                string sendmsg = "{\"op\": \"auth\", \"ex\": {\"method\": \"password\", \"username\": \"ldunn\", \"password\": \"SECRETPASSWORDOMG\"}}";
                conn.WriteString(sendmsg, WriteCall);
                auth = true;
            }
            conn.ReadString(new AsyncCallback(ReadCall));
        }

        void WriteCall(IAsyncResult ar)
        {
            string msg = (string)ar.AsyncState;
            this.BeginInvoke((Action)(() =>
            {
                txtStatus.AppendText("<<< " + msg + "\r\n");
            }));
            conn.ssl.EndWrite(ar);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            conn.WriteString(txtMsg.Text, WriteCall);
        }
    }
}
