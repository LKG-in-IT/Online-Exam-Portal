using System.Data.Entity.ModelConfiguration;
using OEP.Core.DomainModels.Test;
using OEP.Core.DomainModels.Test2;

namespace OEP.Data.Configuration
{
public    class Test2Configuration: EntityTypeConfiguration<Test2>
    {
        public Test2Configuration()
        {
            Property(x => x.Name).HasColumnName("UserName").HasMaxLength(5);
        }
    }
}
