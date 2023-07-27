using DialogSystem.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace DialogSystem.Runtime.Dialogs.EventInvokers
{
    public class DialogEventInvoker : MonoBehaviour,IEventInvoker
    {
        string IEventInvoker.InvokerTag {
            get {
                return _invokerTag;
            }
            set {
                _invokerTag = value;
            }
        }
        [DialogTagSelector][SerializeField]private string _invokerTag = "NONE";
        [SerializeField] private UnityEvent<string> _onInvokeEvent;
        private void Awake() {
            DialogManager.AddEventInvoker(this);
        }

        public void Invoke(string eventName) {
            _onInvokeEvent?.Invoke(eventName);
        }
    }
}