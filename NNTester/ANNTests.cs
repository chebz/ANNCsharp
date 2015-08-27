using ANNCSharp;
using CPAI.GeneticAlgorithm;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace NNTester
{
    public class ANNTests
    {
        private bool mLearningStopped = false;

        public ANNTests() { }

        private void learnPopulation(GeneticAlgorithm ga, List<double> inputs, List<double> expected)
        {
            foreach (var pGenome in ga.Population)
            {
                var pNN = (NeuronNet)pGenome;
                learnNN(pNN, inputs, expected);
            }
        }

        static double calculateFitness(double output, double expected)
        {
            double delta = Math.Abs(output - expected);
            return 0 - delta;
        }

        private void learnNN(NeuronNet pNN, List<double> inputs, List<double> expected)
        {
            pNN.Update(inputs);
            var outputs = pNN.Outputs;
            Debug.Assert(outputs.Count == expected.Count);
            double tempFitness = pNN.Fitness;
            for (int ipOutput = 0; ipOutput < outputs.Count; ipOutput++)
            {
                tempFitness += calculateFitness(outputs[ipOutput], expected[ipOutput]);
            }
            pNN.Fitness = tempFitness;
        }

        private List<double> predict(GeneticAlgorithm ga, List<double> inputs)
        {
            var pNN = (NeuronNet)ga.SortedPopulation[0];
            pNN.Update(inputs);
            return pNN.Outputs;
        }

        private void printNNData(GeneticAlgorithm ga)
        {
            var pNN = (NeuronNet)ga.SortedPopulation[0];

        }

        public void testRandomRange()
        {

        }

        public void test1()
        {
            Console.WriteLine("Running test 1...");

            NeuronSettings nSettings = new NeuronSettings();
            NeuronLayerSettings layerSettings = new NeuronLayerSettings(nSettings);
            layerSettings.mNumNeuronsPerLayerMin = 2;
            layerSettings.mNumNeuronsPerLayerMax = 2;

            NeuronNetSettings nnSettings = new NeuronNetSettings(layerSettings);

            nnSettings.mNumInputs = 1;
            nnSettings.mNumOutputs = 1;
            nnSettings.mNumLayersMin = 0;
            nnSettings.mNumLayersMax = 0;

            GeneticAlgorithmSettings gaSettings = new GeneticAlgorithmSettings(nnSettings.mFactory);
            gaSettings.mMaxPopulation = 100;

            GeneticAlgorithm ga = new GeneticAlgorithm(gaSettings);

            List<double> inputs = new List<double>() { 0 };
            List<double> expected = new List<double>() { 0 };

            int mNumEpochs = 100;

            Console.WriteLine("Learning...");
            for (int iEpoch = 0; iEpoch < mNumEpochs; iEpoch++)
            {
                Console.Write(string.Format("Epoch: {0} / {1}\t", iEpoch + 1, mNumEpochs));

                for (int x = 0; x <= 100; x++)
                {
                    inputs.Clear();
                    expected.Clear();

                    inputs.Add((double)x / 100.0);
                    expected.Add((double)x / 100.0);
                    learnPopulation(ga, inputs, expected);
                }

                Console.Write(string.Format("Average: {0}\t", ga.AverageFitness));
                Console.Write(string.Format("Best: {0}\t", ga.BestFitness));
                Console.Write(string.Format("Worst: {0}\n", ga.WorstFitness));

                ga.epoch();
            }

            Console.WriteLine("Predicting...");

            for (int x = 0; x <= 100; x++)
            {
                inputs[0] = (double)x / 100.0;
                var outputs = predict(ga, inputs);
                Console.WriteLine(string.Format("Input:\t{0}\t\tOutput:\t{1}", inputs[0], outputs[0]));
            }
        }

        public void test2()
        {
            Console.WriteLine("Running test 2...");

            NeuronSettings nSettings = new NeuronSettings();
            NeuronLayerSettings layerSettings = new NeuronLayerSettings(nSettings);
            layerSettings.mNumNeuronsPerLayerMin = 2;
            layerSettings.mNumNeuronsPerLayerMax = 2;

            NeuronNetSettings nnSettings = new NeuronNetSettings(layerSettings);

            nnSettings.mNumInputs = 2;
            nnSettings.mNumOutputs = 1;
            nnSettings.mNumLayersMin = 0;
            nnSettings.mNumLayersMax = 0;

            GeneticAlgorithmSettings gaSettings = new GeneticAlgorithmSettings(nnSettings.mFactory);
            gaSettings.mMaxPopulation = 100;

            GeneticAlgorithm ga = new GeneticAlgorithm(gaSettings);

            List<double> inputs = new List<double>() { 0, 0};
            List<double> expected = new List<double>() { 0 };

            int mNumEpochs = 1000;

            Console.WriteLine("Learning...");
            for (int iEpoch = 0; iEpoch < mNumEpochs; iEpoch++)
            {
                Console.Write(string.Format("Epoch: {0} / {1}\t", iEpoch + 1, mNumEpochs));

                inputs[0] = 0;
                inputs[1] = 0;
                expected[0] = 0;
                learnPopulation(ga, inputs, expected);

                inputs[0] = 1;
                inputs[1] = 0;
                expected[0] = 0;
                learnPopulation(ga, inputs, expected);

                inputs[0] = 0;
                inputs[1] = 1;
                expected[0] = 0;
                learnPopulation(ga, inputs, expected);

                inputs[0] = 1;
                inputs[1] = 1;
                expected[0] = 1;
                learnPopulation(ga, inputs, expected);

                Console.Write(string.Format("Average: {0}\t", ga.AverageFitness));
                Console.Write(string.Format("Best: {0}\t", ga.BestFitness));
                Console.Write(string.Format("Worst: {0}\n", ga.WorstFitness));

                ga.epoch();
            }

            Console.WriteLine("Predicting...");

            inputs[0] = 0;
            inputs[1] = 0;
            var outputs = predict(ga, inputs);
            Console.WriteLine(string.Format("Input:\t{0}, {1}\t\tOutput:\t{2}", inputs[0], inputs[1], outputs[0]));

            inputs[0] = 1;
            inputs[1] = 0;
            outputs = predict(ga, inputs);
            Console.WriteLine(string.Format("Input:\t{0}, {1}\t\tOutput:\t{2}", inputs[0], inputs[1], outputs[0]));

            inputs[0] = 0;
            inputs[1] = 1;
            outputs = predict(ga, inputs);
            Console.WriteLine(string.Format("Input:\t{0}, {1}\t\tOutput:\t{2}", inputs[0], inputs[1], outputs[0]));

            inputs[0] = 1;
            inputs[1] = 1;
            outputs = predict(ga, inputs);
            Console.WriteLine(string.Format("Input:\t{0}, {1}\t\tOutput:\t{2}", inputs[0], inputs[1], outputs[0]));
        }

        public void test3()
        {
            Console.WriteLine("Running test 3, multiplication...");

            NeuronSettings nSettings = new NeuronSettings();
            NeuronLayerSettings layerSettings = new NeuronLayerSettings(nSettings);
            layerSettings.mNumNeuronsPerLayerMin = 2;
            layerSettings.mNumNeuronsPerLayerMax = 2;

            NeuronNetSettings nnSettings = new NeuronNetSettings(layerSettings);

            nnSettings.mNumInputs = 1;
            nnSettings.mNumOutputs = 1;
            nnSettings.mNumLayersMin = 0;
            nnSettings.mNumLayersMax = 0;

            GeneticAlgorithmSettings gaSettings = new GeneticAlgorithmSettings(nnSettings.mFactory);
            gaSettings.mMaxPopulation = 100;

            GeneticAlgorithm ga = new GeneticAlgorithm(gaSettings);

            List<double> inputs = new List<double>() { 0 };
            List<double> expected = new List<double>() { 0 };

            int mNumEpochs = 100;

            Console.WriteLine("Learning...");
            for (int iEpoch = 0; iEpoch < mNumEpochs; iEpoch++)
            {
                Console.Write(string.Format("Epoch: {0} / {1}\t", iEpoch + 1, mNumEpochs));

                for (double x = 0; x <= 1; x += 0.01)
                {
                    inputs[0] = x;
                    expected[0] = x * x;
                    learnPopulation(ga, inputs, expected);
                }

                Console.Write(string.Format("Average: {0}\t", ga.AverageFitness));
                Console.Write(string.Format("Best: {0}\t", ga.BestFitness));
                Console.Write(string.Format("Worst: {0}\n", ga.WorstFitness));

                ga.epoch();
            }

            Console.WriteLine("Predicting...");

            for (double x = 0; x <= 1; x += 0.1)
            {
                inputs[0] = x;
                var outputs = predict(ga, inputs);
                Console.WriteLine(string.Format("Input:\t{0}\t\tOutput:\t{1}", inputs[0], outputs[0]));
            }

            Console.Write(ga.SortedPopulation[0].ToString());
        }

        public void test4()
        {
            Console.WriteLine("Running test 4, cos...");

            NeuronSettings nSettings = new NeuronSettings();
            NeuronLayerSettings layerSettings = new NeuronLayerSettings(nSettings);
            layerSettings.mNumNeuronsPerLayerMin = 1;
            layerSettings.mNumNeuronsPerLayerMax = 1;

            NeuronNetSettings nnSettings = new NeuronNetSettings(layerSettings);

            nnSettings.mNumInputs = 1;
            nnSettings.mNumOutputs = 1;
            nnSettings.mNumLayersMin = 0;
            nnSettings.mNumLayersMax = 0;

            GeneticAlgorithmSettings gaSettings = new GeneticAlgorithmSettings(nnSettings.mFactory);
            gaSettings.mMaxPopulation = 1000;

            GeneticAlgorithm ga = new GeneticAlgorithm(gaSettings);

            List<double> inputs = new List<double>() { 0 };
            List<double> expected = new List<double>() { 0 };

            int mNumEpochs = 100;

            Console.WriteLine("Learning...");
            for (int iEpoch = 0; iEpoch < mNumEpochs; iEpoch++)
            {
                Console.Write(string.Format("Epoch: {0} / {1}\t", iEpoch + 1, mNumEpochs));

                for (double x = -1.5; x <= 1.5; x += 0.001)
                {
                    inputs[0] = x;
                    expected[0] = Math.Sin(x);
                    learnPopulation(ga, inputs, expected);
                }

                Console.Write(string.Format("Average: {0}\t", ga.AverageFitness));
                Console.Write(string.Format("Best: {0}\t", ga.BestFitness));
                Console.Write(string.Format("Worst: {0}\n", ga.WorstFitness));

                ga.epoch();
            }

            
            Console.Write(ga.SortedPopulation[0].ToString());
        }

        public void test5()
        {
            mLearningStopped = false;
            Thread t = new Thread(new ThreadStart(test5Thread));
            t.Start();
            Console.ReadKey();
            mLearningStopped = true;
        }

        public void test5Thread()
        {
            Console.WriteLine("Running test 5, min...");

            DateTime timeStart = DateTime.Now;

            NeuronSettings nSettings = new NeuronSettings();
            NeuronLayerSettings layerSettings = new NeuronLayerSettings(nSettings);
            layerSettings.mNumNeuronsPerLayerMin = 2;
            layerSettings.mNumNeuronsPerLayerMax = 2;

            NeuronNetSettings nnSettings = new NeuronNetSettings(layerSettings);

            nnSettings.mNumInputs = 2;
            nnSettings.mNumOutputs = 1;
            nnSettings.mNumLayersMin = 2;
            nnSettings.mNumLayersMax = 2;

            GeneticAlgorithmSettings gaSettings = new GeneticAlgorithmSettings(nnSettings.mFactory);
            gaSettings.mMaxPopulation = 100;

            GeneticAlgorithm ga = new GeneticAlgorithm(gaSettings);

            List<double> inputs = new List<double>() { 0, 0 };
            List<double> expected = new List<double>() { 0 };

            int iEpoch = 0;

            Console.WriteLine("Learning...");
            while (!mLearningStopped && iEpoch < 100)
            {
                Console.Write(string.Format("Epoch: {0}\t", iEpoch++));

                for (double x = 0; x <= 1; x += 0.01)
                {
                    for (double y = 0; y <= 1; y += 0.01)
                    {
                        inputs[0] = x;
                        inputs[1] = y;

                        expected[0] = x*y;

                        learnPopulation(ga, inputs, expected);
                    }
                }
               
                Console.Write(string.Format("Average: {0:0.00}\t", ga.AverageFitness));
                Console.Write(string.Format("Best: {0:0.00}\t", ga.BestFitness));
                Console.Write(string.Format("Worst: {0:0.00}\n", ga.WorstFitness));

                ga.epoch();
            }
            DateTime timeEnd = DateTime.Now;
            TimeSpan ts = timeEnd - timeStart;
            Console.WriteLine("Learning took: " + ts.ToString());

            Console.WriteLine("Predicting...");

            for (double x = 0; x < 1; x += 0.1)
            {
                for (double y = 0; y < 1; y += 0.1)
                {
                    inputs[0] = x;
                    inputs[1] = y;
                    var outputs = predict(ga, inputs);
                    Console.WriteLine(string.Format("Input:\t{0}, {1}\t\tOutput:\t{2:0.000}", inputs[0], inputs[1], outputs[0]));
                }
            }

        }
    }
}
