using System;
using System.Collections.Generic;
using System.Linq;
using Application.Reporting.ReportTriggers;
using Core;
using Core.Parameters;

namespace Application.Reporting.ReportTemplates
{
    public class ReportFileWriterTemplate
    {
        public static ReportFileWriterTemplate Create(string fileName)
        {
            return new ReportFileWriterTemplate()
            {
                FileName = fileName
            };
        }

        public static ReportFileWriterTemplate Create(ReportFileWriter reportWriter)
        {
            return ReportFileWriterTemplate
                .Create(reportWriter.FileName)
                .WithReportTrigger(reportWriter.ReportTrigger)
                .WithReportElements(reportWriter.ReportElements);
        }

        public string FileName { get; set; }
        public string ReportTriggerTypeName { get; set; }
        public ParametersSnapshot ReportTriggerParameters { get; set; }
        public ReportElementTemplate[] ReportElements { get; set; }

        public ReportFileWriterTemplate WithReportTrigger(ReportTrigger reportTrigger)
        {
            this.ReportTriggerTypeName = reportTrigger.GetType().GetDisplayName();
            this.ReportTriggerParameters = reportTrigger.GetParameterSnapshotWithInnerObjects();

            return this;
        }

        public ReportFileWriterTemplate WithReportElements(IEnumerable<ReportElement> reportElements)
        {
            this.ReportElements = reportElements
                .Select(re => ReportElementTemplate
                    .Create()
                    .WithDataProvider(re.DataProvider)
                    .WithDataAccumulator(re.DataAccumulator))
                .ToArray();

            return this;
        }

        public ReportFileWriter ToReportFileWriter(Type[] reportTriggers, DataSource[] dataSources, Type[] dataAccumulators)
        {
            ReportTrigger rt = reportTriggers
                .Single(t => t.GetDisplayName().Equals(this.ReportTriggerTypeName))
                .InstantiateWithDefaultConstructor<ReportTrigger>();

            rt.SetParametersFromSnapshotWithInnerObjects(this.ReportTriggerParameters);

            ReportFileWriter result = new ReportFileWriter(rt, this.FileName);

            result.AddReportElements(this.ReportElements.Select(re => re.ToReportElement(dataSources, dataAccumulators)));

            return result;
        }
    }
}
