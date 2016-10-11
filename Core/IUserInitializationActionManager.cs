namespace Core
{
    public interface IUserInitializationActionManager
    {
        bool AskUserToPerformInitializationAction(string instructionsLabelText, IUserInitializationActionPredicate userInitializationActionPredicate);
    }
}
