using CPAI.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ANNCSharp
{
    public class NeuronLayer : Poolable
    {
        private ObjectPool mPool;

        private NeuronLayerSettings mSettings;

        private List<Neuron> mNeurons = new List<Neuron>();

        private List<double> mOutputs = new List<double>();

        public NeuronLayer(ObjectPool pool, NeuronLayerSettings settings)
            : base(pool)
        {
            mPool = new ObjectPool(settings.mNSettings.mFactory, settings.mNumNeuronsPerLayerMax);
            mSettings = settings;
        }

        private Neuron AddNeuron()
        {
            Neuron neuron = (Neuron)mPool.GetInstance();
            Debug.Assert(neuron != null);
            mNeurons.Add(neuron);
            mOutputs.Add(0);
            return neuron;
        }

        private Neuron GetNeuron(int iNeuron)
        {
            if (mNeurons.Count > iNeuron)
                return mNeurons[iNeuron];
            return null;
        }

        private int NMutation() { return 0; } //TODO

        public int Init(int numInputsPerNeuron)
        {
            int numNeurons = Util.RandomRange(mSettings.mNumNeuronsPerLayerMin, mSettings.mNumNeuronsPerLayerMax);
            Init(numInputsPerNeuron, numNeurons);
            return numNeurons;
        }

        public int Init(int numInputsPerNeuron, int numNeurons)
        {
            for (int iNeuron = 0; iNeuron < numNeurons; iNeuron++)
            {
                AddNeuron().Init(numInputsPerNeuron);
            }
            return numNeurons;
        }

        public int Init(int numInputsPerNeuron, NeuronLayer parent, double mutationRate)
        {
            if (parent != null)
            {
                int numNeurons = parent.mNeurons.Count + NMutation();
                numNeurons = Util.Clamp(numNeurons, mSettings.mNumNeuronsPerLayerMin, mSettings.mNumNeuronsPerLayerMax);
                return Init(numInputsPerNeuron, numNeurons, parent, mutationRate);
            }
            return Init(numInputsPerNeuron);
        }

        public int Init(int numInputsPerNeuron, int numNeurons, NeuronLayer parent, double mutationRate)
        {
            for (int iNeurom = 0; iNeurom < numNeurons; iNeurom++)
            {
                AddNeuron().Init(numInputsPerNeuron, parent.GetNeuron(iNeurom), mutationRate);
            }
            return numNeurons;
        }

        public int Init(int numInputsPerNeuron, NeuronLayer mum, NeuronLayer dad, double mutationRate)
        {
            if (mum == null && dad != null)
                return Init(numInputsPerNeuron, dad, mutationRate);

            if (mum != null && dad == null)
                return Init(numInputsPerNeuron, mum, mutationRate);

            if (mum == null && dad == null)
                return Init(numInputsPerNeuron);

            int numNeurons = Util.RandomRange(mum.mNeurons.Count, dad.mNeurons.Count) + NMutation();
            numNeurons = Util.Clamp(numNeurons, mSettings.mNumNeuronsPerLayerMin, mSettings.mNumNeuronsPerLayerMax);
            return Init(numInputsPerNeuron, numNeurons, mum, dad, mutationRate);
        }

        public int Init(int numInputsPerNeuron, int numNeurons, NeuronLayer mum, NeuronLayer dad, double mutationRate)
        {
            int iSplit = Util.Random(numNeurons);

            for (int iNeurom = 0; iNeurom < numNeurons; iNeurom++)
            {
                if (iNeurom < iSplit)
                    AddNeuron().Init(numInputsPerNeuron, mum.GetNeuron(iNeurom), mutationRate);
                else
                    AddNeuron().Init(numInputsPerNeuron, dad.GetNeuron(iNeurom), mutationRate);
            }
            return numNeurons;
        }

        public void Update(List<double> inputs)
        {
            for (int iNeuron = 0; iNeuron < mNeurons.Count; iNeuron++)
            {
                mNeurons[iNeuron].Update(inputs);
                mOutputs[iNeuron] = mNeurons[iNeuron].Value;
            }
        }

        public override void Free()
        {            
            foreach (var neuron in mNeurons)
            {
                neuron.Free();
            }
            mNeurons.Clear();
            mOutputs.Clear();
            base.Free();
        }

        public List<double> Outputs { get { return mOutputs; } }

        public override string ToString()
        {
            string strNeurons = "";
            for (int iNeuron = 0; iNeuron < mNeurons.Count; iNeuron++)
            {
                strNeurons += string.Format("Neuron #{0}:\n{1}", 
                    iNeuron,
                    mNeurons[iNeuron].ToString()) + 
                    (iNeuron == mNeurons.Count - 1 ? "" : "\n");
            }

            return string.Format("Neurons Count = {0}\n{1}", mNeurons.Count, strNeurons);
        }
    }
}
