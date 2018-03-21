using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OEP.Core.DomainModels.Test
{
    [Table("MyTest")]
    public class Test:BaseEntity
    {
        [Required]
        public string Name { get; set; }
    }
}
