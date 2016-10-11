using System.Windows.Forms;
using Application.Reporting.ReportTriggers;

namespace Application.Reporting
{
    public class ReportTextBoxWriter : ReportWriter
    {
        public ReportTextBoxWriter(ReportTrigger reportTrigger, TextBox textBox)
            : base(reportTrigger)
        {
            this.textBox = textBox;
        }

        public override void Write(string text)
        {
            this.textBox.Text += text;
        }

        public override void Dispose()
        {
        }

        public override ReportWriter CloneFor(ExperimentBase experiment)
        {
            return new ReportTextBoxWriter(this.ReportTrigger, this.textBox);
        }

        private TextBox textBox;
    }
}
