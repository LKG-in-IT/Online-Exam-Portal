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
        public string Answer { get; set; }


        public int QuestionTypeId { get; set; }
        public QuestionTypeResource QuestionType { get; set; }
    }
}
