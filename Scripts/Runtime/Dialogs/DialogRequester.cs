using System.Collections.Generic;
using DialogSystem.Runtime.Dialogs;
using UnityEngine;
#if HAS_NEW_INPUT
    using UnityEngine.InputSystem;
#endif



namespace DialogSystem.Dialogs
{
    public class DialogRequester : MonoBehaviour
    {
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
            DialogManager.Instance.RequestDialog();
        }
    }
}