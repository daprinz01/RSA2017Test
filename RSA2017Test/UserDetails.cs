using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Numerics;

namespace RSA2017Test
{
    public class RSA2017TestDB
    {
        [Key]
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string PassWord { get; set; }
        public string Email { get; set; }
        public string KeyId { get; set; }
        public virtual MyKeys MyKeys { get; set; }

    }

    //   Add a ** Category** class with the following definition:




    public class MyKeys
    {
        public MyKeys()
        {
            this.RSA2017TestDB = new ObservableCollection<RSA2017TestDB>();
        }
        [Key]
        public string KeyId { get; set; }
        public string D { get; set; }
        public string E { get; set; }
        public string N { get; set; }
        public string Dp { get; set; }
        public string Dq { get; set; }
        public string Qinv { get; set; }
        public string P { get; set; }
        public string Q { get; set; }
        public string Ep { get; set; }
        public string Eq { get; set; }
        public int KeySize { get; set; }

        public virtual ObservableCollection<RSA2017TestDB> RSA2017TestDB { get; private set; }
    }

    public class RSA2017TestDBContext : DbContext
    {
        public RSA2017TestDBContext() : base("UserDBConnectionString")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<RSA2017TestDBContext, RSA2017Test.Migrations.Configuration>("UserDBConnectionString"));

             //Database.SetInitializer<RSA2017TestDBContext>(new CreateDatabaseIfNotExists<RSA2017TestDBContext>());

            // Database.SetInitializer<RSA2017TestDBContext>(new DropCreateDatabaseIfModelChanges<RSA2017TestDBContext>());
            // Database.SetInitializer<RSA2017TestDBContext>(new DropCreateDatabaseAlways<RSA2017TestDBContext>());
            // Database.SetInitializer<RSA2017TestDBContext>(new UserDBInitializer());
        }
        public DbSet<MyKeys> MyKeys { get; set; }
        public DbSet<RSA2017TestDB> RSA2017TestDB { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }
    }


    public class UserDBInitializer : CreateDatabaseIfNotExists<RSA2017TestDBContext>
    {
        protected override void Seed(RSA2017TestDBContext context)
        {
            base.Seed(context);
        }
    }
}
