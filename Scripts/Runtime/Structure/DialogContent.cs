using System;
using UnityEngine;
using UnityEngine.Localization;

namespace DialogSystem.Structure
{
    [Serializable]
    public class DialogContent
    {
        public string Content {
            get {
                //If engine has localization, load from string reference
                #if HAS_LOCALIZATION
                PreloadContent();
                #endif
                //If engine does not have localization, load from content
                return _content;
            }
        }
        [SerializeField][TextArea] private string _content;
        //If engine has localization add string reference and preload function
        #if HAS_LOCALIZATION
            public LocalizedString StringReference;
            public void PreloadContent()
            {
                try {
                    var content = StringReference.GetLocalizedString();
                    _content = content;
                }
                catch (Exception e) {
                    _content = "";
                }
            }
        #endif
    }
}