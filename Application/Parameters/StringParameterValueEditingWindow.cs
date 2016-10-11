using System.Windows.Forms;

namespace Application.Parameters
{
    internal partial class StringParameterValueEditingWindow : Form
    {
        public StringParameterValueEditingWindow(ParameterValueBuffer parameterValueBuffer)
        {
            this.InitializeComponent();
            this.parameterValueTextBox.Text = (string)parameterValueBuffer.NewValue;
            this.parameterValueBuffer = parameterValueBuffer;
        }

        protected void OnClosingWindow(object sender, FormClosedEventArgs e)
        {
            this.parameterValueBuffer.NewValue = this.parameterValueTextBox.Text;
        }

        private ParameterValueBuffer parameterValueBuffer;
    }
}
