using System.Collections.Generic;
using DialogSystem.Runtime.Structure.ScriptableObjects.Interface;
using UnityEngine;

namespace DialogSystem.Runtime.Structure.ScriptableObjects
{
    [CreateAssetMenu(menuName = "DialogSystem/DialogPlotGroup")]
    public class DialogPlotGroup : DialogScriptableObject,IDialogFinder
    {
        [SerializeField] private List<DialogScriptableObject> _dialogPlots;
        public DialogPlotGroup() {
            _plotDataType = PlotDataType.GROUP;
        }
        public DialogPlotGraph FindDialogPlot(string id) {
            string firstId = id.Replace(Id + "/", "");
            string nextId = firstId.Split('/')[0];
            foreach (var dp in _dialogPlots) {
                if (dp.Id != firstId) continue;
                if (dp is IDialogFinder finder) {
                    return finder.FindDialogPlot(nextId);
                }
            }
            Debug.LogError($"Can't find plot {nextId}");
            return null;
        }
        public List<DialogScriptableObject> GetAllData() {
            return _dialogPlots;
        }

        protected override void OnValidate() {
            base.OnValidate();
        }
    }
}