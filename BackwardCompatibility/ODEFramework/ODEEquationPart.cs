namespace BackwardCompatibility.ODEFramework
{
    /// <summary>
    /// A subset of the whole set of ODE equations. This is useful for grouping together
    /// a few variables that belong together, but whose derivatives might be influenced
    /// by other variables in the complete set.
    /// 
    /// </summary>
    public interface IODEEquationPart
    {
        /// <summary>
        /// Returns how many state variables this object expects. </summary>
        /// <returns> The number of doubles in the state of this object. </returns>
        int StateLength { get; }

        /// <summary>
        /// The object should assume the state that it is passed. </summary>
        /// <param name="time"> current time </param>
        /// <param name="state"> The state to assume </param>
        void SetODEState(double time, ODEState state);

        /// <summary>
        /// Returns the state of the object simply as an array of double values,
        /// if this state is passed to another object that was created like this one,
        /// it should be exactly in the same state as this one. </summary>
        /// <returns> The current state as an array of double values. </returns>
        ODEState CurrentODEState { get; }

        /// <summary>
        /// Should return the derivatives of each state variables in the current state
        /// and at the current time. </summary>
        /// <returns> The state derivatives. </returns>
        ODEState ODEStateDerivative { get; }
    }
}