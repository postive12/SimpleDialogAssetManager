using System.Collections.Generic;
using DialogSystem.Runtime.Attributes;
using DialogSystem.Runtime.Structure.ScriptableObjects.Interface;
using UnityEngine;

namespace DialogSystem.Runtime.Structure.ScriptableObjects
{
    public class DialogDB : DialogScriptableObject, IDialogFinder
    {
        public List<DialogScriptableObject> DataList => _dialogPlots;
        [SDAMReadOnly][SerializeField] private List<DialogScriptableObject> _dialogPlots = new List<DialogScriptableObject>();
        public DialogDB() {
            _sdamDataType = SDAMDataType.DB;
            Id = "";
        }


        public DialogPlotGraph FindDialogPlot(string id)
        {
            string findId = id.Split('/')[0];
            //replace first id on front of the string
            string path = id.Replace(findId + "/", "");
            foreach (var dp in _dialogPlots) {
                if (dp.Id != findId) continue;
                if (dp is IDialogFinder finder) {
                    return finder.FindDialogPlot(path);
                }
            }
            Debug.LogError($"Can't find plot {findId}");
            return null;
        }
    }
}