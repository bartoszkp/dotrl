using System.Linq;
using Application.Reporting;
using Application.Reporting.DataAccumulators;
using Application.Reporting.ReportTemplates;
using Application.Reporting.ReportTriggers;
using Core;
using Core.Parameters;

namespace Application.ExperimentTemplates
{
    public class ExperimentTemplate
    {
        public static ExperimentTemplate Create(ExperimentBase experiment)
        {
            ExperimentTemplate result = new ExperimentTemplate()
            {
                ExperimentTypeName = experiment.GetType().FullName
            };
            
            result.ExperimentParameters = experiment.GetParametersSnapshot();

            return result
                .WithAgent(experiment.Agent)
                .WithEnvironment(experiment.Environment)
                .WithReporter(experiment.Reporter);
        }

        public string ExperimentTypeName { get; set; }

        public string AgentTypeName { get; set; }
        
        public string EnvironmentTypeName { get; set; }
        
        public ParametersSnapshot ExperimentParameters { get; set; }

        public ParametersSnapshot AgentParameters { get; set; }

        public ParametersSnapshot EnvironmentParameters { get; set; }
        
        public ReporterTemplate ReporterTemplate { get; set; }

        public ExperimentTemplate WithAgent(Component agent)
        {
            this.AgentTypeName = agent.GetType().FullName;
            this.AgentParameters = agent.GetParameterSnapshotWithInnerObjects();

            return this;
        }

        public ExperimentTemplate WithEnvironment(Component environment)
        {
            this.EnvironmentTypeName = environment.GetType().FullName;
            this.EnvironmentParameters = environment.GetParameterSnapshotWithInnerObjects();

            return this;
        }

        public ExperimentTemplate WithReporter(Reporter reporter)
        {
            this.ReporterTemplate = ReporterTemplate.Create(reporter);

            return this;
        }

        public ExperimentBase ToExperiment()
        {
            Component environment = EnvironmentRegistry
                .GetEnvironments()
                .Single(t => t.FullName.Equals(this.EnvironmentTypeName))
                .InstantiateWithDefaultConstructor<Component>();

            Component presentationEnvironment = environment.Clone();

            Component agent = AgentRegistry
                .GetAgents(environment.ComponentType)
                .Single(t => t.FullName.Equals(this.AgentTypeName))
                .InstantiateWithDefaultConstructor<Component>();

            ExperimentBase result = ExperimentBase.Instantiate(environment, presentationEnvironment, agent);

            result.SetParametersFromSnapshot(this.ExperimentParameters);
            environment.SetParametersFromSnapshotWithInnerObjects(this.EnvironmentParameters);
            presentationEnvironment.SetParametersFromSnapshotWithInnerObjects(this.EnvironmentParameters);
            agent.SetParametersFromSnapshotWithInnerObjects(this.AgentParameters);

            result.Reporter = this.ReporterTemplate.ToReporter(
                ReportTrigger.GetReportTriggers().ToArray(),
                DataSource.GetDataSources(result).ToArray(),
                DataAccumulator.GetDataAccumulators().ToArray());

            return result;
        }
    }
}
