using System.IO;
using System.Net;
using System.Net.Sockets;

namespace Core.Infrastructure
{
    public class RLGlueConnectionManager : IUserInitializationActionPredicate
    {
        public RLGlueConnectionManager(Component owner)
        {
            this.owner = owner;
            this.rlGlueConnection = new DotRLGlueCodec.Network.RlGlueConnection();
            this.connected = false;
        }

        public bool InitializeAgent(int portNumber, string taskSpec)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, portNumber);

            listener.Start(1);

            this.initialConnectionState = DotRLGlueCodec.Network.RlGlueConnection.ConnectionState.AgentConnection;

            listener.BeginAcceptSocket(ConnectionCallback, listener);

            this.owner.UserInitializationActionManager.AskUserToPerformInitializationAction(
                "Run the RL-Glue agent now, configuring it to connect to port " + portNumber + ".\n After the connection will have been established this dialog will automatically close.",
                this);

            if (!this.connected)
            {
                return false;
            }

            this.rlGlueConnection
                .Send()
                .State(DotRLGlueCodec.Network.RlGlueConnection.ConnectionState.AgentInitialize)
                .And()
                .SizeOfState()
                .AndSizeOfString(taskSpec)
                .And()
                .String(taskSpec)
                .Flush();

            ReadAndVerifyState(DotRLGlueCodec.Network.RlGlueConnection.ConnectionState.AgentInitialize);

