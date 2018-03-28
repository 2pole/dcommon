using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCommon.Data
{
    public interface IUnitOfWorkManager : IDisposable
    {
        IUnitOfWork CurrentUnitOfWork { get; }

        IUnitOfWorkScope CurrentScope { get; }

        IUnitOfWorkScope BeginScope();

        void EndScope(IUnitOfWorkScope scope);
    }
}
