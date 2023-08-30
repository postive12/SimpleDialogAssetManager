using System;
using DialogSystem.Runtime.Dialogs;

namespace DialogSystem.Structure
{
    [Serializable]
    public class DialogPlot
    {
        public const string IgnorePlotId = "NONE";
        public string PlotId;
        public DialogPlotGraph DialogPlotGraph;
    }
}