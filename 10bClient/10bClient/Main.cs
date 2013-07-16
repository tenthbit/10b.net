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
            conn.ReadString(ReadCall);
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
                        if (msg.op == "act" && msg.ex.message != null)
                            txtStatus.AppendText("<" + msg.sr + "> " + msg.ex.message + "\r\n");
                        else if (msg.op == "join")
                            txtStatus.AppendText("* " + msg.sr + " has joined " + msg.rm + "\r\n");
                        else if (msg.op == "leave" && msg.rm == null)
                            txtStatus.AppendText("* " + msg.sr + " has quit.\r\n");
                        else
                            txtStatus.AppendText(">>> " + msg.ToString() + "\r\n");
    
                    }
                }));
            }
            conn.Payloads.Clear();
            conn.ReadString(ReadCall);
        }

        void WriteCall(IAsyncResult ar)
        {
            Payload msg = new Payload((string)ar.AsyncState);
            this.BeginInvoke((Action)(() =>
            {
                if (msg.op == "act" && msg.ex.message != null)
                    txtStatus.AppendText("<" + conn.Username + "> " + msg.ex.message + "\r\n");
#if DEBUG
                txtStatus.AppendText("<<< " + msg.ToString() + "\r\n");
#endif
            }));
            if(conn.ssl.CanWrite)
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

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            conn.Leave();
        }
    }
}
