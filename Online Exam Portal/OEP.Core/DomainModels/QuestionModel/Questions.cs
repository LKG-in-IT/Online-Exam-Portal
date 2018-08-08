using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Core.DomainModels.QuestionModel
{
    public class Questions: CommonDetailsEntity
    {
        public string Question { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
        public int Answer { get; set; }

        public int QuestionTypeId { get; set; }
        
        [ForeignKey("QuestionTypeId")]
        public virtual QuestionType QuestionType { get; set; }

        public ICollection<QuestionsLocalized> QuestionsLocalized { get; set; }
    }

    public class QuestionsLocalized : CommonDetailsEntity
    {
        public string Question { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }

        public int QuestionsId { get; set; }

        [ForeignKey("QuestionsId")]
        public virtual Questions Questions { get; set; }

        public int LanguageId { get; set; }

        [ForeignKey("LanguageId")]
        public virtual Language Language { get; set; }
    }
}
