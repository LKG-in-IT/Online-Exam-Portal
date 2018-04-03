using OEP.Core.DomainModels.CategoryModel;
using OEP.Core.DomainModels.PackageModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Data.Configuration
{
  public  class PackageConfig : EntityTypeConfiguration<Package>
    {
        public PackageConfig()
        {
            ToTable("Package");
            HasKey(a => a.Id);
            Property(u => u.Name).HasMaxLength(250);
            
           
        }
    }
}
