using System;
using UnityEngine;
#if HAS_LOCALIZATION
    using UnityEngine.Localization;
#endif

namespace DialogSystem.Structure
{
    [Serializable]
    public class DialogContent
    {
        public string Content {
            get {
                #if HAS_LOCALIZATION
                    //If engine has localization, load from string reference
                    return StringReference.GetLocalizedString();
                #else
                    //If engine does not have localization, load from content
                    return _content;
                #endif
                
            }
        }
        //If engine has localization add string reference and preload function
        #if HAS_LOCALIZATION
            [SerializeField] private LocalizedString StringReference;
        #else
            [SerializeField][TextArea] private string _content;
        #endif
    }
}