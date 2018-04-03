using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Core.DomainModels.PackageModel
{
   public class Package:BaseEntity
    {
        public string Name { get; set; }
        public string Details { get; set; }
        public Double Prize { get; set; }

        public string Duration { get; set; }
    }
}
