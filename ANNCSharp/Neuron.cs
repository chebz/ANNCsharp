using CPAI.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace ANNCSharp
{
    public class Neuron : Poolable
    {
        private double mValue;

        private double mBiasWeight;

        private double mActivationResponse = 1.0;

        private double mAmplitude = 1.0;

        private double mOffset = 0;

        private List<double> mWeights = new List<double>();

        protected NeuronSettings mSettings;

        public Neuron(ObjectPool pool, NeuronSettings settings)
            : base(pool)
        {
            mSettings = settings;
        }

        protected double GetWeight(int iWeight)
        {
            if (mWeights.Count > iWeight)
                return mWeights[iWeight];
            return 0;
        }

        protected static double MutateValue(double value, double mutationRate, double perturbation, double min, double max)
        {
            if (Util.Random01() < mutationRate)
            {
                value += Util.RandomRange(-perturbation, perturbation);
            }
            value = Util.Clamp(value, min, max);
            return value;
        }

        protected double MutateWeight(double weight, double mutationRate)
        {
            return MutateValue(weight, mutationRate, mSettings.mWeightPerturbation, -1, 1);
        }

        protected void MutateAR(double activationResponse, double mutationRate)
        {
            mActivationResponse = MutateValue(activationResponse, mutationRate, mSettings.mActivationResponsePerturbation, -1, 1);
            if (mActivationResponse == 0)
                mActivationResponse = 0.0001;
        }

        protected void MutateAmplitude(double amplitude, double mutationRate)
        {
            mAmplitude = MutateValue(amplitude, mutationRate, mSettings.mAmplitudePerturbation, 0, 1);
        }

        protected void MutateOffset(double offset, double mutationRate)
        {
            mOffset = MutateValue(offset, mutationRate, mSettings.mOffsetPerturbation, -1, 1);
        }

        public virtual void Init(int numInputsPerNeuron)
        {
            mBiasWeight = Util.RandomRange(-1.0, 1.0);

            for (int iInput = 0; iInput < numInputsPerNeuron; iInput++)
            {
                mWeights.Add(Util.RandomRange(-1.0, 1.0));
            }
        }

        public virtual void Init(int numInputsPerNeuron, Neuron parent, double mutationRate)
        {
            if (parent != null)
            {
                mBiasWeight = MutateWeight(parent.mBiasWeight, mutationRate);
                for (int iInput = 0; iInput < numInputsPerNeuron; iInput++)
                {
                    mWeights.Add(MutateWeight(parent.GetWeight(iInput), mutationRate));
                }
                MutateAR(parent.mActivationResponse, mutationRate);
                MutateAmplitude(parent.mAmplitude, mutationRate);
                MutateOffset(parent.mOffset, mutationRate);
            }
            else
            {
                mBiasWeight = MutateWeight(0, mutationRate);
                for (int iInput = 0; iInput < numInputsPerNeuron; iInput++)
                {
                    mWeights.Add(MutateWeight(0, mutationRate));
                }
                MutateAR(mActivationResponse, mutationRate);
                MutateAmplitude(mAmplitude, mutationRate);
                MutateOffset(mOffset, mutationRate);
            }
        }

        public void Update(List<double> inputs)
        {
            if (inputs.Count != mWeights.Count)
                throw new Exception();

            mValue = 0;

            for (int iInput = 0; iInput < inputs.Count; iInput++)
            {
                mValue += inputs[iInput] * mWeights[iInput];
            }

            mValue += mBiasWeight * mSettings.mBias;

            //mValue = 1 / (mAmplitude + Math.Exp(-mValue / mActivationResponse)) - (1 / (mAmplitude * 2)) * (1 + mOffset);
            mValue = 1 / (1 + Math.Exp(-mValue / mActivationResponse));
        }

        public override void Free()
        {
            mWeights.Clear();
            base.Free();
        }

        public double Value { get { return mValue; } }

        public override string ToString()
        {
            string strWeights = "";
            for (int iWeight = 0; iWeight < mWeights.Count; iWeight++)
            {
                strWeights += string.Format("{0:0.000}", mWeights[iWeight]) + (iWeight == mWeights.Count - 1 ? "" : ", ");
            }

            return string.Format(
                "Bias Weight = {0}\n" +
                "Weights = ({1})\n" +
                "ActivationResponse = {2}\n" +
                "Amplitude = {3}\n" +
                "Offset = {4}",
                mBiasWeight, 
                strWeights,
                mActivationResponse,
                mAmplitude,
                mOffset);
        }
    }
}
