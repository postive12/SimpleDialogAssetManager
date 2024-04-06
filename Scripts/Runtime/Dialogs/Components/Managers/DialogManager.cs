using System.Collections.Generic;
using System.Text;
using DialogSystem.Runtime;
using DialogSystem.Runtime.Attributes;
using DialogSystem.Runtime.Structure.ScriptableObjects.Components.Selections;
using DialogSystem.Runtime.Structure.ScriptableObjects;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DialogSystem.Dialogs.Components.Managers
{
    /// <summary>
    /// The dialog manager that manage all dialog
    /// </summary>
    [RequireComponent(typeof(DialogPlotSelector))]
    public class DialogManager : MonoBehaviour
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
        public List<DialogSpeaker> Speakers => _speakers;
        public List<DialogEventInvoker> EventInvokers => _eventInvokers;
        public List<DialogSelector> Selectors => _selectors;
        [SerializeField] private bool _useSingleton = true;
        [DialogSelector] [SerializeField] private string _startUpDialogPlotId = "NONE";
        [SerializeField] private DialogPlotGraph _currentDialogPlot = null;
        [SerializeField] private List<DialogSpeaker> _speakers = new List<DialogSpeaker>();
        [SerializeField] private List<DialogEventInvoker> _eventInvokers = new List<DialogEventInvoker>();
        [SerializeField] private List<DialogSelector> _selectors = new List<DialogSelector>();
        public DialogManager() {
            if (_useSingleton) {
                _instance = this;
                SceneManager.sceneLoaded += OnSceneLoaded;
            }
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
            DialogPlotGraph plot = SDAManager.Instance.FindDialogPlot(plotId);
            if (plot == null) {
                Debug.LogWarning("Plot not found");
                return;
            }
            _currentDialogPlot = plot;
            _currentDialogPlot.PlayPlot();
            Play();
        }
        /// <summary>
        /// Add dialog target to dialog manager
        /// </summary>
        /// <param name="dialogTarget"></param>
        public void AddDialogTarget(DialogTargetComponent dialogTarget) {
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
        public void Play()
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
            _currentDialogPlot.Play(this);
        }
        private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
        {
            if (_startUpDialogPlotId != "NONE") {
                SelectDialogPlot(_startUpDialogPlotId);
            }
        }
        /// <summary>
        /// Invoke when dialog is end
        /// </summary>
        public void EndPlot()
        {
            _currentDialogPlot = null;
            _speakers.ForEach(speaker => speaker.OnEndPlot());
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