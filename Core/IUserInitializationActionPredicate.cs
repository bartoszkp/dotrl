namespace Core
{
    public interface IUserInitializationActionPredicate
    {
        bool UserInitializationActionCompleted();
        void CancelPressed();
    }
}
