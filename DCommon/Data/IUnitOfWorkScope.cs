using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCommon.Data
{
    ///<summary>
    ///</summary>
    public interface IUnitOfWorkScope : IDisposable
    {
        /// <summary>
        /// Gets the unique Id of the <see cref="UnitOfWorkScope"/>.
        /// </summary>
        /// <value>A <see cref="Guid"/> representing the unique Id of the scope.</value>
        Guid ScopeId { get; }

        ///<summary>
        /// Commits the current running transaction in the scope.
        ///</summary>
        void Commit();

        /// <summary>
        /// Gets the current <see cref="IUnitOfWork"/>.
        /// </summary>
        IUnitOfWork UnitOfWork { get; }
    }
}
