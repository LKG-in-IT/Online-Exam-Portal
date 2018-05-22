using OEP.Resources.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Resources.Common
{
   public class ProgressReportResource:BaseResource
    {
        public int ExamCount { get; set; }
        public int Win { get; set; }
        public int Fail { get; set; }
    }
}
