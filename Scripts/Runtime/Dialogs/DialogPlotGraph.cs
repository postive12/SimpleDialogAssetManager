using DialogSystem.Nodes;
using DialogSystem.Structure;
using UnityEngine;
using XNode;

namespace DialogSystem.Runtime.Dialogs
{
    [CreateAssetMenu(menuName = "DialogSystem/DialogPlotGraph", fileName = "DialogPlotGraph")]
    public class DialogPlotGraph : NodeGraph
    {
        public bool IsPlotEnd => (CurrentNode == null && !_isStart);
        //Need to remake start and end point
        public DialogBaseNode CurrentNode { get; private set; } = null;
        private bool _isStart = false;
        public void PlayPlot() {
            _isStart = true;
        }
        public DialogBaseNode Next() {
            if (_isStart) {
                _isStart = false;
                CurrentNode = (DialogBaseNode) nodes[0];
                return CurrentNode;
            }
            if (CurrentNode == null) return null;
            if (!CurrentNode.CanGoNext()) return CurrentNode;
            CurrentNode = CurrentNode.GetNext();
            return CurrentNode;
        }
        public bool IsNextAvailable() {
            return _isStart||(CurrentNode != null && CurrentNode.CanGoNext());
        }
    }
}