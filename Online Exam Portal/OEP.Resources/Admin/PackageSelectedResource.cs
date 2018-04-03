using OEP.Core.DomainModels.PackageModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Resources.Admin
{
   public class PackageSelectedResource:BaseResource
    {
      
        public int PackageId { get; set; }
        public virtual Package Package { get; set; }
       
        public DateTime Datefrom { get; set; }
        public DateTime Dateto { get; set; }
    }
}
