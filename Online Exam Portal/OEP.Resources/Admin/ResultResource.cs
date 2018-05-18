using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Resources.Admin
{
    public class ResultResource:BaseResource
    {
        public int ExamId { get; set; }
    
        public ExamResource Exam { get; set; }

        public int Mark { get; set; }

        public string ResultStatus { get; set; }

        public string TimeTaken { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0: d}")]
        public DateTime CreatedDate { get; set; }
    }
}
