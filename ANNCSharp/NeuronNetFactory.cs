using CPAI.GeneticAlgorithm;
using CPAI.Utils;

namespace ANNCSharp
{
    public class NeuronNetFactory : GenomeFactory
    {
        public NeuronNetFactory(NeuronNetSettings nnSettings)
            : base(nnSettings)
        {
        }

        public override Poolable Create(ObjectPool pool)
        {
            return new NeuronNet(pool, (NeuronNetSettings)mSettings);
        }
    }
}
