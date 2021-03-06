﻿using System;
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

        public void Leave(string roomid, AsyncCallback call)
        {
            WriteString(new Payload("leave", null, roomid, null).SerializeForSend(),call);
        }

        public void Disconnect()
        {
            ssl.Write(System.Text.Encoding.UTF8.GetBytes(new Payload("disconnect", null, null, null).SerializeForSend()));
            ssl.Close();
            tcp.Close();
        }


        public void Join(string roomid, AsyncCallback call)
        {
            WriteString(new Payload("join", null, roomid, null).SerializeForSend(), call); 
        }

        public void ReadString(AsyncCallback call)
        {
            buffer = new byte[65536];
            if(ssl.CanRead)
                ssl.BeginRead(buffer, 0, 65536, ReadCallBack, call);
        }

        public void ReadCallBack(IAsyncResult ar)
        {
            int bytesRead = 0;
            if(ssl.CanRead)
                bytesRead = ssl.EndRead(ar);
            AsyncCallback cb = (AsyncCallback)ar.AsyncState;
            if (bytesRead > 0)
            {
                Payloads.Add(new Payload(System.Text.Encoding.UTF8.GetString(buffer)));             
                cb.Invoke(ar);
            }
            else ReadString(cb);
        }

        public void SendMessage(string msg, string room, bool isaction, AsyncCallback call)
        {
            if (isaction)
            {
                var ex = new { message = msg, isaction = isaction };
                WriteString(new Payload("act", null, room, ex).SerializeForSend(), call);
            }
            else
            {
                var ex = new { message = msg };
                WriteString(new Payload("act", null, room, ex).SerializeForSend(), call);
            }
        }

        public void WriteString(string msg, AsyncCallback call)
        {
            msg += "\n";
            if(ssl.CanWrite)
                ssl.BeginWrite(System.Text.Encoding.UTF8.GetBytes(msg), 0, System.Text.Encoding.UTF8.GetByteCount(msg), call, msg);
        }
    }
}
