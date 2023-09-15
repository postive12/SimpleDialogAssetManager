namespace DialogSystem.Runtime.Dialogs.Interfaces
{
    public interface ISpeaker : IDialogTarget
    {
        public void Speak(string dialog);
        public void EndSpeak();
    }
}