﻿using System.Collections.Generic;
using DialogSystem.Dialogs.Components.Managers;
using UnityEngine;
#if HAS_NEW_INPUT
using UnityEngine.InputSystem;
#endif

namespace DialogSystem.Dialogs.Components
{
    public class IndependentDialogRequester : MonoBehaviour
    {
        [SerializeField] private IndependentDialogManager _independentDialogManager = null;
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
            if (!_independentDialogManager) {
                Debug.LogError("IndependentDialogManager is not implemented!");
                return;
            }
            _independentDialogManager.RequestDialog();
        }
    }
}