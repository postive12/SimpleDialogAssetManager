namespace DialogSystem.Runtime.Dialogs.Interfaces
{
    public interface IDialogTarget
    {
        public string TargetTag { get; protected set; }
        public string GetTargetTag();
    }
}