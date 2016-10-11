using System;
using Core;
using Core.Reporting;

namespace Application.Reporting
{
    class FakeExperiment : ExperimentBase
    {
        public override Component Agent
        { 
            get { throw new NotImplementedException(); } 
        }

        public override Component Environment
        {
            get { throw new NotImplementedException(); } 
        }

        public override Component PresentationEnvironment
        {
            get { throw new NotImplementedException(); }
        }

        public override ISample CurrentSample
        {
            get { throw new NotImplementedException(); } 
        }

        public override int EpisodeStepCountLimit
        {
            get
            {
                return 100;
            }
            protected set
            {
                throw new NotImplementedException();
            }
        }

        public override int EpisodeCountLimit
        {
            get
            {
                return 10;
            }
            protected set
            {
                throw new NotImplementedException();
            }
        }

        public override int TotalStepCountLimit
        {
            get
            {
                return 1000;
            }
            protected set
            {
                throw new NotImplementedException();
            }
        }

        [ReportedValue]
        public override int EpisodeStepCount { get; protected set; }

        [ReportedValue]
        public override int TotalStepCount { get; protected set; }

        [ReportedValue]
        public override int EpisodeCount { get; protected set; }

        public override bool IsEndOfEpisode
        {
            get 
            {
                return EpisodeStepCount >= EpisodeStepCountLimit;    
            }
        }

        public override bool IsEndOfExperiment
        {
            get 
            {
                return TotalStepCount >= TotalStepCountLimit || EpisodeCount >= EpisodeCountLimit;
            }
        }

        public override void Initialize()
        {
            this.EpisodeStepCount = 0;
            this.TotalStepCount = 0;
        }

        public override void StartPresentationEpisode()
        {
            throw new NotImplementedException();
        }

        public override void DoPresentationStep()
        {
            throw new NotImplementedException();
        }

        public override void DoStep()
        {
            if (IsEndOfEpisode)
            {
                this.EpisodeStepCount = 0;
                this.EpisodeCount += 1;
            }

            this.EpisodeStepCount += 1;
            this.TotalStepCount += 1;
        }

        public override void End()
        {
        }
    }
}
