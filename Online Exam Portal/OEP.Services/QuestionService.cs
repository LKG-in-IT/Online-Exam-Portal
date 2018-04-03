using OEP.Core.Data.Repository;
using OEP.Core.DomainModels.QuestionModel;
using OEP.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Services
{
  public  class QuestionService:BaseService<Questions>,IQuestionService
    {
        public QuestionService(IUnitOfWork unitOfWork):base(unitOfWork)
        {

        }
    }
}
