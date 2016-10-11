namespace BackwardCompatibility.ODEFramework
{
	/// <summary>
	/// Runge-Kutta 4th order ODE solver
	/// </summary>
	public class RungeKutta4Solver : ODESolver
	{
		public override ODEState Solve(IODEEquation eq, ODEState initialState, double time, double timeStep)
		{
			ODEState k1 = eq.GetDerivative(time, initialState);
			ODEState k2 = eq.GetDerivative(time + timeStep / 2, initialState.AddScaled(k1, timeStep / 2));
			ODEState k3 = eq.GetDerivative(time + timeStep / 2, initialState.AddScaled(k2, timeStep / 2));
			ODEState k4 = eq.GetDerivative(time + timeStep, initialState.AddScaled(k3, timeStep));
			ODEState sum = k1.AddScaled(k2, 2).AddScaled(k3, 2).AddScaled(k4, 1);
			return initialState.AddScaled(sum, timeStep / 6);
		}
	}
}