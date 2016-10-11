using System.IO;
using System.Net;
using System.Threading;
using Core;
using DotRLGlueCodec.Network;

namespace Application.Integration.RLGlue
{
    public abstract class RLGlueExperiment
    {
        public string ComponentName
        { 
            get 
            { 
                return this.component.GetType().Name; 
            }
        }

        public bool Finished 
        { 
            get 
            {
                return this.rlGlueLoopThread == null; 
            }
        }

        public abstract RlGlueConnection.ConnectionState ConnectionState { get; }

        public abstract double CurrentReward { get; }

        public abstract double AverageReward { get; }

        public abstract double EpisodeAverageReward { get; }

        public abstract void InitializeConnection(IPAddress ipAddress, int portNumber);

        public abstract void CloseConnection();

        public abstract void RunConnectionLoop();

        public RLGlueExperiment(Component component)
        {
            this.rlGlueLoopThread = null;
            this.component = component;
        }

        public void Connect(IPAddress ipAddress, int portNumber)
        {
            this.InitializeConnection(ipAddress, portNumber);

            this.rlGlueLoopThread = new Thread(new ThreadStart(()
               =>
            {
                try
                {
                    this.RunConnectionLoop();
                }
                catch (System.Net.Sockets.SocketException)
                {
                    // TODO: report errors
                }
                catch (IOException)
                {
                    // TODO: report errors
                }
                catch (ThreadAbortException)
                {
                    this.CloseConnection();
                    return;
                }

                this.rlGlueLoopThread = null;
            }));

            this.rlGlueLoopThread.Start();
        }

        public void Disconnect()
        {
            if (this.rlGlueLoopThread != null && (this.rlGlueLoopThread.ThreadState
                & (ThreadState.Unstarted
                | ThreadState.Aborted
                | ThreadState.AbortRequested
                | ThreadState.Stopped
                | ThreadState.StopRequested)) == 0)
            {
                this.rlGlueLoopThread.Abort();
                this.rlGlueLoopThread = null;
            }

            this.CloseConnection();
        }

        private Thread rlGlueLoopThread;
        protected Component component;
    }
}
