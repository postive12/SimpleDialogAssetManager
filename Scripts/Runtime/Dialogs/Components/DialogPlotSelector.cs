using DialogSystem.Dialogs.Components.Managers;
using DialogSystem.Runtime.Dialogs.Interfaces;
using UnityEngine;

namespace DialogSystem.Dialogs.Components
{
    public class DialogPlotSelector :MonoBehaviour, IEventInvoker
    {
        private string _targetTag;
        private const string INVOKER_TAG = "DialogPlotSelector";
        string IDialogTarget.TargetTag {
            get {
                return _targetTag;
            }
            set {
                _targetTag = value;
            }
        }
        public string GetTargetTag() {
            return _targetTag;
        }
        private void Start()
        {
            DialogManager.AddEventInvoker(this);
        }
        public void Invoke(string eventName) {
            DialogManager.Instance.SelectDialogPlot(eventName);
        }

    }
}