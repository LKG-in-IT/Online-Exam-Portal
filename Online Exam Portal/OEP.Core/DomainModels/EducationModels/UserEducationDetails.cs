using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OEP.Core.DomainModels.Identity;

namespace OEP.Core.DomainModels.EducationModels
{
    public class UserEducationDetails:BaseEntity
    {
        public string ApplicationUserId { get; set; }

        [ForeignKey("ApplicationUserId")]
        public ApplicationUser ApplicationUser { get; set; }

        public int EducationDetailsId { get; set; }

        [ForeignKey("EducationDetailsId")]
        public EducationDetails EducationDetails { get; set; }
    }
}
