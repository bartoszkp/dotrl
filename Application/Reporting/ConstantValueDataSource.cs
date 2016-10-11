using Core.Parameters;
using Core.Reporting;

namespace Application.Reporting
{
    public class ConstantValueDataSource : IParametrizedObject
    {
        [ReportedValue]
        [Parameter]
        public readonly double Value = 1;

        public void ParametersChanged()
        {
        }
    }
}
