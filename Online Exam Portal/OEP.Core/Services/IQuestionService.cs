using OEP.Core.DomainModels.QuestionModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Core.Services
{
    public interface IQuestionService : IService<Questions>
    {
       int GetAllCount();
    }
}
