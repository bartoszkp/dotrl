using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Application.Parameters;
using Application.Reporting;
using Presenters;

namespace Application
{
    public partial class ExperimentWindow : Form
    {
        public static void DisplayException(IWin32Window owner, Exception exception)
        {
            Microsoft.NetEnterpriseServers.ExceptionMessageBox emb = new Microsoft.NetEnterpriseServers.ExceptionMessageBox(
                exception,
                Microsoft.NetEnterpriseServers.ExceptionMessageBoxButtons.OK,
                Microsoft.NetEnterpriseServers.ExceptionMessageBoxSymbol.Error);

            emb.Caption = exception.TargetSite.DeclaringType.FullName + " has thrown an exception:";

            emb.Show(owner);
        }

        public ExperimentWindow(ExperimentBase experiment)
        {
            this.context = BufferedGraphicsManager.Current;
            
            var presenterFactory = EnvironmentRegistry.GetPresenterFactoryForEnvironment(experiment.Environment.GetType());

            this.realTimeLearningPresenter = presenterFactory(experiment.Environment);
            this.policyPresentationPresenter = presenterFactory(experiment.PresentationEnvironment);

            InitializeComponent();

            this.experiment = experiment;
            this.experimentWorker = new ExperimentWorker(this.experiment);
            this.experimentWorker.ExperimentFinished += new EventHandler(ExperimentFinished);
            this.ownExperimentWorker = true;

            this.experimentWorker.ConfigureProgressBar(this.progressBar1);

            this.timer.Stop();
            this.timer.Interval = GlobalParameters.TimerInterval;
            
            InitGraphics();
            this.DoubleBuffered = true;
            
            this.progressBar1.Visible = true;
            this.timer.Interval = GlobalParameters.TimerInterval;

            RefreshPresentationContent();
        }

        public ExperimentWindow(ExperimentWorker experimentWorker)
        {
            this.context = BufferedGraphicsManager.Current;
            
            this.experiment = experimentWorker.Experiment;

            var presenterFactory = EnvironmentRegistry.GetPresenterFactoryForEnvironment(this.experiment.Environment.GetType());

            this.realTimeLearningPresenter = presenterFactory(experiment.Environment);
            this.policyPresentationPresenter = presenterFactory(experiment.PresentationEnvironment);

            InitializeComponent();

            this.experimentWorker = experimentWorker;
            this.experimentWorker.ExperimentFinished += new EventHandler(ExperimentFinished);
            this.ownExperimentWorker = false;

            this.experimentWorker.ConfigureProgressBar(this.progressBar1);

            this.timer.Stop();
            this.timer.Interval = GlobalParameters.TimerInterval;

            InitGraphics();
            this.DoubleBuffered = true;

            this.progressBar1.Visible = true;
            this.timer.Interval = GlobalParameters.TimerInterval;

            RefreshPresentationContent();
        }

        private string GetProgressString()
        {
            return string.Format(
                "Total steps: {0}/{1} ; Episodes: {2}/{3} ; Steps in current episode: {4}/{5}",
                this.experiment.TotalStepCount,
                this.experiment.TotalStepCountLimit > 0 ? this.experiment.TotalStepCountLimit.ToString() : "(unlimited)",
                this.experiment.EpisodeCount,
                this.experiment.EpisodeCountLimit > 0 ? this.experiment.EpisodeCountLimit.ToString() : "(unlimited)",
                this.experiment.EpisodeStepCount,
                this.experiment.EpisodeStepCountLimit > 0 ? this.experiment.EpisodeStepCountLimit.ToString() : "(unlimited)");
        }

        private void UpdateStatus()
        {
            string text = string.Empty;
            switch (this.experimentWorker.WorkerMode)
            {
                case ExperimentWorkerMode.BackgroundLearning:
                    text = "Background learning [ " + GetProgressString() + " ] ";
                    break;
                case ExperimentWorkerMode.RealTimeLearning:
                    text = "Real time learning [ " + GetProgressString() + " ] ";
                    break;
                case ExperimentWorkerMode.PolicyPresentation:
                    text = "Presenting current policy"; 
                    break;
                case ExperimentWorkerMode.Paused:
                    text = "Paused";
                    break;
            }

            statusStrip1.Items[0].Text = text;

            lock (this.experimentWorker)
            {
                this.progressBar1.Value = this.experimentWorker.CurrentProgress;
            }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (this.experimentWorker.WorkerMode == ExperimentWorkerMode.Paused)
            {
                return;
            }

            this.UpdateStatus();

            this.experimentWorker.ReleaseSemaphoreIfNeeded();

            if (this.experimentWorker.WorkerMode == ExperimentWorkerMode.RealTimeLearning)
            {
                this.RefreshRealTimeLearningContent();
            }
            else if (this.experimentWorker.WorkerMode == ExperimentWorkerMode.PolicyPresentation)
            {
                this.RefreshPresentationContent();
            }
        }

