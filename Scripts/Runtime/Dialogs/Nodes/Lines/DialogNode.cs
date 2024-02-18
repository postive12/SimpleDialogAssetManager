using DialogSystem.Dialogs.Components.Managers;
using DialogSystem.Structure;
using UnityEngine;

namespace DialogSystem.Nodes.Lines
{
    public class DialogNode : SingleChildNode {
        public Dialog Line => _line;
        [SerializeField] private Dialog _line;
        public override DialogBaseNode GetNext() => Child;
        public override bool IsNextExist() => Child != null;
        public override bool CanGetNext() => IsNextExist();
        public override void Play(IDialogManager target) {
            target.Play(this);
        }
    }
}
