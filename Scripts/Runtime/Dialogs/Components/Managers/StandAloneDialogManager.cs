
using System.Collections.Generic;
using System.Text;
using DialogSystem.Nodes;
using DialogSystem.Runtime.Dialogs.Components.Selections;
using DialogSystem.Structure;
using DialogSystem.Structure.ScriptableObjects;
using UnityEngine;

namespace DialogSystem.Dialogs.Components.Managers
{
    public class StandAloneDialogManager : MonoBehaviour, IDialogManager
    {
        /// <summary>
        /// Pause dialog load when IsPause is true
        /// </summary>
        public bool IsPause { get; set; } = false;
        public bool IsStopRequest { get; set; } = false;
        public bool UseStartUpPlot => _useStartUpPlot;
        [SerializeField] private DialogPlotSet _dialogPlotSet = null;
        [SerializeField] private bool _useStartUpPlot = false;
        [SerializeField] private string _startUpPlotId = "NONE";
        [SerializeField] private DialogPlot _currentDialogPlot = null;
        [SerializeField] private List<DialogSpeaker> _speakers = new List<DialogSpeaker>();
        [SerializeField] private List<DialogEventInvoker> _eventInvokers = new List<DialogEventInvoker>();
        [SerializeField] private List<DialogSelector> _selectors = new List<DialogSelector>();
        private void Start()
        {
            if (_useStartUpPlot) {
                SelectDialogPlot(_startUpPlotId);
            }
        }
        /// <summary>
        /// Load dialog from dialog graph
        /// </summary>
        public void RequestDialog()
        {
            //If dialog is paused, return
            if (IsPause) return;
            //If dialog is stop request, return
            if (IsStopRequest) return;
            //If current dialog plot is null, return
            if (_currentDialogPlot == null) return;
            if (!_currentDialogPlot.DialogPlotGraph) return;
            //If dialog is end, return
            if (_currentDialogPlot.DialogPlotGraph.IsPlotEnd) {
                EndPlot();
                return;
            }
            //Read Plot
            _currentDialogPlot.DialogPlotGraph.Next(this);
        }
        /// <summary>
        /// Select dialog plot from dialog set
        /// </summary>
        /// <param name="plotId">Id of Plot</param>
        public void SelectDialogPlot(string plotId)
        {
            #if UNITY_EDITOR
                Debug.Log("Select Dialog Plot: " + plotId);
            #endif
            if (!_dialogPlotSet) {
                StringBuilder error = new StringBuilder();
                error.Append("No dialog set found!"); 
                Debug.LogError(error.ToString());
                return;
            }
            _currentDialogPlot = _dialogPlotSet.FindDialogById(plotId);
            if (_currentDialogPlot == null) {
                Debug.LogError("Can't find dialog plot with id: " + plotId);
                return;
            }
            _currentDialogPlot.DialogPlotGraph.PlayPlot();
            RequestDialog();
        }
        public void Play(DialogNode node)
        {
            var dialog = node.Line;
            if (dialog.SpeakerTag != "NONE") {
                var targets = _speakers.FindAll(speaker => speaker.GetTargetTag() == dialog.SpeakerTag);
                targets.ForEach(target => target.Speak(dialog.DialogContent.Content));
                foreach (var speaker in _speakers) {
                    if (speaker.GetTargetTag() == dialog.SpeakerTag) {
                        speaker.Speak(dialog.DialogContent.Content);
                    } else if(node.IsEndPast){
                        speaker.EndSpeak();
                    }
                }
            }
            foreach (var dialogEvent in dialog.DialogEvents) {
                var invokers = 
                    _eventInvokers.FindAll(
                        invoker => 
                            (invoker.GetTargetTag() != "NONE" && dialogEvent.EventTargets.Contains(invoker.GetTargetTag()))
                    );
                invokers.ForEach(speaker => speaker.Invoke(dialogEvent.EventName));
            }
        }
        public void Play(DialogBranchNode node)
        {
            //Debug.Log("Play Branch");
            var selections = node.Selections;
            var selector = _selectors.Find(s => s.GetTargetTag() == node.SelectorTag);
            if (selector != null) {
                selector.CreateSelections(selections,node,this);
            }
        }
        public void EndPlot()
        {
            #if UNITY_EDITOR
                Debug.Log("Plot End");
            #endif
            _currentDialogPlot = null;
            _speakers.ForEach(speaker => speaker.EndSpeak());
        }
    }
}