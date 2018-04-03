using OEP.Core.DomainModels.ExamModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Data.Configuration
{
   public class ExamConfig:EntityTypeConfiguration<Exam>
    {
        public ExamConfig()
        {
            ToTable("Exam");
            HasKey(a => a.Id);
            Property(u => u.Name).HasMaxLength(250);

        }

    }
    public class ExamTypeConfig : EntityTypeConfiguration<ExamType>
    {
        public ExamTypeConfig()
        {
            ToTable("ExamType");
            HasKey(a => a.Id);
            Property(u => u.Name).HasMaxLength(250);

        }

    }

  public   class ExamQuestionConfig : EntityTypeConfiguration<ExamQuestion>
    {
        public ExamQuestionConfig()
        {
            ToTable("ExamQuestion");
            HasKey(a => a.Id);
          
        }
    }
}
