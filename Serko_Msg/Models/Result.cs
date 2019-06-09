using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Serko_Msg.Models
{
    public class Result
    {
        public string cost_centre { get; set; }
        public string payment_method { get; set; }
        public double total { get; set; }
        public double total_excluding_GST { get; set; }
        public double GST { get; set; }
    }

  

}