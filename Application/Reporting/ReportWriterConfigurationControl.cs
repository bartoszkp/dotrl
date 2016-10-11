using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Application.Parameters;
using Application.Reporting.DataAccumulators;
using Application.Reporting.ReportTemplates;
using Application.Reporting.ReportTriggers;
using Core;
using Core.Parameters;
using Core.Reporting;

namespace Application.Reporting
{
    public partial class ReportWriterConfigurationControl : UserControl
    {
        public bool Editable { get; private set; }

        public ReportWriterConfigurationControl(
            Type[] reportTriggers,
            Type[] dataAccumulators,
            ExperimentBase experiment,
            bool editable)
        {
            InitializeComponent();

            this.Editable = editable;
            this.reportTriggers = reportTriggers;
            this.experiment = experiment;
            this.dataAccumulators = dataAccumulators;
            this.inhibitParameterWindow = true;
            this.inhibitValueChangedEvent = false;
            this.inhibitUpdatePreview = false;
            this.inhibitCheckBoxEvent = false;
            this.dataSources = DataSource.GetDataSources(experiment);

            this.reportTriggerComboBox.Items.AddRange(this.reportTriggers.Select(t => t.GetDisplayName()).ToArray());
            this.reportTriggerComboBox.SelectedItem = typeof(EpisodeCountReportTrigger).GetDisplayName();

            int rowIndex = this.reportGridView.Rows.Add();
            this.dataProviderObjectRow = this.reportGridView.Rows[rowIndex];
            rowIndex = this.reportGridView.Rows.Add();
            this.dataProviderFieldRow = this.reportGridView.Rows[rowIndex];
            rowIndex = this.reportGridView.Rows.Add();
            this.dataAccumulatorRow = this.reportGridView.Rows[rowIndex];
            this.reportGridView.ReadOnly = !this.Editable;

            this.dataProviderObjectRow.HeaderCell.Value = "Object";
            this.dataProviderFieldRow.HeaderCell.Value = "Field";
            this.dataAccumulatorRow.HeaderCell.Value = "Data accumulator";

            AddReportElement(this.dataSources.First(), "EpisodeCount", typeof(CurrentValueDataAccumulator));
            AddReportElement(this.dataSources.First(), "CurrentReinforcement", typeof(AverageSinceTriggerDataAccumulator));

            this.inhibitParameterWindow = false;
        }

        public void SetReportWriter(ReportWriter reportWriter)
        {
            while (this.reportGridView.ColumnCount > 1)
            {
                this.reportGridView.Columns.RemoveAt(0);
            }

            reportWriter
                .ReportElements
                .ToList()
                .ForEach(i => AddReportElement());
            
            this.inhibitParameterWindow = true;
            this.inhibitValueChangedEvent = true;
            
            this.reportTriggerComboBox.Tag = reportWriter.ReportTrigger;
            this.reportTriggerComboBox.SelectedItem = reportWriter.ReportTrigger.GetType().GetDisplayName();

            for (int i = 0; i < this.reportGridView.ColumnCount - 1; ++i)
            {
                ReportElement reportElement = reportWriter.ReportElements.ElementAt(i);
                this.reportGridView.Columns[i].Tag = reportElement;
                this.reportGridView.Columns[i].HeaderText = reportElement.ColumnHeaderText;

                this.dataProviderObjectRow.Cells[i].Value = reportElement.DataProvider.Object.GetType().GetDisplayName();
                RefreshDataProviderFieldCellItems(this.reportGridView.Columns[i]);

                this.dataProviderFieldRow.Cells[i].Value = reportElement.DataProvider.Field.Name;
                this.dataAccumulatorRow.Cells[i].Value = reportElement.DataAccumulator.GetType().GetDisplayName();

                RefreshReportElement(this.reportGridView.Columns[i]);
            }

            this.inhibitParameterWindow = false;
            this.inhibitValueChangedEvent = false;
        }

        public ReportTrigger GetConfiguredReportTrigger()
        {
            return this.reportTriggerComboBox.Tag as ReportTrigger;
        }

        public IEnumerable<ReportElement> GetConfiguredReportElements()
        {
            return this.reportGridView.Columns
                .Cast<DataGridViewColumn>()
                .Take(this.reportGridView.ColumnCount - 1)
                .OrderBy(c => c.DisplayIndex)
                .Select(column => column.Tag as ReportElement)
                .ToArray();
        }

        private void GridViewColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (!this.Editable || e.Button != System.Windows.Forms.MouseButtons.Left)
            {
                return;
            }

            if (e.ColumnIndex == this.reportGridView.ColumnCount - 1)
            {
                AddReportElement();
            }
        }

