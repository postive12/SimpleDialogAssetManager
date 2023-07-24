using DialogSystem.Attributes;
using DialogSystem.Scripts.Runtime.Dialogs;
using UnityEngine;
using UnityEngine.Events;

namespace DialogSystem.Runtime.Dialogs.EventInvokers
{
    public class DialogEventInvoker : MonoBehaviour,IEventInvoker
    {
        string IEventInvoker.InvokerId {
            get {
                return _invokerId;
            }
            set {
                _invokerId = value;
            }
        }
        [TagSelector][SerializeField]private string _invokerId = "NONE";
        [SerializeField] private UnityEvent<string> _onInvokeEvent;
        private void Start() {
            DialogManager.Instance.AddEventInvoker(this);
        }

        public void Invoke(string eventName) {
            _onInvokeEvent?.Invoke(eventName);
        }
    }
}