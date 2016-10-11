using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Core;
using Core.Reporting;

namespace Application.Reporting
{
    public class DataSource
    {
        public static DataSource[] GetDataSources(ExperimentBase experiment)
        {
            return new object[] { experiment, experiment.Agent, experiment.Environment, new ConstantValueDataSource() }
                .Select(o => new DataSource() { Object = o, ReportingFields = GetReportingFields(o) })
                .Where(ds => ds.ReportingFields.Any())
                .ToArray();
        }

        public static DataSource FindDataSource(ExperimentBase experiment, string dataSourceObjectTypeDisplayName)
        {
            return DataSource.FindDataSource(GetDataSources(experiment), dataSourceObjectTypeDisplayName);
        }

        public static DataSource FindDataSource(DataSource[] dataSources, string dataSourceObjectTypeDisplayName)
        {
            return dataSources
                .Single(ds => ds.Object.GetType().GetDisplayName().Equals(dataSourceObjectTypeDisplayName));
        }

        public object Object { get; set; }

        public IEnumerable<MemberInfo> ReportingFields { get; set; }

        public DataProvider ToDataProvider(string reportingFieldName)
        {
            return new DataProvider(this.Object, this.ReportingFields.Single(rf => rf.Name.Equals(reportingFieldName)));
        }

        private static IEnumerable<MemberInfo> GetReportingFields(object @object)
        {
            return @object
               .GetType()
               .GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static)
               .Concat<MemberInfo>(@object.GetType().GetProperties(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static))
               .Where(fop => HasReportingAttribute(fop))
               .ToArray();
        }

        private static bool HasReportingAttribute(MemberInfo fieldOrProperty)
        {
            return fieldOrProperty.GetCustomAttributes(typeof(ReportedValueAttribute), true).Any();
        }
    }
}
