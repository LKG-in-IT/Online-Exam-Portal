using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Core.DomainModels.EducationModels
{
    public class EducationDetails:BaseEntity
    {
        public int EducationTypeId { get; set; }

        [ForeignKey("EducationTypeId")]
        public virtual EducationType EducationType { get; set; }

        public string InstituteName { get; set; }

        
        public int? YearFromId { get; set; }

        [ForeignKey("YearFromId")]
        public virtual YearDetails YearFrom { get; set; } // Use the same name in [InverseProperty("")]  attribute


        public int? YearToId { get; set; }

        [ForeignKey("YearToId")]
        public virtual YearDetails YearTo { get; set; }
    }

    /*
     multiple EducationDetails<---<  One Year( Year Details)
    multiple EducationDetails<---<   One Education Type (Eductaion Types)
     */
}
