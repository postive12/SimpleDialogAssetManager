using DialogSystem.Attributes;
using DialogSystem.Dialogs.Components.Managers;
using DialogSystem.Runtime.Dialogs.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace DialogSystem.Dialogs.Components
{
    public class DialogSpeaker : DialogTargetComponent, ISpeaker
    {
        [SerializeField] private bool _disableRequestWhenSpeaking = false;
        [SerializeField] private bool _clearTextWhenEnd = false;
        [SerializeField] private UnityEvent<string> _onReceiveDialog;
        [SerializeField] private UnityEvent _onStartDialog;
        [SerializeField] private UnityEvent _onEndDialog;
        public void Speak(string dialog) {
            if (_disableRequestWhenSpeaking) {
                DialogManager.Instance.IsStopRequest = true;
            }
            if (_onStartDialog.GetPersistentEventCount() <= 0) {
                gameObject.SetActive(true);
            } else {
                _onStartDialog?.Invoke();
            }
            _onReceiveDialog?.Invoke(dialog);
        }
        public void EndSpeak() {
            if (_clearTextWhenEnd) {
                _onReceiveDialog?.Invoke(string.Empty);
            }
            if (_onEndDialog.GetPersistentEventCount() <= 0) {
                gameObject.SetActive(false);
                return;
            }
            _onEndDialog?.Invoke();
        }
        /// <summary>
        /// Enable request
        /// </summary>
        public void EnableRequest() {
            DialogManager.Instance.IsStopRequest = false;
        }
    }
}