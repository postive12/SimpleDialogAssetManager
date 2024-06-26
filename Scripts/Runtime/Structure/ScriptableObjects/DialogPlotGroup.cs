﻿using System.Collections.Generic;
using DialogSystem.Runtime.Structure.ScriptableObjects.Interface;
using UnityEngine;

namespace DialogSystem.Runtime.Structure.ScriptableObjects
{
    public class DialogPlotGroup : DialogScriptableObject,IDialogFinder
    {
        public List<DialogScriptableObject> DataList => _dialogPlots;
        [SerializeField] private List<DialogScriptableObject> _dialogPlots = new List<DialogScriptableObject>();
        public DialogPlotGroup() {
            _sdamDataType = SDAMDataType.GROUP;
        }
        public DialogPlotGraph FindDialogPlot(string id) {
            string findId = id.Split('/')[0];
            string path = id.Replace(findId + "/", "");
            foreach (var dp in _dialogPlots) {
                if (dp.Id != findId) continue;
                if (dp is IDialogFinder finder) {
                    return finder.FindDialogPlot(path);
                }
                else {
                    return dp as DialogPlotGraph;
                }
            }
            Debug.LogError($"Can't find plot {findId} from {Id} - {name}");
            return null;
        }
        protected override void OnValidate() {
            base.OnValidate();
        }
    }
}