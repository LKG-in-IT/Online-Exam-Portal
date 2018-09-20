using OEP.Resources.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Resources.Common
{
    public class ExamResultResource
    {
        public ExamResultResource()
        {
            QuestionsResource = new QuestionsResource();
        }
        public QuestionsResource QuestionsResource { get; set; }

        public int SeletecdAnswer { get; set; }
    }
}
