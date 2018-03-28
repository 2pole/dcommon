using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DCommon.Data;

namespace DCommon.Collections
{
    public interface IHashTreeCollection<TEntity, TKey> : ICollection<ITreeNode<TEntity, TKey>>
        where TEntity : class, ITreeEntity<TKey>
        where TKey : struct
    {
        ITreeNode<TEntity, TKey> Get(TKey key);
        ICollection<TKey> Keys { get; }
        ICollection<TEntity> Entities { get; }
        IEnumerable<ITreeNode<TEntity, TKey>> Nodes { get; }
    }

    public interface IHashTreeCollection<TEntity> : ICollection<ITreeNode<TEntity, int>>
        where TEntity : class, ITreeEntity<int>
    {
        ITreeNode<TEntity> Get(int key);
        ICollection<int> Keys { get; }
        ICollection<TEntity> Entities { get; }
        IEnumerable<ITreeNode<TEntity>> Nodes { get; }
    }
}
