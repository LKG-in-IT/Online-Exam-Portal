using System;
using System.Collections.Generic;
using System.Text;
using OEP.Core.Data;

namespace OEP.Core.Services
{
    public interface IService : IDisposable
    {
        IUnitOfWork UnitOfWork { get; }
    }
}
