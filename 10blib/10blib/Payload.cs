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
        public dynamic rm { get; set; }
        private JsonSerializerSettings settings;
        public Payload(string msg)
        {
            dynamic obj = JsonConvert.DeserializeObject<dynamic>(msg);
            if(obj.ts != null) ts = (long)obj.ts;
            if(obj.op != null) op = (string)obj.op;
            if(obj.sr != null) sr = (string)obj.sr;
            if(obj.ex != null) ex = obj.ex;
            settings = new JsonSerializerSettings();
            settings.NullValueHandling = NullValueHandling.Ignore;
        }

        public Payload(string Op, string Sr, string Rm, dynamic Ex)
        {
            op = Op;
            sr = Sr;
            ex = Ex;
            rm = Rm;
        }

        // Exists because ToString() doesn't exclude ts
        public string SerializeForSend()
        {
                return JsonConvert.SerializeObject(new { op = op, sr = sr, ex = ex }, settings);
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, settings);
        }
    }
}
