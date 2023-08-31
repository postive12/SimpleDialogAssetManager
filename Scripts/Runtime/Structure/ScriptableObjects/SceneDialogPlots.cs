using System.Collections.Generic;
using UnityEngine;

namespace DialogSystem.Structure.ScriptableObjects
{
    [CreateAssetMenu(menuName = "DialogSystem/SceneDialogPlots", fileName = "SceneDialogPlots")]
    public class SceneDialogPlots : ScriptableObject
    {
        public string StartUpPlotId => _startUpPlotId;
        public bool UseStartUpPlot => _useStartUpPlot;
        
        [SerializeField] private bool _useStartUpPlot = false;
        [SerializeField] private string _startUpPlotId = "NONE";
        [SerializeField] private List<DialogPlot> DialogPlots;
        [SerializeField] private List<DialogPlotSet> DialogPlotSets;
        public DialogPlot FindDialogById(string dialogId)
        {
            if (dialogId == DialogPlot.IgnorePlotId) {
                Debug.LogWarning("You can't use NONE as a dialog id.");
                return null;
            }
            var paths = dialogId.Split('/');
            switch (paths.Length) {
                case >= 3:
                    Debug.LogError("To many paths in dialog id.");
                    return null;
                case 2:
                    if (paths[0] == DialogPlot.IgnorePlotId) {
                        Debug.LogError("You can't use NONE as a plot set id.");
                        return null;
                    }
                    DialogPlotSet plotSet = DialogPlotSets.Find(set => set.PlotSetId == paths[0]);
                    if (plotSet != null) return plotSet.FindDialogById(paths[1]);
                    Debug.LogError($"Can't find plot set {paths[0]}");
                    return null;
                default:
                    return DialogPlots.Find(plot => plot.PlotId == dialogId);
            }
        }
    }
}