using OEP.Core.DomainModels.ExamModels;
using OEP.Core.DomainModels.QuestionModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace OEP.Resources.Admin
{
   public class ExamQuestionResource:BaseResource
    {
        public int ExamId { get; set; }
        public int ExamTypeId { get; set; }

       public IEnumerable<SelectListItem> SelectListItemExamType { get; set; }

        public IEnumerable<SelectListItem> SelectListItemExam { get; set; }

        public ExamType ExamType { get; set; }
        public Exam Exam { get; set; }
        public Questions Questions { get; set; }
        public int QuestionId { get; set; }
      
    
    }
}
