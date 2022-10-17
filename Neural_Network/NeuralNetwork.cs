using System.Collections.Generic;
using System;
using System.IO;
using RandomGenerator;
using System.Security.Cryptography;

namespace Neural_Network
{
    public class NeuralNetwork : IComparable<NeuralNetwork>
    {
        public readonly static int[] STRUCT = { 8 + 10 + 10, 10 + 10 + 8, 10 };
        public const string FILE_EXSTANTION = "nnp";

        public double Fitnes => _fitness;
        private static Random_Generator _generator = new Random_Generator();

        private int[] layers;//layers
        private double[][] neurons;//neurons
        private double[][] biases;//biasses
        private double[][][] weights;//weights

        private double _fitness;//fitness

        public NeuralNetwork(Network_Struct layers)
        {
            this.layers = new int[layers.HiddenLayers_Count + 2];
            this.layers[0] = layers.InputLayer_Size;
            for (int i = 0; i < layers.HiddenLayers_Count; i++)
            {
                this.layers[1+i] = layers.HiddenLayers_Sizes[i];
            }
            this.layers[layers.HiddenLayers_Count + 1] = layers.OutputLayer_Size;
            _fitness = 0;
            InitNeurons();
            InitBiases();
            InitWeights();
        }

        public void Chainge_Fitnes(double value)
        {
            _fitness += value;
        }

        private void InitNeurons()//create empty storage array for the neurons in the network.
        {
            List<double[]> neuronsList = new List<double[]>();
            for (int i = 0; i < layers.Length; i++)
            {
                neuronsList.Add(new double[layers[i]]);
            }
            neurons = neuronsList.ToArray();
        }

        private void InitBiases()//initializes and populates array for the biases being held within the network.
        {
            List<double[]> biasList = new List<double[]>();
            for (int i = 0; i < layers.Length; i++)
            {
                double[] bias = new double[layers[i]];
                for (int j = 0; j < layers[i]; j++)
                {
                    bias[j] = _generator.Next_Double(-0.5, 0.5);
                }
                biasList.Add(bias);
            }
            biases = biasList.ToArray();
        }

        private void InitWeights()//initializes random array for the weights being held in the network.
        {
            List<double[][]> weightsList = new List<double[][]>();
            for (int i = 1; i < layers.Length; i++)
            {
                List<double[]> layerWeightsList = new List<double[]>();
                int neuronsInPreviousLayer = layers[i - 1];
                for (int j = 0; j < neurons[i].Length; j++)
                {
                    double[] neuronWeights = new double[neuronsInPreviousLayer];
                    for (int k = 0; k < neuronsInPreviousLayer; k++)
                    {
                        double sd = 1f / ((neurons[i].Length + neuronsInPreviousLayer) / 2f);
                        neuronWeights[k] = _generator.Next_Double(-sd, sd);
                    }
                    layerWeightsList.Add(neuronWeights);
                }
                weightsList.Add(layerWeightsList.ToArray());
            }
            weights = weightsList.ToArray();
        }

        public double[] FeedForward(double[] inputs)//feed forward, inputs >==> outputs.
        {
            for (int i = 0; i < inputs.Length; i++)
            {
                neurons[0][i] = inputs[i];
            }
            for (int i = 1; i < layers.Length; i++)
            {
                int layer = i - 1;
                for (int j = 0; j < neurons[i].Length; j++)
                {
                    double value = 0f;
                    for (int k = 0; k < neurons[layer].Length; k++)
                    {
                        value += weights[layer][j][k] * neurons[layer][k];
                    }
                    neurons[i][j] = activate(value + biases[i][j]);
                }
                layer = 0;
            }
            return neurons[neurons.Length - 1];
        }

        public double activate(double value)
        {
            return Math.Tanh(value);
        }

