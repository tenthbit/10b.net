using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace _10blib
{
    public class Payload
    {
        public long ts { get; set; }
        public string op { get; set; }
        public string sr { get; set; }
        public dynamic ex { get; set; }
        public dynamic tp { get; set; }
        public Payload(string msg)
        {
            dynamic obj = JsonConvert.DeserializeObject<dynamic>(msg);
            if(obj.ts != null) ts = (long)obj.ts;
            if(obj.op != null) op = (string)obj.op;
            if(obj.sr != null) sr = (string)obj.sr;
            if(obj.ex != null) ex = obj.ex;
        }

        public Payload(long Ts, string Op, string Sr, string Tp, dynamic Ex)
        {
            ts = Ts;
            op = Op;
            sr = Sr;
            ex = Ex;
            tp = Tp;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
