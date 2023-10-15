using System.Collections.Generic;
using DialogSystem.Dialogs.Components.Managers;
using DialogSystem.Nodes;
using DialogSystem.Structure;

namespace DialogSystem.Runtime.Dialogs.Interfaces
{
    public interface ISelector : IDialogTarget
    {
        public void CreateSelections(List<DialogContent> selections, DialogBranchNode node, IDialogManager currentDialogManager);
        public void Select(int index);
    }
}