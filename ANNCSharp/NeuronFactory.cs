using CPAI.Utils;

namespace ANNCSharp
{
    public class NeuronFactory : PoolableFactory
    {
        NeuronSettings mSettings;

        public NeuronFactory(NeuronSettings settings) :
            base()
        {
            mSettings = settings;
        }

        public override Poolable Create(ObjectPool pool)
        {
            return new Neuron(pool, mSettings);
        }
    }
}
