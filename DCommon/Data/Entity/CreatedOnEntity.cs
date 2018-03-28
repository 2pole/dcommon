using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCommon.Data
{
    public abstract partial class CreatedOnEntity<TKey> : BaseEntity<TKey>
        where TKey : struct
    {
        protected CreatedOnEntity()
        {
            this.CreatedOn = DateTime.Now;
        }

        public DateTime CreatedOn { get; set; }
    }

}
