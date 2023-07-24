using DialogSystem.Structure;
using XNode;

namespace DialogSystem.Nodes
{
    [NodeWidth(width:500)]
    public abstract class DialogBaseNode : Node
    {
        public DialogType Type { get; protected set; } = DialogType.DIALOG;
        [Input(backingValue = ShowBackingValue.Never)] public DialogBaseNode Previous;
        [Output(backingValue = ShowBackingValue.Never)] public DialogBaseNode Next;
        public abstract DialogBaseNode GetNext();
        public override object GetValue(NodePort port) {
            if (port.fieldName == "Next") return GetInputValue<DialogBaseNode>("Previous", Previous);
            else return null;
        }
    }
}