using DialogSystem.Dialogs.Components.Managers;
using UnityEngine;

namespace DialogSystem.Nodes
{
    public class DialogStartNode : DialogBaseNode
    {
        [HideInInspector] public DialogBaseNode Child = null;
        public override DialogBaseNode GetNext() => Child;
        public override bool IsNextExist() => Child != null;
        public override bool CanGetNext() => IsNextExist();
        public override void Play(IDialogManager target) { }
    }
}