        private void AddReportElement()
        {
            AddReportElement(this.dataSources.Single(ds => ds.Object == this.experiment), "TotalStepCount", typeof(CurrentValueDataAccumulator));
        }

        private void AddReportElement(DataSource dataSource, string fieldName, Type dataAccumulatorType)
        {
            this.inhibitUpdatePreview = true;

            DataGridViewComboBoxColumn reportColumn = new DataGridViewComboBoxColumn();
            reportColumn.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;

            this.reportGridView.Columns.Insert(this.reportGridView.ColumnCount - 1, reportColumn);

            var dataProviderObjectCell = this.dataProviderObjectRow.Cells[this.reportGridView.ColumnCount - 2] as DataGridViewComboBoxCell;
            var dataAccumulatorCell = this.dataAccumulatorRow.Cells[this.reportGridView.ColumnCount - 2] as DataGridViewComboBoxCell;

            this.inhibitValueChangedEvent = true;

            dataProviderObjectCell.Items.AddRange(this.dataSources.Select(ds => ds.Object.GetType().GetDisplayName()).ToArray());
            dataAccumulatorCell.Items.AddRange(this.dataAccumulators.Select(t => t.GetDisplayName()).ToArray());

            dataProviderObjectCell.Value = dataSource.Object.GetType().GetDisplayName();
            dataAccumulatorCell.Value = dataAccumulatorType.GetDisplayName();

            RefreshDataProviderFieldCellItems(reportColumn, dataSource.ReportingFields.TakeWhile(mi => !mi.Name.Equals(fieldName)).Count());

            this.inhibitValueChangedEvent = false;

            RefreshReportElement(reportColumn);

            this.inhibitUpdatePreview = false;

            UpdatePreview();
        }

        private void RefreshDataProviderFieldCellItems(DataGridViewColumn column)
        {
            RefreshDataProviderFieldCellItems(column, 0);
        }

        private void RefreshDataProviderFieldCellItems(DataGridViewColumn column, int itemIndex)
        {
            this.inhibitValueChangedEvent = true;

            var dataProviderFieldCell = this.dataProviderFieldRow.Cells[column.Index] as DataGridViewComboBoxCell;

            dataProviderFieldCell.Value = null;

            DataSource dataSource = GetDataSourceFor(column);

            dataProviderFieldCell.Items.Clear();
            dataProviderFieldCell.Items.AddRange(dataSource.ReportingFields.Select(f => f.Name).ToArray());

            dataProviderFieldCell.Value = dataProviderFieldCell.Items[itemIndex];

            this.inhibitValueChangedEvent = false;
        }

        private void ReportTriggerComboBoxValueChanged(object sender, EventArgs e)
        {
            this.reportTriggerComboBox.Tag = null;

            ConfigureReportTrigger();

            UpdatePreview();
        }

