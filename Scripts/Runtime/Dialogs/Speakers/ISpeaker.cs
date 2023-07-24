namespace DialogSystem.Runtime.Dialogs.Speakers
{
    public interface ISpeaker
    {
        public string SpeakerId { get; protected set; }
        public void Speak(string dialog);
    }
}