using DialogSystem.Dialogs.Components.Managers;
using DialogSystem.Nodes;
using DialogSystem.Structure;
using UnityEngine;
using XNode;

namespace DialogSystem.Runtime.Dialogs
{
    [CreateAssetMenu(menuName = "DialogSystem/DialogPlotGraph", fileName = "DialogPlotGraph")]
    public class DialogPlotGraph : NodeGraph
    {
        public bool IsStart => _isStart;
        [HideInInspector] public bool IsPlotEnd = true;
        //Need to remake start and end point
        public DialogBaseNode CurrentNode { get; private set; } = null;
        private bool _isStart = false;
        public void PlayPlot() {
            _isStart = true;
            IsPlotEnd = false;
        }
        public void Next(IDialogManager target) {
            if (IsPlotEnd) {
                return;
            }
            if (_isStart) {
                _isStart = false;
                CurrentNode = (DialogBaseNode)nodes[0];
                CurrentNode.Play(target);
            } else {
                if (CurrentNode.CanGetNext()) {
                    var lastNode = CurrentNode;
                    var nextNode = CurrentNode.GetNext();
                    lastNode.ResetNode();
                    nextNode.ResetNode();
                    CurrentNode = nextNode;
                    CurrentNode.Play(target);
                }
            }
            //If can load next node, load next node
            if (!CurrentNode.IsNextExist()) {
                //Debug.LogWarning("Can't go next");
                IsPlotEnd = true;
                CurrentNode = null;
                return;
            }
            
        }
    }
}