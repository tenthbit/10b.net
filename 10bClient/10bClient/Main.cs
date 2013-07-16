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
        Dictionary<string, TabPage> rooms = new Dictionary<string, TabPage>();
        Dictionary<string, dynamic> roomMeta = new Dictionary<string,dynamic>();
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
                        RichTextBox txtStatus;
                        if(msg.rm != null && msg.rm != "" && rooms.ContainsKey(msg.rm))
                            txtStatus = (RichTextBox)rooms[msg.rm].Controls[0];
                        else
                            txtStatus = (RichTextBox)tbpStatus.Controls[0];

                        DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                        dt = dt.AddMilliseconds(msg.ts).ToLocalTime();
                        txtStatus.AppendText("[" + dt.ToLongTimeString() + "] ");
                        if (msg.op == "act" && msg.ex.message != null)
                        {
                            if (msg.ex.isaction != null && msg.ex.isaction == true) txtStatus.AppendText("* " + msg.sr + " " + msg.ex.message + "\r\n");
                            else txtStatus.AppendText("<" + msg.sr + "> " + msg.ex.message + "\r\n");
                        }
                        else if (msg.op == "join")
                            txtStatus.AppendText("* " + msg.sr + " has joined " + msg.rm + "\r\n");
                        else if (msg.op == "leave")
                        {
                            if (msg.rm == null) txtStatus.AppendText("* " + msg.sr + " has quit.\r\n");
                            else txtStatus.AppendText("* " + msg.sr + " has left " + roomMeta[msg.rm].name + ".\r\n");
                        }
                        else if (msg.op == "meta")
                        {
                            if (msg.rm != null && !roomMeta.ContainsKey(msg.rm))
                                roomMeta[msg.rm] = msg.ex;
                        }
                        else
                            txtStatus.AppendText(">>> " + msg.ToString() + "\r\n");

                    }
                    else
                    {
                        DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                        dt = dt.AddMilliseconds(msg.ts).ToLocalTime();
                        if (msg.op == "join")
                        {
                            if(!rooms.ContainsKey(msg.rm))
                            {
                                tabRooms.TabPages.Add(msg.rm);
                                rooms[msg.rm] = tabRooms.TabPages[tabRooms.TabPages.Count-1];
                                rooms[msg.rm].Text = roomMeta[msg.rm].name;
                                rooms[msg.rm].Controls.Add(new RichTextBox());
                                rooms[msg.rm].Controls[0].Dock = DockStyle.Fill;
                                RichTextBox txtStatus = (RichTextBox)rooms[msg.rm].Controls[0];
                                txtStatus.AppendText("[" + dt.ToLongTimeString() + "] ");
                                txtStatus.AppendText(" You have joined " + roomMeta[msg.rm].name + "\r\n");
                            }
                        }
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
                RichTextBox txtStatus;
                if(msg.rm != null && msg.rm != "" && rooms.ContainsKey(msg.rm))
                    txtStatus = (RichTextBox)rooms[msg.rm].Controls[0];
                else
                    txtStatus = (RichTextBox)tbpStatus.Controls[0];
                txtStatus.AppendText("[" + DateTime.Now.ToLongTimeString() + "] ");
                if (msg.op == "act" && msg.ex.message != null)
                {
                    if (msg.ex.isaction != null && msg.ex.isaction == true) txtStatus.AppendText("* " + conn.Username + " " + msg.ex.message + "\r\n");
                    else txtStatus.AppendText("<" + conn.Username + "> " + msg.ex.message + "\r\n");
                }
#if DEBUG
                txtStatus.AppendText("<<< " + msg.ToString() + "\r\n");
#endif
            }));
            if(conn.ssl.CanWrite)
                conn.ssl.EndWrite(ar);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            var roomsReversed = rooms.ToDictionary(x => x.Value, x => x.Key);
            if (txtMsg.Text == "") return;
            if (tabRooms.SelectedTab == tabRooms.TabPages[0]) return;
            if (txtMsg.Text[0] == '/')
            {
                if (txtMsg.Text.Split(' ')[0].Substring(1) == "me")
                    conn.SendMessage(txtMsg.Text.Substring(4), roomsReversed[tabRooms.SelectedTab], true, WriteCall);
                else if (txtMsg.Text.Split(' ')[0].Substring(1) == "join")
                    conn.Join(txtMsg.Text.Substring(6), WriteCall);
                else if (txtMsg.Text.Split(' ')[0].Substring(1) == "part")
                {
                    conn.Leave(roomsReversed[tabRooms.SelectedTab], WriteCall);
                    rooms.Remove(roomsReversed[tabRooms.SelectedTab]);
                    tabRooms.TabPages.Remove(tabRooms.SelectedTab);
                }
            }
            else
                conn.SendMessage(txtMsg.Text, roomsReversed[tabRooms.SelectedTab], false, WriteCall);
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
            conn.Leave(null,null);
        }
    }
}