        private void RefreshRealTimeLearningContent()
        {
            this.realTimeLearningPresenter.Draw();
            this.graphics.Render(this.panel1.CreateGraphics());
        }

        private void RefreshPresentationContent()
        {
            this.policyPresentationPresenter.Draw();
            this.graphics.Render(this.panel1.CreateGraphics());
        }

        private void RealtimeLearningButtonClick(object sender, EventArgs e)
        {
            this.experimentWorker.ResumeRealtimeLearning();
            this.timer.Start();

            this.progressBar1.MarqueeAnimationSpeed = 50;

            this.UpdateStatus();
        }

        private void BackgroundLearningButtonClick(object sender, EventArgs e)
        {
            this.experimentWorker.ResumeBackgroundLearning();
            this.timer.Start();

            this.progressBar1.MarqueeAnimationSpeed = 50;

            this.UpdateStatus();
        }

        private void PauseButtonClick(object sender, EventArgs e)
        {
            this.PauseWork();
        }

        private void ExperimentParametersButtonClick(object sender, EventArgs e)
        {
            this.PauseWork();

            ParameterWindow.ConfigureParametrizedObject(this.experiment);

            this.experimentWorker.ReconfigureProgress();
            this.experimentWorker.ConfigureProgressBar(this.progressBar1);
            
            this.UpdateStatus();
        }

        private void PresentCurrentPolicyButtonClick(object sender, EventArgs e)
        {
            this.experimentWorker.ResumePresentation();

            this.progressBar1.MarqueeAnimationSpeed = 0;

            this.timer.Start();

            this.UpdateStatus();
        }

        private void PauseWork()
        {
            this.experimentWorker.PauseWork();
            this.timer.Stop();

            while (this.experimentWorker.WorkerMode != ExperimentWorkerMode.Paused)
            {
                System.Windows.Forms.Application.DoEvents();
            }

            this.progressBar1.MarqueeAnimationSpeed = 0;

            this.UpdateStatus();
        }

        private void OnSizeChanged(object sender, EventArgs e)
        {
            this.InitGraphics();
        }

        private void InitGraphics()
        {
            this.context.MaximumBuffer = new Size(this.panel1.Width + 1, this.panel1.Height + 1);
            Rectangle rectangle = new Rectangle(0, 0, this.panel1.Width, this.panel1.Height);
            this.graphics = context.Allocate(this.panel1.CreateGraphics(), rectangle);
            this.policyPresentationPresenter.Resize(this.graphics.Graphics, this.panel1.Width, this.panel1.Height);
            this.realTimeLearningPresenter.Resize(this.graphics.Graphics, this.panel1.Width, this.panel1.Height);
        }

        private void ExperimentFinished(object sender, EventArgs e)
        {
            MethodInvoker mi = new MethodInvoker(ExperimentFinished);

            this.Invoke(mi);
        }

        private void ExperimentFinished()
        {
            this.UpdateStatus();

            if (this.experimentWorker.Error != null)
            {
                this.realtimeLearningButton.Enabled = false;
                this.backgroundLearningButton.Enabled = false;
                this.pauseButton.Enabled = false;
                this.experimentParametersButton.Enabled = false;
                this.presentCurrentPolicyButton.Enabled = false;
                this.reportingParametersButton.Enabled = false;

                RefreshPresentationContent();

                ExperimentWindow.DisplayException(this, this.experimentWorker.Error);

                RefreshPresentationContent();
            }
        }

        private void ReportingParametersButtonClick(object sender, EventArgs e)
        {
            this.PauseWork();

            ReportingConfigurationWindow reportingConfigurationWindow = new ReportingConfigurationWindow(this.experiment);
            reportingConfigurationWindow.SetReportFileWriters(this.experiment.Reporter.ReportWriters.Cast<ReportFileWriter>(), false);
            reportingConfigurationWindow.ShowDialog();

            List<ReportFileWriter> newReportWriters = reportingConfigurationWindow.GetConfiguredEditableReportWriters().ToList();

            newReportWriters.ForEach(reportFile => reportFile.ExperimentStarted(this.experiment));

            this.experiment.Reporter.ReportWriters.AddRange(newReportWriters);
        }

        private void ExperimentWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.PauseWork();
        }

        private void ExperimentWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (this.ownExperimentWorker)
            {
                this.experiment.End();
            }

            this.experimentWorker.ExperimentFinished -= ExperimentFinished;
        }

        private void ExperimentWindow_Paint(object sender, PaintEventArgs e)
        {
            if (this.experimentWorker.WorkerMode == ExperimentWorkerMode.RealTimeLearning)
            {
                this.RefreshRealTimeLearningContent();
            }
            else
            {
                this.RefreshPresentationContent();
            }
        }

        private BufferedGraphics graphics;
        private BufferedGraphicsContext context;
        private ExperimentBase experiment;
        private ExperimentWorker experimentWorker;
        private Presenter realTimeLearningPresenter;
        private Presenter policyPresentationPresenter;
        private bool ownExperimentWorker;
    }
}
