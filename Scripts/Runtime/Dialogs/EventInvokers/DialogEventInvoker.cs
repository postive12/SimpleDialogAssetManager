using DialogSystem.Attributes;
using DialogSystem.Scripts.Runtime.Dialogs;
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
        [TagSelector][SerializeField]private string _invokerTag = "NONE";
        [SerializeField] private UnityEvent<string> _onInvokeEvent;
        private void Start() {
            DialogManager.Instance.AddEventInvoker(this);
        }

        public void Invoke(string eventName) {
            _onInvokeEvent?.Invoke(eventName);
        }
    }
}