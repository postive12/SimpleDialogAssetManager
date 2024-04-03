using System.Collections.Generic;

namespace DialogSystem.Runtime.Structure.ScriptableObjects.Interface
{
    public interface IDialogFinder {

        public DialogPlotGraph FindDialogPlot(string id);
        public List<DialogScriptableObject> GetAllData();
    }
}