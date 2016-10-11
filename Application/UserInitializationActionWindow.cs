using System;
using System.Windows.Forms;
using Core;

namespace Application
{
    public partial class UserInitializationActionWindow : Form, IUserInitializationActionManager
    {
        public UserInitializationActionWindow()
        {
            InitializeComponent();
        }

        public bool AskUserToPerformInitializationAction(string instructionsLabelText, IUserInitializationActionPredicate userInitializationActionPredicate)
        {
            this.instructionsLabel.Text = instructionsLabelText;

            this.userInitializationActionPredicate = userInitializationActionPredicate;
            this.timer.Start();
            return this.ShowDialog() == DialogResult.OK;
        }

        private void CancelButtonClick(object sender, EventArgs e)
        {
            this.userInitializationActionPredicate.CancelPressed();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (this.userInitializationActionPredicate.UserInitializationActionCompleted())
            {
                this.DialogResult = DialogResult.OK;
            }
        }

        private IUserInitializationActionPredicate userInitializationActionPredicate;
    }
}
