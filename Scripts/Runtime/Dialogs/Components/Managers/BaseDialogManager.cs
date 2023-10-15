using DialogSystem.Nodes;
using UnityEngine;

namespace DialogSystem.Dialogs.Components.Managers
{
    public class BaseDialogManager : MonoBehaviour, IDialogManager
    {

        public void SelectDialogPlot(string eventName)
        {
            throw new System.NotImplementedException();
        }
        public void RequestDialog()
        {
            throw new System.NotImplementedException();
        }
        public void Play(DialogNode node)
        {
            throw new System.NotImplementedException();
        }
        public void Play(DialogBranchNode node)
        {
            throw new System.NotImplementedException();
        }
    }
}