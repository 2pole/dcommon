using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using DCommon.Data;

namespace DCommon.Collections
{
    public partial class HashTreeCollection<TEntity, TKey> : Collection<ITreeNode<TEntity, TKey>>, IHashTreeCollection<TEntity, TKey>
        where TEntity : class, ITreeEntity<TKey>
        where TKey : struct
    {
        public static HashTreeCollection<TEntity, TKey> Empty = new HashTreeCollection<TEntity, TKey>(new List<ITreeNode<TEntity, TKey>>(0));

        private readonly IDictionary<TKey, ITreeNode<TEntity, TKey>> _nodeMaps = new Dictionary<TKey, ITreeNode<TEntity, TKey>>();

        protected IDictionary<TKey, ITreeNode<TEntity, TKey>> NodeMaps
        {
            get { return _nodeMaps; }
        }

        protected HashTreeCollection()
        {
        }

        public HashTreeCollection(IEnumerable<TEntity> items)
            : this(items, null)
        {
        }

        public HashTreeCollection(IEnumerable<TEntity> items, Func<TEntity, bool> rootSelector)
            : this(BuildChildren(items, CreateNode, rootSelector))
        {
        }

        protected HashTreeCollection(IList<ITreeNode<TEntity, TKey>> nodes)
            : base(nodes)
        {
            this.BuildMaps(nodes.Union(nodes.SelectMany(x => x.Descendants)));
        }

        protected virtual void BuildMaps(IEnumerable<ITreeNode<TEntity, TKey>> items)
        {
            foreach (var item in items)
            {
                _nodeMaps[item.Id] = item;
            }
        }

        #region IHashTreeCollection<TEntity>

        public ITreeNode<TEntity, TKey> Get(TKey key)
        {
            ITreeNode<TEntity, TKey> node;
            _nodeMaps.TryGetValue(key, out node);
            return node;
        }

        public ICollection<TKey> Keys
        {
            get { return _nodeMaps.Keys; }
        }

        public ICollection<TEntity> Entities
        {
            get { return _nodeMaps.Values.Select(d => d.Entity).ToList(); }
        }

        public IEnumerable<ITreeNode<TEntity, TKey>> Nodes
        {
            get { return _nodeMaps.Values; }
        }

        #endregion

        private static TreeNode<TEntity, TKey> CreateNode(TEntity entity)
        {
            return new TreeNode<TEntity, TKey>(entity);
        }

        protected static IList<ITreeNode<TEntity, TKey>> BuildChildren(IEnumerable<TEntity> source,
            Func<TEntity, TreeNode<TEntity, TKey>> creator,
            Func<TEntity, bool> rootSelector = null)
        {
            if (source == null)
                return new List<ITreeNode<TEntity, TKey>>(0);

            var items = source as TEntity[] ?? source.ToArray();
            if (items.Length == 0)
                return new List<ITreeNode<TEntity, TKey>>(0);

            creator = creator ?? CreateNode;
            var nodes = source.Select(creator).ToList();
            if (rootSelector == null)
                rootSelector = d => d.ParentId == null;
            var rootNodes = BuildChildren(nodes, null, rootSelector);
            return rootNodes;
        }

        protected static IList<ITreeNode<TEntity, TKey>> BuildChildren(IList<TreeNode<TEntity, TKey>> nodes,
            TreeNode<TEntity, TKey> parent,
            Func<TEntity, bool> childrenFilter)
        {
            IList<TreeNode<TEntity, TKey>> items = new List<TreeNode<TEntity, TKey>>();
            foreach (var item in nodes.Where(x => childrenFilter(x.Entity)))
            {
                var id = item.Id;
                var children = BuildChildren(nodes, item, d => Equals(d.ParentId, id));
                item.Parent = parent;
                item.Children = new Collection<ITreeNode<TEntity, TKey>>(children);
                items.Add(item);
            }
            return items.Select(x => (ITreeNode<TEntity, TKey>)x).ToList();
        }
    }

    public class HashTreeCollection<TEntity> : HashTreeCollection<TEntity, int>, IHashTreeCollection<TEntity>
        where TEntity : class, ITreeEntity<int>
    {
        private HashTreeCollection()
        {
        }

        public HashTreeCollection(IEnumerable<TEntity> items)
            : this(items, null)
        {
        }

        public HashTreeCollection(IEnumerable<TEntity> items, Func<TEntity, bool> rootSelector)
            : base(BuildChildren(items, CreateNode, rootSelector))
        {
        }

        private static TreeNode<TEntity> CreateNode(TEntity entity)
        {
            return new TreeNode<TEntity>(entity);
        }

        ITreeNode<TEntity> IHashTreeCollection<TEntity>.Get(int key)
        {
            return base.Get(key) as ITreeNode<TEntity>;
        }

        IEnumerable<ITreeNode<TEntity>> IHashTreeCollection<TEntity>.Nodes
        {
            get { return NodeMaps.Values.Cast<ITreeNode<TEntity>>(); }
        }
    }
}
