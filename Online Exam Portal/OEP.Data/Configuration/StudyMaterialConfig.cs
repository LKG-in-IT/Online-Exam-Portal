using OEP.Core.DomainModels.StudyMaterialsModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.IdentityModel.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Data.Configuration
{
   public  class StudyMaterialConfig : EntityTypeConfiguration<StudyMaterial>
    {
        public StudyMaterialConfig()
        {
            ToTable("StudyMaterial");
            HasKey(a => a.Id);
            Property(u => u.Name).HasMaxLength(250);
        }
    }
}
