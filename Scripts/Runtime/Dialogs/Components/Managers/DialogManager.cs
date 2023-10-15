using System.Collections.Generic;
using System.Text;
using DialogSystem.Nodes;
using DialogSystem.Runtime.Dialogs;
using DialogSystem.Runtime.Dialogs.Components;
using DialogSystem.Runtime.Dialogs.Interfaces;
using DialogSystem.Structure;
using DialogSystem.Structure.ScriptableObjects;
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
        [SerializeField] private SceneDialogPlots _currentSceneDialogPlots = null;
        [SerializeField] private DialogPlot _currentDialogPlot = null;
        private static List<ISpeaker> _speakers = new List<ISpeaker>();
        private static List<IEventInvoker> _eventInvokers = new List<IEventInvoker>();
        private static List<ISelector> _selectors = new List<ISelector>();
        /// <summary>
        /// Add dialog target to dialog manager
        /// </summary>
        /// <param name="dialogTarget"></param>
        public static void AddDialogTarget(DialogTargetComponent dialogTarget) {
            //_dialogTargets.Add(dialogTarget);
            //Check what interface is implemented
            if (dialogTarget is ISpeaker speaker) {
                _speakers.Add(speaker);
            }
            if (dialogTarget is IEventInvoker eventInvoker) {
                _eventInvokers.Add(eventInvoker);
            }
            if (dialogTarget is ISelector selector) {
                _selectors.Add(selector);
            }
        }
        private void Awake()
        {
            if (_instance != null && _instance != this) {
                Destroy(gameObject);
                return;
            }
            //Make DialogManager dontdestroyonload
            if (transform.parent != null && transform.root != null) {
                DontDestroyOnLoad(transform.root.gameObject);
            }
            else {
                DontDestroyOnLoad(gameObject);
            }
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnloaded;
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
            //Debug.Log("_currentDialogPlot != null");
            //If dialog is end, return
            if (_currentDialogPlot.DialogPlotGraph.IsPlotEnd) {
                Debug.Log("Dialog End");
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
            if (!_currentSceneDialogPlots) {
                StringBuilder error = new StringBuilder();
                error.Append("No dialog set found!"); 
                Debug.LogError(error.ToString());
                return;
            }
            _currentDialogPlot = _currentSceneDialogPlots.FindDialogById(plotId);
            if (_currentDialogPlot == null) {
                Debug.LogWarning("Can't find dialog plot with id: " + plotId);
                return;
            }
            _currentDialogPlot.DialogPlotGraph.PlayPlot();
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
                    } else if(node.IsEndPast){
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
        public string GetCurrentDialogPlotId()
        {
            if (_currentDialogPlot == null) return string.Empty;
            return _currentDialogPlot.PlotId;
        }
        /// <summary>
        /// Load dialog set when scene loaded with scene name
        /// </summary>
        /// <param name="scene">Current Scene</param>
        /// <param name="mode">Load mode</param>
        private void OnSceneLoaded(Scene scene,LoadSceneMode mode)
        {
            //Find dialog set with scene name
            var currentScene = scene.name;
            _currentSceneDialogPlots = Resources.Load<SceneDialogPlots>(SDAMConst.SDAM_ASSET_FOLDER + currentScene);
            //If dialog not found, throw error
            if (!_currentSceneDialogPlots) {
                StringBuilder error = new StringBuilder();
                error.Append("No dialog set found!");
                error.Append("\n");
                error.Append("Please create dialog set in Resources/Dialogs/ and name it as target scene name");
                Debug.LogWarning(error.ToString());
                return;
            }
            //If dialog found, load the first dialog plot
            if (_currentSceneDialogPlots.UseStartUpPlot) SelectDialogPlot(_currentSceneDialogPlots.StartUpPlotId);
        }
        /// <summary>
        /// Reset all data when scene unloaded
        /// </summary>
        /// <param name="scene"></param>
        private void OnSceneUnloaded(Scene scene)
        {
            _currentSceneDialogPlots = null;
            _currentDialogPlot = null;
            _eventInvokers.Clear();
            _selectors.Clear();
            _speakers.Clear();
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