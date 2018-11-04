using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KiddyDesktop.Models
{
    public class OrderDTO
    {
        public int id { get; set; }


        public string cusID { get; set; }


        public string datetime { get; set; }


        public string emlID { get; set; }

        public string payment { get; set; }

        public string status { get; set; }

        public string address { get; set; }
    }
}