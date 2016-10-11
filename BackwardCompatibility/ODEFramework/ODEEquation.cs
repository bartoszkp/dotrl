namespace BackwardCompatibility.ODEFramework
{
    /// <summary>
    /// Models a set of ODE equations. These equations are of the form
    /// dy/dt = f(t, y) where y is a vector. This equation appears to be
    /// only first order, but a second order equation can be made simply
    /// by introducing another variable. See Wikipedia for more information on this.
    /// </summary>
    public interface IODEEquation
    {
        /// <summary>
        /// This is the function dy/dt = f(t, y), the returned array must be of the 
        /// same length as y. </summary>
        /// <param name="t"> The time parameter </param>
        /// <param name="y"> The current value of all variables </param>
        /// <returns> The derivative of each variable w.r.t time at the given time and with the given values </returns>
        ODEState GetDerivative(double time, ODEState state);
    }
}