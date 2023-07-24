using System;
using System.Collections.Generic;
using DialogSystem.Attributes;

namespace DialogSystem.Structure
{
    [Serializable]
    public class DialogEvent
    {
        [TagSelector]public List<string> EventTargets;
        public string EventName;
    }
}