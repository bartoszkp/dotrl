using System.Globalization;
using Application.Reporting.DataAccumulators;
using Core.Parameters;

namespace Application.Reporting
{
    public class ReportElement : IParametrizedObject
    {
        [Parameter]
        public string ColumnHeaderText { get; set; }

        public DataProvider DataProvider { get; set; }
        public DataAccumulator DataAccumulator { get; set; }

        public ReportElement()
        {
        }

        public ReportElement(DataProvider dataProvider, DataAccumulator dataAccumulator)
        {
            this.DataProvider = dataProvider;
            this.DataAccumulator = dataAccumulator;

            this.ColumnHeaderText = this.DataProvider.ProposedColumnHeaderText;
        }

        public void ExperimentStarted(ExperimentBase experiment)
        {
            this.DataAccumulator.ExperimentStarted(this.DataProvider);
        }

        public void ReportTriggered(ExperimentBase experiment)
        {
            this.DataAccumulator.ReportTriggered(this.DataProvider);
        }

        public void EpisodeStarted(ExperimentBase experiment)
        {
            this.DataAccumulator.EpisodeStarted(this.DataProvider);
        }

        public void StepDone(ExperimentBase experiment)
        {
            this.DataAccumulator.StepDone(this.DataProvider);
        }

        public void EpisodeEnded(ExperimentBase experiment)
        {
            this.DataAccumulator.EpisodeEnded(this.DataProvider);
        }

        public string GetReportToken()
        {
            double? value = this.DataAccumulator.GetCurrentValue();

            if (value.HasValue)
            {
                return value.Value.ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                return "?";
            }
        }

        public ReportElement CloneFor(ExperimentBase experiment)
        {
            return new ReportElement(this.DataProvider.CloneFor(experiment), this.DataAccumulator.Clone())
            {
                ColumnHeaderText = this.ColumnHeaderText
            };
        }

        public void ParametersChanged()
        {
        }
    }
}
