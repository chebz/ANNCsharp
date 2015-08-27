using CPAI.Utils;
namespace CPAI.GeneticAlgorithm
{
    public abstract class GenomeFactory : PoolableFactory
    {
        protected GenomeSettings mSettings;

        public GenomeFactory(GenomeSettings settings) :
            base()
        {
            mSettings = settings;
        }
    }
}
