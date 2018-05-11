using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Resources.Admin
{
   public  class QuestionsViewResource
    {
        public int id { get; set; }
        public string Question { get; set; }
        public options options { get; set; }
        public string answerselected { get; set; }
      


    }
    public class options
    {
        public string a { get; set; }
        public string b { get; set; }
        public string c { get; set; }
        public string d { get; set; }

    }
}
