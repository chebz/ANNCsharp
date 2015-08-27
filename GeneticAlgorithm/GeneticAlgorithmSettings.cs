
namespace CPAI.GeneticAlgorithm
{
    public class GenomeSettings
    {
    }

    public class GeneticAlgorithmSettings
    {
        public double mMutationRate = 0.1;
        public double mCrossoverRate = 0.7;
        public int mMateThreshold = 5;
        public int mMaxPopulation = 100;

        public GenomeFactory mGenomeFactory;

        public GeneticAlgorithmSettings(GenomeFactory genomeFactory)
        {
            mGenomeFactory = genomeFactory;
        }
    }
}
