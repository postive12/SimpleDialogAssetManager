using System;
using System.Collections.Generic;
using DialogSystem.Attributes;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif
namespace DialogSystem.Structure
{
    [Serializable]
    public class DialogEvent
    {
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
        public List<string> EventTargets;
        
        public string EventName;
    }
}