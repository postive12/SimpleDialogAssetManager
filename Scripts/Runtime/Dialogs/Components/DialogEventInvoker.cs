using DialogSystem.Attributes;
using DialogSystem.Dialogs.Components.Managers;
using DialogSystem.Runtime.Dialogs.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace DialogSystem.Dialogs.Components
{
    public class DialogEventInvoker : MonoBehaviour,IEventInvoker
    {
        
        [DialogTagSelector][SerializeField]private string _invokerTag = "NONE";
        [SerializeField] private UnityEvent<string> _onInvokeEvent;
        string IDialogTarget.TargetTag {
            get {
                return _invokerTag;
            }
            set {
                _invokerTag = value;
            }
        }
        public string GetTargetTag() => _invokerTag;
        private void Awake() {
            DialogManager.AddEventInvoker(this);
        }

        public void Invoke(string eventName) {
            _onInvokeEvent?.Invoke(eventName);
        }

        
    }
}