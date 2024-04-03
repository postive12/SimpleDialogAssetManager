using System;
using System.Collections.Generic;
using DialogSystem.Runtime.Attributes;
using DialogSystem.Runtime.Structure.ScriptableObjects;
using DialogSystem.Runtime.Structure.ScriptableObjects.Interface;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace DialogSystem.Runtime
{
    public class SDAManager : ScriptableObject, IDialogFinder
    {
        private static readonly List<string> STANDARD_TARGETS = new List<string> {
            "===Base Target===",
            "DialogPlotSelector",
        }; 
        public static SDAManager Instance {
            get {
                if (_instance == null) {
                    var loadedData = Resources.LoadAll<SDAManager>("");
                    _instance = loadedData.Length > 0 ? loadedData[0] : CreateInstance<SDAManager>();
                    _instance.Load();
                }
                return _instance;
            }
        }
        private static SDAManager _instance;
        public static List<string> DialogTargetIds {
            get {
                var result = new List<string>();
                if (Instance != null) {
                    result.AddRange(Instance._dialogTargetIds);
                }
                result.AddRange(STANDARD_TARGETS);
                return result;
            }
        }
        public static List<string> DialogPaths {
            get {
                var result = new List<string>();
                if (Instance != null) {
                    foreach (var plot in Instance._dialogDB) {
                        result.AddRange(GetAllDialogPaths("", plot));
                    }
                }
                return result;
            }
        }
        private static List<string> GetAllDialogPaths(string currentPath,DialogScriptableObject data) {
            List<string> result = new List<string>();
            currentPath = string.IsNullOrEmpty(currentPath) ? currentPath : currentPath + "/";
            if (data is IDialogFinder finder) {
                foreach (var plot in finder.GetAllData()) {
                    result.AddRange(GetAllDialogPaths(currentPath + data.Id, plot));
                }
            }
            else {
                result.Add(currentPath + data.Id);
            }
            return result;
        }
        [DialogSelector][SerializeField] private string _id = "";
        [SerializeField] private List<string> _dialogTargetIds = new List<string>();
        [SerializeField] private List<DialogScriptableObject> _dialogDB = new List<DialogScriptableObject>();
        
        [SerializeField] private DialogScriptableObject _testTarget;
        [Button]
        private void Test() {
            var result = FindDataOwnerByGuid(_testTarget.GUID,this);
            Debug.Log(result.Id);
        }
        public DialogPlotGraph FindDialogPlot(string id) {
            string firstPath = id.Split('/')[0];
            string nextPath = id.Replace(firstPath + "/", "");
            foreach (var db in _dialogDB) {
                if (db.Id != firstPath) {
                    continue;
                }
                if (db is IDialogFinder finer) {
                    return finer.FindDialogPlot(nextPath);
                }
            }
            Debug.LogError($"Can't find plot {nextPath}");
            return null;
        }
        public DialogScriptableObject FindDialogByGuid(string guid,IDialogFinder finder) {
            var dso = finder as DialogScriptableObject;
            if (dso != null) {
                Debug.Log(dso.Id);
                if (dso.GUID == guid) {
                    return dso;
                }
            }
            foreach (var data in finder.GetAllData()) {
                if (data.GUID == guid) {
                    return data;
                }
                if (data is IDialogFinder dialogFinder) {
                    var result = FindDialogByGuid(guid, dialogFinder);
                    if (result != null) {
                        return result;
                    }
                }
            }
            return null;
        }
        public DialogScriptableObject FindDataOwnerByGuid(string guid,IDialogFinder finder) {
            foreach (var data in finder.GetAllData()) {
                if (data.GUID == guid) {
                    return finder as DialogScriptableObject;
                }
                if (data is IDialogFinder dialogFinder) {
                    var result = FindDataOwnerByGuid(guid, dialogFinder);
                    if (result != null) {
                        return result;
                    }
                }
            }
            return null;
        }
        public List<DialogScriptableObject> GetAllData() {
            return _dialogDB;
        }

        public List<IDialogFinder> GetAllFinders() {
            List<IDialogFinder> finders = new List<IDialogFinder>();
            foreach (var plot in _dialogDB) {
                if (plot is IDialogFinder finder) {
                    finders.Add(finder);
                }
            }
            return finders;
        }
        #if UNITY_EDITOR
        private void Load() {
            
        }
        #region Editor
        public void AddDialog(DialogScriptableObject dialog) {
            _dialogDB.Add(dialog);
        }
        public void RemoveDialog(DialogScriptableObject dialog) {
            _dialogDB.Remove(dialog);
        }
        #endregion
        
        #endif
    }
}