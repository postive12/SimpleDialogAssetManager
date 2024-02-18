using DialogSystem.Nodes;
using DialogSystem.Nodes.Branches;
using DialogSystem.Nodes.Lines;

namespace DialogSystem.Dialogs.Components.Managers
{
    public interface IDialogManager
    {
        void SelectDialogPlot(string eventName);
        void RequestDialog();
        void Play(DialogNode node);
        void Play(DialogBranchNode node);
    }
}