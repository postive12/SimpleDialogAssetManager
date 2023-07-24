using System;
using System.Collections.Generic;
using DialogSystem.Attributes;
using UnityEngine;

#if HAS_LOCALIZATION
    using UnityEngine.Localization;
#endif
namespace DialogSystem.Structure
{
    [Serializable]
    public class Dialog
    {
        [TagSelector] public string SpeakerId;
        public List<DialogEvent> DialogEvents;
        public DialogContent DialogContent;
        // public string Content {
        //     get {
        //         //If engine has localization, load from string reference
        //         #if HAS_LOCALIZATION
        //             PreloadContent();
        //         #endif
        //         //If engine does not have localization, load from content
        //         return _content;
        //     }
        // }
        // [SerializeField][TextArea] private string _content;
        // //If engine has localization add string reference and preload function
        // #if HAS_LOCALIZATION
        //     public LocalizedString StringReference;
        //     public void PreloadContent()
        //     {
        //         var content = StringReference.GetLocalizedString();
        //         _content = content;
        //     }
        // #endif

    }
}