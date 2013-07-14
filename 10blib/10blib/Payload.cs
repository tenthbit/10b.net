using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace _10blib
{
    public class Payload
    {
        public long Timestamp { get; set; }
        public string Operation { get; set; }
        public string Source { get; set; }
        public dynamic Extra { get; set; }
        public Payload(string msg)
        {
            dynamic obj = JsonConvert.DeserializeObject<dynamic>(msg);
            if(obj.ts != null) Timestamp = (long)obj.ts;
            if(obj.op != null) Operation = (string)obj.op;
            if(obj.sr != null) Source = (string)obj.sr;
            if(obj.ex != null) Extra = obj.ex;
        }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
