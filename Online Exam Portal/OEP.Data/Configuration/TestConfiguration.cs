using System.Data.Entity.ModelConfiguration;
using OEP.Core.DomainModels.Test;

namespace OEP.Data.Configuration
{
    public class TestConfiguration:EntityTypeConfiguration<Test>
    {
        public TestConfiguration()
        {
            Property(x => x.Name).HasColumnName("User Name").HasMaxLength(250);
        }
    }
}
