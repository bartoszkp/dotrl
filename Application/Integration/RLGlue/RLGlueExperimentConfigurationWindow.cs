using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using Core;

namespace Application.Integration.RLGlue
{
    public partial class RLGlueExperimentConfigurationWindow : Form
    {
        public System.Net.IPAddress Host
        {
            get
            {
                return this.ipAddress;
            }
        }

        public int PortNumber
        {
            get
            {
                return this.portNumber;
            }
        }

        // TODO: ambiguity between 'System.Type of Component and ComponentType class - change it to ProblemType?
        public Type ComponentType
        {
            get
            {
                return this.componentTabControl.SelectedTab == this.agentsTabPage
                    ? (Type)this.agentListView.SelectedItems[0].Tag
                    : (Type)this.environmentListView.SelectedItems[0].Tag;
            }
        }

        public RLGlueExperimentConfigurationWindow()
        {
            InitializeComponent();

            this.discreteStateDiscreteActionTabPage.Tag = new ComponentType(typeof(int), typeof(int));
            this.continuousStateDiscreteAction.Tag = new ComponentType(typeof(double), typeof(int));
            this.continuousStateContinuousAction.Tag = new ComponentType(typeof(double), typeof(double));

            RebuildLists();
        
            // Makes validation events fire even when nothing on the form was clicked since it started
            this.hostTextBox.Select();
            this.portNumberTextBox.Select();
            this.componentTabControl.Select();
        }

        private void RebuildLists()
        {
            this.agentListView.Items.Clear();
            this.environmentListView.Items.Clear();

            this.agentListView.Items.AddRange(AgentRegistry
                .GetAgents(GetCurrentComponentType())
                .Select(type => new ListViewItem(type.Name) { Tag = type })
                .ToArray());
            this.environmentListView.Items.AddRange(EnvironmentRegistry
                .GetEnvironments(GetCurrentComponentType())
                .Select(type => new ListViewItem(type.Name) { Tag = type })
                .ToArray());
        }

        private ComponentType GetCurrentComponentType()
        {
            return (ComponentType)this.componentTypeTabControl.SelectedTab.Tag;
        }

        private void ComponentTypeTabControlSelectedIndexChanged(object sender, EventArgs e)
        {
            RebuildLists();
        }

        private void HostTextBoxValidating(object sender, CancelEventArgs e)
        {
            if (!System.Net.IPAddress.TryParse(this.hostTextBox.Text, out this.ipAddress))
            {
                errorProvider.SetError(this.hostTextBox, "Invalid IPv4 address");
                e.Cancel = true;
            }
        }

        private void PortNumberTextBoxValidating(object sender, CancelEventArgs e)
        {
            if (!int.TryParse(this.portNumberTextBox.Text, out this.portNumber)
                || this.portNumber < 256 || this.portNumber > 65535)
            {
                errorProvider.SetError(this.portNumberTextBox, "Invalid port number");
                e.Cancel = true;
            }
        }

        private void ComponentTabControlValidating(object sender, CancelEventArgs e)
        {
            if ((this.componentTabControl.SelectedTab == this.agentsTabPage
                && this.agentListView.SelectedItems.Count != 1)
                || (this.componentTabControl.SelectedTab == this.environmentsTabPage
                && this.environmentListView.SelectedItems.Count != 1))
            {
                errorProvider.SetError(this.componentTabControl, "No component selected");
                e.Cancel = true;
            }
        }

        private System.Net.IPAddress ipAddress;
        private int portNumber;
    }
}
