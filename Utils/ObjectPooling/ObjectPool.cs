using System.Collections.Generic;

namespace CPAI.Utils
{
    public class ObjectPool
    {
        private Stack<Poolable> mObjectsFree = new Stack<Poolable>();

        public ObjectPool(PoolableFactory factory, int numObjects)
        {
            for (int iObject = 0; iObject < numObjects; iObject++)
            {
                mObjectsFree.Push(factory.Create(this));
            }
        }

        internal void FreeInstance(Poolable poolable)
        {
            mObjectsFree.Push(poolable);
        }

        public Poolable GetInstance()
        {
            if (mObjectsFree.Count == 0)
                return null;

            var poolable = mObjectsFree.Pop();
            poolable.mFree = false;
            return poolable;
        }
    }
}
