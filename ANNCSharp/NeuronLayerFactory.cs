using CPAI.Utils;

namespace ANNCSharp
{
    public class NeuronLayerFactory : PoolableFactory
    {
        NeuronLayerSettings mLayerSettings;

        public NeuronLayerFactory(NeuronLayerSettings layerSettings)
            : base()
        {
            mLayerSettings = layerSettings;
        }

        public override Poolable Create(ObjectPool pool)
        {
            return new NeuronLayer(pool, mLayerSettings);
        }
    }
}
