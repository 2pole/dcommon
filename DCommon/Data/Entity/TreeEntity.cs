using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCommon.Data
{
    public class TreeEntity<TKey> : BaseEntity<TKey>, ITreeEntity<TKey>
         where TKey : struct
    {
        public TKey? ParentId { get; set; }
    }
}
