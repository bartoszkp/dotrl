using System;
using System.Diagnostics.Contracts;
using Core;
using MathNet.Numerics.LinearAlgebra.Generic;
using MathNet.Numerics.LinearAlgebra.Double; 

namespace BackwardCompatibility
{
    public class ANeuralAprx : MLPerceptron
    {
        public void Init(
            int size,
            int out_dim,
            CellType cell_type,
            double[] state_average,
            double[] state_stddev)
        {
            Build(state_average.Length, cell_type, new int[] { size, out_dim });
            InitWeights();
            SetInputDescription(state_average, state_stddev);
            if (out_dim == 1)
            {
                aux1 = new double[1];
            }
        }

        public int GetParamDim()
        {
            return WeightsCount;
        }

        public int GetIntStateDim()
        {
            return InternalStateDimension;
        }

        public double GetOutput(double[] input)
        {
            aux1 = Approximate(input);
            return aux1[0];
        }

        public double[] GetGradient(Vector<double> dminusLoss_dOutput)
        {
            double[] gradient = new double[GetParamDim()];
            BackPropagateGradient(dminusLoss_dOutput.ToArray(), gradient);
            return gradient;
        }

        public double[] GetGradient(double dminusLoss_dOutput)
        {
            Contract.Requires(OutDimension == 1, "ReinforcementPlatform.Aprx.ANeuralAprx.GetOutput: output dimension is not equal to 1");
            aux1[0] = dminusLoss_dOutput;
            double[] gradient = new double[GetParamDim()];
            BackPropagateGradient(aux1, gradient);
            return gradient;
        }

        public void AddToParam(MathNet.Numerics.LinearAlgebra.Generic.Vector<double> vect, double scalar)
        {
            this.AddToWeights(vect.ToArray(), scalar);
        }

        private double[] aux1;
    }
}
