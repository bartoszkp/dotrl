using System.Collections.Generic;

namespace Core
{
    public interface ISample
    {
        Reinforcement Reinforcement { get; }

        IEnumerable<double> GetPreviousStateVector();

        IEnumerable<double> GetActionVector();

        IEnumerable<double> GetCurrentStateVector();
    }
}
