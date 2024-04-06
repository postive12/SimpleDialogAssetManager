using DialogSystem.Runtime.Attributes;
using DialogSystem.Dialogs.Components.Managers;
using DialogSystem.Runtime.Structure.ScriptableObjects;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace DialogSystem.Dialogs.Components
{
    public class DialogSpeaker : DialogTargetComponent
    {
        [Header("Dialog Settings")]
        [SerializeField] private bool _disableRequestWhenSpeaking = false;
        [SerializeField] private bool _clearTextWhenEnd = false;
        [SerializeField] private UnityEvent<string> _onReceiveDialog;
        [SerializeField] private UnityEvent _onStartDialog;
        [SerializeField] private UnityEvent _onOtherSpeakerSpeak;
        [SerializeField] private UnityEvent _onEndPlot;
        [Header("Audio Settings")]
        [Tooltip("Enable request delay after audio clip length")]
        [SerializeField] private bool _useAutoRequest = false;
        [SerializeField] private float _autoRequestDelay = 0.3f;
        private AudioSource _audioSource;
        public void Speak(DialogContent dialogContent) {
            if (_disableRequestWhenSpeaking) {
                DialogManager.Instance.IsStopRequest = true;
            }
            if (_onStartDialog.GetPersistentEventCount() <= 0) {
                gameObject.SetActive(true);
            } else {
                _onStartDialog?.Invoke();
            }
            _onReceiveDialog?.Invoke(dialogContent.Content);
            if (dialogContent.Audio == null) {
                _audioSource?.Stop();
                return;
            }
            
            _audioSource = _audioSource ? _audioSource : gameObject.AddComponent<AudioSource>();
            if (_audioSource.isPlaying) {
                _audioSource.Stop();
            }
            _audioSource.clip = dialogContent.Audio;
            _audioSource.outputAudioMixerGroup = dialogContent.MixerGroup;
            _audioSource.Play();
            if (_useAutoRequest) {
                Invoke(nameof(RequestNext), dialogContent.Audio.length + _autoRequestDelay);
            }
        }
        public void OnOtherSpeakerSpeak() {
            _onOtherSpeakerSpeak?.Invoke();
            if (_audioSource == null) return;
            _audioSource.Stop();
        }
        public void OnEndPlot() {
            if (_clearTextWhenEnd) {
                _onReceiveDialog?.Invoke(string.Empty);
            }
            if (_onEndPlot.GetPersistentEventCount() <= 0) {
                gameObject.SetActive(false);
                return;
            }
            _onEndPlot?.Invoke();
        }
        public void RequestNext() {
            DialogManager.Instance.Play();
        }
        /// <summary>
        /// Enable request
        /// </summary>
        public void EnableRequest() {
            DialogManager.Instance.IsStopRequest = false;
        }
    }
}