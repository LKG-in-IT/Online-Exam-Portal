using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Resources.Admin
{
  public  class ExamList
    {
        public string Examtypeid { get; set; }
        public string SubcategoryId { get; set; }
        public string KeyWord { get; set; }
        public int skip { get; set; }
        public int pageSize { get; set; }

    }
}
