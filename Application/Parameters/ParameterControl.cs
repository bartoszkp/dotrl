using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Core;
using Core.Parameters;

namespace Application.Parameters
{
    public partial class ParameterControl : UserControl
    {
        public event EventHandler ParameterValueEdited;
        public event EventHandler ParametersChanged;

        public string OwnerComponentName { get; set; }
        public string OwnerComponentFieldName { get; set; }

        public ParameterControl()
        {
            this.innerParameterControls = new List<ParameterControl>();
            this.inhibitEvents = false;

            this.InitializeComponent();
        }

        public void Initialize(IParametrizedObject parametrizedObject, bool includeInnerObjectParameters)
        {
            this.panel.Controls.Clear();
            this.innerParameterControls.Clear();

            this.parametrizedObject = parametrizedObject;

            this.panel.SuspendLayout();
            this.SuspendLayout();

            AddTitleLabel();

            IEnumerable<Parameter> parameters = this.parametrizedObject.GetParameters();
            IEnumerable<InnerParametrizedObject> innerParametrizedObjects = this.parametrizedObject.GetInnerParametrizedObjects();

            if (parameters.Any())
            {
                foreach (var parameter in parameters)
                {
                    FlowLayoutPanel panel = new FlowLayoutPanel();
                    panel.CausesValidation = true;
                    panel.FlowDirection = FlowDirection.LeftToRight;
                    panel.WrapContents = false;
                    panel.AutoSize = true;
                    panel.AutoScroll = false;
                    panel.AutoSize = true;

                    AddParameterNameLabel(parameter, panel);
                    AddParameterControl(parameter, panel);
                    AddParameterDescriptionLabel(parameter, panel);

                    this.panel.Controls.Add(panel);
                }
            }
            else
            {
                Label noParameterInfoLabel = new Label();
                noParameterInfoLabel.AutoSize = true;
                noParameterInfoLabel.Text = "This object has no parameters";
                noParameterInfoLabel.Enabled = false;
                noParameterInfoLabel.Font = new Font(noParameterInfoLabel.Font, FontStyle.Italic);
                noParameterInfoLabel.TextAlign = ContentAlignment.MiddleCenter;
                noParameterInfoLabel.Margin = new Padding(5);
                this.panel.Controls.Add(noParameterInfoLabel);
            }

            if (includeInnerObjectParameters)
            {
                foreach (InnerParametrizedObject innerParametrizedObject in innerParametrizedObjects)
                {
                    ParameterControl pc = new ParameterControl();
                    pc.ParameterValueEdited = this.ParameterValueEdited;
                    pc.AutoSize = true;
                    pc.OwnerComponentName = this.parametrizedObject.GetType().GetDisplayName();
                    pc.OwnerComponentFieldName = innerParametrizedObject.ParentFieldName;
                    pc.Initialize(innerParametrizedObject.ParametrizedObject, true);
                    
                    this.panel.Controls.Add(pc);
                    this.innerParameterControls.Add(pc);
                }
            }

            this.ResumeLayout();
            this.panel.ResumeLayout();
            this.panel.PerformLayout();
        }

        public void ApplyChanges()
        {
            foreach (ParameterValueBuffer parameterValueBuffer in GetParameters(this.panel))
            {
                parameterValueBuffer.Parameter.SetValue(parameterValueBuffer.NewValue);
            }

            foreach (ParameterControl innerParameterControl in this.innerParameterControls)
            {
                innerParameterControl.ApplyChanges();
            }

            this.parametrizedObject.ParametersChanged();

            if (ParametersChanged != null)
            {
                ParametersChanged(this, EventArgs.Empty);
            }
        }

        public void RevertChanges()
        {
            foreach (ParameterValueBuffer parameterValueBuffer in GetParameters(this.panel))
            {
                RevertParameterValue(parameterValueBuffer);
            }

            foreach (ParameterControl innerParameterControl in this.innerParameterControls)
            {
                innerParameterControl.RevertChanges();
            }
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            return this.panel.GetPreferredSize(proposedSize);
        }

