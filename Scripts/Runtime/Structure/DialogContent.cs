using System;
using System.Collections.Generic;
using DialogSystem.Runtime.Attributes;
using UnityEngine;

#if HAS_LOCALIZATION
using UnityEngine.Localization;
#endif

namespace DialogSystem.Runtime.Structure.ScriptableObjects
{
    [Serializable]
    public class DialogContent
    {
        public string Content {
            #if HAS_LOCALIZATION
            get => _stringReference.GetLocalizedString();
            #else
            get => _content;
            #endif
        }
        //If engine has localization add string reference and preload function
        #if HAS_LOCALIZATION
            [SerializeField] private LocalizedString _stringReference;
        #else
            [SerializeField][TextArea] private string _content;
        #endif
    }
}