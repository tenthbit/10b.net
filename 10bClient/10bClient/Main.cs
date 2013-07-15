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
using _10blib;
namespace _10bClient
{
    public partial class Main : Form
    {
        Connection conn;
        string room = "48557f95";
        public Main(string host, int port, string user, string pass, Connect connForm)
        {
            InitializeComponent();
            connForm.Invoke((Action)(() => { connForm.Close(); }));
            conn = new _10blib.Connection(host, port, user, pass);
            conn.ReadString(new AsyncCallback(ReadCall));
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        void ReadCall(IAsyncResult ar)
        {
            foreach (_10blib.Payload msg in conn.Payloads)
            {
                if (msg.op == "welcome") conn.Auth(WriteCall);
                this.BeginInvoke((Action)(() =>
                {
                    if (msg.ex.isack == null)
                    {
                        if (msg.op == "msg" && msg.ex.type == "msg")
                            txtStatus.AppendText("<" + msg.sr + "> " + msg.ex.data + "\r\n");
                        else
                            txtStatus.AppendText(">>> " + msg.ToString() + "\r\n");
    
                    }
                }));
            }
            conn.Payloads.Clear();
            conn.ReadString(new AsyncCallback(ReadCall));
        }

        void WriteCall(IAsyncResult ar)
        {
            Payload msg = new Payload((string)ar.AsyncState);
            this.BeginInvoke((Action)(() =>
            {
                if (msg.op == "act" && msg.ex.type == "msg")
                    txtStatus.AppendText("<" + conn.Username + "> " + msg.ex.data + "\r\n");
#if DEBUG
                txtStatus.AppendText("<<< " + msg.ToString() + "\r\n");
#endif
            }));
            conn.ssl.EndWrite(ar);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            conn.SendMessage(txtMsg.Text, room, WriteCall);
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
