using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DCommon.Tests
{
    public abstract class TestFixtureBase<T> : IDisposable //IUseFixture<T>, 
        where T : new()
    {
        protected T FixtureContext { get; private set; }

        public virtual void SetFixture(T data)
        {
            this.FixtureContext = data;
        }

        public virtual void Dispose()
        {
            var context = FixtureContext as IDisposable;
            if (context != null)
            {
                context.Dispose();
            }
        }
    }
}
