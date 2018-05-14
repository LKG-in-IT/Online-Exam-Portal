using OEP.Core.Data.Repository;
using OEP.Core.DomainModels.ResultModel;
using OEP.Core.Services;

namespace OEP.Services
{
    public class ResultService:BaseService<Result>, IResultService
    {
        public ResultService(IUnitOfWork unitOfWork):base(unitOfWork )  
        {

        }
    }
}
