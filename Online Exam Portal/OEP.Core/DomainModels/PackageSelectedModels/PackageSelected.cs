using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OEP.Core.DomainModels.Identity;
using OEP.Core.DomainModels.PackageModel;

namespace OEP.Core.DomainModels.PackageSelectedModels
{
   public class PackageSelected:BaseEntity
    {
       

        public int PackageId { get; set; }

        [ForeignKey("PackageId")]
        public virtual Package Package { get; set; }
        public DateTime Datefrom { get; set; }
        public DateTime Dateto { get; set; }

    }
}
