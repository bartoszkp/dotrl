using System.Net;
using Core;
using DotRLGlueCodec;
using DotRLGlueCodec.Network;

namespace Application.Integration.RLGlue
{
    public class RLGlueExperimentWithAgent : RLGlueExperiment
    {
        public override RlGlueConnection.ConnectionState ConnectionState
        {
            get
            {
                return clientAgent.ConnectionState;
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

        public RLGlueExperimentWithAgent(Component agent)
            : base(agent)
        {
            AgentInterface agentInterface = (AgentInterface)typeof(RLGlueAgentInterface<,>).MakeGenericType(
                agent.ComponentType.StateSpaceType,
                agent.ComponentType.ActionSpaceType)
                .GetConstructor(new[] { typeof(Component) })
                .Invoke(new object[] { agent });

            this.clientAgent = new ClientAgent(agentInterface);
            this.rlGlueInterface = (IRLGlueInterface)agentInterface;
        }

        public override void InitializeConnection(IPAddress ipAddress, int portNumber)
        {
            this.clientAgent.Connect(ipAddress, portNumber);
        }

        public override void CloseConnection()
        {
            this.clientAgent.Close();
        }

        public override void RunConnectionLoop()
        {
            this.clientAgent.runAgentEventLoop();
        }

        private ClientAgent clientAgent;
        private IRLGlueInterface rlGlueInterface;
    }
}
