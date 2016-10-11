using System;
using System.Threading;
using System.Windows.Forms;

namespace Application
{
    public class ExperimentWorker
    {
        public event EventHandler ExperimentFinished;

        public ExperimentBase Experiment { get; private set; }

        public ExperimentWorkerMode WorkerMode { get; private set; }

        public Exception Error { get; private set; }

        public int MaximumProgress { get; private set; }

        public int CurrentProgress { get; private set; }

        public object Tag { get; set; }

        public ExperimentWorker(ExperimentBase experiment)
        {
            this.Experiment = experiment;
            this.backgroundThread = null;

            this.semaphore = new SemaphoreSlim(1, 1);
            this.initialized = false;
            this.interruptRequested = false;

            ReconfigureProgress();
        }

        public void ReconfigureProgress()
        {
            if (this.Experiment.TotalStepCountLimit > 0)
            {
                this.MaximumProgress = this.Experiment.TotalStepCountLimit;
                this.CurrentProgress = this.Experiment.TotalStepCount;
            }
            else if (this.Experiment.EpisodeCountLimit > 0)
            {
                this.MaximumProgress = this.Experiment.EpisodeCountLimit;
                this.CurrentProgress = this.Experiment.EpisodeCount;
            }
            else
            {
                this.MaximumProgress = 0;
                this.CurrentProgress = 0;
            }
        }

        public void ConfigureProgressBar(ProgressBar progressBar)
        {
            if (this.Experiment.TotalStepCountLimit > 0)
            {
                progressBar.Style = ProgressBarStyle.Continuous;
            }
            else if (this.Experiment.EpisodeCountLimit > 0)
            {
                progressBar.Style = ProgressBarStyle.Continuous;
            }
            else
            {
                progressBar.Style = ProgressBarStyle.Marquee;
            }

            progressBar.Maximum = this.MaximumProgress;

            if (this.CurrentProgress > this.MaximumProgress)
            {
                this.CurrentProgress = this.MaximumProgress;
            }

            progressBar.Value = this.CurrentProgress;
        }

        public void PauseWork()
        {
            if (this.backgroundThread != null && this.backgroundThread.IsAlive)
            {
                this.interruptRequested = true;

                while (this.backgroundThread.IsAlive)
                {
                    this.ReleaseSemaphoreIfNeeded();
                }

                this.backgroundThread = null;
            }

            this.WorkerMode = ExperimentWorkerMode.Paused;
        }

        public void ResumeBackgroundLearning()
        {
            this.WorkerMode = ExperimentWorkerMode.BackgroundLearning;

            Resume();
        }

        public void ResumeRealtimeLearning()
        {
            this.WorkerMode = ExperimentWorkerMode.RealTimeLearning;

            Resume();
        }

        public void ResumePresentation()
        {
            this.Experiment.StartPresentationEpisode();

            this.WorkerMode = ExperimentWorkerMode.PolicyPresentation;

            Resume();
        }

        public void ReleaseSemaphoreIfNeeded()
        {
            if (this.semaphore.CurrentCount == 0)
            {
                this.semaphore.Release();
            }
        }

        private void Resume()
        {
            if (this.backgroundThread != null)
            {
                if (!this.backgroundThread.IsAlive)
                {
                    this.backgroundThread = null;
                }
            }

            this.interruptRequested = false;

            if (this.backgroundThread == null)
            {
                this.backgroundThread = new Thread(new ThreadStart(()
                    =>
                    {
                        try
                        {
                            DoWork();
                        }
                        catch (Exception e)
                        {
                            this.Error = e;
                            this.interruptRequested = false;
                        }

                        if (!this.interruptRequested)
                        {
                            Completed();
                        }
                    }));

                this.backgroundThread.IsBackground = true;
                this.backgroundThread.Start();
            }
        }

        private void DoWork()
        {
            int step = 0;

            if (!this.initialized)
            {
                this.Experiment.Initialize();
                this.initialized = true;
            }

            while (!this.Experiment.IsEndOfExperiment || this.WorkerMode == ExperimentWorkerMode.PolicyPresentation)
            {
                if (this.interruptRequested)
                {
                    return;
                }

                if ((this.WorkerMode == ExperimentWorkerMode.RealTimeLearning
                    || this.WorkerMode == ExperimentWorkerMode.PolicyPresentation)
                    && ++step >= GlobalParameters.StepsPerFrame)
                {
                    this.semaphore.Wait();
                    step = 0;
                }

                if (this.WorkerMode == ExperimentWorkerMode.PolicyPresentation)
                {
                    this.Experiment.DoPresentationStep();
                }
                else
                {
                    this.Experiment.DoStep();
                }

                this.UpdateProgress();
            }

            this.interruptRequested = false;
        }

        private void UpdateProgress()
        {
            if (this.Experiment.TotalStepCountLimit > 0)
            {
                this.CurrentProgress = Math.Min(this.Experiment.TotalStepCountLimit, this.Experiment.TotalStepCount);
            }
            else if (this.Experiment.EpisodeCountLimit > 0)
            {
                this.CurrentProgress = Math.Min(this.Experiment.EpisodeCountLimit, this.Experiment.EpisodeCount);
            }
        }

        private void Completed()
        {
            this.WorkerMode = ExperimentWorkerMode.Paused;

            if (this.ExperimentFinished != null)
            {
                this.ExperimentFinished(this, EventArgs.Empty);
            }
        }

        private Thread backgroundThread;
        private SemaphoreSlim semaphore;
        private bool initialized;
        private bool interruptRequested;
    }
}
