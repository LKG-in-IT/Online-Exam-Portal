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
    public class QuestionService : BaseService<Questions>, IQuestionService
    {
        IRepository<Questions> _questionsrepository;

        public QuestionService(IUnitOfWork unitOfWork, IRepository<Questions> questionsrepository) : base(unitOfWork)
        {
            _questionsrepository = questionsrepository;
        }
        

        public int GetAllCount()
        {
           return _questionsrepository.Table.Count();
        }
    }
}
