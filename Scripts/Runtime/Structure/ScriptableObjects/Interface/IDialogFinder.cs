using System.Collections.Generic;

namespace DialogSystem.Runtime.Structure.ScriptableObjects.Interface
{
    public interface IDialogFinder {

        public List<DialogScriptableObject> DataList { get; }
        public DialogPlotGraph FindDialogPlot(string id);
    }
}