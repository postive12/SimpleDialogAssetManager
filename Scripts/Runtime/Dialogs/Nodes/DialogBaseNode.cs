using System;
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
        [Output(backingValue = ShowBackingValue.Never)] public DialogBaseNode Next;
        public bool IsEndPast {
            get {
                return _isEndPast;
            }
        }
        [SerializeField] private bool _isEndPast = true;
        public abstract DialogBaseNode GetNext();
        public virtual void ResetNode() { }
        public virtual bool CanGoNext() { return true; }
        public override object GetValue(NodePort port) {
            if (port.fieldName == "Next") return GetInputValue<DialogBaseNode>("Previous", Previous);
            else return null;
        }
    }
}