using System.Collections.Generic;
using System.Text;
using DialogSystem.Nodes;
using DialogSystem.Nodes.Branches;
using DialogSystem.Nodes.Lines;
using DialogSystem.Runtime;
using DialogSystem.Runtime.Structure.ScriptableObjects.Components;
using DialogSystem.Runtime.Structure.ScriptableObjects.Components.Selections;
using DialogSystem.Runtime.Structure.ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace DialogSystem.Dialogs.Components.Managers
{
    /// <summary>
    /// The dialog manager that manage all dialog
    /// </summary>
    [RequireComponent(typeof(DialogPlotSelector))]
    public class DialogManager : MonoBehaviour, IDialogManager
    {
        /// <summary>
        /// Singleton instance of DialogManager
        /// </summary>
        public static DialogManager Instance {
            get {
                if (_instance != null) {
                    return _instance;
                }
                _instance = FindObjectOfType<DialogManager>();
                if (_instance != null) {
                    return _instance;
                }
                var singleton = new GameObject("DialogManager");
                _instance = singleton.AddComponent<DialogManager>();
                return _instance;
            }
        }
        private static DialogManager _instance;
        /// <summary>
        /// Pause dialog load when IsPause is true
        /// </summary>
        public bool IsPause { get; set; } = false;
        public bool IsStopRequest { get; set; } = false;
        [SerializeField] private bool _useSingleton = true;
        [SerializeField] private DialogPlotGraph _currentDialogPlot = null;
        [SerializeField] private List<DialogSpeaker> _speakers = new List<DialogSpeaker>();
        [SerializeField] private List<DialogEventInvoker> _eventInvokers = new List<DialogEventInvoker>();
        [SerializeField] private List<DialogSelector> _selectors = new List<DialogSelector>();

        public DialogManager() {
            if (_useSingleton) {
                _instance = this;
            }
        }
        /// <summary>
        /// Add dialog target to dialog manager
        /// </summary>
        /// <param name="dialogTarget"></param>
        public void AddDialogTarget(DialogTargetComponent dialogTarget) {
            //_dialogTargets.Add(dialogTarget);
            //Check what interface is implemented
            if (dialogTarget is DialogSpeaker speaker) {
                _speakers.Add(speaker);
            }
            if (dialogTarget is DialogEventInvoker eventInvoker) {
                _eventInvokers.Add(eventInvoker);
            }
            if (dialogTarget is DialogSelector selector) {
                _selectors.Add(selector);
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
            if (_currentDialogPlot.IsPlotEnd) {
                Debug.Log("Dialog End");
                EndPlot();
                return;
            }
            //Read Plot
            _currentDialogPlot.Next(this);
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
            if (!_currentDialogPlot) {
                StringBuilder error = new StringBuilder();
                error.Append("No dialog set found!"); 
                Debug.LogError(error.ToString());
                return;
            }
            _currentDialogPlot = SDAManager.Instance.FindDialogPlot(plotId);
            if (_currentDialogPlot == null) {
                Debug.LogWarning("Can't find dialog plot with id: " + plotId);
                return;
            }
            _currentDialogPlot.PlayPlot();
            RequestDialog();
        }
        public void Play(DialogNode node)
        {
            var dialog = node.Line;
            if (dialog.SpeakerTag != "NONE") {
                var targets = _speakers.FindAll(speaker => speaker.TargetTag == dialog.SpeakerTag);
                targets.ForEach(target => target.Speak(dialog.DialogContent.Content));
                foreach (var speaker in _speakers) {
                    if (speaker.TargetTag == dialog.SpeakerTag) {
                        speaker.Speak(dialog.DialogContent.Content);
                    } else if(node.IsPlayed){
                        speaker.EndSpeak();
                    }
                }
            }
            foreach (var dialogEvent in dialog.DialogEvents) {
                var invokers = 
                    _eventInvokers.FindAll(
                        invoker => 
                            (invoker.TargetTag != "NONE" && dialogEvent.EventTargets.Contains(invoker.TargetTag))
                    );
                invokers.ForEach(speaker => speaker.Invoke(dialogEvent.EventName));
            }
        }
        public void Play(DialogBranchNode node)
        {
            var selections = node.Selections;
            var selector = _selectors.Find(s => s.TargetTag == node.SelectorTag);
            if (selector != null) {
                selector.CreateSelections(selections,node,this);
            }
        }
        /// <summary>
        /// Invoke when dialog is end
        /// </summary>
        public void EndPlot()
        {
            _currentDialogPlot = null;
            _speakers.ForEach(speaker => speaker.EndSpeak());
        }
        /// <summary>
        /// Get current dialog plot id
        /// </summary>
        /// <returns>The id of current plot</returns>
        public string GetCurrentDialogPlotId() {
            if (_currentDialogPlot == null) return string.Empty;
            return _currentDialogPlot.Id;
        }
        /// <summary>
        /// Clear all data when dialog manager disabled
        /// </summary>
        private void OnDisable()
        {
            _eventInvokers.Clear();
            _selectors.Clear();
            _speakers.Clear();
        }

    }
}