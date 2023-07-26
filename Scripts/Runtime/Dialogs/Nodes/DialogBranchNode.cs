using System.Collections.Generic;
using DialogSystem.Structure;
using UnityEngine;
using XNode;

namespace DialogSystem.Nodes
{
    public class DialogBranchNode : DialogBaseNode
    {
        public string SelectorTag = "Selections";
        public int SelectIndex = 0;
        [Output(dynamicPortList = true)] public List<DialogContent> Selections;
        protected override void Init()
        {
            base.Init();
            Type = DialogType.BRANCH;
        }
        public override DialogBaseNode GetNext()
        {
            if (SelectIndex >= Selections.Count) return null;
            if (SelectIndex < 0) return null;
            NodePort port = GetOutputPort("Selections");
            var connections = port.GetConnections();
            if (connections.Count == 0) return null;
            return connections[SelectIndex].node as DialogBaseNode;
        }
    }
}