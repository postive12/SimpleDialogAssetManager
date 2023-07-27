namespace DialogSystem.Runtime.Dialogs.Speakers
{
    public interface ISpeaker
    {
        public string SpeakerTag { get; protected set; }
        public void Speak(string dialog);
        public void EndSpeak();
    }
}