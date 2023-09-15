using System;
using System.Collections.Generic;
using System.Text;
using DialogSystem.Nodes;
using DialogSystem.Runtime.Dialogs.Components;
using DialogSystem.Runtime.Dialogs.Components.Selections;
using DialogSystem.Structure;
using DialogSystem.Structure.ScriptableObjects;
using UnityEngine;

namespace DialogSystem.Dialogs.Components.Managers
{
    public class IndependentDialogManager : MonoBehaviour
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
            if (_currentDialogPlot.DialogPlotGraph == null) return;
            //If can't get next dialog node, return
            if (!_currentDialogPlot.DialogPlotGraph.IsNextAvailable()) {
                Debug.Log("Dialog Next Not Available");
                return;
            }
            //Get next dialog node
            var data = _currentDialogPlot.DialogPlotGraph.Next();
            //If dialog is end, return
            if (_currentDialogPlot.DialogPlotGraph.IsPlotEnd) {
                Debug.Log("Dialog End");
                EndPlot();
                return;
            }
            //If dialog node is null, return
            if (data == null) {
                StringBuilder error = new StringBuilder();
                error.Append("Can't find type dialog node!");
                error.Append("\n");
                error.Append("You should not use other type of node in dialog graph!");
                Debug.LogError(error.ToString());
                return;
            }
            //Show dialog from dialog node
            var type = data.Type;
            switch (type) {
                case DialogType.DIALOG:
                    var dialogNode = data as DialogNode;
                    if (dialogNode != null) ShowDialog(dialogNode);
                    break;
                case DialogType.BRANCH:
                    var branchNode = data as DialogBranchNode;
                    if (branchNode != null) ShowBranch(branchNode);
                    break;
            }
        }
        /// <summary>
        /// Show dialog from dialog parameter
        /// </summary>
        /// <param name="dialog">The dialog that contains data</param>
        public void ShowDialog(DialogNode dialogNode)
        {
            var dialog = dialogNode.Line;
            if (dialog.SpeakerTag != "NONE") {
                var targets = _speakers.FindAll(speaker => speaker.GetTargetTag() == dialog.SpeakerTag);
                targets.ForEach(target => target.Speak(dialog.DialogContent.Content));
                foreach (var speaker in _speakers) {
                    if (speaker.GetTargetTag() == dialog.SpeakerTag) {
                        speaker.Speak(dialog.DialogContent.Content);
                    } else if(dialogNode.IsEndPast){
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
        /// <summary>
        /// Show branch from branch node
        /// </summary>
        /// <param name="branchNode">Target branch node</param>
        private void ShowBranch(DialogBranchNode branchNode)
        {
            var selections = branchNode.Selections;
            var selector = _selectors.Find(s => s.GetTargetTag() == branchNode.SelectorTag);
            if (selector != null) {
                selector.CreateSelections(selections,branchNode);
            }
        }
        /// <summary>
        /// Select dialog plot from dialog set
        /// </summary>
        /// <param name="plotId">Id of Plot</param>
        public void SelectDialogPlot(string plotId)
        {
            Debug.Log("Select Dialog Plot: " + plotId);
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
        public void EndPlot()
        {
            _currentDialogPlot = null;
            _speakers.ForEach(speaker => speaker.EndSpeak());
        }
    }
}