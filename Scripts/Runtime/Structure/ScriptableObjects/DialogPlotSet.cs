using System.Collections.Generic;
using UnityEngine;

namespace DialogSystem.Structure.ScriptableObjects
{
    [CreateAssetMenu(menuName = "DialogSystem/DialogPlotSet", fileName = "DialogPlotSet")]
    public class DialogPlotSet : ScriptableObject
    {
        public string PlotSetId => _plotSetId;
        [SerializeField] private string _plotSetId = "NONE";
        [SerializeField] private List<DialogPlot> DialogPlots;
        public DialogPlot FindDialogById(string dialogId)
        {
            if (dialogId == DialogPlot.IgnorePlotId) {
                Debug.LogWarning("You can't use NONE as a dialog id.");
                return null;
            }
            return DialogPlots.Find(plot => plot.PlotId == dialogId);
        }
    }
}