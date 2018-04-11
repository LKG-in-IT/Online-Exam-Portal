using System.Collections.Generic;

namespace OEP.Core.DomainModels.QuestionModel
{
    public class QuestionType: CommonDetailsEntity
    {
        public string Name { get; set; }

        public ICollection<Questions> Questions { get; set; }
    }
}