using System;
using System.Linq;
using DialogSystem.Runtime.Dialogs.EventInvokers;
using UnityEngine;

namespace DialogSystem.Runtime.Dialogs
{
    public class DialogPlotSelector :MonoBehaviour, IEventInvoker
    {
        private const string INVOKER_TAG = "DialogPlotSelector";
        string IEventInvoker.InvokerTag {
            get {
                return INVOKER_TAG;
            }
            set {
                return;
            }
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