using OEP.Core.Data.Repository;
using OEP.Core.DomainModels.StudyMaterialsModel;
using OEP.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OEP.Services
{
  public  class StudyMaterialService:BaseService<StudyMaterial>,IStudyMaterial
    {
        public StudyMaterialService(IUnitOfWork unitOfWork) :base(unitOfWork)
        {

        }

    }
}
