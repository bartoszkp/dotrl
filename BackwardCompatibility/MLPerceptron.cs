using System;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;

namespace BackwardCompatibility
{
    [Serializable]
    public class MLPerceptron : CellSet
    {
        private Can one;
        private Can[] input;
        private Can[] dL_dOutput;
        private Can[] output;
        private Cell[][] neuron;

        protected Can[] weight;
        private Can[] dL_dWeight;
        private double[] inputAverage;
        private double[] inputStddev;

        public void AddInputDimension(int index, double average, double stdDev)
        {
            this.inputAverage = this.inputAverage.Where((e, i) => i < index).Concat(new double[] { average }).Concat(this.inputAverage.Where((e, i) => i >= index)).ToArray();
            this.inputStddev = this.inputStddev.Where((e, i) => i < index).Concat(new double[] { stdDev }).Concat(this.inputStddev.Where((e, i) => i >= index)).ToArray();
            this.input = this.input.Where((e, i) => i < index).Concat(Enumerable.Repeat(new Can(), 1)).Concat(this.input.Where((e, i) => i >= index)).ToArray();

            foreach (Cell cell in this.neuron[0])
            {
                cell.AddInputDimension(index);
                cell.Input[index] = this.input[index];
            }

            this.FlattenWeights();
        }

        public void RemoveInputDimension(int index)
        {
            foreach (Cell cell in this.neuron[0])
            {
                cell.Weight.Last().Value += cell.Weight[index].Value * this.inputAverage[index];
            }

            this.input = this.input.Where((e, i) => i != index).ToArray();
            this.inputAverage = this.inputAverage.Where((e, i) => i != index).ToArray();
            this.inputStddev = this.inputStddev.Where((e, i) => i != index).ToArray();

            foreach (Cell cell in this.neuron[0])
            {
                cell.RemoveInputDimension(index);
            }

            this.FlattenWeights();
        }

        public void Build(int in_dim, CellType[][] types)
        {
            #region checking correctenss of arguments
            int i, j, k;
            int layers_nr = types.GetLength(0);
            int out_dim = types[layers_nr - 1].GetLength(0);
            if (in_dim < 0 || layers_nr <= 0)
            {
                throw new Exception("Neural.PureStructure.Build: in_dim or layers_nr");
            }

            for (i = 0; i < layers_nr; i++)
            {
                if (types[i].GetLength(0) < 1)
                {
                    throw new Exception("Neural.PureStructure.Build: layer types no < 1");
                }
            }
            #endregion

            #region Initializing structures
            this.one = new Can();
            this.one.Value = 1;
            this.inputAverage = new double[in_dim];
            this.inputStddev = new double[in_dim];
            this.input = new Can[in_dim];
            for (i = 0; i < in_dim; i++)
            {
                this.inputAverage[i] = 0;
                this.inputStddev[i] = 1;
                this.input[i] = new Can();
            }

            this.neuron = new Cell[layers_nr][];
            int[] width = new int[layers_nr + 2];	//	numbers of neurons in layers
            width[0] = in_dim;
            for (i = 0; i < layers_nr; i++)
            {
                width[i + 1] = types[i].GetLength(0);
            }

            width[layers_nr + 1] = 1;
            for (i = 0; i < layers_nr; i++)
            {
                this.neuron[i] = new Cell[width[i + 1]];
                for (j = 0; j < width[i + 1]; j++)
                {
                    this.neuron[i][j] = new Cell();
                    this.neuron[i][j].Init(width[i] + 1, types[i][j], width[i + 2]);
                }
            }

            this.output = new Can[out_dim];
            this.dL_dOutput = new Can[out_dim];
            for (i = 0; i < out_dim; i++)
            {
                this.output[i] = this.neuron[layers_nr - 1][i].Output;
                this.dL_dOutput[i] = new Can();
            }
            #endregion

            #region Connecting neurons backward
            for (j = 0; j < width[1]; j++)
            {
                for (k = 0; k < width[0]; k++)
                {
                    this.neuron[0][j].Input[k] = this.input[k];
                }

                this.neuron[0][j].Input[k] = this.one;
            }

            for (i = 1; i < layers_nr; i++)
            {
                for (j = 0; j < width[i + 1]; j++)
                {
                    for (k = 0; k < width[i]; k++)
                    {
                        this.neuron[i][j].Input[k] = this.neuron[i - 1][k].Output;
                    }

                    this.neuron[i][j].Input[k] = this.one;
                }
            }
            #endregion

            #region Connecting neurons forward
            for (i = 0; i < layers_nr - 1; i++)
            {
                for (j = 0; j < width[i + 1]; j++)
                {
                    for (k = 0; k < width[i + 2]; k++)
                    {
                        this.neuron[i][j].dL_dNextX[k] = this.neuron[i + 1][k].dL_dX;
                        this.neuron[i][j].dNextX_dOutput[k] = this.neuron[i + 1][k].Weight[j];
                    }
                }
            }

            for (j = 0; j < width[i + 1]; j++)
            {
                for (k = 0; k < width[i + 2]; k++)
                {
                    this.neuron[i][j].dL_dNextX[k] = this.dL_dOutput[j];
                    this.neuron[i][j].dNextX_dOutput[k] = this.one;
                }
            }
            #endregion

            this.FlattenWeights();
        }

