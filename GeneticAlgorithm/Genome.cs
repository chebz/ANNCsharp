using CPAI.Utils;
using System;
namespace CPAI.GeneticAlgorithm
{
    public abstract class Genome : Poolable
    {
        internal int mOrder = 0;

        protected GenomeSettings mSettings;

        protected double mFitness = 0;

        protected Genome(ObjectPool pool, GenomeSettings settings)
            : base(pool)
        {
            mSettings = settings;
        }

        public abstract void Init();

        public abstract void Init(Genome parent, double mutationRate);

        public abstract void Init(Genome mum, Genome dad, double mutationRate);

        public override void Free()
        {
            Fitness = 0;
            base.Free();
        }

        public double Fitness
        {
            get { return mFitness; }
            set
            {
                value = Util.Clamp(value, double.MinValue, double.MaxValue);
                var ga = (GeneticAlgorithm)Pool;
                mFitness = ga.UpdateFitness(mFitness, value);
            }
        }
    }
}
