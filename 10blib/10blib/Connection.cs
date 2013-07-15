using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Net.Sockets;
using System.Net.Security;
using System.Threading;

namespace _10blib
{
    public class Connection
    {
        TcpClient tcp;
        public SslStream ssl;
        byte[] buffer;
        public List<Payload> Payloads = new List<Payload>();
        public string Host { get; set; }
        public string Username { get; set; }
        public string Pass { get; set; }
        public int Port { get; set; }
        public Connection(string host, int port, string user, string pass)
        {
            tcp = new TcpClient(host, port);
            ssl = new SslStream(tcp.GetStream(), false);
            Username = user;
            Pass = pass;
            try
            {
                ssl.AuthenticateAsClient(host);
            }
            catch (System.Security.Authentication.AuthenticationException e)
            {
                tcp.Close();
                throw e;
            }
            buffer = new byte[65536];
        }

        public void Auth(AsyncCallback call)
        {
            var ex = new { method = "password", username = Username, password = Pass };
            WriteString(new Payload("auth", null, null, ex).SerializeForSend(),call);          
        }

        public void Leave(AsyncCallback call)
        {
            var ex = new { user = Username };
            WriteString(new Payload("leave", null, null, ex).SerializeForSend(), call);
            ssl.Close();
            tcp.Close();
        }

        public void ReadString(AsyncCallback call)
        {
            buffer = new byte[65536];
            ssl.BeginRead(buffer, 0, 65536, ReadCallBack, call);
        }

        public void ReadCallBack(IAsyncResult ar)
        {
            int bytesRead = ssl.EndRead(ar);
            AsyncCallback cb = (AsyncCallback)ar.AsyncState;
            if (bytesRead > 0)
            {
                Payloads.Add(new Payload(System.Text.Encoding.UTF8.GetString(buffer)));             
                cb.Invoke(ar);
            }
            else ReadString(cb);
        }

        public void SendMessage(string msg, string room, AsyncCallback call)
        {
            var ex = new { data = msg, type="msg" };
            WriteString(new Payload("act", null, room, ex).SerializeForSend(), call);
        }

        public void WriteString(string msg, AsyncCallback call)
        {
            msg += "\n";
            ssl.BeginWrite(System.Text.Encoding.UTF8.GetBytes(msg), 0, System.Text.Encoding.UTF8.GetByteCount(msg), call, msg);
        }
    }
}
