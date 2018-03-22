using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Core.DomainModels.Test2
{
    [Table("MyTest1")]
    public    class Test2:BaseEntity
    {
        public string Name { get; set; }
    }
}
