using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DCommon.Data;

namespace DCommon.Collections
{
    public interface ITreeNode<TEntity, TKey> : IEquatable<ITreeNode<TEntity, TKey>>
        where TEntity : ITreeEntity<TKey>
        where TKey : struct
    {
        TKey Id { get; }
        TKey? ParentId { get; }
        TEntity Entity { get; }

        ITreeNode<TEntity, TKey> Parent { get; }
        ICollection<ITreeNode<TEntity, TKey>> Children { get; }

        bool HasParent { get; }
        bool HasChildren { get; }

        IEnumerable<ITreeNode<TEntity, TKey>> Descendants { get; }
        IEnumerable<ITreeNode<TEntity, TKey>> Parents { get; }
        ITreeNode<TEntity, TKey> Root { get; }
    }

    public interface ITreeNode<TEntity> : ITreeNode<TEntity, int>
        where TEntity : ITreeEntity<int>
    {
    }
}
