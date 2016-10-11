using System.Net;
using Core;
using DotRLGlueCodec;
using DotRLGlueCodec.Network;

namespace Application.Integration.RLGlue
{
    public class RLGlueExperimentWithEnvironment : RLGlueExperiment
    {
        public override RlGlueConnection.ConnectionState ConnectionState
        {
            get
            {
                return clientEnvironment.ConnectionState;
            }
        }

        public override double CurrentReward
        {
            get { return this.rlGlueInterface.CurrentReward; }
        }

        public override double AverageReward
        {
            get { return this.rlGlueInterface.AverageReward; }
        }

        public override double EpisodeAverageReward
        {
            get { return this.rlGlueInterface.EpisodeAverageReward; }
        }

        public RLGlueExperimentWithEnvironment(Component environment)
            : base(environment)
        {
            EnvironmentInterface environmentInterface = (EnvironmentInterface)typeof(RLGlueEnvironmentInterface<,>).MakeGenericType(
                environment.ComponentType.StateSpaceType,
                environment.ComponentType.ActionSpaceType)
                .GetConstructor(new[] { typeof(Component) })
                .Invoke(new object[] { environment });

            this.clientEnvironment = new ClientEnvironment(environmentInterface);
            this.rlGlueInterface = (IRLGlueInterface)environmentInterface;
        }

        public override void InitializeConnection(IPAddress ipAddress, int portNumber)
        {
            this.clientEnvironment.Connect(ipAddress, portNumber);
        }

        public override void CloseConnection()
        {
            this.clientEnvironment.Close();
        }

        public override void RunConnectionLoop()
        {
            this.clientEnvironment.runEnvironmentEventLoop();
        }

        private ClientEnvironment clientEnvironment;
        private IRLGlueInterface rlGlueInterface;
    }
}
