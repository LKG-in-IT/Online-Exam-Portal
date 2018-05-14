using OEP.Core.DomainModels.ResultModel;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Data.Configuration
{
   public  class ResultConfig:EntityTypeConfiguration<Result>
    {

        public ResultConfig()
        {
            ToTable("Result");
            HasKey(a => a.Id);
           
        }
    }
}
