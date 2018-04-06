using OEP.Core.DomainModels.EducationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Resources.Admin
{
   public  class EducationDetailsResource:BaseResource
    {
        public int EducationTypeId { get; set; }
        public string InstituteName { get; set; }
        public int YearFromId { get; set; }
        public int YearToId { get; set; }
        public string ApplicationUserID { get; set; }
        public EducationType educationType { get; set; }
        public virtual YearDetails YearFrom { get; set; }
        public virtual YearDetails YearTo { get; set; }
    }
}
