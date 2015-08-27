using CPAI.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CPAI.GeneticAlgorithm
{
    public class GeneticAlgorithm : ObjectPool
    {
        private GeneticAlgorithmSettings mSettings;

        private List<Genome> mPopulation = new List<Genome>();

        internal double mTotalFitness;

        internal double mNormalizedTotalFitness;

        internal double mWorstFitness, mBestFitness;

        internal bool sorted;

        public GeneticAlgorithm(GeneticAlgorithmSettings settings)
            : base(settings.mGenomeFactory, settings.mMaxPopulation * 2)
        {
            mSettings = settings;

            for (int iGenome = 0; iGenome < settings.mMaxPopulation; iGenome++)
            {
                AddGenome().Init();
            }
        }

        private Genome AddGenome() {
            Genome genome = (Genome)GetInstance();
            Debug.Assert(genome != null);
            mPopulation.Add(genome);
            return genome;
        }

        private Genome Cross(Genome mum, Genome dad)
        {
            var baby = (Genome)GetInstance();
            Debug.Assert(baby != null);
            baby.Init(mum, dad, mSettings.mMutationRate);
            return baby;
        }

        private Genome Cross(Genome parent)
        {
            var baby = (Genome)GetInstance();
            Debug.Assert(baby != null);
            baby.Init(parent, mSettings.mMutationRate);
            return baby;
        }

        private void Sort()
        {
            if (sorted)
                return;
            mPopulation.Sort((x, y) => y.Fitness.CompareTo(x.Fitness));

            mWorstFitness = mPopulation[mPopulation.Count - 1].Fitness;
            mBestFitness = mPopulation[0].Fitness;
            mNormalizedTotalFitness = 0;
            foreach (var g in mPopulation)
            {
                mNormalizedTotalFitness += g.Fitness - mWorstFitness;
            }
            sorted = true;
        }

        public void epoch() {
            Sort();

            while (mPopulation.Count < mSettings.mMaxPopulation * 2)
            {
                var genomeA = RouletteSelect();
                Debug.Assert(genomeA != null);
                var genomeB = SelectMate(genomeA);
                Debug.Assert(genomeB != null);

                if (Util.Random01() < mSettings.mCrossoverRate)
                {
                    mPopulation.Add(Cross(genomeA, genomeB));
                    mPopulation.Add(Cross(genomeB, genomeA));
                }
                else
                {
                    mPopulation.Add(Cross(genomeA));
                    mPopulation.Add(Cross(genomeB));
                }
            }

            for (int iGenome = 0; iGenome < mSettings.mMaxPopulation; iGenome++)
            {
                mPopulation[0].Free();
                mPopulation.RemoveAt(0);
            }
        }

        private Genome RouletteSelect()
        {
            double slice = Util.RandomRange(0, mNormalizedTotalFitness);
            slice = Util.RandomRange(0, slice); //take random twice for higher selection bias

            double fitness = 0;
            int order = 0;

            for (int iGenome = 0; iGenome < mSettings.mMaxPopulation; iGenome++)
            {
                var genome = mPopulation[iGenome];
                genome.mOrder = order++;
                fitness += (genome.Fitness - mWorstFitness);

                if (fitness >= slice)
                {
                    return genome;
                }
            }
            throw new Exception();
        }

        private Genome SelectMate(Genome mateA)
        {
            if (mPopulation.Count <= 1)
                return null;

            if (mateA == null)
                throw new Exception();

            int orderMin = Math.Max(0, mateA.mOrder - mSettings.mMateThreshold);
            int orderMax = Math.Min(mateA.mOrder + mSettings.mMateThreshold, mPopulation.Count);
            int iMateB = Util.RandomRangeExcept(orderMin, orderMax, mateA.mOrder);
            var mateB = mPopulation[iMateB];
            mateB.mOrder = iMateB;
            return mateB;
        }

        internal double UpdateFitness(double fitnessOld, double fitnessNew)
        {
            mTotalFitness += (fitnessNew - fitnessOld);
            sorted = false;
            return fitnessNew;
        }

        public double AverageFitness
        {
            get
            {
                return mTotalFitness / mPopulation.Count;
            }
        }

        public double BestFitness
        {
            get
            {
                return mBestFitness;
            }
        }

        public double WorstFitness
        {
            get
            {
                return mWorstFitness;
            }
        }

        public List<Genome> Population { get { return mPopulation; } }

        public List<Genome> SortedPopulation
        {
            get
            {
                Sort();
                return mPopulation;
            }
        }
    }
}
