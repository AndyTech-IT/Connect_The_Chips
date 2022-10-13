using System.Collections.Generic;
using System;
using System.IO;
using RandomGenerator;
using System.Linq;

namespace Neural_Network
{
    public class NeuralNetwork
    {
        public const string FILE_EXSTANTION = "nnp";

        public double Fitnes => _fitness;

        private static Random_Generator _generator = new Random_Generator();
        /// <summary>
        /// Network architecture
        /// </summary>
        private Network_Struct NetworkStruct;
        /// <summary>
        /// Layers sizes array
        /// </summary>
        private int[] layers_sizes;


        private double[][] Inputs_Neurons => neurons_outputs.Take(NetworkStruct.InputLayers_Count).ToArray();
        private double[][] Hidden_Neurons => neurons_outputs.Skip(NetworkStruct.InputLayers_Count).Take(NetworkStruct.HiddenLayers_Count).ToArray();
        private double[] Outputs_Neurons => neurons_outputs.Last().ToArray();

        /// <summary>
        /// Neurons output value
        /// </summary>
        private double[][] neurons_outputs;
        /// <summary>
        /// Biasis neurons output
        /// </summary>
        private double[][] biases;
        /// <summary>
        /// Neurons weights coefficients
        /// </summary>
        private double[][][] weights;

        /// <summary>
        /// Network fitness score
        /// </summary>
        private double _fitness;

        public NeuralNetwork(Network_Struct network_struct)
        {
            NetworkStruct = network_struct;
            layers_sizes = new int[NetworkStruct.InputLayers_Count + NetworkStruct.HiddenLayers_Count + 1];
            int index = 0;

            // Inputs
            for (int i = 0; i < NetworkStruct.InputLayers_Count; i++, index++)
            {
                layers_sizes[index] = NetworkStruct.InputLayers_Sizes[i];
            }
            // Hiddens
            for (int i = 0; i < NetworkStruct.HiddenLayers_Count; i++, index++)
            {
                layers_sizes[index] = NetworkStruct.HiddenLayers_Sizes[i];
            }
            //Outputs
            layers_sizes[index] = NetworkStruct.OutputLayer_Size;

            _fitness = 0;
            neurons_outputs = layers_sizes.Select(size => new double[size]).ToArray(); 
            biases = layers_sizes.Select(size => new double[size][].Select(bias => _generator.Next_Double(-0.5, 0.5)).ToArray()).ToArray();
            weights = new double[0][][];

            //InitBiases();
            InitWeights();
        }

        public void Chainge_Fitnes(double value)
        {
            _fitness += value;
        }

        private void InitBiases()//initializes and populates array for the biases being held within the network.
        {
            List<double[]> biasList = new List<double[]>();
            for (int i = 0; i < layers_sizes.Length; i++)
            {
                double[] bias = new double[layers_sizes[i]];
                for (int j = 0; j < layers_sizes[i]; j++)
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
            for (int i = 1; i < layers_sizes.Length; i++)
            {
                List<double[]> layerWeightsList = new List<double[]>();
                int neuronsInPreviousLayer = layers_sizes[i - 1];
                for (int j = 0; j < neurons_outputs[i].Length; j++)
                {
                    double[] neuronWeights = new double[neuronsInPreviousLayer];
                    for (int k = 0; k < neuronsInPreviousLayer; k++)
                    {
                        double sd = 1f / ((neurons_outputs[i].Length + neuronsInPreviousLayer) / 2f);
                        neuronWeights[k] = _generator.Next_Double(-sd, sd);
                    }
                    layerWeightsList.Add(neuronWeights);
                }
                weightsList.Add(layerWeightsList.ToArray());
            }
            weights = weightsList.ToArray();
        }

        /// <summary>
        /// Feed forward, inputs >==> outputs.
        /// </summary>
        /// <param name="inputs">Input layer data arr</param>
        /// <returns>Output layer data arr</returns>
        public double[] FeedForward(double[][] inputs)
        {
            if (inputs.Length != Inputs_Neurons.Length)
                throw new ArgumentOutOfRangeException(nameof(inputs));

            // Fill inputs layer
            for (int i = 0; i < inputs.Length; i++)
            {
                int prev_layer = i - 1;
                for (int j = 0; j < inputs[i].Length; j++)
                {
                    if (i == 0)
                    {
                        neurons_outputs[i][j] = inputs[i][j];
                        continue;
                    }
                    double value = inputs[i][j];
                    for (int k = 0; k < neurons_outputs[prev_layer].Length; k++)
                    {
                        value += weights[prev_layer][j][k] * neurons_outputs[prev_layer][k];
                    }
                    neurons_outputs[i][j] = Activate(value + biases[i][j]);
                }
            }
            // Feed forward
            for (int i = Inputs_Neurons.Length; i < layers_sizes.Length; i++)
            {
                int prev_layer = i - 1;
                for (int j = 0; j < neurons_outputs[i].Length; j++)
                {
                    double value = 0f;
                    for (int k = 0; k < neurons_outputs[prev_layer].Length; k++)
                    {
                        value += weights[prev_layer][j][k] * neurons_outputs[prev_layer][k];
                    }
                    neurons_outputs[i][j] = Activate(value + biases[i][j]);
                }
            }
            return Outputs_Neurons;
        }

        private static double Activate(double value)
        {
            return Math.Tanh(value);
        }

        public void Mutate(double chance, double val)//used as a simple mutation function for any genetic implementations.
        {
            for (int i = 0; i < biases.Length; i++)
            {
                for (int j = 0; j < biases[i].Length; j++)
                {
                    biases[i][j] = _generator.Next_Double(1) <= chance ? biases[i][j] += _generator.Next_Double(1) * val : biases[i][j];
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
                        weights[i][j][k] = _generator.Next_Double(1) <= chance ? weights[i][j][k] += _generator.Next_Double(1) * val : weights[i][j][k];
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
                        biases[i][j] = other.biases[i][j];
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
