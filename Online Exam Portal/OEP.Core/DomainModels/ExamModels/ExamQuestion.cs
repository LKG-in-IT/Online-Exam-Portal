using OEP.Core.DomainModels.QuestionModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Core.DomainModels.ExamModels
{
   public class ExamQuestion:BaseEntity
    {
     
        public int ExamId { get; set; }
        [ForeignKey("ExamId")]
        public virtual Exam Exam { get; set; }
        public int QuestionId { get; set; }
        [ForeignKey("QuestionId")]
        public virtual Questions Questions { get; set; }
    }
}
