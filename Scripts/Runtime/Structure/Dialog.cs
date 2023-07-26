using System;
using System.Collections.Generic;
using DialogSystem.Attributes;
using UnityEngine;
namespace DialogSystem.Structure
{
    [Serializable]
    public class Dialog
    {
        [TagSelector] public string SpeakerTag;
        public List<DialogEvent> DialogEvents;
        public DialogContent DialogContent;
    }
}