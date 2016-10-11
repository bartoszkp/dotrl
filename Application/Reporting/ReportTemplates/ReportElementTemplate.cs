using System;
using System.Linq;
using Application.Reporting.DataAccumulators;
using Core;
using Core.Parameters;

namespace Application.Reporting.ReportTemplates
{
    public class ReportElementTemplate
    {
        public static ReportElementTemplate Create()
        {
            return new ReportElementTemplate();
        }

        public string DataSourceObjectTypeName { get; set; }
        public string DataSourceObjectFieldName { get; set; }
        public string DataAccumulatorTypeName { get; set; }
        public ParametersSnapshot DataAccumulatorParameters { get; set; }

        public ReportElementTemplate WithDataProvider(DataProvider dataProvider)
        {
            this.DataSourceObjectTypeName = dataProvider.Object.GetType().GetDisplayName();
            this.DataSourceObjectFieldName = dataProvider.Field.Name;

            return this;
        }

        public ReportElementTemplate WithDataAccumulator(DataAccumulator dataAccumulator)
        {
            this.DataAccumulatorTypeName = dataAccumulator.GetType().GetDisplayName();
            this.DataAccumulatorParameters = dataAccumulator.GetParameterSnapshotWithInnerObjects();

            return this;
        }

        public ReportElement ToReportElement(DataSource[] dataSources, Type[] dataAccumulators)
        {
            DataSource dataSource = DataSource.FindDataSource(dataSources, this.DataSourceObjectTypeName);
            DataProvider dataProvider = dataSource.ToDataProvider(this.DataSourceObjectFieldName);
            DataAccumulator dataAccumulator = dataAccumulators
                .Single(da => da.GetDisplayName().Equals(this.DataAccumulatorTypeName))
                .InstantiateWithDefaultConstructor<DataAccumulator>();

            dataAccumulator.SetParametersFromSnapshotWithInnerObjects(this.DataAccumulatorParameters);

            return new ReportElement(dataProvider, dataAccumulator);
        }
    }
}
