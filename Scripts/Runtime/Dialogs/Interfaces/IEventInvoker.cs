namespace DialogSystem.Runtime.Dialogs.Interfaces
{
    public interface IEventInvoker : IDialogTarget
    {
        public void Invoke(string eventName);
    }
}