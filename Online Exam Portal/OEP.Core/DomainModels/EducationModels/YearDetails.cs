using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Core.DomainModels.EducationModels
{
    public class YearDetails: CommonDetailsEntity
    {
        public string Year { get; set; }

        [InverseProperty("YearFrom")]
        public ICollection<EducationDetails> From { get; set; }
        [InverseProperty("YearTo")]
        public ICollection<EducationDetails> To { get; set; }
    }
}
