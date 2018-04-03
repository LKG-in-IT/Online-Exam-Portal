using OEP.Core.DomainModels.ExamModels;
using OEP.Core.DomainModels.QuestionModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Resources.Admin
{
   public class ExamQuestionResource:BaseResource
    {
        public int ExamId { get; set; }
       
      public Exam Exam { get; set; }
        public Questions Questions { get; set; }
        public int QuestionId { get; set; }
      
    
    }
}
