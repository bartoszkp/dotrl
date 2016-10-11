using System;

namespace BackwardCompatibility
{
    public interface INormalSampler
    {
        double SampleFromNormal(double mean, double std_dev);
    }

    public class ASampler : Random, INormalSampler
    {
        public ASampler()
        {
        }

        public double SampleFromNormal(double mean, double std_dev)
        {
            double z = -Math.Log(1.0 - NextDouble());
            double alpha = NextDouble() * Math.PI * 2;
            double norm = Math.Sqrt(z * 2) * Math.Cos(alpha);
            return mean + (norm * std_dev);
        }
    }
}
