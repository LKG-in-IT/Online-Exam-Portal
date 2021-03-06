﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Resources.Admin
{
    public class ResultResource : BaseResource
    {
        public int ExamId { get; set; }

        public ExamResource Exam { get; set; }
        public int TotalQuestions { get; set; }

        public int TotalQuestionsAttended { get; set; }
        public int Mark { get; set; }

        public string ResultStatus { get; set; }

        public string TimeTaken { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0: d}")]
        public DateTime CreatedDate { get; set; }
    }

    public class ResultListResource
    {
        public ResultListResource()
        {
            ResultResourceList = new List<ResultResource>();
        }
        public List<ResultResource> ResultResourceList { get; set; }

        public int TotalPages { get; set; }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }


    }
}
