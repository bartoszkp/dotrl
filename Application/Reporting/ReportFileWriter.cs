using System.IO;
using System.Linq;
using Application.Reporting.ReportTriggers;

namespace Application.Reporting
{
    public class ReportFileWriter : ReportWriter
    {
        public const int FlushLineInterval = 100;

        public string FileName { get; private set; }
        
        public ReportFileWriter(ReportTrigger reportTrigger, string fileName)
            : base(reportTrigger)
        {
            this.FileName = fileName;
            this.LineCount = 0;
        }

        public override void ExperimentStarted(ExperimentBase experiment)
        {
            string fileName = Path.GetFileNameWithoutExtension(this.FileName);
            string extension = Path.GetExtension(this.FileName);
            string directory = Path.GetDirectoryName(this.FileName);

            if (this.BatchModeIndex.HasValue)
            {
                fileName += "-" + (this.BatchModeIndex.Value + 1);
            }

            this.streamWriter = new StreamWriter(Path.Combine(directory, fileName + extension));

            base.ExperimentStarted(experiment);
        }

        public override void Write(string text)
        {
            this.streamWriter.Write(text);
            ++this.LineCount;

            if (FlushLineInterval != 0 && this.LineCount % FlushLineInterval == 0)
            {
                this.streamWriter.Flush();
            }
        }

        public override void Dispose()
        {
            if (this.streamWriter != null)
            {
                this.streamWriter.Close();
                this.streamWriter.Dispose();
            }
        }

        public override ReportWriter CloneFor(ExperimentBase experiment)
        {
            ReportFileWriter result = new ReportFileWriter(this.ReportTrigger.Clone(), this.FileName);

            result.AddReportElements(this.ReportElements.Select(re => re.CloneFor(experiment)));

            return result;
        }

        private StreamWriter streamWriter;
        private int LineCount;
    }
}
