using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Resources.Common
{
    public class ExamAnswersResourceCollection
    {
        public ExamAnswersResourceCollection()
        {
            ExamAnswersResourceList = new List<ExamAnswersResource>();
        }
        public List<ExamAnswersResource> ExamAnswersResourceList { get; set; }
        public int ExamId { get; set; }
    }

    public class ExamAnswersResource
    {        
        public int Answer { get; set; }
        public int QuestionId { get; set; }
    }
}
