using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCommon.Data.Entity
{
    public interface ILogicDeletable
    {
        bool Delete { get; set; }
    }
}
