using OEP.Core.DomainModels.ExamModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Core.Services
{
    public interface IExamQuestionService : IService<ExamQuestion>
    {
        Task<List<ExamQuestion>> GetAllIncludingAsync(params Expression<Func<ExamQuestion, object>>[] includeProperties);
        

    }
}