        public void Mutate(double chance, double val)//used as a simple mutation function for any genetic implementations.
        {
            for (int i = 0; i < biases.Length; i++)
            {
                for (int j = 0; j < biases[i].Length; j++)
                {
                    biases[i][j] = (_generator.Next_Double(1) <= chance) ? biases[i][j] += _generator.Next_Double(-val, val) : biases[i][j];
                    if (biases[i][j] > 1)
                        biases[i][j] = 1;
                    if (biases[i][j] < -1)
                        biases[i][j] = -1;

                }
            }

            for (int i = 0; i < weights.Length; i++)
            {
                for (int j = 0; j < weights[i].Length; j++)
                {
                    for (int k = 0; k < weights[i][j].Length; k++)
                    {
                        weights[i][j][k] = (_generator.Next_Double(1) <= chance) ? weights[i][j][k] += _generator.Next_Double(-val, val) : weights[i][j][k];
                        if (weights[i][j][k] > 1)
                            weights[i][j][k] = .99;
                        if (weights[i][j][k] < -1)
                            weights[i][j][k] = -.99;
                    }
                }
            }
        }

        public void Union(double chance, NeuralNetwork other)
        {
            for (int i = 0; i < biases.Length; i++)
            {
                for (int j = 0; j < biases[i].Length; j++)
                {
                    if (_generator.Next_Double(1) <= chance)
                        biases[i][j] =  other.biases[i][j];
                }
            }

            for (int i = 0; i < weights.Length; i++)
            {
                for (int j = 0; j < weights[i].Length; j++)
                {
                    for (int k = 0; k < weights[i][j].Length; k++)
                    {
                        if (_generator.Next_Double(1) <= chance)
                            weights[i][j][k] = other.weights[i][j][k];
                    }
                }
            }
        }

        public int CompareTo(NeuralNetwork other) //Comparing For NeuralNetworks performance.
        {
            if (other == null) return 1;

            if (_fitness > other._fitness)
                return 1;
            else if (_fitness < other._fitness)
                return -1;
            else
                return 0;
        }

        public NeuralNetwork Copy(NeuralNetwork nn) //For creatinga deep copy, to ensure arrays are serialzed.
        {
            for (int i = 0; i < biases.Length; i++)
            {
                for (int j = 0; j < biases[i].Length; j++)
                {
                    nn.biases[i][j] = biases[i][j];
                }
            }
            for (int i = 0; i < weights.Length; i++)
            {
                for (int j = 0; j < weights[i].Length; j++)
                {
                    for (int k = 0; k < weights[i][j].Length; k++)
                    {
                        nn.weights[i][j][k] = weights[i][j][k];
                    }
                }
            }
            return nn;
        }

        public void Load(string path)//this loads the biases and weights from within a file into the neural network.
        {
            if (File.Exists(path) == false)
                return;

            TextReader tr = new StreamReader(path);
            int NumberOfLines = (int)new FileInfo(path).Length;
            string[] ListLines = new string[NumberOfLines];
            int index = 1;
            for (int i = 1; i < NumberOfLines; i++)
            {
                ListLines[i] = tr.ReadLine();
            }
            tr.Close();
            if (new FileInfo(path).Length > 0)
            {
                for (int i = 0; i < biases.Length; i++)
                {
                    for (int j = 0; j < biases[i].Length; j++)
                    {
                        biases[i][j] = double.Parse(ListLines[index]);
                        index++;
                    }
                }

                for (int i = 0; i < weights.Length; i++)
                {
                    for (int j = 0; j < weights[i].Length; j++)
                    {
                        for (int k = 0; k < weights[i][j].Length; k++)
                        {
                            weights[i][j][k] = double.Parse(ListLines[index]);
                            index++;
                        }
                    }
                }
            }
        }

        public void Save(string path)//this is used for saving the biases and weights within the network to a file.
        {
            File.Create(path).Close();
            StreamWriter writer = new StreamWriter(path, true);

            for (int i = 0; i < biases.Length; i++)
            {
                for (int j = 0; j < biases[i].Length; j++)
                {
                    writer.WriteLine(biases[i][j]);
                }
            }

            for (int i = 0; i < weights.Length; i++)
            {
                for (int j = 0; j < weights[i].Length; j++)
                {
                    for (int k = 0; k < weights[i][j].Length; k++)
                    {
                        writer.WriteLine(weights[i][j][k]);
                    }
                }
            }
            writer.Close();
        }
    }
}
