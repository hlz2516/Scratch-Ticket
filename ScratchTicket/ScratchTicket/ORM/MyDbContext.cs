using SQLite.CodeFirst;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScratchTicket.ORM
{
    public class MyDbContext:DbContext
    {
        public MyDbContext():base("MyDbContext")
        {
             
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //var connInitializer = new SqliteCreateDatabaseIfNotExists<MyDbContext>(modelBuilder);
            var connInitializer = new SqliteDropCreateDatabaseWhenModelChanges<MyDbContext>(modelBuilder);
            Database.SetInitializer(connInitializer);
        }

        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<CardBundle> CardBundles { get; set; }
        public DbSet<PurchasedCardBundle> PurchasedCardBundles { get; set; }
    }
}
