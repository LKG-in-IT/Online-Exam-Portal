using OEP.Core.DomainModels;
using OEP.Core.DomainModels.QuestionModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Infrastructure.Annotations;
using System.Data.Entity.ModelConfiguration;

namespace OEP.Data.Configuration
{
    public class QuestionsConfig : EntityTypeConfiguration<Questions>
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
            Property(u => u.Answer);

        }
    }

    public class QuestionsLocalizedConfig : EntityTypeConfiguration<QuestionsLocalized>
    {
        public QuestionsLocalizedConfig()
        {
            ToTable("QuestionsLocalized");
            HasKey(a => a.Id);
            Property(u => u.Question).HasMaxLength(250);
            Property(u => u.OptionA).HasMaxLength(250);
            Property(u => u.OptionB).HasMaxLength(250);
            Property(u => u.OptionC).HasMaxLength(250);
            Property(u => u.OptionD).HasMaxLength(250);

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

    public class LanguageConfig : EntityTypeConfiguration<Language>
    {
        public LanguageConfig()
        {
            ToTable("Languages");
            HasKey(l => l.Id);
            Property(l => l.Name).HasMaxLength(250).IsRequired().HasColumnAnnotation("Index",
                                                                       new IndexAnnotation(
                                                                                            new[] {
                                                                                                     new IndexAttribute("Index") { IsUnique = true }
                                                                                                   }
                                                                                           )
                                                                      );
            Property(l => l.DisplayOrder);
        }
    }
}
