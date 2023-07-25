using DialogSystem.Nodes;
using DialogSystem.Structure;
using UnityEngine;
using XNode;

namespace DialogSystem.Scripts.Runtime.Dialogs
{
    [CreateAssetMenu(menuName = "DialogSystem/DialogPlotGraph", fileName = "DialogPlotGraph")]
    public class DialogPlotGraph : NodeGraph
    {
        public bool IsPlotEnd => CurrentNode == null;
        public DialogBaseNode CurrentNode { get; private set; } = null;
        public void PlayPlot()
        {
            CurrentNode = (DialogBaseNode) nodes[0];
        }
        public DialogBaseNode Next(int index = -1) {
            if (CurrentNode == null) return null;
            if (index == -1) {
                CurrentNode = CurrentNode.GetNext();
                return CurrentNode;
            }
            //Check is the dialog is a branch
            if (CurrentNode.Type != DialogType.BRANCH) return CurrentNode;
            
            //If dialog is a branch, set the index and get the next node
            var branch = CurrentNode as DialogBranchNode;
            if (branch == null) return null;
            branch.SelectIndex = index;
            CurrentNode = branch.GetNext();
            return CurrentNode;
        }
    }
}