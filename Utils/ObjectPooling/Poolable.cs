namespace CPAI.Utils
{
    public abstract class Poolable
    {
        private ObjectPool mPool;

        internal bool mFree = true;

        public Poolable(ObjectPool pool)
        {
            mPool = pool;
        }

        public virtual void Free()
        {
            if (!mFree)
            {
                mFree = true;
                mPool.FreeInstance(this);
            }
        }

        public ObjectPool Pool { get { return mPool; } }
    }
}