        private void GridViewCellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0 || this.inhibitValueChangedEvent)
            {
                return;
            }

            var cell = this.reportGridView.Rows[e.RowIndex].Cells[e.ColumnIndex];

            if (e.RowIndex == this.dataProviderObjectRow.Index)
            {
                RefreshDataProviderFieldCellItems(cell.OwningColumn);
            }

            RefreshReportElement(cell.OwningColumn);

            UpdatePreview();
        }

        private void RefreshReportElement(DataGridViewColumn column)
        {
            ReportElement reportElement = column.Tag as ReportElement;

            if (reportElement == null)
            {
                reportElement = new ReportElement(InstantiateDataProviderFor(column), InstantiateDataAccumulatorFor(column));
                column.Tag = reportElement;
                column.HeaderText = reportElement.ColumnHeaderText;
            }
            else
            {
                DataSource dataSource = GetDataSourceFor(column);
                MemberInfo field = GetDataProviderFieldFor(column);
                Type dataAccumulatorType = GetDataAccumulatorTypeFor(column);

                if (reportElement.DataProvider.Field != field)
                {
                    reportElement.DataProvider = InstantiateDataProviderFor(column);
                    reportElement.ColumnHeaderText = reportElement.DataProvider.ProposedColumnHeaderText;
                    column.HeaderText = reportElement.ColumnHeaderText;
                }

                if (!reportElement.DataAccumulator.GetType().Equals(dataAccumulatorType))
                {
                    reportElement.DataAccumulator = InstantiateDataAccumulatorFor(column);
                }
            }

            RefreshCheckBoxes();
        }

        private void RefreshCheckBoxes()
        {
            this.inhibitCheckBoxEvent = true;

            this.episodeCountCheckBox.Checked = false;
            this.episodeStepCountCheckBox.Checked = false;
            this.totalStepCountCheckBox.Checked = false;
            this.averageReinforcementSinceCheckBox.Checked = false;

            foreach (var re in GetConfiguredReportElements())
            {
                if (IsExperimentReportElement(re, "EpisodeCount", typeof(CurrentValueDataAccumulator)))
                {
                    this.episodeCountCheckBox.Checked = true;
                }
                else if (IsExperimentReportElement(re, "EpisodeStepCount", typeof(CurrentValueDataAccumulator)))
                {
                    this.episodeStepCountCheckBox.Checked = true;
                }
                else if (IsExperimentReportElement(re, "TotalStepCount", typeof(CurrentValueDataAccumulator)))
                {
                    this.totalStepCountCheckBox.Checked = true;
                }
                else if (IsExperimentReportElement(re, "CurrentReinforcement", typeof(AverageSinceTriggerDataAccumulator)))
                {
                    this.averageReinforcementSinceCheckBox.Checked = true;
                }
            }

            this.inhibitCheckBoxEvent = false;
        }

        private DataSource GetDataSourceFor(string objectTypeName)
        {
            return this.dataSources.Single(ds => ds.Object.GetType().GetDisplayName().Equals(objectTypeName));
        }

        private DataSource GetDataSourceFor(DataGridViewColumn column)
        {
            return GetDataSourceFor(this.dataProviderObjectRow.Cells[column.Index].Value.ToString());
        }

        private MemberInfo GetDataProviderFieldFor(DataSource dataSource, string fieldName)
        {
            return dataSource.ReportingFields.Single(f => f.Name.Equals(fieldName));
        }

        private MemberInfo GetDataProviderFieldFor(DataGridViewColumn column)
        {
            DataSource dataSource = GetDataSourceFor(column);
            return GetDataProviderFieldFor(dataSource, this.dataProviderFieldRow.Cells[column.Index].Value.ToString());
        }

        private Type GetDataAccumulatorTypeFor(string dataAccumulatorTypeName)
        {
            return this.dataAccumulators.Single(t => t.Name.Equals(dataAccumulatorTypeName));
        }

        private Type GetDataAccumulatorTypeFor(DataGridViewColumn column)
        {
            return GetDataAccumulatorTypeFor(this.dataAccumulatorRow.Cells[column.Index].Value.ToString());
        }

        private DataProvider InstantiateDataProviderFor(DataSource dataSource, string fieldName)
        {
            MemberInfo field = GetDataProviderFieldFor(dataSource, fieldName);
            return new DataProvider(dataSource.Object, field);
        }

        private DataProvider InstantiateDataProviderFor(string objectTypeName, string fieldName)
        {
            DataSource dataSource = GetDataSourceFor(objectTypeName);
            return InstantiateDataProviderFor(dataSource, fieldName);
        }

        private DataProvider InstantiateDataProviderFor(DataGridViewColumn column)
        {
            DataSource dataSource = GetDataSourceFor(column);
            MemberInfo field = GetDataProviderFieldFor(column);
            return new DataProvider(dataSource.Object, field);
        }

        private DataAccumulator InstantiateDataAccumulatorFor(string dataAccumulatorTypeName)
        {
            return GetDataAccumulatorTypeFor(dataAccumulatorTypeName)
                .InstantiateWithDefaultConstructor<DataAccumulator>();
        }

        private DataAccumulator InstantiateDataAccumulatorFor(DataGridViewColumn column)
        {
            return GetDataAccumulatorTypeFor(column)
                .InstantiateWithDefaultConstructor<DataAccumulator>();
        }

        private void ConfigureReportTrigger()
        {
            if (this.reportTriggerComboBox.Tag == null)
            {
                Type reportTriggerType = this.reportTriggers.Single(t => t.Name.Equals(this.reportTriggerComboBox.SelectedItem.ToString()));

                this.reportTriggerComboBox.Tag = reportTriggerType.GetConstructor(Type.EmptyTypes).Invoke(null);
            }

            ConfigureObjectWithInnerObjects(this.reportTriggerComboBox.Tag as IParametrizedObject);
        }

        private void ConfigureReportElement(DataGridViewColumn column)
        {
            ReportElement reportElement = column.Tag as ReportElement;

            ConfigureObject(reportElement);

            column.HeaderText = reportElement.ColumnHeaderText;

            UpdatePreview();
        }

        private void ConfigureObjectWithInnerObjects(IParametrizedObject parametrizedObject)
        {
            if (this.inhibitParameterWindow)
            {
                return;
            }

            ParameterWindow.ConfigureParametrizedObjectAndInnerObjects(parametrizedObject);
        }

        private void ConfigureObject(IParametrizedObject parametrizedObject)
        {
            if (this.inhibitParameterWindow)
            {
                return;
            }

            ParameterWindow.ConfigureParametrizedObject(parametrizedObject);
        }

        private void GridViewCurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            DataGridView dataGridView = (DataGridView)sender;

            dataGridView.EndEdit();

            UpdatePreview();
        }

        private void ConfigureHeaderTextClick(object sender, EventArgs e)
        {
            DataGridViewColumn column = this.gridViewMenuStrip.Tag as DataGridViewColumn;
            ConfigureReportElement(column);
        }

        private void ConfigureItemClick(object sender, EventArgs e)
        {
            IParametrizedObject parametrizedObject = this.gridViewMenuStrip.Tag as IParametrizedObject;

            if (!parametrizedObject.GetParameters().Any())
            {
                MessageBox.Show("This item has no configuration parameters", "Nothing to configure", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            ConfigureObjectWithInnerObjects(parametrizedObject);
        }

        private void DeleteItemClick(object sender, EventArgs e)
        {
            DataGridViewColumn column = this.gridViewMenuStrip.Tag as DataGridViewColumn;

            this.reportGridView.Columns.Remove(column);

            RefreshCheckBoxes();
            UpdatePreview();
        }

        private void UpdatePreview()
        {
            if (this.inhibitUpdatePreview)
            {
                return;
            }

            this.previewTextBox.Clear();

            Reporter fakeReporter = new Reporter();
            ReportWriter rp = new ReportTextBoxWriter(GetConfiguredReportTrigger(), this.previewTextBox);
            rp.AddReportElements(GetConfiguredReportElements()
                .Select(reportElement => new ReportElement(FakeDataProvider.Create(reportElement.DataProvider), reportElement.DataAccumulator)));

            fakeReporter.ReportWriters.Add(rp);

            FakeExperiment fe = new FakeExperiment();
            fakeReporter.ExperimentStarted(fe);
            fakeReporter.EpisodeStarted(fe);
            while (!fe.IsEndOfExperiment)
            {
                if (fe.IsEndOfEpisode)
                {
                    fakeReporter.EpisodeEnded(fe);
                    fakeReporter.EpisodeStarted(fe);
                }

                fe.DoStep();

                fakeReporter.StepDone(fe);
            }
        }

        private void GridViewMouseDown(object sender, MouseEventArgs e)
        {
            this.configureHeaderTextMenuItem.Visible = false;
            this.configureMenuItem.Visible = false;
            this.deleteColumnMenuItem.Visible = false;

            if (e.Button != System.Windows.Forms.MouseButtons.Right)
            {
                return;
            }

            DataGridView.HitTestInfo hitTestInfo = this.reportGridView.HitTest(e.X, e.Y);

            this.reportGridView.ClearSelection();

            this.gridViewMenuStrip.Tag = hitTestInfo.ColumnIndex >= 0 ? this.reportGridView.Columns[hitTestInfo.ColumnIndex] : null;

            if (hitTestInfo.RowIndex < 0)
            {
                if (hitTestInfo.ColumnIndex >= 0 && hitTestInfo.ColumnIndex < this.reportGridView.ColumnCount - 1)
                {
                    this.configureHeaderTextMenuItem.Visible = true;
                    this.deleteColumnMenuItem.Visible = true;
                    this.gridViewMenuStrip.Show(this.reportGridView, e.Location);
                }

                return;
            }

            DataGridViewRow row = this.reportGridView.Rows[hitTestInfo.RowIndex];
            if (hitTestInfo.ColumnIndex >= 0 && hitTestInfo.ColumnIndex < this.reportGridView.ColumnCount - 1)
            {
                var cell = this.reportGridView.Rows[hitTestInfo.RowIndex].Cells[hitTestInfo.ColumnIndex];
                if (cell.Value == null)
                {
                    return;
                }

                ReportElement re = cell.OwningColumn.Tag as ReportElement;

                if (cell.RowIndex == this.dataAccumulatorRow.Index)
                {
                    this.gridViewMenuStrip.Tag = re.DataAccumulator;
                    this.configureMenuItem.Visible = true;
                    this.gridViewMenuStrip.Show(this.reportGridView, e.Location);
                }
                else if (cell.RowIndex == this.dataProviderObjectRow.Index)
                {
                    this.gridViewMenuStrip.Tag = re.DataProvider.Object;
                    this.configureMenuItem.Visible = true;
                    this.gridViewMenuStrip.Show(this.reportGridView, e.Location);
                }

                cell.Selected = true;
            }
        }

        private void GridViewMouseUp(object sender, MouseEventArgs e)
        {
            if (this.addNewColumnColumn.DisplayIndex != this.reportGridView.ColumnCount - 1)
            {
                this.addNewColumnColumn.DisplayIndex = this.reportGridView.ColumnCount - 1;
            }
        }

        private void GridViewColumnDisplayIndexChanged(object sender, DataGridViewColumnEventArgs e)
        {
            UpdatePreview();
        }

        private void ConfigureReportTriggerButtonClick(object sender, EventArgs e)
        {
            ConfigureReportTrigger();
        }

        private void SaveReportTemplateButtonClick(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.AddExtension = true;
            sfd.DefaultExt = "rt";
            sfd.Filter = "Report templates (*.rt)|*.rt";
            sfd.OverwritePrompt = true;

            if (sfd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            ReportFileWriterTemplate rt = ReportFileWriterTemplate
                .Create(string.Empty)
                .WithReportTrigger(GetConfiguredReportTrigger())
                .WithReportElements(GetConfiguredReportElements());

            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ReportFileWriterTemplate));
            using (System.IO.StreamWriter sw = new System.IO.StreamWriter(sfd.FileName))
            {
                serializer.Serialize(sw, rt);
            }
        }

        private void LoadReportTemplateButtonClick(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.AddExtension = true;
            ofd.DefaultExt = "rt";
            ofd.Filter = "Report templates (*.rt)|*.rt";
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;

            if (ofd.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            ReportFileWriterTemplate rt = null;
            System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(ReportFileWriterTemplate));
            using (System.IO.StreamReader sw = new System.IO.StreamReader(ofd.FileName))
            {
                rt = (ReportFileWriterTemplate)serializer.Deserialize(sw.BaseStream);
            }

            this.SetReportWriter(rt.ToReportFileWriter(this.reportTriggers, this.dataSources, this.dataAccumulators));
        }

        private void EpisodeCountCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            ExperimentRelatedCheckBoxCheckedChanged(sender as CheckBox, "EpisodeCount", typeof(CurrentValueDataAccumulator));
        }

        private void EpisodeStepCountCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            ExperimentRelatedCheckBoxCheckedChanged(sender as CheckBox, "EpisodeStepCount", typeof(CurrentValueDataAccumulator));
        }

        private void StepCountCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            ExperimentRelatedCheckBoxCheckedChanged(sender as CheckBox, "TotalStepCount", typeof(CurrentValueDataAccumulator));
        }

        private void EpisodeAverageReinforcementCheckBoxCheckedChanged(object sender, EventArgs e)
        {
            ExperimentRelatedCheckBoxCheckedChanged(sender as CheckBox, "CurrentReinforcement", typeof(AverageSinceTriggerDataAccumulator));
        }

        private void ExperimentRelatedCheckBoxCheckedChanged(CheckBox checkBox, string fieldName, Type dataAccumulatorType)
        {
            if (this.inhibitCheckBoxEvent)
            {
                return;
            }

            IEnumerable<DataGridViewColumn> relatedColumns = this
                .reportGridView
                .Columns
                .Cast<DataGridViewColumn>()
                .Take(this.reportGridView.ColumnCount - 1)
                .Where(c => IsExperimentReportElement(c.Tag as ReportElement, fieldName, dataAccumulatorType));

            if (checkBox.Checked && !relatedColumns.Any())
            {
                AddReportElement(this.dataSources.First(), fieldName, dataAccumulatorType);
            }
            else if (!checkBox.Checked && relatedColumns.Any())
            {
                foreach (var c in relatedColumns.ToArray())
                {
                    this.reportGridView.Columns.Remove(c);
                }
            }
        }

        private bool IsExperimentReportElement(ReportElement reportElement, string fieldName, Type dataAccumulatorType)
        {
            return reportElement.DataProvider.Object == this.experiment
                && reportElement.DataProvider.Field.Name.Equals(fieldName)
                && reportElement.DataAccumulator.GetType().Equals(dataAccumulatorType);
        }

        private Type[] reportTriggers;
        private Type[] dataAccumulators;
        private ExperimentBase experiment;
        private DataGridViewRow dataProviderObjectRow;
        private DataGridViewRow dataProviderFieldRow;
        private DataGridViewRow dataAccumulatorRow;
        private bool inhibitParameterWindow;
        private bool inhibitValueChangedEvent;
        private bool inhibitUpdatePreview;
        private bool inhibitCheckBoxEvent;
        private DataSource[] dataSources;
    }
}
