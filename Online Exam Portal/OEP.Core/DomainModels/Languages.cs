using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Core.DomainModels
{
    public class Language: CommonDetailsEntity
    {
        //[Index(IsUnique = true)]  or we can fluent API to achieve this .HasColumnAnnotation("Index",
        public string Name { get; set; }
        public int DisplayOrder { get; set; }        
        public bool Default { get; set; }

    }

}
