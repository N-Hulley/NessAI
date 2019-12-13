using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

    public class ResponseEventArgs : EventArgs 
    {
        public string Message { get; set; }
    public Dictionary<string, string> RequestValues { get; set; }
    }

