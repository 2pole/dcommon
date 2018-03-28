using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace DCommon
{
    [DataContract]
    public class Result<T> : Result, IResult<T>
    {
        [DataMember]
        public T Data { get; set; }
    }

    [DataContract]
    public class ListResult<TItem> : Result<IList<TItem>>, IListResult<TItem>
    {
        public IEnumerator<TItem> GetEnumerator()
        {
            if (base.Data == null)
                return Enumerable.Empty<TItem>().GetEnumerator();

            return this.Data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
