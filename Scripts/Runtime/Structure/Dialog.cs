using System;
using System.Collections.Generic;
using DialogSystem.Attributes;
using UnityEngine;

namespace DialogSystem.Structure
{
    [Serializable]
    public class Dialog
    {
        public List<DialogEvent> DialogEvents => _dialogEvents;
        public DialogContent DialogContent => _dialogContent;
        [DialogTagSelector] public string SpeakerTag;
        [SerializeField] private List<DialogEvent> _dialogEvents;
        [SerializeField] private DialogContent _dialogContent;
    }
}