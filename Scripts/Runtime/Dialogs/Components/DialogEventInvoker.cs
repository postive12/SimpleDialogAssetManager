using DialogSystem.Attributes;
using DialogSystem.Dialogs.Components.Managers;
using DialogSystem.Runtime.Dialogs.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace DialogSystem.Dialogs.Components
{
    public class DialogEventInvoker : DialogTargetComponent ,IEventInvoker
    {
        [SerializeField] private UnityEvent<string> _onInvokeEvent;
        public void Invoke(string eventName) {
            _onInvokeEvent?.Invoke(eventName);
        }

        
    }
}