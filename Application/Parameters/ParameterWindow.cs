using System;
using System.Windows.Forms;
using Core.Parameters;

namespace Application.Parameters
{
    public partial class ParameterWindow : Form
    {
        public static DialogResult ConfigureParametrizedObjectAndInnerObjects(IParametrizedObject parametrizedObject)
        {
            return (new ParameterWindow(parametrizedObject, true)).ShowDialog();
        }

        public static DialogResult ConfigureParametrizedObject(IParametrizedObject parametrizedObject)
        {
            return (new ParameterWindow(parametrizedObject, false)).ShowDialog();
        }

        public ParameterWindow(IParametrizedObject parametrizedObject, bool withInnerObjects)
        {
            InitializeComponent();

            this.parameterControl.Initialize(parametrizedObject, withInnerObjects);
        }

        private void OkButtonClick(object sender, EventArgs e)
        {
            this.parameterControl.ApplyChanges();
        }

        private void CancelButtonClick(object sender, EventArgs e)
        {
            this.parameterControl.RevertChanges();
        }
    }
}
