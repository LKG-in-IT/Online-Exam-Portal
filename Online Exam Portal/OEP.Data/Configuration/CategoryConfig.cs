using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OEP.Core.DomainModels.CategoryModel;

namespace OEP.Data.Configuration
{
    public class CategoryConfig:EntityTypeConfiguration<Category>
    {
        public CategoryConfig()
        {
            ToTable("Category");
            HasKey(a => a.Id);
            Property(u => u.Name).HasMaxLength(250);
        }
    }
}
