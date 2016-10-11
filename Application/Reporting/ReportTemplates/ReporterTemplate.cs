using System.Linq;

namespace Application.Reporting.ReportTemplates
{
    public class ReporterTemplate
    {
        public static ReporterTemplate Create(Reporter reporter)
        {
            ReporterTemplate result = new ReporterTemplate();

            result.ReportTemplates = reporter
                .ReportWriters
                .Select(rw => ReportFileWriterTemplate.Create(rw as ReportFileWriter))
                .ToArray();

            return result;
        }

        public ReportFileWriterTemplate[] ReportTemplates { get; set; }

        public Reporter ToReporter(System.Type[] reportTriggers, DataSource[] dataSources, System.Type[] dataAccumulators)
        {
            Reporter result = new Reporter();
            result
                .ReportWriters
                .AddRange(ReportTemplates.Select(rt => rt.ToReportFileWriter(reportTriggers, dataSources, dataAccumulators)));

            return result;
        }
    }
}