        public void Build(int in_dim, CellType type, int[] width)
        {
            int i, j;
            int layers_nr = width.GetLength(0);	// number of layers in the network 
            if (in_dim < 0 || layers_nr < 1)
            {
                throw new Exception("Thrown at Neural.PureStructure.Build(int, CellType, int[])");
            }

            CellType[][] types = new CellType[layers_nr][];
            for (i = 0; i < layers_nr; i++)
            {
                types[i] = new CellType[width[i]];
            }

            for (i = 0; i < layers_nr - 1; i++)
            {
                for (j = 0; j < width[i]; j++)
                {
                    types[i][j] = type;
                }
            }

            for (j = 0; j < width[i]; j++)
            {
                types[i][j] = CellType.Linear;
            }

            this.Build(in_dim, types);
        }

        public void SetInputDescription(double[] in_av, double[] in_stddev)
        {
            int i, in_dim = this.input.GetLength(0);
            if (in_av.GetLength(0) != in_dim
                || in_stddev.GetLength(0) != in_dim)
            {
                throw new ArgumentException();
            }

            for (i = 0; i < in_dim; i++)
            {
                if (in_stddev[i] <= 0)
                {
                    throw new ArgumentException();
                }
            }

            for (i = 0; i < in_dim; i++)
            {
                this.inputAverage[i] = in_av[i];
                this.inputStddev[i] = in_stddev[i];
            }
        }

        public void InitWeights(double std_dev)
        {
            int i, j, layers_nr = this.neuron.GetLength(0);
            for (i = 0; i < layers_nr - 1; i++)
            {
                for (j = 0; j < this.neuron[i].GetLength(0); j++)
                {
                    this.neuron[i][j].InitWeights(this.TheSampler, std_dev);
                }
            }

            // i=layers_nr-1; 
            for (j = 0; j < this.neuron[i].GetLength(0); j++)
            {
                this.neuron[i][j].InitWeights(this.TheSampler, 0);
            }
        }

        public void InitWeights()
        {
            this.InitWeights(1);
        }

        public int InDimension
        {
            get { return (this.input.Length); }
        }

        public int OutDimension
        {
            get { return (this.output.Length); }
        }

        public int WeightsCount
        {
            get { return this.weight.Length; }
        }

        public int NeuronNo
        {
            get
            {
                int no = 0;
                for (int i = 0; i < this.neuron.GetLength(0); i++)
                {
                    no += this.neuron[i].GetLength(0);
                }

                return (no);
            }
        }

        public int InternalStateDimension
        {
            get
            {
                return this.InDimension + this.NeuronNo * 2;
            }
        }

        public void GetWeights(ref double[] weights)
        {
            int no = this.WeightsCount;
            if (weights == null || weights.Length != no)
            {
                weights = new double[no];
            }

            for (int i = 0; i < no; i++)
            {
                weights[i] = this.weight[i].Value;
            }
        }

        public void SetWeights(double[] weights)
        {
            int no = this.WeightsCount;
            Contract.Requires(weights != null);
            Contract.Requires(weights.Length == no);
            for (int i = 0; i < no; i++)
            {
                this.weight[i].Value = weights[i];
            }
        }

        public void AddToWeights(double[] vect, double scalar)
        {
            int no = this.WeightsCount;
            if (vect == null || vect.Length != no)
            {
                vect = new double[no];
            }

            for (int i = 0; i < no; i++)
            {
                this.weight[i].Value += vect[i] * scalar;
            }
        }

        public double[] Approximate(double[] inputArray)
        {
            int outDimension = this.output.GetLength(0);
            this.SetInput(inputArray);
            this.CalculateAhead();
            double[] outputArray = new double[outDimension];
            for (int i = 0; i < outDimension; i++)
            {
                outputArray[i] = this.output[i].Value;
            }

            return outputArray;
        }

        // preceding forward calculations required 
        public void BackPropagateGradient(double[] out_gradient, ref double[] weight_gradient)
        {
            int no = this.WeightsCount;
            if (weight_gradient == null || weight_gradient.Length != no)
            {
                weight_gradient = new double[no];
            }

            this.BackPropagateGradient(out_gradient, weight_gradient);
        }

