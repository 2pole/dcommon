using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DCommon.Data;

namespace DCommon.Collections
{
    public class TreeNode<TEntity, TKey> : ITreeNode<TEntity, TKey>
        where TEntity : class, ITreeEntity<TKey>
        where TKey : struct
    {
        public TreeNode()
        {
        }

        public TreeNode(TEntity entity)
        {
            this.Entity = entity;
        }

        public TKey Id
        {
            get { return Entity == null ? default(TKey) : Entity.Id; }
        }

        public TKey? ParentId
        {
            get { return Entity == null ? new TKey?() : Entity.ParentId; }
        }

        public TEntity Entity { get; set; }

        public ITreeNode<TEntity, TKey> Parent { get; set; }

        public ICollection<ITreeNode<TEntity, TKey>> Children { get; set; }

        public bool HasParent
        {
            get { return Parent != null; }
        }

        public bool HasChildren
        {
            get { return Children != null && Children.Count > 0; }
        }

        public IEnumerable<ITreeNode<TEntity, TKey>> Descendants
        {
            get
            {
                return this.Children.Union(this.Children.SelectMany(d => d.Descendants));
            }
        }

        public IEnumerable<ITreeNode<TEntity, TKey>> Parents
        {
            get
            {
                var parent = this.Parent;
                while (parent != null)
                {
                    yield return parent;
                    parent = parent.Parent;
                }
            }
        }

        public ITreeNode<TEntity, TKey> Root
        {
            get { return this.HasParent ? this.Parents.Last() : this; }
        }

        //public virtual bool IsDescendant(TKey id)
        //{
        //    return this.Descendants.Any(d => Equals(d.Id, id));
        //}

        //public virtual bool IsAncestor(TKey id)
        //{
        //    return this.Parents.Any(d => Equals(d.Id, id));
        //}

        public bool Equals(ITreeNode<TEntity, TKey> other)
        {
            return this.Id.Equals(other.Id);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            return this.Equals(obj as ITreeNode<TEntity, TKey>);
        }
    }

    public class TreeNode<TEntity> : TreeNode<TEntity, int>, ITreeNode<TEntity>
         where TEntity : class, ITreeEntity<int>
    {
        public TreeNode()
            : base()
        {
        }

        public TreeNode(TEntity entity)
            : base(entity)
        {
        }
    }
}
