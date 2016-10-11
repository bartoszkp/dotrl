using Core.Parameters;

namespace Application
{
    class GlobalParameters : IParametrizedObject
    {
        [Parameter(1, int.MaxValue)]
        static public int TimerInterval { get; set; }

        [Parameter(1, int.MaxValue)]
        static public int StepsPerFrame { get; set; }

        static GlobalParameters()
        {
            TimerInterval = 1;
            StepsPerFrame = 1;
        }

        public virtual void ParametersChanged()
        {
        }
    }
}
