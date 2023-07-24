using System.Collections.Generic;
using UnityEngine;

namespace DialogSystem.Structure
{
    [CreateAssetMenu(menuName = "DialogSystem/DialogSet", fileName = "DialogSet")]
    public class DialogSet : ScriptableObject
    {
        public const string IgnorePlotId = "NONE";
        public string StartUpPlotId = "NONE";
        public List<DialogPlot> DialogPlots;

        public DialogPlot FindDialogById(string dialogId)
        {
            if (dialogId == IgnorePlotId) {
                Debug.LogWarning("You can't use NONE as a dialog id.");
                return null;
            }
            return DialogPlots.Find(plot => plot.PlotId == dialogId);
        }
    }
}