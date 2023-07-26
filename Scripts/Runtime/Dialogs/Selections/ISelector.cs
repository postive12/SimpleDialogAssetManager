using System.Collections.Generic;
using DialogSystem.Nodes;
using DialogSystem.Structure;

namespace DialogSystem.Runtime.Dialogs.Selections
{
    public interface ISelector
    {
        public string SelectorTag { get; protected set; }
        public void CreateSelections(List<DialogContent> selections, DialogBranchNode node);
        public void Select(int index);
    }
}