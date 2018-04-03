using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Core.DomainModels.EducationModels
{
    public class EducationType: CommonDetailsEntity
    {
        public string Name { get; set; }
        public virtual ICollection<EducationDetails> EducationDetailsModel { get; set; }
    }
}
