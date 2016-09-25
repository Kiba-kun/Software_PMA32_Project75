using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
namespace SoftwarePractice_10.Models.Configs
{
    class UnitConfig : EntityTypeConfiguration<Unit>
    {
        public UnitConfig()
        {
            HasKey(x => x.Id);
            Property(x => x.Id).HasDatabaseGeneratedOption(null);
        }
    }

    class FilmConfig : EntityTypeConfiguration<Film>
    {
        public FilmConfig()
        {
            Property(x => x.Name).HasMaxLength(20);
            Property(x => x.Rating).IsOptional();
            Property(x => x.Summary).HasMaxLength(500);
        }
    }

    class UserConfig : EntityTypeConfiguration<User>
    {
        public UserConfig()
        {
            Property(x => x.FirstName).IsRequired();
            Property(x => x.LastName).IsRequired();
            Property(x => x.ReturnDate).IsOptional();
        }
    }
}
