using CPAI.GeneticAlgorithm;
using CPAI.Utils;
using System.Collections.Generic;
using System.Diagnostics;

namespace ANNCSharp
{
    public class NeuronNet : Genome
    {
        private ObjectPool mPool;

        private List<NeuronLayer> mLayers = new List<NeuronLayer>();

        private NeuronLayer mOutputLayer;

        public NeuronNet(ObjectPool pool, NeuronNetSettings settings)
            : base(pool, settings)
        {
            mPool = new ObjectPool(settings.mLayerSettings.mFactory, settings.mNumLayersMax);
            mOutputLayer = new NeuronLayer(mPool, settings.mLayerSettings);
        }

        private NeuronLayer AddLayer()
        {
            var layer = (NeuronLayer)mPool.GetInstance();
            Debug.Assert(layer != null);
            mLayers.Add(layer);
            return layer;
        }

        private NeuronLayer GetLayer(int iLayer)
        {
            if (mLayers.Count > iLayer)
                return mLayers[iLayer];
            return null;
        }

        public override void Init()
        {
            var nnSettings = (NeuronNetSettings)mSettings;
            int numLayers = Util.RandomRange(nnSettings.mNumLayersMin, nnSettings.mNumLayersMax);
            int numNeurons = 0;

            if (numLayers > 0)
            {
                numNeurons = AddLayer().Init(nnSettings.mNumInputs);

                for (int iHiddenLayer = 0; iHiddenLayer < numLayers - 1; iHiddenLayer++)
                {
                    numNeurons = AddLayer().Init(numNeurons);
                }

                mOutputLayer.Init(numNeurons, nnSettings.mNumOutputs);
            }
            else
            {
                mOutputLayer.Init(nnSettings.mNumInputs, nnSettings.mNumOutputs);
            }
        }

        public override void Init(Genome parent, double mutationRate)
        {
            var nnParent = (NeuronNet)parent;
            var nnSettings = (NeuronNetSettings)mSettings;
            int numLayers = nnParent.mLayers.Count + LayerMutation();
            numLayers = Util.Clamp(numLayers, nnSettings.mNumLayersMin, nnSettings.mNumLayersMax);
            int numNeurons = 0;
            int iLayer = 0;

            if (numLayers > 0)
            {
                numNeurons = AddLayer().Init(nnSettings.mNumInputs, nnParent.GetLayer(iLayer), mutationRate);
                iLayer++;

                for (int iHiddenLayer = 0; iHiddenLayer < numLayers - 1; iHiddenLayer++)
                {
                    numNeurons = AddLayer().Init(numNeurons, nnParent.GetLayer(iLayer), mutationRate);
                    iLayer++;
                }

                mOutputLayer.Init(numNeurons, nnSettings.mNumOutputs, nnParent.mOutputLayer, mutationRate);
            }
            else
            {
                mOutputLayer.Init(nnSettings.mNumInputs, nnSettings.mNumOutputs, nnParent.mOutputLayer, mutationRate);
            }
        }

        public override void Init(Genome mum, Genome dad, double mutationRate)
        {
            var nnMum = (NeuronNet)mum;
            var nnDad = (NeuronNet)dad;
            var nnSettings = (NeuronNetSettings)mSettings;

            int numLayers = Util.RandomRange(nnMum.mLayers.Count, nnDad.mLayers.Count) + LayerMutation();
            numLayers = Util.Clamp(numLayers, nnSettings.mNumLayersMin, nnSettings.mNumLayersMax);

            int numNeurons = 0;
            int iLayer = 0;

            if (numLayers > 0)
            {
                numNeurons = AddLayer().Init(nnSettings.mNumInputs, nnMum.GetLayer(iLayer), nnDad.GetLayer(iLayer), mutationRate);
                iLayer++;

                for (int iHiddenLayer = 0; iHiddenLayer < numLayers - 1; iHiddenLayer++)
                {
                    numNeurons = AddLayer().Init(numNeurons, nnMum.GetLayer(iLayer), nnDad.GetLayer(iLayer), mutationRate);
                    iLayer++;
                }

                mOutputLayer.Init(numNeurons, nnSettings.mNumOutputs, nnMum.mOutputLayer, nnDad.mOutputLayer, mutationRate);
            }
            else
            {
                mOutputLayer.Init(nnSettings.mNumInputs, nnSettings.mNumOutputs, nnMum.mOutputLayer, nnDad.mOutputLayer, mutationRate);
            }
        }

        public void Update(List<double> inputs)
        {
            var nnSettings = (NeuronNetSettings)mSettings;

            Debug.Assert(inputs.Count == nnSettings.mNumInputs);

            foreach (var layer in mLayers)
            {
                layer.Update(inputs);
                inputs = layer.Outputs;
            }

            mOutputLayer.Update(inputs);
        }

        public List<double> Outputs
        {
            get { return mOutputLayer.Outputs; }
        }

        private int LayerMutation() { return 0; } //TODO

        public override void Free()
        {
            foreach (var layer in mLayers)
            {
                layer.Free();
            }
            mLayers.Clear();
            mOutputLayer.Free();

            base.Free();
        }

        public override string ToString()
        {
            string strLayers = "";
            for (int iLayer = 0; iLayer < mLayers.Count; iLayer++)
            {
                strLayers += string.Format("Layer #{0}:\n{1}\n\n", mLayers[iLayer].ToString());
            }
            strLayers += string.Format("Output Layer:\n{0}\n", mOutputLayer);
            return strLayers;
        }
    }
}
