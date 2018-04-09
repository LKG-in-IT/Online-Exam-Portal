using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Core.DomainModels
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        
    }

    public class CommonDetailsEntity: BaseEntity
    {
        public bool Status { get; set; }
        [Column(TypeName = "DateTime2")]
        public DateTime CreatedDate { get; set; }
        [Column(TypeName = "DateTime2")]
        public DateTime UpdatedDate { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
