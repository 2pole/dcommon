using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DCommon
{
    public interface IResult<T> : IResult
    {
        T Data { get; set; }
    }

    public interface IListResult<TItem> : IResult<IList<TItem>>, IEnumerable<TItem>
    {   
    }
}
