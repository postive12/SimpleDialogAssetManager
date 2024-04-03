using DialogSystem.Dialogs.Components.Managers;
using UnityEngine;

namespace DialogSystem.Dialogs.Components
{
    public class DialogPlotRequester : MonoBehaviour
    {
        [SerializeField] private bool _useDefaultDialogManager = true;
        [SerializeField] private StandAloneDialogManager _dialogManager;
        [SerializeField] private string _plotId;
        
        public void RequestDialogPlot() {
            if (_useDefaultDialogManager) {
                DialogManager.Instance.SelectDialogPlot(_plotId);
            } else {
                _dialogManager.SelectDialogPlot(_plotId);
            }
        }
    }
}