        // dOutput_dInput[j,i] = dOutput[j]/dInput[i]
        // preceding forward calculations required 
        public void Get_dOutput_dInput(ref double[,] doutput_dinput)
        {
            int inputIndex;
            int outputIndex;
            int inputDimension = this.input.GetLength(0);
            int outputDimension = this.output.GetLength(0);

            if (doutput_dinput == null || doutput_dinput.GetLength(0) != outputDimension || doutput_dinput.GetLength(1) != inputDimension)
            {
                doutput_dinput = new double[outputDimension, inputDimension];
            }

            for (outputIndex = 0; outputIndex < outputDimension; outputIndex++)
            {
                this.dL_dOutput[outputIndex].Value = 0;
            }

            for (outputIndex = 0; outputIndex < outputDimension; outputIndex++)
            {
                this.dL_dOutput[outputIndex].Value = 1;
                this.BackPropagateGradient();

                for (inputIndex = 0; inputIndex < inputDimension; inputIndex++)
                {
                    doutput_dinput[outputIndex, inputIndex] = 0;
                    for (int k = 0; k < this.neuron[0].GetLength(0); k++)
                    {
                        doutput_dinput[outputIndex, inputIndex] += this.neuron[0][k].dL_dX.Value * this.neuron[0][k].Weight[inputIndex].Value;
                    }

                    doutput_dinput[outputIndex, inputIndex] /= this.inputStddev[inputIndex];
                }

                this.dL_dOutput[outputIndex].Value = 0;
            }
        }

        // dOutput_dWeight[j,i] = dOutput[j]/dWeight[i]
        // preceding forward calculations required 
        public void Get_dOutput_dWeight(ref double[,] doutput_dweight)
        {
            int j, i;
            int out_dim = this.output.GetLength(0);
            int w_dim = this.weight.Length;

            if (doutput_dweight == null || doutput_dweight.GetLength(0) != out_dim || doutput_dweight.GetLength(1) != w_dim)
            {
                doutput_dweight = new double[out_dim, w_dim];
            }

            for (j = 0; j < out_dim; j++)
            {
                this.dL_dOutput[j].Value = 0;
            }

            for (j = 0; j < out_dim; j++)
            {
                this.dL_dOutput[j].Value = 1;
                this.BackPropagateGradient();
                for (i = 0; i < w_dim; i++)
                {
                    doutput_dweight[j, i] = this.dL_dWeight[i].Value;
                }

                this.dL_dOutput[j].Value = 0;
            }
        }

        public byte[] Serialize()
        {
            MemoryStream ms = new MemoryStream();
            new BinaryFormatter().Serialize(ms, this);
            return ms.GetBuffer();
        }

        public static MLPerceptron Deserialize(byte[] serialized)
        {
            return new BinaryFormatter().Deserialize(new MemoryStream(serialized)) as MLPerceptron;
        }

        protected void SetInput(double[] arguments)
        {
            int in_dim = this.input.GetLength(0);
            Contract.Requires(arguments.GetLength(0) == in_dim);
            for (int i = 0; i < in_dim; i++)
            {
                this.input[i].Value = (arguments[i] - this.inputAverage[i]) / this.inputStddev[i];
            }
        }

        protected void SetOutGradient(double[] out_gradient)
        {
            int out_dim = this.OutDimension;
            Contract.Requires(out_gradient.GetLength(0) == out_dim);
            for (int i = 0; i < out_dim; i++)
            {
                this.dL_dOutput[i].Value = out_gradient[i];
            }
        }

        protected void CalculateAhead()
        {
            for (int i = 0; i < this.neuron.GetLength(0); i++)
            {
                for (int j = 0; j < this.neuron[i].GetLength(0); j++)
                {
                    this.neuron[i][j].CalculateAhead();
                }
            }
        }

        protected void BackPropagateGradient()
        {
            for (int i = this.neuron.GetLength(0) - 1; i >= 0; i--)
            {
                for (int j = 0; j < this.neuron[i].GetLength(0); j++)
                {
                    this.neuron[i][j].BackPropagateGradient();
                }
            }
        }

        // preceding forward calculations required 
        protected void BackPropagateGradient(double[] out_gradient, double[] weight_gradient)
        {
            this.SetOutGradient(out_gradient);
            this.BackPropagateGradient();
            int no = this.WeightsCount;
            for (int i = 0; i < no; i++)
            {
                weight_gradient[i] = this.dL_dWeight[i].Value;
            }
        }

        private void FlattenWeights()
        {
            int layers_nr = this.neuron.Length;

            int[] width = new int[layers_nr + 2];
            width[0] = this.inputAverage.Length;
            for (int i = 0; i < layers_nr; ++i)
            {
                width[i + 1] = this.neuron[i].Length;
            }

            int weights_nr = 0;
            for (int i = 0; i < layers_nr; i++)
            {
                for (int j = 0; j < width[i + 1]; j++)
                {
                    weights_nr += width[i] + 1;
                }
            }

            this.weight = new Can[weights_nr];
            this.dL_dWeight = new Can[weights_nr];

            int w = 0;
            for (int i = 0; i < layers_nr; i++)
            {
                for (int j = 0; j < width[i + 1]; j++)
                {
                    for (int k = 0; k < width[i] + 1; k++)
                    {
                        this.weight[w] = this.neuron[i][j].Weight[k];
                        this.dL_dWeight[w++] = this.neuron[i][j].dL_dWeight[k];
                    }
                }
            }
        }
    }
}
