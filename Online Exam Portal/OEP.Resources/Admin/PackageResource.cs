using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Resources.Admin
{
   public  class PackageResource:BaseResource
    {
        public string Name { get; set; }
        public string Details { get; set; }
        public Double Prize { get; set; }

        public string Duration { get; set; }
      

    }
}
