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
        Dictionary<string, List<string>> userList = new Dictionary<string, List<string>>();
        List<string> roomsToJoin = new List<string>(); // Purely for keeping a queue of channels to join from favourites
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
                        RichTextBox txtRoom;
                        if (msg.rm != null && msg.rm != "" && rooms.ContainsKey(msg.rm))
                            txtRoom = (RichTextBox)rooms[msg.rm].Controls.Find("txtRoom", true).ToList()[0];
                        else
                            txtRoom = txtStatus;

                        DateTime dt = new DateTime(1970, 1, 1, 0, 0, 0, 0);
                        dt = dt.AddMilliseconds(msg.ts).ToLocalTime();
                        if (msg.op == "act" && msg.ex.message != null)
                        {
                            txtRoom.AppendText("[" + dt.ToLongTimeString() + "] ");
                            if (msg.ex.isaction != null && msg.ex.isaction == true) txtRoom.AppendText("* " + msg.sr + " " + msg.ex.message + "\r\n");
                            else txtRoom.AppendText("<" + msg.sr + "> " + msg.ex.message + "\r\n");
                        }
                        else if (msg.op == "join")
                        {
                            txtRoom.AppendText("[" + dt.ToLongTimeString() + "] ");
                            txtRoom.AppendText("* " + msg.sr + " has joined " + roomMeta[msg.rm].name + "\r\n");
                            ListView lst = (ListView)(rooms[msg.rm].Controls.Find("lstUsers", true).ToList()[0]);
                            ListViewItem item = new ListViewItem(msg.sr);
                            item.Name = msg.sr;
                            lst.Items.Add(item);
                            userList[msg.rm].Add(msg.sr);
                        }

                        else if (msg.op == "leave")
                        {
                            txtRoom.AppendText("[" + dt.ToLongTimeString() + "] ");
                            txtRoom.AppendText("[" + dt.ToLongTimeString() + "] ");
                            txtRoom.AppendText("* " + msg.sr + " has left " + roomMeta[msg.rm].name + ".\r\n");
                            ListView lst = (ListView)(rooms[msg.rm].Controls.Find("lstUsers", true).ToList()[0]);
                            lst.Items.Remove(lst.Items.Find(msg.sr, false)[0]);
                        }
                        else if (msg.op == "disconnect")
                        {
                            foreach(KeyValuePair<string, List<string>> keyval in userList)
                            {
                                if(keyval.Value.Contains(msg.sr))
                                {

                                    ListView lst = (ListView)(rooms[keyval.Key].Controls.Find("lstUsers", true).ToList()[0]);
                                    lst.Items.Remove(lst.Items.Find(msg.sr, false).ToList()[0]);
                                    RichTextBox txt = (RichTextBox)rooms[keyval.Key].Controls.Find("txtRoom", true).ToList()[0];
                                    txt.AppendText("[" + dt.ToLongTimeString() + "] ");
                                    txt.AppendText("* " + msg.sr + " has quit.\r\n");
                                }
                            }
                        }
                        else if (msg.op == "meta")
                        {
                            if (msg.rm != null && !roomMeta.ContainsKey(msg.rm))
                            {
                                roomMeta[msg.rm] = msg.ex;
                                userList[msg.rm] = msg.ex.users.ToObject<List<string>>();
                            }
                            else if (msg.ex != null && msg.ex.favs != null)
                            {
                                foreach (var room in msg.ex.favs)
                                    roomsToJoin.Add((string)room.id);
                                conn.Join(roomsToJoin[0], WriteCall);
                            }
                            else
                            {
                                txtRoom.AppendText("[" + dt.ToLongTimeString() + "] ");
                                txtRoom.AppendText(">>> " + msg.ToString() + "\r\n");
                            }
                        }
                        else
                            txtRoom.AppendText(">>> " + msg.ToString() + "\r\n");
                        txtRoom.SelectionStart = txtRoom.Text.Length;
                        txtRoom.ScrollToCaret();

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
                                rooms[msg.rm].Text = roomMeta[msg.rm].name + " (" + msg.rm + ")";
                                RichTextBox txtNew = new RichTextBox();
                                txtNew.Name = "txtRoom";
                                txtNew.Anchor = txtStatus.Anchor;
                                txtNew.Height = txtStatus.Height;
                                txtNew.Width = txtStatus.Width;
                                txtNew.Font = txtStatus.Font;
                                txtNew.ReadOnly = true;
                                rooms[msg.rm].Controls.Add(txtNew);
                                ListView lstUsers = new ListView();
                                lstUsers.Anchor = (AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Top);
                                foreach (var item in userList[msg.rm])
                                { 
                                    ListViewItem listitem = new ListViewItem(item);
                                    listitem.Name = item;
                                    lstUsers.Items.Add(listitem);
                                }
                                lstUsers.Items.Add(conn.Username);
                                lstUsers.Location = lstDummy.Location;
                                lstUsers.Height = lstDummy.Height;
                                lstUsers.Width = lstDummy.Width;
                                lstUsers.Anchor = lstDummy.Anchor;
                                lstUsers.Name = "lstUsers";
                                rooms[msg.rm].Controls.Add(lstUsers);
                                userList[msg.rm].Add(conn.Username);
                                txtNew.AppendText("[" + dt.ToLongTimeString() + "] ");
                                txtNew.AppendText(" You have joined " + roomMeta[msg.rm].name + "\r\n");
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
            if (msg.op == "join")
            {
                // Not too happy with this business, but SslStream is fucking annoying, so meh
                if (roomsToJoin.Count > 0)
                {
                    if (conn.ssl.CanWrite)
                        conn.ssl.EndWrite(ar);
                    conn.Join(roomsToJoin[0], WriteCall);
                    roomsToJoin.RemoveAt(0);
                    return;
                }
            }
            this.BeginInvoke((Action)(() =>
            {
                RichTextBox txtRoom;
                if (msg.rm != null && msg.rm != "" && rooms.ContainsKey(msg.rm))
                    txtRoom = (RichTextBox)rooms[msg.rm].Controls.Find("txtRoom", true).ToList()[0];
                else
                    txtRoom = txtStatus;

                if (msg.op == "act" && msg.ex.message != null)
                {
                    txtRoom.AppendText("[" + DateTime.Now.ToLongTimeString() + "] ");
                    if (msg.ex.isaction != null && msg.ex.isaction == true) txtRoom.AppendText("* " + conn.Username + " " + msg.ex.message + "\r\n");
                    else txtRoom.AppendText("<" + conn.Username + "> " + msg.ex.message + "\r\n");
                }
#if DEBUG
                txtRoom.AppendText("<<< " + msg.ToString() + "\r\n");
#endif
                txtRoom.SelectionStart = txtRoom.Text.Length;
                txtRoom.ScrollToCaret();
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
            if (e.KeyCode == Keys.Enter && !ModifierKeys.HasFlag(Keys.Shift))
            {
                e.SuppressKeyPress = true;
                btnSend_Click(sender, e);
            }
            else if (e.KeyCode == Keys.A && ModifierKeys.HasFlag(Keys.Control))
            {
                e.SuppressKeyPress = true;
                txtMsg.SelectAll();
            }
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            conn.Disconnect();
        }
    }
}
