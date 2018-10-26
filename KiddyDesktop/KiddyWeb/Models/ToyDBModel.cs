namespace KiddyWeb.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class ToyDBModel : DbContext
    {
        public ToyDBModel()
            : base("name=ToyDBModel2")
        {
        }

        public virtual DbSet<tblCustomer> tblCustomers { get; set; }
        public virtual DbSet<tblToy> tblToys { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<tblCustomer>()
                .Property(e => e.username)
                .IsUnicode(false);

            modelBuilder.Entity<tblCustomer>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<tblToy>()
                .Property(e => e.createdBy)
                .IsUnicode(false);

            modelBuilder.Entity<tblToy>()
                .Property(e => e.category)
                .IsUnicode(false);
        }
    }
}
