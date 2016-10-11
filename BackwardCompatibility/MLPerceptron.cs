using System;
using System.Diagnostics.Contracts;
using System.Linq;

namespace BackwardCompatibility
{
    /// <summary>
    /// Summary description for MLPerceptron.
    /// </summary>
    [Serializable]
    public class MLPerceptron : CellSet
    {
        #region Data structures

        private Can one;
        private Can[] input;
        private Can[] dL_dOutput;
        private Can[] output;
        private Cell[][] neuron;

        [NonSerializedAttribute]
        protected Can[] weight;
        private Can[] dL_dWeight;
        private double[] inputAverage;
        private double[] inputStddev;
        #endregion

        #region Construction and initialization
        public MLPerceptron()
        {
        }

        public void AddInputDimension(int index, double average, double stdDev)
        {
            inputAverage = inputAverage.Where((e, i) => i < index).Concat(new double[] { average }).Concat(inputAverage.Where((e, i) => i >= index)).ToArray();
            inputStddev = inputStddev.Where((e, i) => i < index).Concat(new double[] { stdDev }).Concat(inputStddev.Where((e, i) => i >= index)).ToArray();
            input = input.Where((e, i) => i < index).Concat(Enumerable.Repeat(new Can(), 1)).Concat(input.Where((e, i) => i >= index)).ToArray();

            foreach (Cell cell in neuron[0])
            {
                cell.AddInputDimension(index);
                cell.Input[index] = input[index];
            }

            FlattenWeights();
        }

        public void RemoveInputDimension(int index)
        {
            foreach (Cell cell in neuron[0])
            {
                cell.Weight.Last().Value += cell.Weight[index].Value * inputAverage[index];
            }

            input = input.Where((e, i) => i != index).ToArray();
            inputAverage = inputAverage.Where((e, i) => i != index).ToArray();
            inputStddev = inputStddev.Where((e, i) => i != index).ToArray();

            ////int weightNumber = 0;
            foreach (Cell cell in neuron[0])
            {
                cell.RemoveInputDimension(index);
            }

            FlattenWeights();
        }
       
        public void Build(int in_dim, CellType[][] types)
        {
            #region checking correctenss of arguments
            int i, j, k;
            int layers_nr = types.GetLength(0);
            int out_dim = types[layers_nr - 1].GetLength(0);
            if (in_dim < 0 || layers_nr <= 0)
            {
                throw (new System.Exception("Neural.PureStructure.Build: in_dim or layers_nr"));
            }

            for (i = 0; i < layers_nr; i++)
            {
                if (types[i].GetLength(0) < 1)
                {
                    throw (new System.Exception("Neural.PureStructure.Build: layer types no < 1"));
                }
            }
            #endregion

            #region Initializing structures
            one = new Can();
            one.Value = 1;
            inputAverage = new double[in_dim];
            inputStddev = new double[in_dim];
            input = new Can[in_dim];
            for (i = 0; i < in_dim; i++)
            {
                inputAverage[i] = 0;
                inputStddev[i] = 1;
                input[i] = new Can();
            }

            neuron = new Cell[layers_nr][];
            int[] width = new int[layers_nr + 2];	//	numbers of neurons in layres
            width[0] = in_dim;
            for (i = 0; i < layers_nr; i++)
            {
                width[i + 1] = types[i].GetLength(0);
            }

            width[layers_nr + 1] = 1;
            for (i = 0; i < layers_nr; i++)
            {
                neuron[i] = new Cell[width[i + 1]];
                for (j = 0; j < width[i + 1]; j++)
                {
                    neuron[i][j] = GetNewCell();
                    neuron[i][j].Init(width[i] + 1, types[i][j], width[i + 2]);
                }
            }

            output = new Can[out_dim];
            dL_dOutput = new Can[out_dim];
            for (i = 0; i < out_dim; i++)
            {
                output[i] = neuron[layers_nr - 1][i].Output;
                dL_dOutput[i] = new Can();
            }
            #endregion

            #region Connecting neurons backward
            //	i=0
            if (true)
            {
                for (j = 0; j < width[1]; j++)
                {
                    for (k = 0; k < width[0]; k++)
                    {
                        neuron[0][j].Input[k] = input[k];
                    }

                    neuron[0][j].Input[k] = one;
                }
            }

            for (i = 1; i < layers_nr; i++)
            {
                for (j = 0; j < width[i + 1]; j++)
                {
                    for (k = 0; k < width[i]; k++)
                    {
                        neuron[i][j].Input[k] = neuron[i - 1][k].Output;
                    }

                    neuron[i][j].Input[k] = one;
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
                        neuron[i][j].dL_dNextX[k] = neuron[i + 1][k].dL_dX;
                        neuron[i][j].dNextX_dOutput[k] = neuron[i + 1][k].Weight[j];
                    }
                }
            }

            // i=layers_nr-1
            if (true)
            {
                for (j = 0; j < width[i + 1]; j++)
                {
                    for (k = 0; k < width[i + 2]; k++)
                    {
                        neuron[i][j].dL_dNextX[k] = dL_dOutput[j];
                        neuron[i][j].dNextX_dOutput[k] = one;
                    }
                }
            }
            #endregion

