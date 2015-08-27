
namespace CPAI.Utils
{
    public abstract class PoolableFactory
    {
        public abstract Poolable Create(ObjectPool pool);
    }
}
