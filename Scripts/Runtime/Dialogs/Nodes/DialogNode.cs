using DialogSystem.Structure;
using UnityEngine;

namespace DialogSystem.Nodes
{
    public class DialogNode : DialogBaseNode {
        public Dialog Line;
        public override DialogBaseNode GetNext() {
            
            //if next is null log error
            var port = GetOutputPort("Next");
            var connections = port.GetConnections();
            if (connections.Count == 0) return null;
            return connections[0].node as DialogBaseNode;
        }
    }
}
