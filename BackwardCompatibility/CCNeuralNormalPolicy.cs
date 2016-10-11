using System;
using System.Linq;
using System.Diagnostics.Contracts;
using Core;
using MathNet.Numerics.LinearAlgebra.Double;
using MathNet.Numerics.LinearAlgebra.Generic;

namespace BackwardCompatibility
{
    public class CCNeuralNormalPolicy 
    {
        public CCNeuralNormalPolicy()
        {
            network = new ANeuralAprx();
            sampler = new ASampler();
        }

        public void Init(int actionDimension, int network_size, double[] state_av, double[] state_stddev)
        {
            this.actionDimension = actionDimension;
            network.Init(network_size, actionDimension, CellType.Arcustangent, state_av, state_stddev);
            X = new DenseVector(this.actionDimension);
            dLnDensity_dNetworkOutput = new DenseVector(actionDimension);
        }

        public void SetStdDev(double std_dev)
        {
            standardDeviation = std_dev;
        }

        public int GetThetaDim()
        {
            return network.GetParamDim();
        }

        public double[] GenerateActionWithoutNoise(double[] state)
        {
            return CalculateNetworkForward(state);
        }

        public double[] GenerateActionWithNoise(double[] state)
        {
            X.SetValues(Enumerable
                .Range(0, actionDimension)
                .Select(d => sampler.SampleFromNormal(0, standardDeviation))
                .ToArray());

            return GenerateActionWithoutNoise(state)
                .Zip(X, (action, noise) => action + noise)
                .ToArray();
        }

        public double Get_Density()
        {
            double density = Math.Pow(Math.Sqrt(2.0 * Math.PI) * standardDeviation, -actionDimension);
            density *= Math.Exp(-0.5 * (X * X) / (standardDeviation * standardDeviation));
            return (density);
        }

        public double[] Get_dLnDensity_dTheta()
        {
            for (int i = 0; i < actionDimension; i++)
            {
                dLnDensity_dNetworkOutput[i] = X[i] / (standardDeviation * standardDeviation);
            }

            return network.GetGradient(dLnDensity_dNetworkOutput);
        }

        public void AddToTheta(MathNet.Numerics.LinearAlgebra.Generic.Vector<double> vect, double scalar)
        {
            network.AddToParam(vect, scalar);
        }

        #region Fields
        private ANeuralAprx network;
        private ASampler sampler;

        private double standardDeviation;

        private int actionDimension;
        private Vector<double> X;
        private Vector<double> dLnDensity_dNetworkOutput;

        // the internal state of the policy encompasses: 
        // - the internal state of Network
        // - X
        #endregion

        private double[] CalculateNetworkForward(double[] state)
        {
            return network.Approximate(state);
        }
    }
}
