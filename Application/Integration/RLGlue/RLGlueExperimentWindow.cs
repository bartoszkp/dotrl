using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using Agents.Infrastructure;
using Core;
using Environments.Infrastructure;

namespace Application.Integration.RLGlue
{
    public partial class RLGlueExperimentWindow : Form
    {
        public RLGlueExperimentWindow(IPAddress ipAddress, int portNumber, Component component)
        {
            InitializeComponent();

            this.ipAddress = ipAddress;
            this.portNumber = portNumber;
            this.component = component;

            this.experiment = null;
            this.refreshTimer = null;
            this.titleLabel.Text = "Initializing...";
        }

        private void Disconnect()
        {
            if (this.connectionThread != null)
            {
                if ((this.connectionThread.ThreadState
                    & (ThreadState.Unstarted
                    | ThreadState.Aborted
                    | ThreadState.AbortRequested
                    | ThreadState.Stopped
                    | ThreadState.StopRequested)) == 0)
                {
                    this.connectionThread.Abort();
                }

                this.connectionThread = null;
            }

            if (this.experiment != null)
            {
                this.experiment.Disconnect();
                this.experiment = null;
            }

            if (this.refreshTimer != null)
            {
                this.refreshTimer.Stop();
                this.refreshTimer = null;
            }
        }

        private void Reconnect()
        {
            Disconnect();

            if (this.component.GetType().IsAgent())
            {
                this.experiment = new RLGlueExperimentWithAgent(this.component);
            }
            else if (this.component.GetType().IsEnvironment())
            {
                this.experiment = new RLGlueExperimentWithEnvironment(this.component);
            }
            else
            {
                throw new InvalidOperationException();
            }

            this.connectionThread = new Thread(new ThreadStart(()
            =>
            {
                SocketException error = null;
                try
                {
                    this.experiment.Connect(ipAddress, portNumber);
                }
                catch (SocketException se)
                {
                    error = se;
                }
                catch (ThreadAbortException)
                {
                    return;
                }

                this.Invoke(new Action(() => ConnectionCallback(error)));
            }));

            this.connectionStatusTextBox.Text = "Connecting...";
            this.finishButton.Text = "Cancel";

            this.connectionThread.Start();
        }

        private void ConnectionCallback(SocketException error)
        {
            this.connectionThread = null;

            if (error != null)
            {
                this.connectionStatusTextBox.Text = "Connection failed";
                this.finishButton.Text = "Exit";
                Disconnect();

                MessageBox.Show(error.Message, "Connection failed.", MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (MessageBox.Show("Retry?", "Connection failed.", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    this.Reconnect();
                    return;
                }
            }
            else
            {
                this.connectionStatusTextBox.Text = "Connected";
                this.finishButton.Text = "Disconnect";
                this.titleLabel.Text = "Exposing " + this.experiment.ComponentName + " to RL-Glue Core at " + this.ipAddress.ToString() + ":" + this.portNumber;

                this.refreshTimer = new System.Windows.Forms.Timer();
                this.refreshTimer.Interval = 100;
                this.refreshTimer.Tick += new EventHandler(RefreshTimerTick);
                this.refreshTimer.Start();
            }
        }

        private void RefreshTimerTick(object sender, EventArgs e)
        {
            if (this.experiment != null)
            {
                this.rlGlueConnectionStateTextBox.Text = this.experiment.ConnectionState.ToString();
                this.currentRewardTextBox.Text = this.experiment.CurrentReward.ToString();
                this.averageRewardTextBox.Text = this.experiment.AverageReward.ToString();
                this.episodeAverageRewardTextBox.Text = this.experiment.EpisodeAverageReward.ToString();

                if (this.experiment.Finished)
                {
                    this.refreshTimer.Stop();
                    this.refreshTimer = null;
                    this.connectionStatusTextBox.Text = "Disconnected";
                    this.finishButton.Text = "Close";
                    this.titleLabel.Text = "RL-Glue experiment running at " + this.ipAddress.ToString() + ":" + this.portNumber + " has finished";
                    this.experiment.CloseConnection();
                    this.experiment = null;
                }
            }
            else
            {
                this.refreshTimer.Stop();
                this.refreshTimer = null;
            }
        }

        private void FinishButtonClick(object sender, System.EventArgs e)
        {
            if (this.connectionThread != null)
            {
                if (MessageBox.Show("Really abort connecting?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
            }
            else if (this.experiment != null)
            {
                if (MessageBox.Show("Really disconnect?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.No)
                {
                    return;
                }
            }

            Disconnect();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void RLGlueAgentExperimentWindowShown(object sender, EventArgs e)
        {
            Reconnect();
        }

        private IPAddress ipAddress;
        private int portNumber;
        private Component component;
        private RLGlueExperiment experiment;
        private Thread connectionThread;
        private System.Windows.Forms.Timer refreshTimer;
    }
}