        private IEnumerable<ParameterValueBuffer> GetParameters(Control control)
        {
            List<ParameterValueBuffer> result = new List<ParameterValueBuffer>();
            foreach (Control innerControl in control.Controls)
            {
                ParameterValueBuffer parameterValueBuffer = innerControl.Tag as ParameterValueBuffer;

                if (parameterValueBuffer != null)
                {
                    result.Add(parameterValueBuffer);
                }

                if (innerControl is ParameterControl)
                {
                    continue;
                }

                result.AddRange(GetParameters(innerControl));
            }

            return result;
        }

        private void AddTitleLabel()
        {
            Panel labelPanel = new Panel();
            labelPanel.Margin = new Padding(0, 0, 0, 5);
            labelPanel.Padding = Padding.Empty;
            labelPanel.Dock = DockStyle.Top;
            labelPanel.Width = this.panel.ClientSize.Width - 20;
            labelPanel.AutoSize = true;
            
            Label titleLabel = new Label();
            titleLabel.AutoSize = true;
            titleLabel.Location = new Point(labelPanel.ClientRectangle.Left, labelPanel.ClientRectangle.Top);
            titleLabel.Text = parametrizedObject.GetType().GetDisplayName();
            if (!string.IsNullOrEmpty(OwnerComponentName))
            {
                titleLabel.Text += " (from " + OwnerComponentName + "." + OwnerComponentFieldName + ")";
            }
            titleLabel.Text += " parameters:";
            titleLabel.BackColor = labelPanel.BackColor;

            Label lineLabel = new Label();
            lineLabel.Location = new Point(0, titleLabel.Bounds.Bottom + 2 - titleLabel.Height / 2);
            lineLabel.AutoSize = false;
            lineLabel.Size = new Size(labelPanel.ClientSize.Width, 2);
            lineLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            
            labelPanel.Controls.Add(lineLabel);
            labelPanel.Controls.Add(titleLabel);

            this.panel.Controls.Add(labelPanel);
        }

        private void AddParameterNameLabel(Parameter parameter, Panel container)
        {
            Label parameterLabel = new Label();
            parameterLabel.AutoSize = true;
            parameterLabel.Margin = new Padding(0, 3, 0, 0);
            parameterLabel.Name = parameter.Field.Name;
            parameterLabel.Text = parameter.Field.Name;

            container.Controls.Add(parameterLabel);
        }

        private void AddParameterControl(Parameter parameter, Panel container)
        {
            ParameterValueBuffer parameterValueBuffer = new ParameterValueBuffer()
            {
                Parameter = parameter,
                Control = null,
                NewValue = parameter.GetValue()
            };

            Control parameterControl = null;

            if (parameter.IsNumeric)
            {
                parameterControl = AddNumericParameterControls(parameterValueBuffer);
            }
            else if (parameter.IsBoolean)
            {
                parameterControl = AddBoolParameterControls(parameterValueBuffer);
            }
            else if (parameter.IsString)
            {
                parameterControl = AddStringParameterControls(parameterValueBuffer);
            }
            else if (parameter.IsEnum)
            {
                parameterControl = AddEnumParameterControls(parameterValueBuffer);
            }
            else
            {
                throw new InvalidOperationException();
            }

            parameterControl.Margin = new Padding(0, 0, 15, 0);

            container.Controls.Add(parameterControl);

            parameterValueBuffer.Control = parameterControl;

            this.inhibitEvents = true;
            RevertParameterValue(parameterValueBuffer);
            this.inhibitEvents = false;
        }

        private void AddParameterDescriptionLabel(Parameter parameter, Panel container)
        {
            if (!string.IsNullOrEmpty(parameter.Description))
            {
                Label descriptionLabel = new Label();
                descriptionLabel.Name = parameter.Field.Name + "_description";
                descriptionLabel.Text = parameter.Description;
                descriptionLabel.AutoSize = true;

                container.Controls.Add(descriptionLabel);
            }
        }

