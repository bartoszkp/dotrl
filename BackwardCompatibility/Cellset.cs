using System;
using System.Diagnostics.Contracts;
using System.Linq;

namespace BackwardCompatibility
{
    public enum CellType
    {
        Linear, Expotential, Arcustangent
    }

    [System.Diagnostics.DebuggerDisplay("Can({Value})")]
    public class Can
    {
        public double Value { get; set; }
    }

    /// <summary>
    /// Summary description CellSet
    /// </summary>
    [Serializable]
    public class CellSet
    {
        [Serializable]
        protected class Cell
        {
            #region Components
            public Can[] Input { get; set; }

            public double X { get; set; } // X = Sum[i] {Input[i]*Weight[i]}

            public Can Output { get; set; } // Output = Function(X)

            public Can[] Weight { get; set; }

            public Can dL_dX { get; set; }

            public Can[] dL_dWeight { get; set; }

            public Can[] dL_dNextX { get; set; }

            public Can[] dNextX_dOutput { get; set; }

            public CellType Type { get; set; }

            private delegate double Delegate11(double x);

            private Delegate11 function;

            private delegate double Delegate12(double x, double y);

            private Delegate12 derivative;
            #endregion

            #region Initialization
            public Cell()
            {
            }
            #endregion

            #region Neuron functions
            public static double FunctionLinear(double x)
            {
                return (x);
            }

            public static double DerivativeLinear(double x, double y)
            {
                return (1);
            }

            public static double FunctionExpotential(double x)
            {
                return (1.0 / (1.0 + System.Math.Exp(-x)));
            }

            public static double DerivativeExpotential(double x, double y)
            {
                return (y * (1.0 - y));
            }

            public static double FunctionArcustangent(double x)
            {
                return (System.Math.Atan(x));
            }

            public static double DerivativeArcustangent(double x, double y)
            {
                return (1.0 / (1.0 + x * x));
            }
            #endregion

            #region Initialization
            public void Init(int input_dim, CellType typ, int out_dim)
            {
                Contract.Requires(input_dim >= 1, "Wrong dimension");
                Contract.Requires(out_dim >= 1, "Wrong dimension");
                Contract.Requires(typ == CellType.Linear || typ == CellType.Expotential || typ == CellType.Arcustangent);
                switch (typ)
                {
                    case CellType.Linear:
                        function = new Delegate11(FunctionLinear);
                        derivative = new Delegate12(DerivativeLinear);
                        break;
                    case CellType.Expotential:
                        function = new Delegate11(FunctionExpotential);
                        derivative = new Delegate12(DerivativeExpotential);
                        break;
                    case CellType.Arcustangent:
                        function = new Delegate11(FunctionArcustangent);
                        derivative = new Delegate12(DerivativeArcustangent);
                        break;
                    default:
                        return;
                }
                
                Type = typ;

                Input = new Can[input_dim];
                Output = new Can();
                Weight = new Can[input_dim];
                dL_dX = new Can();
                dL_dWeight = new Can[input_dim];
                for (int i = 0; i < input_dim; i++)
                {
                    Weight[i] = new Can();
                    dL_dWeight[i] = new Can();
                }

                dL_dNextX = new Can[out_dim];
                dNextX_dOutput = new Can[out_dim];
            }

            public void AddInputDimension(int index)
            {
                Input = Input.Where((e, i) => i < index).Concat(Enumerable.Repeat(new Can(), 1)).Concat(Input.Where((e, i) => i >= index)).ToArray();
                Weight = Weight.Where((e, i) => i < index).Concat(Enumerable.Repeat(new Can(), 1)).Concat(Weight.Where((e, i) => i >= index)).ToArray();
                dL_dWeight = dL_dWeight.Where((e, i) => i < index).Concat(Enumerable.Repeat(new Can(), 1)).Concat(dL_dWeight.Where((e, i) => i >= index)).ToArray();

                Weight[index].Value = 0.01;
            }

            public void RemoveInputDimension(int index)
            {
                Input = Input.Where((e, i) => i != index).ToArray();
                Weight = Weight.Where((e, i) => i != index).ToArray();
                dL_dWeight = dL_dWeight.Where((e, i) => i != index).ToArray();
            }

            public void InitWeights(INormalSampler rand, double factor)
            {
                int dim = Weight.GetLength(0);
                double std_dev = factor; // *System.Math.Sqrt((double)dim); 
                for (int i = 0; i < dim; i++)
                {
                    Weight[i].Value = rand.SampleFromNormal(0, std_dev);
                }
            }
            #endregion

            #region Calculations
            public void CalculateAhead()
            {
                X = 0;
                for (int i = 0; i < Weight.GetLength(0); i++)
                {
                    X += Weight[i].Value * Input[i].Value;
                }

                Output.Value = function(X);
            }

            public void BackPropagateGradient()
            {
                int i;

                ////  calculations:
                ////              dL      dL   dOutput         dL    d_nextX  dOutput
                ////              -- = ------- ------- = Sum[------- -------] -------
                ////              dX   dOutput    dX         d_nextX dOutput     dX
                ////  d_nextX
                ////  -------  are weights of neurons in next layers
                ////  dOutput  which is not visible here

                double dL_dOutput = 0;
                for (i = 0; i < dL_dNextX.GetLength(0); i++)
                {
                    dL_dOutput += dL_dNextX[i].Value * dNextX_dOutput[i].Value;
                }

                dL_dX.Value = dL_dOutput * derivative(X, Output.Value);

                ////  calculations:
                ////              dQ   dQ dX   dQ
                ////              -- = -- -- = -- Input
                ////              dW   dX dW   dX
                ////  Input - output of the previous layer

                for (i = 0; i < Weight.GetLength(0); i++)
                {
                    dL_dWeight[i].Value = dL_dX.Value * Input[i].Value;
                }
            }
            #endregion
        }

        #region Components
        protected ASampler TheSampler { get; set; }
        #endregion
 
        #region Initialization
        public CellSet()
        {
            TheSampler = new ASampler();
        }
        #endregion
    }
}
