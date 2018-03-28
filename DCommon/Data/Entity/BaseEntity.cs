using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCommon.Data
{
    /// <summary>
    /// Base class for entities
    /// </summary>
    public abstract partial class BaseEntity<T> : IEntity, IEntity<T>
        where T : struct
    {
        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        public T Id { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as BaseEntity<T>);
        }

        private static bool IsTransient(BaseEntity<T> obj)
        {
            return obj != null && Equals(obj.Id, default(T));
        }

        private Type GetUnproxiedType()
        {
            return GetType();
        }

        public bool Equals(BaseEntity<T> other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (!IsTransient(this) &&
                !IsTransient(other) &&
                Equals(Id, other.Id))
            {
                var otherType = other.GetUnproxiedType();
                var thisType = GetUnproxiedType();
                return thisType.IsAssignableFrom(otherType) ||
                        otherType.IsAssignableFrom(thisType);
            }

            return false;
        }

        public override int GetHashCode()
        {
            if (Equals(Id, default(T)))
                return base.GetHashCode();
            return Id.GetHashCode();
        }

        public static bool operator ==(BaseEntity<T> x, BaseEntity<T> y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(BaseEntity<T> x, BaseEntity<T> y)
        {
            return !(x == y);
        }

        #region IEntity
        object IEntity.Id
        {
            get { return this.Id; }
            set { this.Id = (T)value; }
        }
        #endregion
    }
}