        private void RevertParameterValue(ParameterValueBuffer parameterValueBuffer)
        {
            Parameter parameter = parameterValueBuffer.Parameter;

            if (parameter.IsNumeric)
            {
                RevertNumericParameterValue(parameterValueBuffer);
            }
            else if (parameter.IsBoolean)
            {
                RevertBoolParameterValue(parameterValueBuffer);
            }
            else if (parameter.IsString)
            {
                RevertStringParameterValue(parameterValueBuffer);
            }
            else if (parameter.IsEnum)
            {
                RevertEnumParameterValue(parameterValueBuffer);
            }
        }

        private Control AddNumericParameterControls(ParameterValueBuffer parameterValueBuffer)
        {
            TextBox parameterTextBox = new TextBox();
            parameterTextBox.CausesValidation = true;
            parameterTextBox.Tag = parameterValueBuffer;
            parameterTextBox.TextChanged += new EventHandler(NumericParameterEdited);
            parameterTextBox.Validating += new CancelEventHandler(NumericParameterValidating);
            
            return parameterTextBox;
        }

        private void RevertNumericParameterValue(ParameterValueBuffer parameterValueBuffer)
        {
            TextBox parameterTextBox = parameterValueBuffer.Control as TextBox;

            parameterValueBuffer.NewValue = parameterValueBuffer.Parameter.GetValue();
            parameterTextBox.Text = parameterValueBuffer.NewValue.ToString();
        }

        private Control AddBoolParameterControls(ParameterValueBuffer parameterValueBuffer)
        {
            CheckBox parameterCheckBox = new CheckBox();
            parameterCheckBox.Tag = parameterValueBuffer;
            parameterCheckBox.CheckedChanged += new EventHandler(BoolParameterChanged);
            return parameterCheckBox;
        }

        private void RevertBoolParameterValue(ParameterValueBuffer parameterValueBuffer)
        {
            CheckBox parameterCheckBox = parameterValueBuffer.Control as CheckBox;

            parameterValueBuffer.NewValue = parameterValueBuffer.Parameter.GetValue();
            parameterCheckBox.Checked = (bool)parameterValueBuffer.NewValue;
        }

        private Control AddStringParameterControls(ParameterValueBuffer parameterValueBuffer)
        {
            if (parameterValueBuffer.Parameter.StringType == StringParameterType.Multiline)
            {
                Button editParameterButton = new Button();
                editParameterButton.Tag = parameterValueBuffer;
                editParameterButton.AutoSize = true;
                editParameterButton.Text = "Edit";
                editParameterButton.Click += new EventHandler(StringParameterEditButtonClick);
                return editParameterButton;
            }

            TextBox parameterTextBox = new TextBox();
            parameterTextBox.Tag = parameterValueBuffer;
            parameterTextBox.TextChanged += new EventHandler(StringParameterChanged);
            return parameterTextBox;
        }

        private void RevertStringParameterValue(ParameterValueBuffer parameterValueBuffer)
        {
            parameterValueBuffer.NewValue = parameterValueBuffer.Parameter.GetValue();

            if (parameterValueBuffer.Parameter.StringType != StringParameterType.Multiline)
            {
                TextBox parameterTextBox = parameterValueBuffer.Control as TextBox;

                parameterTextBox.Text = (string)parameterValueBuffer.NewValue;
            }
        }

        private Control AddEnumParameterControls(ParameterValueBuffer parameterValueBuffer)
        {
            ComboBox parameterValueComboBox = new ComboBox();
            parameterValueComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            parameterValueComboBox.Tag = parameterValueBuffer;
            parameterValueComboBox.Items.AddRange(parameterValueBuffer.Parameter.FieldType.GetEnumNames());
            parameterValueComboBox.SelectedIndexChanged += new EventHandler(EnumParameterValueChanged);
            return parameterValueComboBox;
        }

        private void RevertEnumParameterValue(ParameterValueBuffer parameterValueBuffer)
        {
            ComboBox parameterComboBox = parameterValueBuffer.Control as ComboBox;

            parameterValueBuffer.NewValue = parameterValueBuffer.Parameter.GetValue();
            parameterComboBox.SelectedItem = parameterValueBuffer.NewValue.ToString();
        }

        private void NumericParameterEdited(object sender, EventArgs e)
        {
            RaiseParameterValueEdited();
        }

