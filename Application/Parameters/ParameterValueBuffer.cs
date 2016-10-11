using System.Windows.Forms;
using Core.Parameters;

namespace Application.Parameters
{
    internal class ParameterValueBuffer
    {
        public Parameter Parameter { get; set; }
        public Control Control { get; set; }
        public object NewValue { get; set; }
    }
}
