using OEP.Core.DomainModels.SubCategoryModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Data.Configuration
{
 public   class SubCAtegoryConfig:EntityTypeConfiguration<SubCategory>
    {
        public SubCAtegoryConfig()
        {
            ToTable("SubCategory");
            HasKey(a => a.Id);
            Property(u => u.Name).HasMaxLength(250);
        }
    }
}