            FlattenWeights();

            AfterConstruction();
        }

        public void Build(int in_dim, CellType type, int[] width)
        {
            int i, j;
            int layers_nr = width.GetLength(0);	// number of layers in the network 
            if (in_dim < 0 || layers_nr < 1)
            {
                throw (new System.Exception("Thrown at Neural.PureStructure.Build(int, CellType, int[])"));
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

            //	i = layers_nr-1
            for (j = 0; j < width[i]; j++)
            {
                types[i][j] = CellType.Linear;
            }

            Build(in_dim, types);
        }
 
        public void SetInputDescription(double[] in_av, double[] in_stddev)
        {
            int i, in_dim = input.GetLength(0);
            if (in_av.GetLength(0) != in_dim
                || in_stddev.GetLength(0) != in_dim)
            {
                throw (new System.ArgumentException());
            }

            for (i = 0; i < in_dim; i++)
            {
                if (in_stddev[i] <= 0)
                {
                    throw (new System.ArgumentException());
                }
            }

            for (i = 0; i < in_dim; i++)
            {
                inputAverage[i] = in_av[i];
                inputStddev[i] = in_stddev[i];
            }
        }

        public void InitWeights(double std_dev)
        {
            int i, j, layers_nr = neuron.GetLength(0);
            for (i = 0; i < layers_nr - 1; i++)
            {
                for (j = 0; j < neuron[i].GetLength(0); j++)
                {
                    neuron[i][j].InitWeights(TheSampler, std_dev);
                }
            }
            
            // i=layers_nr-1; 
            for (j = 0; j < neuron[i].GetLength(0); j++)
            {
                neuron[i][j].InitWeights(TheSampler, 0);
            }
        }

        public void InitWeights()
        {
            InitWeights(1);
        }
        #endregion

        #region Properties
        public int InDimension
        {
            get { return (input.Length); }
        }

        public int OutDimension
        {
            get { return (output.Length); }
        }

        public int WeightsCount
        {
            get { return weight.Length; }
        }

        public int NeuronNo
        {
            get
            {
                int no = 0;
                for (int i = 0; i < neuron.GetLength(0); i++)
                {
                    no += neuron[i].GetLength(0);
                }

                return (no);
            }
        }

        public int InternalStateDimension
        {
            get
            {
                return InDimension + NeuronNo * 2;
            }
        }
        #endregion

        #region Access
        public void GetWeights(ref double[] weights)
        {
            int no = WeightsCount;
            if (weights == null || weights.Length != no)
            {
                weights = new double[no];
            }

            for (int i = 0; i < no; i++)
            {
                weights[i] = weight[i].Value;
            }
        }

        public void SetWeights(double[] weights)
        {
            int no = WeightsCount;
            Contract.Requires(weights != null);
            Contract.Requires(weights.Length == no);
            for (int i = 0; i < no; i++)
            {
                weight[i].Value = weights[i];
            }
        }

        public void AddToWeights(double[] vect, double scalar)
        {
            int no = WeightsCount;
            if (vect == null || vect.Length != no)
            {
                vect = new double[no];
            }

            for (int i = 0; i < no; i++)
            {
                weight[i].Value += vect[i] * scalar;
            }
        }
        #endregion

        #region Public calculations
        public double[] Approximate(double[] inputArray)
        {
            int outDimension = output.GetLength(0);
            SetInput(inputArray);
            CalculateAhead();
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
            int no = WeightsCount;
            if (weight_gradient == null || weight_gradient.Length != no)
            {
                weight_gradient = new double[no];
            }

            BackPropagateGradient(out_gradient, weight_gradient);
        }

        // dOutput_dInput[j,i] = dOutput[j]/dInput[i]
        // preceding forward calculations required 
        public void Get_dOutput_dInput(ref double[,] doutput_dinput)
        {
            int inputIndex;
            int outputIndex;
            int inputDimension = input.GetLength(0);
            int outputDimension = this.output.GetLength(0);

            if (doutput_dinput == null || doutput_dinput.GetLength(0) != outputDimension || doutput_dinput.GetLength(1) != inputDimension)
            {
                doutput_dinput = new double[outputDimension, inputDimension];
            }

            for (outputIndex = 0; outputIndex < outputDimension; outputIndex++)
            {
                dL_dOutput[outputIndex].Value = 0;
            }

            for (outputIndex = 0; outputIndex < outputDimension; outputIndex++)
            {
                dL_dOutput[outputIndex].Value = 1;
                BackPropagateGradient();

                for (inputIndex = 0; inputIndex < inputDimension; inputIndex++)
                {
                    doutput_dinput[outputIndex, inputIndex] = 0;
                    for (int k = 0; k < neuron[0].GetLength(0); k++)
                    {
                        doutput_dinput[outputIndex, inputIndex] += neuron[0][k].dL_dX.Value * neuron[0][k].Weight[inputIndex].Value;
                    }

                    doutput_dinput[outputIndex, inputIndex] /= inputStddev[inputIndex];
                }

                dL_dOutput[outputIndex].Value = 0;
            }
        }

        // dOutput_dWeight[j,i] = dOutput[j]/dWeight[i]
        // preceding forward calculations required 
        public void Get_dOutput_dWeight(ref double[,] doutput_dweight)
        {
            int j, i;
            int out_dim = output.GetLength(0);
            int w_dim = weight.Length;

            if (doutput_dweight == null || doutput_dweight.GetLength(0) != out_dim || doutput_dweight.GetLength(1) != w_dim)
            {
                doutput_dweight = new double[out_dim, w_dim];
            }

            for (j = 0; j < out_dim; j++)
            {
                dL_dOutput[j].Value = 0;
            }

            for (j = 0; j < out_dim; j++)
            {
                dL_dOutput[j].Value = 1;
                BackPropagateGradient();
                for (i = 0; i < w_dim; i++)
                {
                    doutput_dweight[j, i] = dL_dWeight[i].Value;
                }

                dL_dOutput[j].Value = 0;
            }
        }
        #endregion

        #region Saving and restoring
        public double[] SaveForwardState()
        {
            double[] state = new double[InternalStateDimension];

            int k = 0, i, j;
            for (i = 0; i < InDimension; i++)
            {
                state[k++] = input[i].Value;
            }

            for (i = 0; i < neuron.GetLength(0); i++)
            {
                for (j = 0; j < neuron[i].GetLength(0); j++)
                {
                    state[k++] = neuron[i][j].X;
                    state[k++] = neuron[i][j].Output.Value;
                }
            }

            return state;
        }

        public void RestoreForwardState(double[] state)
        {
            int state_dim = InternalStateDimension;
            if (state == null || state.Length != state_dim)
            {
                throw new Exception("Wrong state dimension");
            }

            int k = 0, i, j;
            for (i = 0; i < InDimension; i++)
            {
                input[i].Value = state[k++];
            }

            for (i = 0; i < neuron.GetLength(0); i++)
            {
                for (j = 0; j < neuron[i].GetLength(0); j++)
                {
                    neuron[i][j].X = state[k++];
                    neuron[i][j].Output.Value = state[k++];
                }
            }
        }
        #endregion

        #region Protected calculations
        protected void SetInput(double[] arguments)
        {
            int in_dim = input.GetLength(0);
            Contract.Requires(arguments.GetLength(0) == in_dim);
            for (int i = 0; i < in_dim; i++)
            {
                input[i].Value = (arguments[i] - inputAverage[i]) / inputStddev[i];
            }
        }

        protected void SetOutGradient(double[] out_gradient)
        {
            int out_dim = OutDimension;
            Contract.Requires(out_gradient.GetLength(0) == out_dim);
            for (int i = 0; i < out_dim; i++)
            {
                dL_dOutput[i].Value = out_gradient[i];
            }
        }

        protected void CalculateAhead()
        {
            for (int i = 0; i < neuron.GetLength(0); i++)
            {
                for (int j = 0; j < neuron[i].GetLength(0); j++)
                {
                    neuron[i][j].CalculateAhead();
                }
            }
        }

        protected void BackPropagateGradient()
        {
            for (int i = neuron.GetLength(0) - 1; i >= 0; i--)
            {
                for (int j = 0; j < neuron[i].GetLength(0); j++)
                {
                    neuron[i][j].BackPropagateGradient();
                }
            }
        }

        // preceding forward calculations required 
        protected void BackPropagateGradient(double[] out_gradient, double[] weight_gradient)
        {
            SetOutGradient(out_gradient);
            BackPropagateGradient();
            int no = WeightsCount;
            for (int i = 0; i < no; i++)
            {
                weight_gradient[i] = dL_dWeight[i].Value;
            }
        }
        #endregion

        virtual protected void AfterConstruction()
        {
        }

        protected virtual Cell GetNewCell()
        {
            return new Cell();
        }

        private void FlattenWeights()
        {
            int layers_nr = neuron.Length;

            int[] width = new int[layers_nr + 2];
            width[0] = inputAverage.Length;
            for (int i = 0; i < layers_nr; ++i)
            {
                width[i + 1] = neuron[i].Length;
            }

            int weights_nr = 0;
            for (int i = 0; i < layers_nr; i++)
            {
                for (int j = 0; j < width[i + 1]; j++)
                {
                    weights_nr += width[i] + 1;
                }
            }

            weight = new Can[weights_nr];
            dL_dWeight = new Can[weights_nr];

            int w = 0;
            for (int i = 0; i < layers_nr; i++)
            {
                for (int j = 0; j < width[i + 1]; j++)
                {
                    for (int k = 0; k < width[i] + 1; k++)
                    {
                        weight[w] = neuron[i][j].Weight[k];
                        dL_dWeight[w++] = neuron[i][j].dL_dWeight[k];
                    }
                }
            }
        }
    }
}
