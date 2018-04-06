using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OEP.Core.DomainModels.CategoryModel;
using OEP.Core.DomainModels.EducationModels;

namespace OEP.Data.Configuration
{
    public class EducationConfig : EntityTypeConfiguration<EducationType>
    {
        public EducationConfig()
        {
            ToTable("EducationType");
            HasKey(a => a.Id);
            Property(u => u.Name).HasMaxLength(250);
        }
    }

    public class EducationYearConfig : EntityTypeConfiguration<YearDetails>
    {
        public EducationYearConfig()
        {
            ToTable("EducationYearDetails");
            HasKey(a => a.Id);
            Property(u => u.Year).HasMaxLength(250);
        }
    }

    public class EducationDetailsConfig : EntityTypeConfiguration<EducationDetails>
    {
        public EducationDetailsConfig()
        {
            ToTable("EducationDetails");
            HasKey(a => a.Id);
            Property(u => u.InstituteName).HasMaxLength(1000);
            Property(x => x.EducationTypeId).IsRequired();
            Property(x => x.ApplicationUserId).IsRequired();


        }
    }


}