        private void NumericParameterValidating(object sender, CancelEventArgs e)
        {
            TextBox parameterTextBox = (TextBox)sender;
            ParameterValueBuffer parameterValueBuffer = (ParameterValueBuffer)parameterTextBox.Tag;
            string stringValue = parameterTextBox.Text;

            var converter = TypeDescriptor.GetConverter(parameterValueBuffer.Parameter.FieldType);

            object value;

            try
            {
                value = converter.ConvertFromString(parameterTextBox.Text);
            }
            catch (Exception)
            {
                errorProvider.SetError(parameterTextBox, "Incorrect " + parameterValueBuffer.Parameter.TypeDescription);
                e.Cancel = true;
                return;
            }

            if (parameterValueBuffer.Parameter.MinimumValue != null && parameterValueBuffer.Parameter.MaximumValue != null)
            {
                if (((IComparable)value).CompareTo(parameterValueBuffer.Parameter.MinimumValue) < 0
                    || ((IComparable)value).CompareTo(parameterValueBuffer.Parameter.MaximumValue) > 0)
                {
                    errorProvider.SetError(parameterTextBox, "Value between '" + parameterValueBuffer.Parameter.MinimumValue + "' and '" + parameterValueBuffer.Parameter.MaximumValue + "' expected");
                    e.Cancel = true;
                    return;
                }
            }
            else if (parameterValueBuffer.Parameter.MinimumValue != null)
            {
                if (((IComparable)value).CompareTo(parameterValueBuffer.Parameter.MinimumValue) < 0)
                {
                    errorProvider.SetError(parameterTextBox, "Value greater than '" + parameterValueBuffer.Parameter.MinimumValue + "' expected");
                    e.Cancel = true;
                    return;
                }
            }
            else if (parameterValueBuffer.Parameter.MaximumValue != null)
            {
                if (((IComparable)value).CompareTo(parameterValueBuffer.Parameter.MaximumValue) > 0)
                {
                    errorProvider.SetError(parameterTextBox, "Value less than '" + parameterValueBuffer.Parameter.MaximumValue + "' expected");
                    e.Cancel = true;
                    return;
                }
            }

            errorProvider.SetError(parameterTextBox, null);

            parameterValueBuffer.NewValue = value; 
        }

        private void BoolParameterChanged(object sender, EventArgs e)
        {
            CheckBox parameterCheckBox = (CheckBox)sender;
            ParameterValueBuffer parameterValueBuffer = (ParameterValueBuffer)parameterCheckBox.Tag;

            parameterValueBuffer.NewValue = parameterCheckBox.Checked;

            RaiseParameterValueEdited();
        }

        private void StringParameterChanged(object sender, EventArgs e)
        {
            TextBox parameterTextBox = (TextBox)sender;
            ParameterValueBuffer parameterValueBuffer = (ParameterValueBuffer)parameterTextBox.Tag;

            parameterValueBuffer.NewValue = parameterTextBox.Text;

            RaiseParameterValueEdited();
        }

        private void StringParameterEditButtonClick(object sender, EventArgs e)
        {
            ParameterValueBuffer parameterValueBuffer = (ParameterValueBuffer)((Button)sender).Tag;

            new StringParameterValueEditingWindow(parameterValueBuffer).ShowDialog();

            RaiseParameterValueEdited();
        }

        private void EnumParameterValueChanged(object sender, EventArgs e)
        {
            ComboBox parameterComboBox = (ComboBox)sender;
            ParameterValueBuffer parameterValueBuffer = (ParameterValueBuffer)parameterComboBox.Tag;

            parameterValueBuffer.NewValue = (Enum)(Enum.Parse(parameterValueBuffer.Parameter.FieldType, parameterComboBox.SelectedItem.ToString()));

            RaiseParameterValueEdited();
        }

        private void RaiseParameterValueEdited()
        {
            if (ParameterValueEdited != null && !this.inhibitEvents)
            {
                ParameterValueEdited(this, EventArgs.Empty);
            }
        }

        private IParametrizedObject parametrizedObject;
        private List<ParameterControl> innerParameterControls;
        private bool inhibitEvents;
    }
}
