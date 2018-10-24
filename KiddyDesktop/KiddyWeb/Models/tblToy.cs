namespace KiddyWeb.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("tblToy")]
    public partial class tblToy
    {
        public int id { get; set; }

        [StringLength(50)]
        public string name { get; set; }

        public string desciption { get; set; }

        public double? price { get; set; }

        public int? quantity { get; set; }

        public bool? isActived { get; set; }

        [StringLength(50)]
        public string createdBy { get; set; }

        [StringLength(30)]
        public string category { get; set; }
    }
}
