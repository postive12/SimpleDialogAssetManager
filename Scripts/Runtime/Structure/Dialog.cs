using System;
using System.Collections.Generic;
using DialogSystem.Attributes;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace DialogSystem.Structure
{
    [Serializable]
    public class Dialog
    {
        public List<DialogEvent> DialogEvents => _dialogEvents;
        public DialogContent DialogContent => _dialogContent;
        #if ODIN_INSPECTOR
        private static IEnumerable<string> GetEventTargets() {
            List<string> tagList = new List<string>();
            tagList.Add("NONE");
            tagList.AddRange(UnityEditorInternal.InternalEditorUtility.tags);
            tagList.AddRange(DialogTagSelectorPropertyDrawer.CustomTags);
            return tagList;
        }
        [ValueDropdown("GetEventTargets",IsUniqueList = false,DrawDropdownForListElements = true)]
        #else
        [DialogTagSelector]
        #endif
        public string SpeakerTag;
        [SerializeField] private List<DialogEvent> _dialogEvents;
        [SerializeField] private DialogContent _dialogContent;
    }
}