using DialogSystem.Attributes;
using DialogSystem.Scripts.Runtime.Dialogs;
using UnityEngine;
using UnityEngine.Events;

namespace DialogSystem.Runtime.Dialogs.Speakers
{
    public class DialogSpeaker : MonoBehaviour, ISpeaker
    {
        string ISpeaker.SpeakerTag {
            get {
                return _speakerTag;
            }
            set {
                _speakerTag = value;
            }
        }
        [DialogTagSelector][SerializeField] private string _speakerTag = "NONE";
        [SerializeField] private UnityEvent<string> _onReceiveDialog;
        [SerializeField] private UnityEvent _onStartDialog;
        [SerializeField] private UnityEvent _onEndDialog;
        private void Awake() {
            DialogManager.AddSpeaker(this);
        }

        public void Speak(string dialog) {
            if (_onStartDialog.GetPersistentEventCount() <= 0) {
                gameObject.SetActive(true);
            } else {
                _onStartDialog?.Invoke();
            }
            _onReceiveDialog?.Invoke(dialog);
        }
        public void EndSpeak() {
            if (_onEndDialog.GetPersistentEventCount() <= 0) {
                gameObject.SetActive(false);
                return;
            }
            _onEndDialog?.Invoke();
        }
    }
}