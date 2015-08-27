using CPAI.GeneticAlgorithm;

namespace ANNCSharp
{
    public class NeuronSettings
    {
        public double mWeightPerturbation = 0.1;
        public double mActivationResponsePerturbation = 0.2;
        public double mBias = -1.0;
        public double mAmplitudePerturbation = 0.2;
        public double mOffsetPerturbation = 0.06;
        public NeuronFactory mFactory;

        public NeuronSettings()
        {
            mFactory = new NeuronFactory(this);
        }
    }

    public class NeuronLayerSettings
    {
        public int mNumNeuronsPerLayerMin = 1;
        public int mNumNeuronsPerLayerMax = 1;
        public int mNumNeuronsPertrubation = 0;

        public NeuronLayerFactory mFactory;

        public NeuronSettings mNSettings;

        public NeuronLayerSettings(NeuronSettings nSettings)
        {
            mNSettings = nSettings;
            mFactory = new NeuronLayerFactory(this);
        }
    }

    public class NeuronNetSettings : GenomeSettings
    {
        public int mNumInputs = 1;
        public int mNumOutputs = 1;

        public int mNumLayersMin = 1;
        public int mNumLayersMax = 1;
        public int mNumLayersPertrubation = 0;

        public NeuronNetFactory mFactory;

        public NeuronLayerSettings mLayerSettings;

        public NeuronNetSettings(NeuronLayerSettings layerSettings)
            : base()
        {
            mLayerSettings = layerSettings;
            mFactory = new NeuronNetFactory(this);
        }
    }
}
