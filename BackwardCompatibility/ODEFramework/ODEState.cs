namespace BackwardCompatibility.ODEFramework
{
    public class ODEState
    {
        public double[] State { get; private set; }

        public ODEState(double[] state)
        {
            State = state;
        }

        public virtual ODEState AddScaled(ODEState state, double scale)
        {
            ODEState result = new ODEState(new double[State.Length]);
            for (int i = 0; i < State.Length; i++)
            {
                result.State[i] = State[i] + scale * state.State[i];
            }

            return result;
        }
    }
}