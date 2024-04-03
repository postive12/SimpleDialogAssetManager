using System;
using DialogSystem.Runtime.Attributes;
using DialogSystem.Runtime.Structure.ScriptableObjects.Interface;
using UnityEngine;

namespace DialogSystem.Runtime.Structure.ScriptableObjects
{
    public abstract class DialogScriptableObject : ScriptableObject, IDialogIdentifier
    {
        public PlotDataType PlotDataType => _plotDataType;
        protected PlotDataType _plotDataType = PlotDataType.NONE;
        public virtual string GUID {
            get => _guid;
            set => _guid = value;
        }
        public string Id {
            get => _id;
            set {
                _id = value;
                #if UNITY_EDITOR
                OnDataChanged?.Invoke();
                #endif
            }
        }
        [SDAMReadOnly][SerializeField] private string _guid = System.Guid.NewGuid().ToString();
        [SDAMReadOnly][SerializeField] private string _id = "Unnamed";
        #if UNITY_EDITOR
        public Action OnDataChanged;
        public Action OnDataHardChanged;
        #endif
        protected virtual void OnValidate() {
            #if UNITY_EDITOR
            OnDataChanged?.Invoke();
            #endif
        }


    }
}