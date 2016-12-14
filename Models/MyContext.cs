using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace SoftwarePractice_10.Models
{
    class MyContext : DbContext
    {
        public MyContext():
            base("customConnection")
        {

        }
        public DbSet<Film> Films{ get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<MainActor> Actors { get; set; }
        public DbSet<ContactInfo> ContactInfos{ get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Ignore<Unit>();
            modelBuilder.Configurations.Add(new Configs.FilmConfig());
            modelBuilder.Configurations.Add(new Configs.UnitConfig());
            modelBuilder.Configurations.Add(new Configs.UserConfig());
            modelBuilder.Configurations.Add(new Configs.ContactInfoConfig());

        }
    }
}
