using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KiddyWeb.Models
{
    public class OrderDetailDTO
    {
        public int id { get; set; }

        public int orderID { get; set; }

        public int toyID { get; set; }

        public int quantity { get; set; }
    }
}