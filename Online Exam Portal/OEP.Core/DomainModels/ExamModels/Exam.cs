using OEP.Core.DomainModels.SubCategoryModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace OEP.Core.DomainModels.ExamModels
{
    public  class Exam: CommonDetailsEntity
    {
        public string Name { get; set; }
        public int ExamtypeId { get; set; }
        [ForeignKey("ExamtypeId")]
        public virtual  ExamType ExamType { get; set; }

        public int SubcategoryId { get; set; }
        [ForeignKey("SubcategoryId")]
        public virtual SubCategory SubCategory { get; set; }
        public int Passmark { get; set; }   
    }
}
