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
    public partial class Main : Form
    {
        _10blib.Connection conn;
        public Main(string host, int port, string user, string pass)
        {
            InitializeComponent();
            conn = new _10blib.Connection(host, port, user, pass);
            conn.ReadString(new AsyncCallback(ReadCall));
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        void ReadCall(IAsyncResult ar)
        {
            foreach (_10blib.Message msg in conn.Messages)
            {
                if (msg.Contents.Contains("welcome")) conn.Auth(WriteCall);
                this.BeginInvoke((Action)(() =>
                {
                    txtStatus.AppendText(">>> " + msg.Contents + "\r\n");
                }));
            }
            conn.Messages.Clear();
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
            txtMsg.Text = "";
            txtMsg.Focus();
        }

        private void txtMsg_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;
                btnSend_Click(sender, e);
            }
        }
    }
}
