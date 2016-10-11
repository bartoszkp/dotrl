namespace BackwardCompatibility.ODEFramework
{
    public abstract class ODESolver
    {
        /// <summary>
        /// This method solves the ODE from the initial time to the final time
        /// by dividing this interval into timesteps and calling the other
        /// solve method for each time step.
        /// </summary>
        /// <param name="eq"> The ODE to solve </param>
        /// <param name="initialState"> Initial state of the ODE </param>
        /// <param name="initialTime"> Time at which to start </param>
        /// <param name="finalTime"> Time at which to stop </param>
        /// <param name="timeStep"> Hint of the time step to use </param>
        /// <returns> The state of the ODE at finalTime </returns>
        public virtual ODEState Solve(IODEEquation eq, ODEState initialState, double initialTime, double finalTime, double timeStep)
        {
            double t;
            ODEState y = initialState;
            for (t = initialTime; t < finalTime; t += timeStep)
            {
                y = Solve(eq, y, t, timeStep);
            }

            if (t < finalTime)
            {
                y = Solve(eq, y, t, finalTime - t);
            }

            return y;
        }

        public abstract ODEState Solve(IODEEquation eq, ODEState initialState, double time, double timeStep);
    }
}