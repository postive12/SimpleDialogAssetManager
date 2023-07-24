namespace DialogSystem.Runtime.Dialogs.EventInvokers
{
    public interface IEventInvoker
    {
        public string InvokerId { get;protected set; }
        public void Invoke(string eventName);
    }
}