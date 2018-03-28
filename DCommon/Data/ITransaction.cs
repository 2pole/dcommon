using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCommon.Data
{
    public interface ITransaction : IDisposable
    {
        Guid TransactionId { get; }

        void Commit();

        void Rollback();
    }
}
