using OEP.Core.Data.Repository;
using OEP.Core.DomainModels.ExamModels;
using OEP.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Services
{
   public  class ExamQuestionService:BaseService<ExamQuestion>, IExamQuestionService
   {
       private readonly IRepository<ExamQuestion> _examQuestionRepository; 
        public ExamQuestionService(IUnitOfWork unitOfWork, IRepository<ExamQuestion> examQuestionRepository):base(unitOfWork)
        {
            _examQuestionRepository = examQuestionRepository;
        }

       public Task<List<ExamQuestion>> GetAllIncludingAsync(params Expression<Func<ExamQuestion, object>>[] includeProperties)
       {
           return _examQuestionRepository.GetAllIncludingAsync(includeProperties);
       }
    }
}