            return true;
        }

        public string InitializeEnvironment(int portNumber)
        {
            TcpListener listener = new TcpListener(IPAddress.Any, portNumber);

            listener.Start(1);

            this.initialConnectionState = DotRLGlueCodec.Network.RlGlueConnection.ConnectionState.EnvironmentConnection;

            listener.BeginAcceptSocket(ConnectionCallback, listener);

            this.owner.UserInitializationActionManager.AskUserToPerformInitializationAction(
                "Run the RL-Glue environment now, configuring it to connect to port " + portNumber + ".\n After the connection will have been established this dialog will automatically close.",
                this);

            if (!this.connected)
            {
                return string.Empty;
            }

            this.rlGlueConnection
              .Send()
              .State(DotRLGlueCodec.Network.RlGlueConnection.ConnectionState.EnvironmentInitialize)
              .And()
              .SizeOfState()
              .Flush();
            ReadAndVerifyState(DotRLGlueCodec.Network.RlGlueConnection.ConnectionState.EnvironmentInitialize);

            return this.rlGlueConnection.Receive().String();
        }

        public void DisconnectAgent()
        {
            Disconnect(DotRLGlueCodec.Network.RlGlueConnection.ConnectionState.AgentCleanup);
        }

        public void DisconnectEnvironment()
        {
            Disconnect(DotRLGlueCodec.Network.RlGlueConnection.ConnectionState.EnvironmentCleanup);
        }

        private void Disconnect(DotRLGlueCodec.Network.RlGlueConnection.ConnectionState cs)
        {
            if (this.rlGlueConnection == null)
            {
                return;
            }

            this.rlGlueConnection
                .Send()
                .State(cs)
                .And()
                .SizeOfState()
                .Flush();
            ReadAndVerifyState(cs);

            this.rlGlueConnection
                .Send()
                .State(DotRLGlueCodec.Network.RlGlueConnection.ConnectionState.RLTerminate)
                .And()
                .SizeOfState()
                .Flush();

            this.rlGlueConnection.Close();
        }

        public DotRLGlueCodec.Types.Action StartEpisodeAgent(DotRLGlueCodec.Types.Observation observation)
        {
            this.rlGlueConnection
               .Send()
               .State(DotRLGlueCodec.Network.RlGlueConnection.ConnectionState.AgentStart)
               .And()
               .SizeOfState()
               .AndSizeOfObservation(observation)
               .And()
               .Observation(observation)
               .Flush();

            ReadAndVerifyState(DotRLGlueCodec.Network.RlGlueConnection.ConnectionState.AgentStart);

            return this.rlGlueConnection.Receive().Action();
        }

        public DotRLGlueCodec.Types.Observation StartEpisodeEnvironment()
        {
            this.rlGlueConnection
                .Send()
                .State(DotRLGlueCodec.Network.RlGlueConnection.ConnectionState.EnvironmentStart)
                .And()
                .SizeOfState()
                .Flush();
            
            ReadAndVerifyState(DotRLGlueCodec.Network.RlGlueConnection.ConnectionState.EnvironmentStart);

            return this.rlGlueConnection.Receive().Observation();
        }

        public DotRLGlueCodec.Types.Action StepAgent(DotRLGlueCodec.Types.Observation observation, double reinforcement)
        {
            this.rlGlueConnection
                .Send()
                .State(DotRLGlueCodec.Network.RlGlueConnection.ConnectionState.AgentStep)
                .And()
                .SizeOfState()
                .AndSizeOfDouble()
                .AndSizeOfObservation(observation)
                .And()
                .Double(reinforcement)
                .And()
                .Observation(observation)
                .Flush();
            
            ReadAndVerifyState(DotRLGlueCodec.Network.RlGlueConnection.ConnectionState.AgentStep);

            return this.rlGlueConnection.Receive().Action();
        }

        public double StepEnvironment(
            DotRLGlueCodec.Types.Action action,
            System.Action<bool> receiveIsTerminal,
            System.Action<DotRLGlueCodec.Types.Observation> receiveObservation)
        {
            this.rlGlueConnection
                .Send()
                .State(DotRLGlueCodec.Network.RlGlueConnection.ConnectionState.EnvironmentStep)
                .And()
                .SizeOfState()
                .AndSizeOfAction(action)
                .And()
                .Action(action)
                .Flush();
            ReadAndVerifyState(DotRLGlueCodec.Network.RlGlueConnection.ConnectionState.EnvironmentStep);

            bool terminal;
            double reward;
            DotRLGlueCodec.Types.Observation o;

            this.rlGlueConnection
                .Receive()
                .Boolean(out terminal)
                .And()
                .Double(out reward)
                .And()
                .Observation(out o);

            receiveIsTerminal(terminal);
            receiveObservation(o);

            return reward;
        }

        public void EndEpisodeAgent(double reinforcement)
        {
            this.rlGlueConnection
                .Send()
                .State(DotRLGlueCodec.Network.RlGlueConnection.ConnectionState.AgentEnd)
                .And()
                .SizeOfState()
                .AndSizeOfDouble()
                .And()
                .Double(reinforcement)
                .Flush();

            ReadAndVerifyState(DotRLGlueCodec.Network.RlGlueConnection.ConnectionState.AgentEnd);
        }

        public bool UserInitializationActionCompleted()
        {
            return this.connected;
        }

        public void CancelPressed()
        {
        }

        public void ReadAndVerifyState(DotRLGlueCodec.Network.RlGlueConnection.ConnectionState state)
        {
            DotRLGlueCodec.Network.RlGlueConnection.ConnectionState rlGlueState;

            this.rlGlueConnection
                .Receive()
                .State(out rlGlueState)
                .And()
                .DiscardInteger();

            if (rlGlueState != state)
            {
                throw new InvalidDataException();
            }
        }
        
        private void ConnectionCallback(System.IAsyncResult connectionResult)
        {
            Socket socket = ((TcpListener)connectionResult.AsyncState).EndAcceptSocket(connectionResult);

            this.rlGlueConnection.Accept(socket);

            this.ReadAndVerifyState(initialConnectionState);

            this.connected = true;
        }
                
        private Component owner;
        private DotRLGlueCodec.Network.RlGlueConnection rlGlueConnection;
        private DotRLGlueCodec.Network.RlGlueConnection.ConnectionState initialConnectionState;
        private bool connected;
    }
}
