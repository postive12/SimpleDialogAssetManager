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
        [TagSelector][SerializeField] private string _speakerTag = "NONE";
        [SerializeField] private UnityEvent<string> _onReceiveDialog;
        private void Start() {
            DialogManager.Instance.AddSpeaker(this);
        }

        public void Speak(string dialog) {
            _onReceiveDialog?.Invoke(dialog);
        }
    }
}