using System;
using System.Collections.Generic;
using UnityEngine;

namespace DialogSystem.Structure.ScriptableObjects
{
    [CreateAssetMenu(menuName = "DialogSystem/DialogPlotSet", fileName = "DialogPlotSet")]
    public class DialogPlotSet : ScriptableObject
    {
        public string PlotSetId => _plotSetId;
        [SerializeField] private string _plotSetId;
        [SerializeField] private List<DialogPlot> _dialogPlots;
        public DialogPlot FindDialogById(string dialogId)
        {
            if (dialogId == DialogPlot.IgnorePlotId) {
                Debug.LogWarning("You can't use NONE as a dialog id.");
                return null;
            }
            return _dialogPlots.Find(plot => plot.PlotId == dialogId);
        }
        private void OnValidate()
        {
            if (string.IsNullOrEmpty(_plotSetId)) {
                _plotSetId = name;
            }
            foreach (var dp in _dialogPlots) {
                if (!string.IsNullOrEmpty(dp.PlotId)) continue;
                if (dp.DialogPlotGraph == null) continue;
                dp.PlotId = dp.DialogPlotGraph.name;
            }
        }
    }
}