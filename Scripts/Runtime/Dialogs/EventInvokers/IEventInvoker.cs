namespace DialogSystem.Runtime.Dialogs.EventInvokers
{
    public interface IEventInvoker
    {
        public string InvokerTag { get;protected set; }
        public void Invoke(string eventName);
    }
}