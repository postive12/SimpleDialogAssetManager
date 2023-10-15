using System.Collections.Generic;
using DialogSystem.Dialogs.Components.Managers;
using UnityEngine;
using UnityEngine.Serialization;
#if HAS_NEW_INPUT
using UnityEngine.InputSystem;
#endif

namespace DialogSystem.Dialogs.Components
{
    public class StandAloneDialogRequester : MonoBehaviour
    {
        [SerializeField] private StandAloneDialogManager _standAloneDialogManager = null;
        #if HAS_NEW_INPUT
        [SerializeField] private List<Key> _requestKeyboardKey = new List<Key>();
        #else
            [SerializeField] private List<KeyCode> _requestKeyboardKey = new List<KeyCode>();
        #endif
        
        private void Update()
        {
            #if HAS_NEW_INPUT
            foreach (var key in _requestKeyboardKey) {
                if (Keyboard.current[key].wasPressedThisFrame) {
                    RequestDialog();
                }
            }
            #else
                foreach (var keyCode in _requestKeyboardKey) {
                    if (Input.GetKeyDown(keyCode)) {
                        RequestDialog();
                    }
                }
            #endif
        }
        public void RequestDialog()
        {
            if (!_standAloneDialogManager) {
                Debug.LogError("StandAloneDialogManager is not implemented!");
                return;
            }
            _standAloneDialogManager.RequestDialog();
        }
    }
}