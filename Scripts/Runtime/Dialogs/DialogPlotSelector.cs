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
        private void OnValidate()
        {
            //Check DialogPlotSelector tag is in the unity tag list
            var tagList = UnityEditorInternal.InternalEditorUtility.tags;
            if (tagList.Any(t => t == INVOKER_TAG)) {
                UnityEditorInternal.InternalEditorUtility.RemoveTag(INVOKER_TAG);
                Debug.LogError("You should not \"DialogPlotSelector\" as a tag, it is used by DialogPlotSelector.");
                Debug.LogError("Automatically remove \"DialogPlotSelector\" tag from tag list.");
            }
        }
    }
}