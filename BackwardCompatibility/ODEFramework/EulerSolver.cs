namespace BackwardCompatibility.ODEFramework
{
    public class EulerSolver : ODESolver
    {
        public override ODEState Solve(IODEEquation eq, ODEState initialState, double time, double timeStep)
        {
            ODEState deriv = eq.GetDerivative(time, initialState);

            // Compute new state using Euler's method
            // Yn+1 = Yn + deltaT * dYn/dt
            return initialState.AddScaled(deriv, timeStep);
        }
    }
}