using System.Collections.Generic;
using System.Text;
using DialogSystem.Nodes;
using DialogSystem.Runtime.Dialogs.EventInvokers;
using DialogSystem.Runtime.Dialogs.Speakers;
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
        public void RequestDialog()
        {
            if (_currentDialogPlot == null) return;
            if (_currentDialogPlot.DialogPlotGraph.IsPlotEnd) {
                _currentDialogPlot = null;
                return;
            }
            var data = _currentDialogPlot.DialogPlotGraph.CurrentNode;
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
                    break;
            }
            _currentDialogPlot.DialogPlotGraph.Next();
        }
        public void ShowDialog(Dialog dialog)
        {
            var targets = _speakers.FindAll(speaker => speaker.SpeakerId == dialog.SpeakerId);
            targets.ForEach(target => target.Speak(dialog.DialogContent.Content));
            foreach (var dialogEvent in dialog.DialogEvents) {
                var invokers = _eventInvokers.FindAll(invoker => dialogEvent.EventTargets.Contains(invoker.InvokerId));
                invokers.ForEach(speaker => speaker.Invoke(dialogEvent.EventName));
            }
        }
        public void SelectDialogPlot(string dialogId)
        {
            if (!_currentDialogSet) {
                StringBuilder error = new StringBuilder();
                error.Append("No dialog set found!"); 
                Debug.LogError(error.ToString());
                return;
            }
            _currentDialogPlot = _currentDialogSet.FindDialogById(dialogId);
            if (_currentDialogPlot == null) {
                Debug.LogError("Can't find dialog plot with id: " + dialogId);
                return;
            }
            _currentDialogPlot.DialogPlotGraph.PlayPlot();
        }
        public void OnSceneLoaded(Scene scene,LoadSceneMode mode)
        {
            _speakers.Clear();
            _eventInvokers.Clear();
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
        public void AddSpeaker(DialogSpeaker dialogSpeaker) {
            _speakers.Add(dialogSpeaker);
        }
        public void AddEventInvoker(IEventInvoker eventInvoker) {
            _eventInvokers.Add(eventInvoker);
        }
        private void OnDisable()
        {
            _eventInvokers.Clear();
            _speakers.Clear();
        }
    }
}