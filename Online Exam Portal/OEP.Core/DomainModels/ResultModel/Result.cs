using OEP.Core.DomainModels.ExamModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Core.DomainModels.ResultModel
{
  public  class Result:CommonDetailsEntity
    {
        public int ExamId { get; set; }
        [ForeignKey("ExamId")]
        public virtual Exam Exam { get; set; }

        public  int Mark { get; set; }

        public int TotalQuestions { get; set; }

        public int TotalQuestionsAttended { get; set; }

        public string ResultStatus { get; set; }

        public string TimeTaken { get; set; }


    }
}
