using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCommon.Data
{
    public interface IEntity
    {
        object Id { get; set; }
    }

    public interface IEntity<TKey> : IEntity
    {
        TKey Id { get; set; }
    }
}
