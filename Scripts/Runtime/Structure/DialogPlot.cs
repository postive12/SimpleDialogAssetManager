using System;
using DialogSystem.Runtime.Dialogs;
using UnityEngine;

namespace DialogSystem.Structure
{
    [Serializable]
    public class DialogPlot
    {
        public DialogPlotGraph DialogPlotGraph => _dialogPlotGraph;
        public const string IgnorePlotId = "NONE";
        public string PlotId = "NONE";
        [SerializeField] private DialogPlotGraph _dialogPlotGraph;
    }
}