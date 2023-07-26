using System.Collections.Generic;
using System.Text;
using DialogSystem.Nodes;
using DialogSystem.Runtime.Dialogs.EventInvokers;
using DialogSystem.Runtime.Dialogs.Speakers;
using DialogSystem.Runtime.Dialogs.Selections;
using DialogSystem.Structure;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DialogSystem.Scripts.Runtime.Dialogs
{
    //Create DialogManager as a singleton
    public class DialogManager : MonoBehaviour
    {
        public const string DIALOG_PATH = "Dialogs/";
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
        [SerializeField] private DialogSet _currentDialogSet = null;
        [SerializeField] private DialogPlot _currentDialogPlot = null;
        private List<ISpeaker> _speakers = new List<ISpeaker>();
        private List<IEventInvoker> _eventInvokers = new List<IEventInvoker>();
        private List<ISelector> _selectors = new List<ISelector>();
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
        }
        /// <summary>
        /// Load dialog from dialog graph
        /// </summary>
        public void RequestDialog()
        {
            if (_currentDialogPlot == null) return;
            if (!_currentDialogPlot.DialogPlotGraph.IsNextAvailable()) {
                Debug.Log("Dialog Next Not Available");
                return;
            }
            var data = _currentDialogPlot.DialogPlotGraph.Next();
            if (_currentDialogPlot.DialogPlotGraph.IsPlotEnd) {
                Debug.Log("Dialog End");
                _currentDialogPlot = null;
                return;
            }
            if (data == null) {
                StringBuilder error = new StringBuilder();
                error.Append("Can't find type dialog node!");
                error.Append("\n");
                error.Append("You should not use other type of node in dialog graph!");
                Debug.LogError(error.ToString());
                return;
            }
            var type = data.Type;
            switch (type) {
                case DialogType.DIALOG:
                    var dialogNode = data as DialogNode;
                    if (dialogNode != null) ShowDialog(dialogNode.Line);
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
        public void ShowDialog(Dialog dialog)
        {
            var targets = _speakers.FindAll(speaker => speaker.SpeakerTag == dialog.SpeakerTag);
            targets.ForEach(target => target.Speak(dialog.DialogContent.Content));
            foreach (var dialogEvent in dialog.DialogEvents) {
                var invokers = _eventInvokers.FindAll(invoker => dialogEvent.EventTargets.Contains(invoker.InvokerTag));
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
            var selector = _selectors.Find(s => s.SelectorTag == branchNode.SelectorTag);
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
            if (!_currentDialogSet) {
                StringBuilder error = new StringBuilder();
                error.Append("No dialog set found!"); 
                Debug.LogError(error.ToString());
                return;
            }
            _currentDialogPlot = _currentDialogSet.FindDialogById(plotId);
            if (_currentDialogPlot == null) {
                Debug.LogError("Can't find dialog plot with id: " + plotId);
                return;
            }
            _currentDialogPlot.DialogPlotGraph.PlayPlot();
        }
        /// <summary>
        /// Load dialog set when scene loaded with scene name
        /// </summary>
        /// <param name="scene">Current Scene</param>
        /// <param name="mode">Load mode</param>
        public void OnSceneLoaded(Scene scene,LoadSceneMode mode)
        {
            //When scene loaded, clear all data
            _speakers.Clear();
            _eventInvokers.Clear();
            _selectors.Clear();
            //Find dialog set with scene name
            var currentScene = scene.name;
            _currentDialogSet = Resources.Load<DialogSet>(DIALOG_PATH + currentScene);
            //If dialog not found, throw error
            if (!_currentDialogSet) {
                StringBuilder error = new StringBuilder();
                error.Append("No dialog set found!");
                error.Append("\n");
                error.Append("Please create dialog set in Resources/Dialogs/ and name it as target scene name");
                Debug.LogError(error.ToString());
                return;
            }
            //If dialog found, load the first dialog plot
            SelectDialogPlot(_currentDialogSet.StartUpPlotId);
        }
        /// <summary>
        /// Add speaker to dialog manager
        /// </summary>
        /// <param name="dialogSpeaker"></param>
        public void AddSpeaker(DialogSpeaker dialogSpeaker) {
            _speakers.Add(dialogSpeaker);
        }
        /// <summary>
        /// Add event invoker to dialog manager
        /// </summary>
        /// <param name="eventInvoker"></param>
        public void AddEventInvoker(IEventInvoker eventInvoker) {
            _eventInvokers.Add(eventInvoker);
        }
        /// <summary>
        /// Add selector to dialog manager
        /// </summary>
        /// <param name="selector"></param>
        public void AddSelector(ISelector selector) {
            _selectors.Add(selector);
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