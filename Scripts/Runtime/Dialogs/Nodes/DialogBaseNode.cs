using System;
using DialogSystem.Dialogs.Components.Managers;
using DialogSystem.Structure;
using UnityEngine;
using UnityEngine.Serialization;
using XNode;

namespace DialogSystem.Nodes
{
    [NodeWidth(width:500)]
    public abstract class DialogBaseNode : Node
    {
        public DialogType Type { get; protected set; } = DialogType.DIALOG;
        [Input(backingValue = ShowBackingValue.Never)] public DialogBaseNode Previous;
        public bool IsEndPast {
            get {
                return _isEndPast;
            }
        }
        [SerializeField] private bool _isEndPast = true;
        public abstract DialogBaseNode GetNext();
        public abstract void Play(IDialogManager target);
        public virtual void ResetNode() { }
        public virtual bool IsNextExist() {
            var port = GetOutputPort("Next");
            return port.ConnectionCount > 0;
        }
        public virtual bool CanGetNext() {
            return IsNextExist();
        }
        public override object GetValue(NodePort port) {
            if (port.fieldName == "Next") return GetInputValue<DialogBaseNode>("Previous", Previous);
            else return null;
        }
    }
}