using OEP.Core.DomainModels.QuestionModel;
using System.Data.Entity.ModelConfiguration;

namespace OEP.Data.Configuration
{
    public class QuestionsConfig: EntityTypeConfiguration<Questions>
    {
        public QuestionsConfig()
        {
            ToTable("Questions");
            HasKey(a => a.Id);
            Property(u => u.Question).HasMaxLength(250);
            Property(u => u.OptionA).HasMaxLength(250);
            Property(u => u.OptionB).HasMaxLength(250);
            Property(u => u.OptionC).HasMaxLength(250);
            Property(u => u.OptionD).HasMaxLength(250);
            Property(u => u.Answer).HasMaxLength(250);

        }
    }

    public class QuestionsTypeConfig : EntityTypeConfiguration<QuestionType>
    {
        public QuestionsTypeConfig()
        {
            ToTable("QuestionType");
            HasKey(a => a.Id);
            Property(u => u.Name).HasMaxLength(250).IsRequired();
        }
    }
}
