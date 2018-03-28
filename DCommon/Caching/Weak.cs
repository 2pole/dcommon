using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DCommon.Caching
{
    public class Weak<T>
    {
        private readonly WeakReference _target;

        public Weak(T target)
        {
            _target = new WeakReference(target);
        }

        public Weak(T target, bool trackResurrection)
        {
            _target = new WeakReference(target, trackResurrection);
        }

        public T Target
        {
            get { return (T)_target.Target; }
            set { _target.Target = value; }
        }
    }
}
