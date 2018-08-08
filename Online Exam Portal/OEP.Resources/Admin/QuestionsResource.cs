using OEP.Core.DomainModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Resources.Admin
{
    public class QuestionsResource : BaseResource
    {
        [Required]       
        public string Question { get; set; }
        [Required]
        public string OptionA { get; set; }
        [Required]
        public string OptionB { get; set; }
        [Required]
        public string OptionC { get; set; }
        [Required]
        public string OptionD { get; set; }
        [Required]
        public int Answer { get; set; }


        public int QuestionTypeId { get; set; }
        public QuestionTypeResource QuestionType { get; set; }

        public List<QuestionsLocalizedResource> QuestionsLocalized { get; set; }
    }

    public class QuestionsLocalizedResource : BaseResource
    {
        public string Question { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }

        public int LanguageId { get; set; }
        public LanguageResource Language { get; set; }
    }

    public class LanguageResource : BaseResource
    {
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
    }
}
