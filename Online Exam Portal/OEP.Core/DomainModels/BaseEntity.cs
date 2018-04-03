using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        [Required]
        public string UserId { get; set; }
    }
}
