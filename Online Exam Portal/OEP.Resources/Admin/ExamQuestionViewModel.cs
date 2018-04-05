using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Resources.Admin
{
    public class ExamQuestionViewModel
    {
        public ExamQuestionViewModel()
        {
            ExamQuestionResourceList = new List<ExamQuestionResource>();
        }
        public int ExamId { get; set; }

        public string ExamName { get; set; }

        public IEnumerable<ExamQuestionResource> ExamQuestionResourceList { get; set; }
    }
}
