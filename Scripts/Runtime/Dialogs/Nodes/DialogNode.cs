using DialogSystem.Structure;
using UnityEngine;

namespace DialogSystem.Nodes
{
    public class DialogNode : DialogBaseNode {
        public Dialog Line => _line;
        [SerializeField] private Dialog _line;
        public override DialogBaseNode GetNext() 
        {
            //Debug.Log("DialogNode : GetNext");
            //if next is null log error
            var port = GetOutputPort("Next");
            if (port.ConnectionCount == 0) {
                return null;
            }
            return port?.GetConnection(0)?.node as DialogBaseNode;
        }
    }
}
